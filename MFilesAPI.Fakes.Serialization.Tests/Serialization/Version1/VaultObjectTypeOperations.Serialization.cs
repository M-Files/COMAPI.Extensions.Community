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
	public class VaultObjectTypeOperationsSerializationTests
	{
		[TestMethod]
		public void IsNotNull()
		{
			var vaultObjectTypeOperations = new VaultObjectTypeOperations();
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();

			var output = serializer.Serialize(vaultObjectTypeOperations);

			Assert.IsNotNull(output);
		}

		[TestMethod]
		public void IsEmpty()
		{
			var vaultObjectTypeOperations = new VaultObjectTypeOperations();
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();

			var output = serializer.Serialize(vaultObjectTypeOperations);

			Assert.AreEqual("[]", output.ToString());
		}

		[TestMethod]
		public void Item_Id()
		{
			var vaultObjectTypeOperations = new VaultObjectTypeOperations();
			vaultObjectTypeOperations.Add(new ObjTypeAdmin()
			{
				ObjectType = new ObjType()
				{
					ID = 1234
				}
			});
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();

			var output = serializer.Serialize(vaultObjectTypeOperations) as JArray;
			Assert.AreEqual(1, output.Count);

			var item = output[0];

			Assert.AreEqual(1234, item["id"]);
		}

		[TestMethod]
		public void Item_Aliases_Single()
		{
			var vaultObjectTypeOperations = new VaultObjectTypeOperations();
			vaultObjectTypeOperations.Add(new ObjTypeAdmin()
			{
				SemanticAliases = new SemanticAliases()
				{
					Value = "hello"
				}
			});
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();

			var output = serializer.Serialize(vaultObjectTypeOperations) as JArray;
			Assert.AreEqual(1, output.Count);

			var item = output[0];

			var aliases = item["aliases"] as JArray;
			Assert.IsNotNull(aliases);
			Assert.AreEqual(1, aliases.Count);

			Assert.AreEqual("hello", aliases[0]);
		}

		[TestMethod]
		public void Item_Aliases_Multiple()
		{
			var vaultObjectTypeOperations = new VaultObjectTypeOperations();
			vaultObjectTypeOperations.Add(new ObjTypeAdmin()
			{
				SemanticAliases = new SemanticAliases()
				{
					Value = "hello;world;moar aliases"
				}
			});
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();

			var output = serializer.Serialize(vaultObjectTypeOperations) as JArray;
			Assert.AreEqual(1, output.Count);

			var item = output[0];

			var aliases = item["aliases"] as JArray;
			Assert.IsNotNull(aliases);
			Assert.AreEqual(3, aliases.Count);

			Assert.AreEqual("hello", aliases[0]);
			Assert.AreEqual("world", aliases[1]);
			Assert.AreEqual("moar aliases", aliases[2]);
		}

		[TestMethod]
		public void Item_Name()
		{
			var vaultObjectTypeOperations = new VaultObjectTypeOperations();
			vaultObjectTypeOperations.Add(new ObjTypeAdmin()
			{
				ObjectType = new ObjType()
				{
					NameSingular = "object",
					NamePlural = "objects"
				}
			});
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();

			var output = serializer.Serialize(vaultObjectTypeOperations) as JArray;
			Assert.AreEqual(1, output.Count);

			var item = output[0]["name"];
			Assert.IsNotNull(item);

			Assert.AreEqual("object", item["singular"]);
			Assert.AreEqual("objects", item["plural"]);
		}

		[TestMethod]
		public void Item_PropertyDefinitions()
		{
			var vaultObjectTypeOperations = new VaultObjectTypeOperations();
			vaultObjectTypeOperations.Add(new ObjTypeAdmin()
			{
				ObjectType = new ObjType()
				{
					OwnerPropertyDef = 123,
					DefaultPropertyDef = 456
				}
			});
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();

			var output = serializer.Serialize(vaultObjectTypeOperations) as JArray;
			Assert.AreEqual(1, output.Count);

			var item = output[0]["propertyDefinitions"];
			Assert.IsNotNull(item);

			Assert.AreEqual(123, item["owner"]);
			Assert.AreEqual(456, item["default"]);
		}

		[TestMethod]
		public void Item_Real_False()
		{
			var vaultObjectTypeOperations = new VaultObjectTypeOperations();
			vaultObjectTypeOperations.Add(new ObjTypeAdmin()
			{
				ObjectType = new ObjType()
				{
					RealObjectType = false
				}
			});
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();

			var output = serializer.Serialize(vaultObjectTypeOperations) as JArray;
			Assert.AreEqual(1, output.Count);

			var item = output[0];

			Assert.AreEqual(false, item["real"]);
		}

		[TestMethod]
		public void Item_Real_True()
		{
			var vaultObjectTypeOperations = new VaultObjectTypeOperations();
			vaultObjectTypeOperations.Add(new ObjTypeAdmin()
			{
				ObjectType = new ObjType()
				{
					RealObjectType = true
				}
			});
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();

			var output = serializer.Serialize(vaultObjectTypeOperations) as JArray;
			Assert.AreEqual(1, output.Count);

			var item = output[0];

			Assert.AreEqual(true, item["real"]);
		}

		[TestMethod]
		public void Item_CanHaveFiles_False()
		{
			var vaultObjectTypeOperations = new VaultObjectTypeOperations();
			vaultObjectTypeOperations.Add(new ObjTypeAdmin()
			{
				ObjectType = new ObjType()
				{
					CanHaveFiles = false
				}
			});
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();

			var output = serializer.Serialize(vaultObjectTypeOperations) as JArray;
			Assert.AreEqual(1, output.Count);

			var item = output[0];

			Assert.AreEqual(false, item["canHaveFiles"]);
		}

		[TestMethod]
		public void Item_CanHaveFiles_True()
		{
			var vaultObjectTypeOperations = new VaultObjectTypeOperations();
			vaultObjectTypeOperations.Add(new ObjTypeAdmin()
			{
				ObjectType = new ObjType()
				{
					CanHaveFiles = true
				}
			});
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();

			var output = serializer.Serialize(vaultObjectTypeOperations) as JArray;
			Assert.AreEqual(1, output.Count);

			var item = output[0];

			Assert.AreEqual(true, item["canHaveFiles"]);
		}

		[TestMethod]
		public void Item_AllowAdding_False()
		{
			var vaultObjectTypeOperations = new VaultObjectTypeOperations();
			vaultObjectTypeOperations.Add(new ObjTypeAdmin()
			{
				ObjectType = new ObjType()
				{
					AllowAdding = false
				}
			});
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();

			var output = serializer.Serialize(vaultObjectTypeOperations) as JArray;
			Assert.AreEqual(1, output.Count);

			var item = output[0];

			Assert.AreEqual(false, item["allowAdding"]);
		}

		[TestMethod]
		public void Item_AllowAdding_True()
		{
			var vaultObjectTypeOperations = new VaultObjectTypeOperations();
			vaultObjectTypeOperations.Add(new ObjTypeAdmin()
			{
				ObjectType = new ObjType()
				{
					AllowAdding = true
				}
			});
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();

			var output = serializer.Serialize(vaultObjectTypeOperations) as JArray;
			Assert.AreEqual(1, output.Count);

			var item = output[0];

			Assert.AreEqual(true, item["allowAdding"]);
		}

		[TestMethod]
		public void Item_Owner()
		{
			var vaultObjectTypeOperations = new VaultObjectTypeOperations();
			vaultObjectTypeOperations.Add(new ObjTypeAdmin()
			{
				ObjectType = new ObjType()
				{
					HasOwnerType = true,
					OwnerType = 876
				}
			});
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();

			var output = serializer.Serialize(vaultObjectTypeOperations) as JArray;
			Assert.AreEqual(1, output.Count);

			var item = output[0]["owner"];
			Assert.IsNotNull(item);

			Assert.AreEqual(true, item["hasOwner"]);
			Assert.AreEqual(876, item["ownerType"]);
		}
	}
}
