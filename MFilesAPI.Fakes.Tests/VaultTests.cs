using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MFilesAPI.Fakes.Tests
{
	[TestClass]
	public class VaultTests
	{
		[TestMethod]
		public void PropertiesNotNull()
		{
			var v = new Vault();
			Assert.IsNotNull(v.ObjectTypeOperations);
			Assert.IsNotNull(v.ClassOperations);
			Assert.IsNotNull(v.PropertyDefOperations);
			Assert.IsNotNull(v.SessionInfo);
			Assert.IsNotNull(v.VaultServerAttachments);
		}
	}
}
