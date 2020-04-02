using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MFilesAPI;

namespace MFilesAPI.Extensions
{
	/// <summary>
	/// A location for temporary file downloads to be held.
	/// </summary>
	public class FileDownloadLocation
		: DisposableBase
	{
		/// <summary>
		/// The <see cref="DirectoryInfo"/> into which temporary files are placed.
		/// </summary>
		public DirectoryInfo Directory { get; protected set; }

		/// <summary>
		/// If true, the <see cref="Directory"/> will be cleaned of files when this object is disposed.
		/// </summary>
		public bool CleanDirectoryOnDisposal { get; set; }

		/// <summary>
		/// The default file extension for temporary files, if no file extension is provided/available.
		/// </summary>
		public const string DefaultFileExtension = ".tmp";

		/// <summary>
		/// Creates a <see cref="FileDownloadLocation"/> pointing at the provided <paramref name="directory"/>.
		/// </summary>
		/// <param name="directory">The location for the files to be downloaded to.</param>
		protected FileDownloadLocation
		(
			DirectoryInfo directory
		)
		{
			// Sanity.
			this.Directory = directory ?? throw new ArgumentNullException(nameof(directory));

			// If the location does not exist then create it.
			if (false == this.Directory.Exists)
				this.Directory.Create();
		}

		/// <summary>
		/// Creates a <see cref="FileDownloadLocation"/>.
		/// Creates a folder named <paramref name="temporaryPath"/> within <paramref name="folderName"/> to hold the downloaded files.
		/// </summary>
		/// <param name="temporaryPath">The parent temporary path.</param>
		/// <param name="folderName">The folder name to create in <paramref name="temporaryPath"/>.  All files will be saved into this subfolder.</param>
		public FileDownloadLocation
		(
			string temporaryPath,
			string folderName
		)
			: this
			(
				new DirectoryInfo(System.IO.Path.Combine(temporaryPath, folderName))
			)
		{
		}

		/// <summary>
		/// Creates a <see cref="FileDownloadLocation"/> pointing at the system temporary area.
		/// Will create a subfolder called the application domain name.
		/// </summary>
		public FileDownloadLocation()
			: this
			(
				System.IO.Path.GetTempPath(),
				System.Reflection.Assembly.GetCallingAssembly().GetName().Name
			)
		{
		}

		/// <summary>
		/// Deletes all temporary files from this location.
		/// </summary>
		/// <param name="suppressErrors">If true then errors deleting files are not thrown.</param>
		public virtual void CleanTemporaryFiles
		(
			bool suppressErrors = true
		)
		{
			// Remove all files.
			foreach (var file in this.Directory.GetFiles())
			{
				try
				{
					file.Delete();
				}
				catch
				{
					// If we are not to suppress errors then throw the exception.
					if(false == suppressErrors)
						throw;
				}
			}
		}
		
		/// <summary>
		/// Generates a unique (GUID-based) temporary file in <see cref="Directory"/> with the given <paramref name="extension"/>.
		/// </summary>
		/// <param name="extension">The extension for the file.  If not provided defaults to ".tmp".</param>
		/// <returns>A <see cref="FileInfo"/> for the temporary file.</returns>
		protected FileInfo GenerateTemporaryFileInfo(string extension = null)
		{
			// Ensure the extension is valid.
			if (string.IsNullOrWhiteSpace(extension))
				extension = FileDownloadLocation.DefaultFileExtension;
			if (false == extension.StartsWith("."))
				extension = "." + extension;

			// Create the file info.
			return new FileInfo(Path.Combine(this.Directory.FullName, Guid.NewGuid() + extension));
		}
		
		/// <summary>
		/// Generates a unique (GUID-based) temporary file in <see cref="Directory"/> with the given <paramref name="objectFile"/>.
		/// </summary>
		/// <param name="objectFile">The file that will be downloaded.  Uses the <see cref="ObjectFile.Extension"/>.</param>
		/// <returns>A <see cref="FileInfo"/> for the temporary file.</returns>
		protected FileInfo GenerateTemporaryFileInfo(ObjectFile objectFile)
		{
			// Sanity.
			if (null == objectFile)
				throw new ArgumentNullException(nameof(objectFile));

			// Use the other overload.
			return this.GenerateTemporaryFileInfo(objectFile.Extension);
		}

		/// <summary>
		/// Downloads the <paramref name="objectFile"/> from the <paramref name="vault"/>.
		/// </summary>
		/// <param name="objectFile">The file to download.</param>
		/// <param name="vault">The vault to download from.</param>
		/// <param name="blockSize">The size of blocks to use to transfer the file from the M-Files vault to this machine.</param>
		/// <param name="fileFormat">The format of file to request from server.</param>
		/// <returns>A <see cref="TemporaryFileDownload"/> representing the downloaded file.</returns>
		public TemporaryFileDownload DownloadFile
		(
			ObjectFile objectFile,
			Vault vault,
			int blockSize = FileTransfers.DefaultBlockSize,
			MFFileFormat fileFormat = MFFileFormat.MFFileFormatNative
		)
		{
			return objectFile.Download
			(
				vault,
				this.GenerateTemporaryFileInfo(objectFile),
				blockSize,
				fileFormat
			);
		}

		/// <inheritdoc />
		protected override void DisposeManagedObjects()
		{
			// If we should clean on disposal then clean.
			if (this.CleanDirectoryOnDisposal)
				this.CleanTemporaryFiles();

			// Call the base implementation.
			base.DisposeManagedObjects();
		}

	}
}