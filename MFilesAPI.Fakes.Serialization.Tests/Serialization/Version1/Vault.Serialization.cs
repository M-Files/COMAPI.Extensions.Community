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

		[TestMethod]
		public void Name()
		{
			var vault = new Vault() { Name = "hello world" };
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();

			var output = serializer.Serialize(vault);

			Assert.AreEqual("hello world", output["name"]);
		}

		[TestMethod]
		public void Guid()
		{
			var guid = System.Guid.NewGuid();
			var vault = new Vault() { Guid = guid };
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();

			var output = serializer.Serialize(vault);

			Assert.AreEqual(guid.ToString("B"), output["guid"]);
		}

		[TestMethod]
		public void ObjectTypes_Empty()
		{
			var vault = new Vault();
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();

			var output = serializer.Serialize(vault);
			Assert.IsNotNull(output);
			var objectTypes = output["objectTypes"] as JArray;
			Assert.IsNotNull(objectTypes);
			Assert.AreEqual(0, objectTypes.Count);
		}

		[TestMethod]
		public void ObjectTypes_Items()
		{
			var vault = new Vault();
			vault.ObjectTypeOperations.AddObjectTypeAdmin(new ObjTypeAdmin()
			{
				ObjectType = new ObjType()
				{
					ID = 12345
				}
			});
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();

			var output = serializer.Serialize(vault);
			Assert.IsNotNull(output);
			var objectTypes = output["objectTypes"] as JArray;
			Assert.IsNotNull(objectTypes);
			Assert.AreEqual(1, objectTypes.Count);
			Assert.AreEqual(12345, objectTypes[0]["id"]);
		}
	}
}
