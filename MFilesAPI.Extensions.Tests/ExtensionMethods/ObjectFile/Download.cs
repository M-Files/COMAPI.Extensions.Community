using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFilesAPI.Extensions.Tests.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MFilesAPI.Extensions.Tests.ExtensionMethods.ObjectFile
{
	[TestClass]
	public class Download
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DownloadThrowsException_ObjectFile()
		{
			((MFilesAPI.ObjectFile) null)
				.Download
				(
					Moq.Mock.Of<Vault>(),
					@"C:\temp\download.tmp"
				);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DownloadThrowsException_Vault()
		{
			(new ObjectFileStub())
				.Download
					(
						vault: null, 
						@"C:\temp\download.tmp"
					);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DownloadThrowsException_FileInfo()
		{
			(new ObjectFileStub())
				.Download
				(
					vault: Moq.Mock.Of<Vault>(), 
					downloadTo: null
				);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void DownloadThrowsException_NegativeBlockSize()
		{
			(new ObjectFileStub())
				.Download
				(
					vault: Moq.Mock.Of<Vault>(), 
					@"C:\temp\download.tmp",
					blockSize: -1
				);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void DownloadThrowsException_ZeroBlockSize()
		{
			(new ObjectFileStub())
				.Download
				(
					vault: Moq.Mock.Of<Vault>(), 
					@"C:\temp\download.tmp",
					blockSize: 0
				);
		}
	}
}
