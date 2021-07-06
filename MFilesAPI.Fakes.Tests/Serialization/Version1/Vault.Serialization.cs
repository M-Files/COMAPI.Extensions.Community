using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAPI.Fakes.Tests.Serialization.Version1
{
	[TestClass]
	public class VaultSerializationTests
	{
		[TestMethod]
		public void IsNotNull()
		{
			var vault = new Vault();
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();

			var output = serializer.Serialize(vault);

			Assert.IsNotNull(output);
		}
	}
}
