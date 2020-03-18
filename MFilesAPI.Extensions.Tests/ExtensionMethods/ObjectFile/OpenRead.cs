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
	public class OpenRead
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void OpenRead_ObjectFile()
		{
			((MFilesAPI.ObjectFile) null)
				.OpenRead
				(
					Moq.Mock.Of<Vault>()
				);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void OpenReadThrowsException_Vault()
		{
			(new ObjectFileStub())
				.OpenRead
					(
						vault: null
					);
		}
	}
}
