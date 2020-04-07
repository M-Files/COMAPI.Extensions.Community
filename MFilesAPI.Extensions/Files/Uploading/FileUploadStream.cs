using System;
using System.IO;

namespace MFilesAPI.Extensions
{
	/// <summary>
	/// A write-only implementation of <see cref="Stream"/> to allow file contents within the M-Files vault to be written.
	/// </summary>
	public class FileUploadStream
		: Stream
	{
		/// <summary>
		/// The vault to upload to.
		/// </summary>
		public Vault Vault { get; protected set; }

		/// <summary>
		/// The file to overwrite.
		/// </summary>
		public FileVer FileToOverwrite { get; protected set; }

		/// <summary>
		/// The file to overwrite.
		/// </summary>
		public ObjID ObjId { get; protected set; }

		/// <summary>
		/// The open file upload session Id.
		/// </summary>
		public int? UploadSessionId { get; protected set; }

		/// <summary>
		/// If true, will automatically save the file contents to the vault when this stream is disposed of.
		/// </summary>
		public bool AutomaticallyCommitOnDisposal { get;set; }

		/// <summary>
		/// Creates a <see cref="FileUploadStream"/> but does not open the upload session.
		/// </summary>
		/// <param name="fileToOverwrite">The file to overwrite.</param>
		/// <param name="objId">The object the file belongs to.</param>
		/// <param name="vault">The vault to download from.</param>
		/// <param name="automaticallyCommitOnDisposal">If true, will automatically save the file contents to the vault when this stream is disposed of.</param>
		public FileUploadStream
		(
			FileVer fileToOverwrite,
			ObjID objId,
			Vault vault,
			bool automaticallyCommitOnDisposal = true
		)
		{
			// Set properties.
			this.FileToOverwrite = fileToOverwrite ?? throw new ArgumentNullException(nameof(fileToOverwrite));
			this.ObjId = objId ?? throw new ArgumentNullException(nameof(objId));
			this.Vault = vault ?? throw new ArgumentNullException(nameof(vault));
			this.AutomaticallyCommitOnDisposal = automaticallyCommitOnDisposal;
		}

		/// <summary>
		/// Opens an upload session to replace the file contents.
		/// </summary>
		/// <remarks>Closes any existing upload session.</remarks>
		public void OpenUploadSession()
		{
			// Throw away any existing upload session.
			if (null != this.UploadSessionId)
				this.Close(false);


			// Start the upload session.
			this.UploadSessionId = this
				.Vault
				.ObjectFileOperations
				.UploadFileBlockBegin();
		}

		/// <summary>
		/// Closes any active upload session with the M-Files server and resets the current position.
		/// </summary>
		/// <param name="commit">If true, commits the data to the file.</param>
		public void Close(bool commit)
		{
			// If we do not have an upload session Id then die.
			if (null != this.UploadSessionId)
			{
				// Should we commit?
				if (commit)
				{
					// Commit the blocks to the file.
					this.Vault.ObjectFileOperations.UploadFileCommitEx
					(
						this.UploadSessionId.Value,
						this.ObjId,
						this.FileToOverwrite,
						this.position
					);
				}

				// Close the uplaod session.
				this.Vault?.ObjectFileOperations.CloseUploadSession(this.UploadSessionId.Value);
			}

			this.UploadSessionId = null;
			this.position = 0;
		}

		#region Overrides of Stream

		public override void Close()
		{
			this.Close(this.AutomaticallyCommitOnDisposal);
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
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		public override void Write(byte[] buffer, int offset, int count)
		{
			// Sanity.
			if (null == buffer)
				throw new ArgumentNullException(nameof(buffer));
			if (offset < 0 || offset >= buffer.Length)
				throw new ArgumentOutOfRangeException(nameof(offset));
			if (count < 0 || offset + count > buffer.Length)
				throw new ArgumentOutOfRangeException(nameof(count));

			// Do we need to start the session?
			if (null == this.UploadSessionId)
				this.OpenUploadSession();
			if (null == this.UploadSessionId)
				throw new InvalidOperationException("Upload session could not be started");

			// If the count is zero then die.
			if(count == 0)
				return;

			// Extract just the valid data from the buffer.
			var internalBuffer = new byte[count];
			Array.Copy(buffer, offset, internalBuffer, 0, count);

			// Upload this block.
			this.Vault.ObjectFileOperations.UploadFileBlock
			(
				this.UploadSessionId.Value,
				internalBuffer.Length,
				this.position,
				internalBuffer
			);

			// Update the offset.
			this.position += internalBuffer.Length;
		}

		/// <inheritdoc />
		public override bool CanRead => false;

		/// <inheritdoc />
		public override bool CanSeek => false;

		/// <inheritdoc />
		public override bool CanWrite => true;

		/// <inheritdoc />
		/// <remarks>This stream will increase in length as data is written to it.</remarks>
		public override long Length => this.position;
		
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
