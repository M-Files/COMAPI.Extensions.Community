using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAPI.Extensions.Tests.Files.Uploading.FileUploadStream
{
	[TestClass]
	public partial class FileUploadStreamTests
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullFileVerThrows()
		{
			new Extensions.FileUploadStream(null, new ObjID(), Moq.Mock.Of<Vault>());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullObjIDThrows()
		{
			new Extensions.FileUploadStream(Moq.Mock.Of<FileVer>(), null, Moq.Mock.Of<Vault>());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullVaultThrows()
		{
			new Extensions.FileUploadStream(Moq.Mock.Of<FileVer>(), new ObjID(), null);
		}

		[TestMethod]
		public void FileToDownloadSetsProperty()
		{
			var fileVer = Moq.Mock.Of<FileVer>();
			var stream = new Extensions.FileUploadStream(fileVer, new ObjID(), Moq.Mock.Of<Vault>());
			Assert.AreEqual(fileVer, stream.FileToOverwrite);
		}

		[TestMethod]
		public void ObjIDSetsProperty()
		{
			var objId = Moq.Mock.Of<ObjID>();
			var stream = new Extensions.FileUploadStream(Moq.Mock.Of<FileVer>(), objId, Moq.Mock.Of<Vault>());
			Assert.AreEqual(objId, stream.ObjId);
		}

		[TestMethod]
		public void VaultSetsProperty()
		{
			var vault = Moq.Mock.Of<Vault>();
			var stream = new Extensions.FileUploadStream(Moq.Mock.Of<FileVer>(), new ObjID(), vault);
			Assert.AreEqual(vault, stream.Vault);
		}

		[TestMethod]
		public void UploadSessionDefaultsToNull()
		{
			var stream = new Extensions.FileUploadStream(Moq.Mock.Of<FileVer>(), new ObjID(),Moq.Mock.Of<Vault>());
			Assert.IsNull(stream.UploadSessionId);
		}

		[TestMethod]
		public void CanReadReturnsFalse()
		{
			Assert.IsFalse
			(
				new Extensions.FileUploadStream(Mock.Of<FileVer>(), new ObjID(), Moq.Mock.Of<Vault>()).CanRead
			);
		}

		[TestMethod]
		public void CanSeekReturnsFalse()
		{
			Assert.IsFalse
			(
				new Extensions.FileUploadStream(Mock.Of<FileVer>(), new ObjID(), Moq.Mock.Of<Vault>()).CanSeek
			);
		}

		[TestMethod]
		public void CanWriteReturnsTrue()
		{
			Assert.IsTrue
			(
				new Extensions.FileUploadStream(Mock.Of<FileVer>(), new ObjID(), Moq.Mock.Of<Vault>()).CanWrite
			);
		}

		[TestMethod]
		public void PositionDefaultsToZero()
		{
			Assert.AreEqual
			(
				0,
				new Extensions.FileUploadStream(Mock.Of<FileVer>(), new ObjID(), Moq.Mock.Of<Vault>()).Position
			);
		}

		[TestMethod]
		[ExpectedException(typeof(NotSupportedException))]
		public void SettingPositionThrows()
		{
			new Extensions.FileUploadStream(Mock.Of<FileVer>(), new ObjID(), Moq.Mock.Of<Vault>())
				.Position = 123;
		}

		[TestMethod]
		[ExpectedException(typeof(NotSupportedException))]
		public void SettingLengthThrows()
		{
			new Extensions.FileUploadStream(Mock.Of<FileVer>(), new ObjID(), Moq.Mock.Of<Vault>())
				.SetLength(123);
		}

		[TestMethod]
		[ExpectedException(typeof(NotSupportedException))]
		public void SeekThrows()
		{
			new Extensions.FileUploadStream(Mock.Of<FileVer>(), new ObjID(), Moq.Mock.Of<Vault>())
				.Seek(123, System.IO.SeekOrigin.Begin);
		}

		[TestMethod]
		[ExpectedException(typeof(NotSupportedException))]
		public void ReadThrows()
		{
			new Extensions.FileUploadStream(Mock.Of<FileVer>(), new ObjID(), Moq.Mock.Of<Vault>())
				.Read(new byte[0], 0, 0);
		}

		[TestMethod]
		public void OpenUploadSessionPopulatesUploadSession()
		{
			// Set up a file to upload to.
			var fileVerMock = new Mock<FileVer>();
			fileVerMock.SetupGet(m => m.ID).Returns(1);
			fileVerMock.SetupGet(m => m.Version).Returns(123);

			// Set up the vault object file operations mock.
			var vaultObjectFileOperationsMock = new Mock<VaultObjectFileOperations>();

			// When UploadFileBlockBegin is called, return a dummy session.
			vaultObjectFileOperationsMock
				.Setup(m => m.UploadFileBlockBegin
				(
				))
				.Returns(() => 1)
				.Verifiable();

			// Set up the mock vault.
			var vaultMock = new Mock<Vault>();
			vaultMock
				.SetupGet(m => m.ObjectFileOperations)
				.Returns(vaultObjectFileOperationsMock.Object);

			// Act.
			var stream = new Extensions.FileUploadStream(fileVerMock.Object, new ObjID(), vaultMock.Object);
			stream.OpenUploadSession();

			// Ensure object is populated.
			Assert.IsNotNull(stream.UploadSessionId);

			// Ensure we got hit as expected.
			vaultMock.Verify();
			vaultObjectFileOperationsMock.Verify();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void WriteThrowsWithNullBuffer()
		{
			var stream = new Extensions.FileUploadStream(Mock.Of<FileVer>(), new ObjID(), Moq.Mock.Of<Vault>());
			stream.Write(null, 0, 1);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void WriteThrowsWithNegativeOffset()
		{
			var data = new byte[1];
			var stream = new Extensions.FileUploadStream(Mock.Of<FileVer>(), new ObjID(), Moq.Mock.Of<Vault>());
			stream.Write(data, -1, 1);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void WriteThrowsWithOffsetLargerThanByteArray()
		{
			var data = new byte[1];
			var stream = new Extensions.FileUploadStream(Mock.Of<FileVer>(), new ObjID(), Moq.Mock.Of<Vault>());
			stream.Write(data, 2, 1);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void WriteThrowsWithNegativeCount()
		{
			var data = new byte[1];
			var stream = new Extensions.FileUploadStream(Mock.Of<FileVer>(), new ObjID(), Moq.Mock.Of<Vault>());
			stream.Write(data, 0, -1);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ReadThrowsWithCountLargerThanByteArray()
		{
			var data = new byte[1];
			var stream = new Extensions.FileUploadStream(Mock.Of<FileVer>(), new ObjID(), Moq.Mock.Of<Vault>());
			stream.Write(data, 0, 2);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void WriteThrowsWithCountLargerThanByteArray2()
		{
			var data = new byte[1];
			var stream = new Extensions.FileUploadStream(Mock.Of<FileVer>(), new ObjID(), Moq.Mock.Of<Vault>());
			stream.Write(data, 1, 1);
		}

		[TestMethod]
		public void WriteCountZeroDoesNotThrow()
		{
			// Set up the vault object file operations mock.
			var vaultObjectFileOperationsMock = new Mock<VaultObjectFileOperations>();

			// When UploadFileBlockBegin is called, return a dummy session.
			vaultObjectFileOperationsMock
				.Setup(m => m.UploadFileBlockBegin
				(
				))
				.Returns(() => 1)
				.Verifiable();

			// Set up the mock vault.
			var vaultMock = new Mock<Vault>();
			vaultMock
				.SetupGet(m => m.ObjectFileOperations)
				.Returns(vaultObjectFileOperationsMock.Object);

			// Set up the data to read.
			var data = new byte[1];

			// Read some data.
			var stream = new Extensions.FileUploadStream(Mock.Of<FileVer>(), new ObjID(), vaultMock.Object);
			stream.Write(data, 0, 0);
		}

		[TestMethod]
		public void CloseUploadSessionDoesNotThrowWithEmptyUploadSession()
		{
			// Create the stream.
			var stream = new Extensions.FileUploadStream(Mock.Of<FileVer>(), new ObjID(), Moq.Mock.Of<Vault>());

			// Ensure the session is null.
			Assert.IsNull(stream.UploadSessionId);

			// Close.
			stream.Close();
		}

		[TestMethod]
		public void CloseUploadSessionCleansUpUploadSession()
		{
			// Set up a file to upload to.
			var fileVer = new Mock<FileVer>();
			fileVer.SetupGet(m => m.ID).Returns(12345);
			fileVer.SetupGet(m => m.Version).Returns(1);

			// Set up the vault object file operations mock.
			var vaultObjectFileOperationsMock = new Mock<VaultObjectFileOperations>();

			// When CancelFileDownloadSession is called, ensure that the correct data is passed.
			vaultObjectFileOperationsMock
				.Setup(m => m.CloseUploadSession
				(
					Moq.It.IsAny<int>()
				))
				.Callback((int sessionId) => { Assert.AreEqual(12345, sessionId); })
				.Verifiable();

			// Set up the mock vault.
			var vaultMock = new Mock<Vault>();
			vaultMock
				.SetupGet(m => m.ObjectFileOperations)
				.Returns(vaultObjectFileOperationsMock.Object);

			// Create the stream.
			var stream = new FileUploadStreamProxy(fileVer.Object, new ObjID(), vaultMock.Object, false);
			
			// Set up the session.
			stream.UploadSessionId = 12345;

			// Close the session.
			stream.Close();

			// Ensure that the session is empty.
			Assert.IsNull(stream.UploadSessionId);

			// Ensure we got hit as expected.
			vaultMock.Verify();
			vaultObjectFileOperationsMock.Verify();
		}

		[TestMethod]
		public void WriteOpensUploadSessionIfNotOpen()
		{
			// Set up a file to upload to.
			var fileVerMock = new Mock<FileVer>();
			fileVerMock.SetupGet(m => m.ID).Returns(12345);
			fileVerMock.SetupGet(m => m.Version).Returns(1);

			// Set up the vault object file operations mock.
			var vaultObjectFileOperationsMock = new Mock<VaultObjectFileOperations>();

			// When UploadFileBlockBegin is called, return a dummy session.
			vaultObjectFileOperationsMock
				.Setup(m => m.UploadFileBlockBegin
				(
				))
				.Returns(() => 1)
				.Verifiable();

			// Set up the mock vault.
			var vaultMock = new Mock<Vault>();
			vaultMock
				.SetupGet(m => m.ObjectFileOperations)
				.Returns(vaultObjectFileOperationsMock.Object);

			// Create the stream.
			var stream = new FileUploadStreamProxy(fileVerMock.Object, new ObjID(), vaultMock.Object);
			Assert.IsNull(stream.UploadSessionId);

			// Attempt to write.
			byte[] data = new byte[4096];
			stream.Write(data, 0, 4096);

			// Ensure that the session is not empty.
			Assert.IsNotNull(stream.UploadSessionId);

			// Ensure we got hit as expected.
			vaultMock.Verify();
			vaultObjectFileOperationsMock.Verify();
		}

		private class FileUploadStreamProxy
			: Extensions.FileUploadStream
		{
			public new int? UploadSessionId
			{
				get => base.UploadSessionId;
				set => base.UploadSessionId = value;
			}

			/// <inheritdoc />
			public FileUploadStreamProxy(FileVer fileVer, ObjID objId, Vault vault, bool automaticallyCommitOnDispoal = true)
				: base(fileVer, objId, vault, automaticallyCommitOnDispoal)
			{
			}
		}
	}
}
