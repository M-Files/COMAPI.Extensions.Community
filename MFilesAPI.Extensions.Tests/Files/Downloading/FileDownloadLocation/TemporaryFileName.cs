using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MFilesAPI.Extensions.Tests.Files.Downloading.FileDownloadLocation
{
	[TestClass]
	public class TemporaryFileName
	{

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TemporaryFileNameThrowsIfObjectFileIsNull()
		{
			new FileDownloadLocationProxy()
				.GenerateTemporaryFileInfo((ObjectFile) null);
		}

		[TestMethod]
		public void TemporaryFileNameExtensionIsCorrectForNullExtension()
		{
			// Generate the temporary file information.
			var output = new FileDownloadLocationProxy()
				.GenerateTemporaryFileInfo((string) null);

			// Ensure the extension is correct.
			Assert.IsNotNull(output);
			Assert.IsTrue(output.Extension == ".tmp");
		}

		[TestMethod]
		[DynamicData(nameof(TemporaryFileName.GetValidExtensionData), DynamicDataSourceType.Method)]
		public void TemporaryFileNameExtensionIsCorrect
			(
			object input,
			string expectedExtension
			)
		{
			// Create the file download location proxy.
			var fileDownloadLocation = new FileDownloadLocationProxy();

			// Call the correct method depending on the type of the input.
			FileInfo output;
			if (input is string extensionString)
			{
				output = fileDownloadLocation.GenerateTemporaryFileInfo(extensionString);
			}
			else if (input is ObjectFile objectFile)
			{
				output = fileDownloadLocation.GenerateTemporaryFileInfo(objectFile);
			}
			else
			{
				throw new ArgumentException(nameof(input), $"Input type not supported: {input?.GetType().FullName ?? "(null)"}");
			}

			// Ensure the extension is correct.
			Assert.IsNotNull(output);
			Assert.IsTrue(output.Extension == expectedExtension);

		}

		public static IEnumerable<object[]> GetValidExtensionData()
		{
			// String extension with dot.
			yield return new [] { ".docx", ".docx" };

			// String extension without dot.
			yield return new [] { "docx", ".docx" };

			// Object file
			yield return new object[]
			{
				new ObjectFileStub()
				{
					Extension = ".docx"
				},
				".docx" };
		}
	}
}
