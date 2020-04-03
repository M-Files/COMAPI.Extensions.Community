using System;
using System.IO;
using MFilesAPI;

namespace MFilesAPI.Extensions
{
	public static class ObjectFileExtensionMethods
	{
		/// <summary>
		/// Downloads the file to disk.
		/// </summary>
		/// <param name="objectFile">The file to download.</param>
		/// <param name="vault">The vault to download from.</param>
		/// <param name="downloadTo">The location on disk to download to.</param>
		/// <param name="blockSize">The size of blocks to use to transfer the file from the M-Files vault to this machine.</param>
		/// <param name="fileFormat">The format of file to request from server.</param>
		/// <returns>A <see cref="TemporaryFileDownload"/> representing the completed file download.</returns>
		public static TemporaryFileDownload Download
		(
			this ObjectFile objectFile,
			Vault vault,
			FileInfo downloadTo,
			int blockSize = FileTransfers.DefaultBlockSize,
			MFFileFormat fileFormat = MFFileFormat.MFFileFormatNative
		)
		{
			// Sanity.
			if (null == objectFile)
				throw new ArgumentNullException(nameof(objectFile));
			if (null == vault)
				throw new ArgumentNullException(nameof(vault));

			// Download the file.
			return TemporaryFileDownload.Download
			(
				objectFile,
				vault,
				downloadTo,
				false,
				blockSize,
				fileFormat
			);
		}

		/// <summary>
		/// Downloads the file to disk.
		/// </summary>
		/// <param name="objectFile">The file to download.</param>
		/// <param name="vault">The vault to download from.</param>
		/// <param name="filePath">The location on disk to download to.</param>
		/// <param name="blockSize">The size of blocks to use to transfer the file from the M-Files vault to this machine.</param>
		/// <param name="fileFormat">The format of file to request from server.</param>
		/// <returns>A <see cref="TemporaryFileDownload"/> representing the completed file download.</returns>
		public static TemporaryFileDownload Download
		(
			this ObjectFile objectFile,
			Vault vault,
			string filePath,
			int blockSize = FileTransfers.DefaultBlockSize,
			MFFileFormat fileFormat = MFFileFormat.MFFileFormatNative
		)
		{
			return objectFile
				.Download
				(
					vault, 
					new FileInfo(filePath), 
					blockSize, 
					fileFormat
				);
		}

		/// <summary>
		/// Downloads the file to disk.
		/// </summary>
		/// <param name="objectFile">The file to download.</param>
		/// <param name="vault">The vault to download from.</param>
		/// <param name="fileDownloadLocation">The location on disk to download to.</param>
		/// <param name="blockSize">The size of blocks to use to transfer the file from the M-Files vault to this machine.</param>
		/// <param name="fileFormat">The format of file to request from server.</param>
		/// <returns>A <see cref="TemporaryFileDownload"/> representing the completed file download.</returns>
		public static TemporaryFileDownload Download
		(
			this ObjectFile objectFile,
			Vault vault,
			FileDownloadLocation fileDownloadLocation,
			int blockSize = FileTransfers.DefaultBlockSize,
			MFFileFormat fileFormat = MFFileFormat.MFFileFormatNative
		)
		{
			// Sanity.
			if (null == objectFile)
				throw new ArgumentNullException(nameof(objectFile));
			if (null == vault)
				throw new ArgumentNullException(nameof(vault));
			if (null == fileDownloadLocation)
				throw new ArgumentNullException(nameof(fileDownloadLocation));

			// Download the file.
			return fileDownloadLocation
				.DownloadFile
				(
					objectFile,
					vault,
					blockSize,
					fileFormat
				);
		}

		/// <summary>
		/// Opens a read-only stream to access the existing file contents.
		/// </summary>
		/// <param name="objectFile">The file to download.</param>
		/// <param name="vault">The vault to download from.</param>
		/// <param name="fileFormat">The format of file to request from server.</param>
		/// <returns>A <see cref="FileDownloadStream"/> that can be used to read the file from the vault.</returns>
		/// <remarks>Ensure that the stream is correctly closed and disposed of (e.g. with a <see langword="using"/> statement).</remarks>
		public static FileDownloadStream OpenRead
		(
			this ObjectFile objectFile,
			Vault vault,
			MFFileFormat fileFormat = MFFileFormat.MFFileFormatNative
		)
		{
			return new FileDownloadStream(objectFile, vault, fileFormat);
		}

		/// <summary>
		/// Replaces an existing file with data from another stream.
		/// </summary>
		/// <param name="objectFile">The file to replace.</param>
		/// <param name="vault">The vault from in which the file exists.</param>
		/// <param name="input">The new content for the file.</param>
		/// <param name="blockSize">The block size to use.</param>
		/// <remarks>It is more performant to use <see cref="ReplaceFileContent(ObjectFile, Vault, ObjID, Stream, int)"/> if you already have the file ID.</remarks>
		public static void ReplaceFileContent
		(
			this ObjectFile objectFile,
			Vault vault,
			Stream input,
			int blockSize = FileTransfers.DefaultBlockSize
		)
		{
			// Sanity.
			if (null == objectFile)
				throw new ArgumentNullException(nameof(objectFile));
			if (null == vault)
				throw new ArgumentNullException(nameof(vault));

			// Attempt to get the object ID for this file.
			var objId = vault.ObjectFileOperations.GetObjIDOfFile(objectFile.ID);

			// Use the other overload.
			objectFile.ReplaceFileContent(vault, objId, input, blockSize);
		}

		/// <summary>
		/// Replaces an existing file with data from another stream.
		/// </summary>
		/// <param name="objectFile">The file to replace.</param>
		/// <param name="vault">The vault from in which the file exists.</param>
		/// <param name="objectId">The object to which this file belongs.</param>
		/// <param name="input">The new content for the file.</param>
		/// <param name="blockSize">The block size to use.</param>
		public static void ReplaceFileContent
		(
			this ObjectFile objectFile,
			Vault vault,
			ObjID objectId,
			Stream input,
			int blockSize = FileTransfers.DefaultBlockSize
		)
		{
			// Sanity.
			if (null == objectFile)
				throw new ArgumentNullException(nameof(objectFile));
			if (null == vault)
				throw new ArgumentNullException(nameof(vault));
			if (null == objectId)
				throw new ArgumentNullException(nameof(objectId));
			if (null == input)
				throw new ArgumentNullException(nameof(input));
			if (false == input.CanRead)
				throw new ArgumentException("The input stream is not readable.", nameof(input));

			// Documentation says block size must be between 1KB and 4MB.
			if (blockSize < FileTransfers.MinimumUploadBlockSize)
				blockSize = FileTransfers.MinimumUploadBlockSize;
			if(blockSize > FileTransfers.MaximumUploadBlockSize)
				blockSize = FileTransfers.MaximumUploadBlockSize;

			// Create the (write-only) upload stream.
			using (var uploadStream = new FileUploadStream(objectFile.FileVer, objectId, vault))
			{
				// Copy the input stream to the upload stream.
				input.CopyTo(uploadStream, blockSize);
			}
		}

		/// <summary>
		/// Opens a write-only stream to replace the existing file contents.
		/// </summary>
		/// <param name="objectFile">The file to update.</param>
		/// <param name="vault">The vault to download from.</param>
		/// <param name="objectId">The object to which this file belongs.</param>
		/// <param name="automaticallyCommitOnDisposal">If true, will automatically save the file contents to the vault when this stream is disposed of.</param>
		/// <returns>A <see cref="FileUploadStream"/> that can be used to write file data to vault.</returns>
		/// <remarks>Ensure that the stream is correctly closed and disposed of (e.g. with a <see langword="using"/> statement).</remarks>
		public static FileUploadStream OpenWrite
		(
			this ObjectFile objectFile,
			Vault vault,
			ObjID objectId,
			bool automaticallyCommitOnDisposal = true
		)
		{
			return new FileUploadStream(objectFile.FileVer, objectId, vault, automaticallyCommitOnDisposal);
		}
	}
}
