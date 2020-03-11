using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MFilesAPI.Extensions.Tests.Files.Downloading.FileDownloadLocation
{
	[TestClass]
	public class FileDownloadLocationTests
	{
		[TestMethod]
		public void DirectoryDefaultsToTemp()
		{
			// Create the file download location with default locations.
			var fileDownloadLocation = new Extensions.FileDownloadLocation();

			// Ensure that the parent directory of the directory is the system temporary path.
			Assert.AreEqual
			(
				System.IO.Path.GetTempPath(),
				fileDownloadLocation?.Directory?.Parent?.FullName + @"\"
			);
		}

		[TestMethod]
		public void DirectoryDefaultsToCurrentAssemblyName()
		{
			// Create the file download location with default locations.
			var fileDownloadLocation = new Extensions.FileDownloadLocation();

			// Ensure that the parent directory of the directory is the name of the currently executing assembly.
			Assert.AreEqual
			(
				System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
				fileDownloadLocation?.Directory?.Name
			);
		}
		[TestMethod]
		public void CustomDirectoryInformationIsCorrect()
		{
			// Create the file download location with default locations.
			var fileDownloadLocation = new Extensions.FileDownloadLocation
			(
				@"C:\temp\",
				"subfolder"
				);

			// Ensure that directory information is correct.
			Assert.AreEqual
			(
				@"C:\temp\",
				fileDownloadLocation?.Directory?.Parent?.FullName + @"\"
			);
			Assert.AreEqual
			(
				"subfolder",
				fileDownloadLocation?.Directory?.Name
			);
		}
	}
}
