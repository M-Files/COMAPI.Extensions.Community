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
		/// <returns>A <see cref="FileDownload"/> representing the completed file download.</returns>
		public static FileDownload Download
		(
			this ObjectFile objectFile,
			Vault vault,
			FileInfo downloadTo,
			int blockSize = FileDownload.DefaultDownloadBlockSize,
			MFFileFormat fileFormat = MFFileFormat.MFFileFormatNative
		)
		{
			// Sanity.
			if (null == objectFile)
				throw new ArgumentNullException(nameof(objectFile));
			if (null == vault)
				throw new ArgumentNullException(nameof(vault));

			// Download the file.
			return FileDownload.DownloadFile
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
		/// <returns>A <see cref="FileDownload"/> representing the completed file download.</returns>
		public static FileDownload Download
		(
			this ObjectFile objectFile,
			Vault vault,
			string filePath,
			int blockSize = FileDownload.DefaultDownloadBlockSize,
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
		/// <returns>A <see cref="FileDownload"/> representing the completed file download.</returns>
		public static FileDownload Download
		(
			this ObjectFile objectFile,
			Vault vault,
			FileDownloadLocation fileDownloadLocation,
			int blockSize = FileDownload.DefaultDownloadBlockSize,
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
			var fileDownload = fileDownloadLocation
				.DownloadFile
				(
					objectFile,
					vault,
					blockSize,
					fileFormat
				);

			// Return the file download for the caller to work with.
			return fileDownload;
		}
	}
}
