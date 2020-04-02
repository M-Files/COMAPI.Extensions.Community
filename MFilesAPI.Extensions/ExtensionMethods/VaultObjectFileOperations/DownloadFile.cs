using System;
using System.IO;
using System.Linq;

namespace MFilesAPI.Extensions
{
	public static partial class VaultObjectFileOperationsExtensionMethods
	{
		/// <summary>
		/// Extension method for accessing a file's contents as a stream.
		/// </summary>
		/// <param name="objectFileOperations">The instance of <see cref="VaultObjectFileOperations"/> to use.</param>
		/// <param name="objectFile">The file to open for reading.</param>
		/// <param name="vault">The vault the file comes from.</param>
		/// <param name="fileFormat">The format of the file to read as.</param>
		/// <returns>The file opened as a Stream.</returns>
		public static Stream ReadFileAsStream
		(
			this VaultObjectFileOperations objectFileOperations,
			ObjectFile objectFile,
			Vault vault,
			MFFileFormat fileFormat = MFFileFormat.MFFileFormatNative
		)
		{
			// Sanity.
			if (null == objectFileOperations)
				throw new ArgumentNullException(nameof(objectFileOperations));
			if (null == objectFile)
				throw new ArgumentNullException(nameof(objectFile));
			if (null == vault)
				throw new ArgumentNullException(nameof(vault));

			// Return a file stream of the object.
			return new FileDownloadStream(objectFile, vault, fileFormat);
		}

	}
}
