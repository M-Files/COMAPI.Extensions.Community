using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAPI.Extensions.Tests.Files.Downloading.FileDownloadStream
{
	[TestClass]
	public partial class FileDownloadStreamTests
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullFileToDownloadThrows()
		{
			new Extensions.FileDownloadStream(null, Moq.Mock.Of<Vault>());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullVaultThrows()
		{
			new Extensions.FileDownloadStream(Moq.Mock.Of<ObjectFile>(), null);
		}

		[TestMethod]
		public void FileToDownloadSetsProperty()
		{
			var objectFile = Moq.Mock.Of<ObjectFile>();
			var stream = new Extensions.FileDownloadStream(objectFile, Moq.Mock.Of<Vault>());
			Assert.AreEqual(objectFile, stream.FileToDownload);
		}

		[TestMethod]
		public void VaultSetsProperty()
		{
			var vault = Moq.Mock.Of<Vault>();
			var stream = new Extensions.FileDownloadStream(Moq.Mock.Of<ObjectFile>(), vault);
			Assert.AreEqual(vault, stream.Vault);
		}

		[TestMethod]
		public void FileFormatDefaultsToNative()
		{
			var stream = new Extensions.FileDownloadStream(Moq.Mock.Of<ObjectFile>(), Moq.Mock.Of<Vault>());
			Assert.AreEqual(MFFileFormat.MFFileFormatNative, stream.FileFormat);
		}

		[TestMethod]
		public void DownloadSessionDefaultsToNull()
		{
			var stream = new Extensions.FileDownloadStream(Moq.Mock.Of<ObjectFile>(), Moq.Mock.Of<Vault>());
			Assert.IsNull(stream.DownloadSession);
		}

		[TestMethod]
		public void CanReadReturnsTrue()
		{
			Assert.IsTrue
			(
				new Extensions.FileDownloadStream(Mock.Of<ObjectFile>(), Moq.Mock.Of<Vault>()).CanRead
			);
		}

		[TestMethod]
		public void CanSeekReturnsFalse()
		{
			Assert.IsFalse
			(
				new Extensions.FileDownloadStream(Mock.Of<ObjectFile>(), Moq.Mock.Of<Vault>()).CanSeek
			);
		}

		[TestMethod]
		public void CanWriteReturnsFalse()
		{
			Assert.IsFalse
			(
				new Extensions.FileDownloadStream(Mock.Of<ObjectFile>(), Moq.Mock.Of<Vault>()).CanWrite
			);
		}

		[TestMethod]
		public void PositionDefaultsToZero()
		{
			Assert.AreEqual
			(
				0,
				new Extensions.FileDownloadStream(Mock.Of<ObjectFile>(), Moq.Mock.Of<Vault>()).Position
			);
		}

		[TestMethod]
		[ExpectedException(typeof(NotSupportedException))]
		public void SettingPositionThrows()
		{
			new Extensions.FileDownloadStream(Mock.Of<ObjectFile>(), Moq.Mock.Of<Vault>())
				.Position = 123;
		}

		[TestMethod]
		[ExpectedException(typeof(NotSupportedException))]
		public void SettingLengthThrows()
		{
			new Extensions.FileDownloadStream(Mock.Of<ObjectFile>(), Moq.Mock.Of<Vault>())
				.SetLength(123);
		}

		[TestMethod]
		[ExpectedException(typeof(NotSupportedException))]
		public void SeekThrows()
		{
			new Extensions.FileDownloadStream(Mock.Of<ObjectFile>(), Moq.Mock.Of<Vault>())
				.Seek(123, System.IO.SeekOrigin.Begin);
		}

		[TestMethod]
		[ExpectedException(typeof(NotSupportedException))]
		public void WriteThrows()
		{
			new Extensions.FileDownloadStream(Mock.Of<ObjectFile>(), Moq.Mock.Of<Vault>())
				.Write(new byte[0], 0, 0);
		}

		[TestMethod]
		[DataRow(12345, 5, MFFileFormat.MFFileFormatNative)]
		[DataRow(12345, 5, MFFileFormat.MFFileFormatPDF)]
		[DataRow(12345, 5, MFFileFormat.MFFileFormatDisplayOnlyPDF)]
		public void OpenDownloadSessionPopulatesDownloadSession
			(
			int fileId,
			int fileVersion,
			MFFileFormat fileFormat
		)
		{
			// Set up a file to download.
			var objectFileMock = new Mock<ObjectFile>();
			objectFileMock.SetupGet(m => m.ID).Returns(fileId);
			objectFileMock.SetupGet(m => m.Version).Returns(fileVersion);

			// Set up the vault object file operations mock.
			var vaultObjectFileOperationsMock = new Mock<VaultObjectFileOperations>();

			// When DownloadFileInBlocks_BeginEx is called, return a dummy session.
			vaultObjectFileOperationsMock
				.Setup(m => m.DownloadFileInBlocks_BeginEx
				(
					Moq.It.IsAny<int>(),
					Moq.It.IsAny<int>(),
					Moq.It.IsAny<MFFileFormat>()
				))
				.Returns((int receivedFileId, int receivedFileVersion, MFFileFormat receivedFileFormat) =>
				{
					// Ensure we got the right data.
					Assert.AreEqual(fileId, receivedFileId);
					Assert.AreEqual(fileVersion, receivedFileVersion);
					Assert.AreEqual(fileFormat, receivedFileFormat);

					// Mock a download session to return.
					var downloadSessionMock = new Mock<FileDownloadSession>();
					downloadSessionMock.SetupGet(m => m.DownloadID).Returns(1);
					downloadSessionMock.SetupGet(m => m.FileSize).Returns(1000);
					downloadSessionMock.SetupGet(m => m.FileSize32).Returns(1000);
					return downloadSessionMock.Object;
				})
				.Verifiable();

			// Set up the mock vault.
			var vaultMock = new Mock<Vault>();
			vaultMock
				.SetupGet(m => m.ObjectFileOperations)
				.Returns(vaultObjectFileOperationsMock.Object);

			// Act.
			var stream = new Extensions.FileDownloadStream(objectFileMock.Object, vaultMock.Object, fileFormat);
			stream.OpenDownloadSession();

			// Ensure object is populated.
			Assert.IsNotNull(stream.DownloadSession);

			// Ensure we got hit as expected.
			vaultMock.Verify();
			vaultObjectFileOperationsMock.Verify();
		}

		[TestMethod]
		public void CloseDownloadSessionDoesNotThrowWithEmptyDownloadSession()
		{
			// Create the stream.
			var stream = new Extensions.FileDownloadStream(Moq.Mock.Of<ObjectFile>(), Moq.Mock.Of<Vault>());

			// Ensure the session is null.
			Assert.IsNull(stream.DownloadSession);

			// Close.
			stream.CloseDownloadSession();
		}

		[TestMethod]
		public void CloseDownloadSessionCleansUpDownloadSession()
		{
			// Set up a file to download.
			var objectFileMock = new Mock<ObjectFile>();
			objectFileMock.SetupGet(m => m.ID).Returns(12345);
			objectFileMock.SetupGet(m => m.Version).Returns(1);

			// Set up the vault object file operations mock.
			var vaultObjectFileOperationsMock = new Mock<VaultObjectFileOperations>();

			// When CancelFileDownloadSession is called, ensure that the correct data is passed.
			vaultObjectFileOperationsMock
				.Setup(m => m.CancelFileDownloadSession
				(
					Moq.It.IsAny<int>()
				))
				.Callback((int downloadSession) => { Assert.AreEqual(1, downloadSession); })
				.Verifiable();

			// Set up the mock vault.
			var vaultMock = new Mock<Vault>();
			vaultMock
				.SetupGet(m => m.ObjectFileOperations)
				.Returns(vaultObjectFileOperationsMock.Object);

			// Create the stream.
			var stream = new FileDownloadStreamProxy(objectFileMock.Object, vaultMock.Object);
			
			// Set up the download session.
			var downloadSessionMock = new Mock<FileDownloadSession>();
			downloadSessionMock.SetupGet(m => m.DownloadID).Returns(1);
			stream.DownloadSession = downloadSessionMock.Object;

			// Close the download session.
			stream.CloseDownloadSession();

			// Ensure that the download session is empty.
			Assert.IsNull(stream.DownloadSession);

			// Ensure we got hit as expected.
			vaultMock.Verify();
			vaultObjectFileOperationsMock.Verify();
		}

		[TestMethod]
		public void ReadOpensDownloadSessionIfNotOpen()
		{
			// Set up a file to download.
			var objectFileMock = new Mock<ObjectFile>();
			objectFileMock.SetupGet(m => m.ID).Returns(12345);
			objectFileMock.SetupGet(m => m.Version).Returns(1);

			// Set up the vault object file operations mock.
			var vaultObjectFileOperationsMock = new Mock<VaultObjectFileOperations>();

			// When DownloadFileInBlocks_BeginEx is called (starting a download session), return a dummy session.
			vaultObjectFileOperationsMock
				.Setup(m => m.DownloadFileInBlocks_BeginEx
				(
					Moq.It.IsAny<int>(),
					Moq.It.IsAny<int>(),
					Moq.It.IsAny<MFFileFormat>()
				))
				.Returns((int receivedFileId, int receivedFileVersion, MFFileFormat receivedFileFormat) =>
				{
					// Mock a download session to return.
					var downloadSessionMock = new Mock<FileDownloadSession>();
					downloadSessionMock.SetupGet(m => m.DownloadID).Returns(1);
					downloadSessionMock.SetupGet(m => m.FileSize).Returns(1000);
					downloadSessionMock.SetupGet(m => m.FileSize32).Returns(1000);
					return downloadSessionMock.Object;
				})
				.Verifiable();

			// When DownloadFileInBlocks_ReadBlock is called (reading a block of content), return something.
			vaultObjectFileOperationsMock
				.Setup(m => m.DownloadFileInBlocks_ReadBlock
				(
					Moq.It.IsAny<int>(),
					Moq.It.IsAny<int>(),
					Moq.It.IsAny<long>()
				))
				.Returns((int downloadSession, int blockSize, long offset) =>
				{
					// Return some sample data.
					return new byte[]
					{
						0,
						1,
						2
					};
				})
				.Verifiable();

			// Set up the mock vault.
			var vaultMock = new Mock<Vault>();
			vaultMock
				.SetupGet(m => m.ObjectFileOperations)
				.Returns(vaultObjectFileOperationsMock.Object);

			// Create the stream.
			var stream = new FileDownloadStreamProxy(objectFileMock.Object, vaultMock.Object);
			Assert.IsNull(stream.DownloadSession);

			// Attempt to read.
			byte[] data = new byte[4096];
			stream.Read(data, 0, 4096);

			// Ensure that the download session is not empty.
			Assert.IsNotNull(stream.DownloadSession);

			// Ensure we got hit as expected.
			vaultMock.Verify();
			vaultObjectFileOperationsMock.Verify();
		}

		[TestMethod]
		public void ReadPopulatesByteArrayAndReturnsCorrectData()
		{
			// Set up a file to download.
			var objectFileMock = new Mock<ObjectFile>();
			objectFileMock.SetupGet(m => m.ID).Returns(12345);
			objectFileMock.SetupGet(m => m.Version).Returns(1);

			// Set up the vault object file operations mock.
			var vaultObjectFileOperationsMock = new Mock<VaultObjectFileOperations>();

			// When DownloadFileInBlocks_BeginEx is called (starting a download session), return a dummy session.
			vaultObjectFileOperationsMock
				.Setup(m => m.DownloadFileInBlocks_BeginEx
				(
					Moq.It.IsAny<int>(),
					Moq.It.IsAny<int>(),
					Moq.It.IsAny<MFFileFormat>()
				))
				.Returns((int receivedFileId, int receivedFileVersion, MFFileFormat receivedFileFormat) =>
				{
					// Mock a download session to return.
					var downloadSessionMock = new Mock<FileDownloadSession>();
					downloadSessionMock.SetupGet(m => m.DownloadID).Returns(1);
					downloadSessionMock.SetupGet(m => m.FileSize).Returns(1000);
					downloadSessionMock.SetupGet(m => m.FileSize32).Returns(1000);
					return downloadSessionMock.Object;
				})
				.Verifiable();

			// When DownloadFileInBlocks_ReadBlock is called (reading a block of content), return 3 bytes.
			vaultObjectFileOperationsMock
				.Setup(m => m.DownloadFileInBlocks_ReadBlock
				(
					Moq.It.IsAny<int>(),
					Moq.It.IsAny<int>(),
					Moq.It.IsAny<long>()
				))
				.Returns((int downloadSession, int blockSize, long offset) =>
				{
					// Return some sample data.
					return new byte[]
					{
						0,
						10,
						200
					};
				})
				.Verifiable();

			// Set up the mock vault.
			var vaultMock = new Mock<Vault>();
			vaultMock
				.SetupGet(m => m.ObjectFileOperations)
				.Returns(vaultObjectFileOperationsMock.Object);

			// Create the stream.
			var stream = new FileDownloadStreamProxy(objectFileMock.Object, vaultMock.Object);

			// Attempt to read.
			byte[] data = new byte[4096];
			int readBytes = stream.Read(data, 0, 4096);

			// Ensure that we only got 3 bytes, and that they are correct.
			Assert.AreEqual(3, readBytes);
			Assert.AreEqual(0, data[0]);
			Assert.AreEqual(10, data[1]);
			Assert.AreEqual(200, data[2]);

			// Ensure we got hit as expected.
			vaultMock.Verify();
			vaultObjectFileOperationsMock.Verify();
		}

		private class FileDownloadStreamProxy
			: Extensions.FileDownloadStream
		{
			public new FileDownloadSession DownloadSession
			{
				get => base.DownloadSession;
				set => base.DownloadSession = value;
			}

			/// <inheritdoc />
			public FileDownloadStreamProxy(ObjectFile fileToDownload, Vault vault, MFFileFormat fileFormat = MFFileFormat.MFFileFormatNative)
				: base(fileToDownload, vault, fileFormat)
			{
			}
		}
	}
}
