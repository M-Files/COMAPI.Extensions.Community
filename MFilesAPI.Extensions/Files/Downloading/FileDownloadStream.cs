using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFilesAPI.Extensions
{
	/// <summary>
	/// Allows access to files from within the M-Files vault in an implementation of <see cref="Stream"/>.
	/// </summary>
	public class FileDownloadStream
		: Stream
	{
		/// <summary>
		/// The vault to download from.
		/// </summary>
		public Vault Vault { get; protected set; }

		/// <summary>
		/// The file id to download.
		/// </summary>
		public int FileID { get; protected set; }

		/// <summary>
		/// The file version to download.
		/// </summary>
		public int FileVersion { get; protected set; }

		/// <summary>
		/// The file size of the downloaded file.
		/// </summary>
		public long FileSize { get; protected set; }

		/// <summary>
		/// The format of the file to download.
		/// </summary>
		public MFFileFormat FileFormat { get; protected set; }

		/// <summary>
		/// The open file download session.
		/// </summary>
		public FileDownloadSession DownloadSession { get; protected set; }

		/// <summary>
		/// The M-Files API limits download block size.
		/// If a block larger than this is requested
		/// via <see cref="Read(byte[], int, int)"/>
		/// then data will be read from M-Files in blocks of this size.
		/// </summary>
		public const int MaximumBlockSize = 4 * 1024 * 1024;

		/// <summary>
		/// Creates a <see cref="FileDownloadStream"/> but does not open the download session.
		/// </summary>
		/// <param name="fileToDownload">The file to download.</param>
		/// <param name="vault">The vault to download from.</param>
		/// <param name="fileFormat">The format to request the file in from the server.</param>
		public FileDownloadStream(
			ObjectFile fileToDownload,
			Vault vault,
			MFFileFormat fileFormat = MFFileFormat.MFFileFormatNative
		)
		{
			// Set properties.
			if (null == fileToDownload)
				throw new ArgumentNullException(nameof(fileToDownload));

			this.Vault = vault ?? throw new ArgumentNullException(nameof(vault));
			this.FileID = fileToDownload.ID;
			this.FileVersion = fileToDownload.Version;
			this.FileFormat = fileFormat;
			this.FileSize = fileToDownload.LogicalSize;
		}


		/// <summary>
		///  Creates a <see cref="FileDownloadStream"/> but does not open the download session.
		/// </summary>
		/// <param name="fileIDToDownload">The file ID to download.</param>
		/// <param name="fileVersionToDownload">The file version to download.</param>
		/// <param name="vault">The vault to download from.</param>
		/// <param name="fileFormat">The format to request the file in from the server.</param>
		public FileDownloadStream(
			int fileIDToDownload,
			int fileVersionToDownload,
			Vault vault,
			MFFileFormat fileFormat = MFFileFormat.MFFileFormatNative
		)
		{
			// Set properties.
			this.Vault = vault ?? throw new ArgumentNullException(nameof(vault));
			this.FileID = fileIDToDownload;
			this.FileVersion = fileVersionToDownload;
			this.FileFormat = fileFormat;
			this.FileSize = Vault?.ObjectFileOperations?.GetFileSize(new FileVer { ID = FileID, Version = FileVersion }) ?? 0;
		}

		/// <summary>
		/// Opens a download session to download the file.
		/// </summary>
		/// <remarks>Closes any existing download session.</remarks>
		public void OpenDownloadSession()
		{
			// Close any existing download session.
			if (null != this.DownloadSession)
				this.Close();

			// Start the download session.
			this.DownloadSession = this
				.Vault
				.ObjectFileOperations
				.DownloadFileInBlocks_BeginEx
				(
					this.FileID,
					this.FileVersion,
					this.FileFormat
				);
		}

		#region Overrides of Stream

		/// <inheritdoc />
		public override void Close()
		{
			if (null != this.DownloadSession)
			{
				this.Vault?.ObjectFileOperations.CancelFileDownloadSession(this.DownloadSession.DownloadID);
			}

			this.DownloadSession = null;
			this.position = 0;

			this.Dispose(true);
			GC.SuppressFinalize(true);
		}

		/// <inheritdoc />
		public override void Flush()
		{
		}

		/// <inheritdoc />
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		/// <remarks>Calling <see cref="Read"/> will call <see cref="OpenDownloadSession"/> if no session already exists.</remarks>
		public override int Read(byte[] buffer, int offset, int count)
		{
			// Sanity.
			if (null == buffer)
				throw new ArgumentNullException(nameof(buffer));
			if (offset < 0 || offset > buffer.Length)
				throw new ArgumentOutOfRangeException(nameof(offset));
			if (count < 0 || count + offset > buffer.Length)
				throw new ArgumentOutOfRangeException(nameof(count));

			// Do we need to start the session?
			if (null == this.DownloadSession)
				this.OpenDownloadSession();

			// If the count is zero then die.
			if(count == 0)
				return 0;

			// Are we at the end?
			if (this.Position >= this.DownloadSession.FileSize)
				return 0;

			// Loop through and get max 4MB blocks.
			int dataRead = 0;
			bool atEnd = false;
			while (dataRead < count && !atEnd)
			{
				// Work out how much to get.
				var blockSize = count;
				if(blockSize > MaximumBlockSize)
					blockSize = MaximumBlockSize;
				if (dataRead + blockSize > this.Length)
					blockSize = (int)(this.Length - dataRead);
				if (blockSize <= 0)
					break;

				// Read the block.
				var blockData = this
					.Vault
					.ObjectFileOperations
					.DownloadFileInBlocks_ReadBlock
					(
						this.DownloadSession.DownloadID,
						blockSize,
						this.position
					);

				// Check the buffer is big enough.
				if (dataRead + blockData.Length > buffer.Length)
					throw new ArgumentException($"The buffer size ({buffer.Length}) is not big enough to hold the amount of data requested ({dataRead + blockData.Length}).", nameof(buffer));

				// Copy the data into the supplied buffer.
				Array.Copy(blockData, 0, buffer, dataRead + offset, blockData.Length);

				// Update our position with the number of bytes read.
				this.position += blockData.Length;
				dataRead += blockData.Length;
				atEnd = blockData.Length < blockSize;

			}


			return dataRead;
		}

		/// <inheritdoc />
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		public override bool CanRead => true;

		/// <inheritdoc />
		public override bool CanSeek => false;

		/// <inheritdoc />
		public override bool CanWrite => false;

		/// <inheritdoc />
		public override long Length => (long)(this.DownloadSession?.FileSize ?? this.FileSize);

		private long position = 0;

		/// <inheritdoc />
		/// <remarks>Setting <see cref="Position"/> will call <see cref="OpenDownloadSession"/> if no session already exists.</remarks>
		public override long Position
		{
			get => this.position;
			set => throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		#endregion

	}
}