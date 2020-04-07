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
		/// The file to download.
		/// </summary>
		public ObjectFile FileToDownload { get; protected set; }

		/// <summary>
		/// The format of the file to download.
		/// </summary>
		public MFFileFormat FileFormat { get; protected set; }

		/// <summary>
		/// The open file download session.
		/// </summary>
		public FileDownloadSession DownloadSession { get; protected set; }

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
			this.FileToDownload = fileToDownload ?? throw new ArgumentNullException(nameof(fileToDownload));
			this.Vault = vault ?? throw new ArgumentNullException(nameof(vault));
			this.FileFormat = fileFormat;
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
					this.FileToDownload.ID,
					this.FileToDownload.Version,
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

			// Read the block.
			byte[] blockData = this
				.Vault
				.ObjectFileOperations
				.DownloadFileInBlocks_ReadBlock
				(
					this.DownloadSession.DownloadID,
					count,
					this.position
				);

			// Check the buffer is big enough.
			if (blockData.Length > buffer.Length)
				throw new ArgumentException($"The buffer size ({buffer.Length}) is not big enough to hold the amount of data requested ({count}).", nameof(buffer));

			// Copy the data into the supplied buffer.
			Buffer.BlockCopy(blockData, 0, buffer, 0, blockData.Length);

			// Return the number of bytes read.
			this.position += blockData.Length;
			return blockData.Length;
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
		public override long Length => this.DownloadSession?.FileSize ?? this.FileToDownload?.LogicalSize ?? 0;

		private long position = 0;

		/// <inheritdoc />
		public override long Position
		{
			get => this.position;
			set => throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override void Dispose(bool disposing)
		{
			this.Close();

			base.Dispose(disposing);
		}

		#endregion

	}
}