using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAPI.Fakes.Tests.Serialization.Version1
{
	[TestClass]
	public class VaultDeserializationTests
	{
		[TestMethod]
		public void IsNotNull()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.Deserialize(JObject.Parse("{}"));
			Assert.IsNotNull(output);
		}

		[TestMethod]
		public void Name()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.Deserialize(JObject.Parse("{ \"name\" : \"hello world\" }"));
			Assert.IsNotNull("hello world", output.Name);
		}

		[TestMethod]
		public void Guid()
		{
			var guid = System.Guid.NewGuid();
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.Deserialize(JObject.Parse($"{{ \"guid\" : \"{guid.ToString("B")}\" }}"));

			Assert.AreEqual(guid.ToString("B"), output.GetGUID());
		}
	}
}
