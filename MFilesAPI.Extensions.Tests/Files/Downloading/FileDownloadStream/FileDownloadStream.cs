﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
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
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullVaultThrows2()
		{
			var objectFile = Moq.Mock.Of<ObjectFile>();
			new Extensions.FileDownloadStream(objectFile.ID, objectFile.Version, null);
		}

		[TestMethod]
		public void FileToDownloadSetsProperty()
		{
			var objectFile = Moq.Mock.Of<ObjectFile>();
			var stream = new Extensions.FileDownloadStream(objectFile, Moq.Mock.Of<Vault>());
			Assert.AreEqual(objectFile.ID, stream.FileID);
			Assert.AreEqual(objectFile.Version, stream.FileVersion);
		}

		[TestMethod]
		public void FileToDownloadSetsProperty2()
		{
			var objectFile = Moq.Mock.Of<ObjectFile>();
			var stream = new Extensions.FileDownloadStream(objectFile.ID, objectFile.Version, Moq.Mock.Of<Vault>());
			Assert.AreEqual(objectFile.ID, stream.FileID);
			Assert.AreEqual(objectFile.Version, stream.FileVersion);
		}

		[TestMethod]
		public void VaultSetsProperty()
		{
			var vault = Moq.Mock.Of<Vault>();
			var stream = new Extensions.FileDownloadStream(Moq.Mock.Of<ObjectFile>(), vault);
			Assert.AreEqual(vault, stream.Vault);
		}

		[TestMethod]
		public void VaultSetsProperty2()
		{
			var objectFile = Moq.Mock.Of<ObjectFile>();
			var vault = Moq.Mock.Of<Vault>();
			var stream = new Extensions.FileDownloadStream(objectFile.ID, objectFile.Version, vault);
			Assert.AreEqual(vault, stream.Vault);
		}

		[TestMethod]
		public void FileFormatDefaultsToNative()
		{
			var stream = new Extensions.FileDownloadStream(Moq.Mock.Of<ObjectFile>(), Moq.Mock.Of<Vault>());
			Assert.AreEqual(MFFileFormat.MFFileFormatNative, stream.FileFormat);
		}

		[TestMethod]
		public void FileFormatDefaultsToNative2()
		{
			var objectFile = Moq.Mock.Of<ObjectFile>();
			var stream = new Extensions.FileDownloadStream(objectFile.ID, objectFile.Version, Moq.Mock.Of<Vault>());
			Assert.AreEqual(MFFileFormat.MFFileFormatNative, stream.FileFormat);
		}

		[TestMethod]
		public void DownloadSessionDefaultsToNull()
		{
			var stream = new Extensions.FileDownloadStream(Moq.Mock.Of<ObjectFile>(), Moq.Mock.Of<Vault>());
			Assert.IsNull(stream.DownloadSession);
		}

		[TestMethod]
		public void DownloadSessionDefaultsToNull2()
		{
			var objectFile = Moq.Mock.Of<ObjectFile>();
			var stream = new Extensions.FileDownloadStream(objectFile.ID, objectFile.Version, Moq.Mock.Of<Vault>());
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
		[ExpectedException(typeof(ArgumentNullException))]
		public void ReadThrowsWithNullBuffer()
		{
			var stream = new Extensions.FileDownloadStream(Mock.Of<ObjectFile>(), Moq.Mock.Of<Vault>());
			stream.Read(null, 0, 1);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ReadThrowsWithNegativeOffset()
		{
			var data = new byte[1];
			var stream = new Extensions.FileDownloadStream(Mock.Of<ObjectFile>(), Moq.Mock.Of<Vault>());
			stream.Read(data, -1, 1);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ReadThrowsWithOffsetLargerThanByteArray()
		{
			var data = new byte[1];
			var stream = new Extensions.FileDownloadStream(Mock.Of<ObjectFile>(), Moq.Mock.Of<Vault>());
			stream.Read(data, 2, 1);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ReadThrowsWithNegativeCount()
		{
			var data = new byte[1];
			var stream = new Extensions.FileDownloadStream(Mock.Of<ObjectFile>(), Moq.Mock.Of<Vault>());
			stream.Read(data, 0, -1);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ReadThrowsWithCountLargerThanByteArray()
		{
			var data = new byte[1];
			var stream = new Extensions.FileDownloadStream(Mock.Of<ObjectFile>(), Moq.Mock.Of<Vault>());
			stream.Read(data, 0, 2);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ReadThrowsWithCountLargerThanByteArray2()
		{
			var data = new byte[1];
			var stream = new Extensions.FileDownloadStream(Mock.Of<ObjectFile>(), Moq.Mock.Of<Vault>());
			stream.Read(data, 1, 1);
		}

		[TestMethod]
		public void ReadCountZeroDoesNotThrow()
		{
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

			// Set up the mock vault.
			var vaultMock = new Mock<Vault>();
			vaultMock
				.SetupGet(m => m.ObjectFileOperations)
				.Returns(vaultObjectFileOperationsMock.Object);

			// Set up the data to read.
			var data = new byte[1];

			// Read some data.
			var stream = new Extensions.FileDownloadStream(Mock.Of<ObjectFile>(), vaultMock.Object);
			stream.Read(data, 0, 0);
		}

		[TestMethod]
		public void CloseDownloadSessionDoesNotThrowWithEmptyDownloadSession()
		{
			// Create the stream.
			var stream = new Extensions.FileDownloadStream(Moq.Mock.Of<ObjectFile>(), Moq.Mock.Of<Vault>());

			// Ensure the session is null.
			Assert.IsNull(stream.DownloadSession);

			// Close.
			stream.Close();
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
			stream.Close();

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
					downloadSessionMock.SetupGet(m => m.FileSize).Returns(3);
					downloadSessionMock.SetupGet(m => m.FileSize32).Returns(3);
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
			Assert.AreEqual(3, stream.Read(data, 0, 4096));

			// Ensure that the download session is not empty.
			Assert.IsNotNull(stream.DownloadSession);

			// Ensure we got hit as expected.
			vaultMock.Verify();
			vaultObjectFileOperationsMock.Verify();
		}

		[TestMethod]
		public void ReadWithLargeBlockSizeRetrievesMultipleBlocks()
		{

			// Set up a file to download.
			var objectFileMock = new Mock<ObjectFile>();
			objectFileMock.SetupGet(m => m.ID).Returns(12345);
			objectFileMock.SetupGet(m => m.Version).Returns(1);

			// Set up the vault object file operations mock.
			var vaultObjectFileOperationsMock = new Mock<VaultObjectFileOperations>();

			// We don't really care what we download, as long as we can check it happened.
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
					downloadSessionMock.SetupGet(m => m.FileSize).Returns(8 * 1024 * 1024 + 4);
					downloadSessionMock.SetupGet(m => m.FileSize32).Returns(8 * 1024 * 1024 + 4);
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
					if (offset < 8 * 1024 * 1024)
						return new byte[4 * 1024 * 1024];
					return new byte[4];
				})
				.Verifiable();

			// Set up the mock vault.
			var vaultMock = new Mock<Vault>();
			vaultMock
				.SetupGet(m => m.ObjectFileOperations)
				.Returns(vaultObjectFileOperationsMock.Object);

			// Create the stream.
			var stream = new FileDownloadStreamProxy(objectFileMock.Object, vaultMock.Object);

			// Read a large set of data (just over 8MB)
			// to cause the method to go back to the server for
			// multiple blocks.
			byte[] data = new byte[8 * 1024 * 1024 + 4];
			int ret = stream.Read(data, 0, data.Length);

			// Make sure that we got the data we expected.
			Assert.AreEqual(ret, data.Length);

			// Ensure we got hit as expected.
			vaultMock.Verify();
			vaultObjectFileOperationsMock
				.Verify(
					// Did it get block one (0 -> 4MB)?
					(m) => m.DownloadFileInBlocks_ReadBlock(1, 4 * 1024 * 1024, 0),
					Times.Once
				);
			vaultObjectFileOperationsMock
				.Verify(
					// Did it get block two (4MB -> 8MB)?
					(m) => m.DownloadFileInBlocks_ReadBlock(1, 4 * 1024 * 1024, 4 * 1024 * 1024),
					Times.Once
				);
			vaultObjectFileOperationsMock
				.Verify(
					// Did it get block three (8MB -> end)?
					(m) => m.DownloadFileInBlocks_ReadBlock(1, 4, 8 * 1024 * 1024),
					Times.Once
				);

		}

		[TestMethod]
		public void ReadIteratesThroughData()
		{
			// File data.
			var fileData = new byte[]
			{
				0,
				10,
				200,
				20,
				10
			};

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
					// Validate data.
					if(offset + blockSize > fileData.Length)
						blockSize = ((int)(fileData.Length - offset));
					// Return just the chunk requested.
					var output = new byte[blockSize];
					Array.Copy(fileData, offset, output, 0, blockSize);
					return output;
				})
				.Verifiable();

			// Set up the mock vault.
			var vaultMock = new Mock<Vault>();
			vaultMock
				.SetupGet(m => m.ObjectFileOperations)
				.Returns(vaultObjectFileOperationsMock.Object);

			// Create the stream.
			var stream = new FileDownloadStreamProxy(objectFileMock.Object, vaultMock.Object);

			// Attempt to read two bytes at a time.
			for (var i = 0; i < fileData.Length; i = i + 2)
			{
				var output = new byte[2];
				var bytesRead = stream.Read(output, 0, 2);
				switch (i)
				{
					case 0:
					case 2:
						Assert.AreEqual(2, bytesRead);
						Assert.AreEqual(fileData[i], output[0]);
						Assert.AreEqual(fileData[i + 1], output[1]);
							break;
					case 4:
						Assert.AreEqual(1, bytesRead);
						Assert.AreEqual(fileData[i], output[0]);
							break;
					default:
						throw new InvalidOperationException("Should never get here!");
				}
			}

			// Ensure that the download session is not empty.
			Assert.IsNotNull(stream.DownloadSession);

			// Ensure we got hit as expected.
			vaultMock.Verify();
			vaultObjectFileOperationsMock.Verify();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void BufferSizeTooSmall()
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

			// Attempt to read 4k bytes into a 1 byte buffer.
			byte[] data = new byte[1];
			stream.Read(data, 0, 4096);

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

			private long? length;
			public override long Length => this.length ?? base.Length;

			/// <inheritdoc />
			public override void SetLength(long value)
			{
				this.length = value;
			}

			/// <inheritdoc />
			public FileDownloadStreamProxy(ObjectFile fileToDownload, Vault vault, MFFileFormat fileFormat = MFFileFormat.MFFileFormatNative)
				: base(fileToDownload, vault, fileFormat)
			{
			}
		}
	}
}
