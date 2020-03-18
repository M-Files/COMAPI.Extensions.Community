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
			int blockSize = TemporaryFileDownload.DefaultDownloadBlockSize,
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
			int blockSize = TemporaryFileDownload.DefaultDownloadBlockSize,
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
			int blockSize = TemporaryFileDownload.DefaultDownloadBlockSize,
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
	}
}
