using System;
using System.IO;
using System.Linq;

namespace MFilesAPI.Extensions
{
	public static partial class VaultObjectFileOperationsExtensionMethods
	{
		/// <summary>
		/// Accesses a file's contents as a stream for writing.
		/// </summary>
		/// <param name="objectFileOperations">The instance of <see cref="VaultObjectFileOperations"/> to use.</param>
		/// <param name="objectFile">The file to open for writing.</param>
		/// <param name="vault">The vault the file comes from.</param>
		/// <param name="automaticallyCommitOnDisposal">If true, will automatically save the file contents to the vault when this stream is disposed of.</param>
		/// <returns>The file opened as a write-only <see cref="Stream"/>.</returns>
		/// <remarks>Use <see cref="OpenFileForWriting(VaultObjectFileOperations, ObjectFile, ObjID, Vault, bool)"/> if you have the <see cref="ObjID"/> as this is more efficient.</remarks>
		public static Stream OpenFileForWriting
		(
			this VaultObjectFileOperations objectFileOperations,
			ObjectFile objectFile,
			Vault vault,
			bool automaticallyCommitOnDisposal = true
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
			return new FileUploadStream
			(
				objectFile.FileVer,
				vault.ObjectFileOperations.GetObjIDOfFile(objectFile.ID),
				vault,
				automaticallyCommitOnDisposal
			);
		}

		/// <summary>
		/// Accesses a file's contents as a stream for writing.
		/// </summary>
		/// <param name="objectFileOperations">The instance of <see cref="VaultObjectFileOperations"/> to use.</param>
		/// <param name="objectFile">The file to open for writing.</param>
		/// <param name="objId">The object that this file resides in.</param>
		/// <param name="vault">The vault the file comes from.</param>
		/// <param name="automaticallyCommitOnDisposal">If true, will automatically save the file contents to the vault when this stream is disposed of.</param>
		/// <returns>The file opened as a write-only <see cref="Stream"/>.</returns>
		public static Stream OpenFileForWriting
		(
			this VaultObjectFileOperations objectFileOperations,
			ObjectFile objectFile,
			ObjID objId,
			Vault vault,
			bool automaticallyCommitOnDisposal = true
		)
		{
			// Sanity.
			if (null == objectFileOperations)
				throw new ArgumentNullException(nameof(objectFileOperations));
			if (null == objectFile)
				throw new ArgumentNullException(nameof(objectFile));
			if (null == objId)
				throw new ArgumentNullException(nameof(objId));
			if (null == vault)
				throw new ArgumentNullException(nameof(vault));

			// Return a file stream of the object.
			return new FileUploadStream
			(
				objectFile.FileVer, 
				objId, 
				vault, 
				automaticallyCommitOnDisposal
			);
		}

	}
}
