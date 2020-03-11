using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFilesAPI.Extensions.Tests.Files.Downloading.FileDownloadLocation
{
	internal class FileDownloadLocationProxy
		: MFilesAPI.Extensions.FileDownloadLocation
	{
		/// <summary>
		/// Generates a unique (GUID-based) temporary file in <see cref="Directory"/> with the given <paramref name="extension"/>.
		/// </summary>
		/// <param name="extension">The extension for the file.  If not provided defaults to ".tmp".</param>
		/// <returns>A <see cref="FileInfo"/> for the temporary file.</returns>
		public new FileInfo GenerateTemporaryFileInfo(string extension)
		{
			return base.GenerateTemporaryFileInfo(extension);
		}
		
		/// <summary>
		/// Generates a unique (GUID-based) temporary file in <see cref="Directory"/> with the given <paramref name="objectFile"/>.
		/// </summary>
		/// <param name="objectFile">The file that will be downloaded.  Uses the <see cref="ObjectFile.Extension"/>.</param>
		/// <returns>A <see cref="FileInfo"/> for the temporary file.</returns>
		public new FileInfo GenerateTemporaryFileInfo(ObjectFile objectFile)
		{
			return base.GenerateTemporaryFileInfo(objectFile);
		}
	}
}