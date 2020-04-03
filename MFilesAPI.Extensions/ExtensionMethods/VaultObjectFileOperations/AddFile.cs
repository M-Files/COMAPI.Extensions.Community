using System;
using System.IO;
using System.Linq;

namespace MFilesAPI.Extensions
{
	public static partial class VaultObjectFileOperationsExtensionMethods
	{
		/// <summary>
		/// Adds a new file to the specified object.
		/// </summary>
		/// <param name="objectFileOperations">The instance of <see cref="VaultObjectFileOperations"/> to use.</param>
		/// <param name="objVer">The object version to add the file to.  Must already be checked out.</param>
		/// <param name="vault">The vault to add the file to.</param>
		/// <param name="title">The title of the file (without an extension).</param>
		/// <param name="extension">The file extension.  Can be supplied with or without preceeding ".".</param>
		/// <param name="fileContents">The contents of the file.</param>
		public static void AddFile
		(
			this VaultObjectFileOperations objectFileOperations,
			ObjVer objVer,
			Vault vault,
			string title,
			string extension,
			Stream fileContents
		)
		{
			// Sanity.
			if (null == objectFileOperations)
				throw new ArgumentNullException(nameof(objectFileOperations));
			if (null == objVer)
				throw new ArgumentNullException(nameof(objVer));
			if (null == vault)
				throw new ArgumentNullException(nameof(vault));
			if (String.IsNullOrWhiteSpace(title))
				throw new ArgumentException("The file must have a title/name.", nameof(title));
			if (null == fileContents)
				throw new ArgumentNullException(nameof(fileContents));
			if (false == fileContents.CanRead)
				throw new ArgumentException("The file contents must represent a readable stream.", nameof(fileContents));
			extension = extension ?? string.Empty;
			if (extension.StartsWith("."))
				extension = extension.Substring(1);

			// Add the empty file.
			var fileVer = vault
				.ObjectFileOperations
				.AddEmptyFile
				(
					objVer,
					title,
					extension
				);

			// Set the file contents.
			using (var uploadStream = new FileUploadStream(fileVer, objVer.ObjID, vault))
			{
				fileContents.CopyTo(uploadStream);
			}
		}

	}
}
