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
	public class VaultObjectTypeOperationsDeserializationTests
	{
		[TestMethod]
		public void IsNotNull()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[]"));
			Assert.IsNotNull(output);
		}

		[TestMethod]
		public void Item_Id()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"id\" : 1234} ]"));
			Assert.AreEqual(1, output.Count);

			var item = output.FirstOrDefault();
			Assert.AreEqual(1234, item.Key);
			Assert.AreEqual(1234, item.Value.ObjectType.ID);
		}

		[TestMethod]
		public void Item_SemanticAliases_Single()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"aliases\" : [\"alias1\"]} ]"));
			Assert.AreEqual(1, output.Count);

			var item = output.FirstOrDefault();
			Assert.AreEqual("alias1", item.Value.SemanticAliases.Value);
		}

		[TestMethod]
		public void Item_SemanticAliases_Multiple()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"aliases\" : [\"hello\",\"world\",\"another-alias\"]} ]"));
			Assert.AreEqual(1, output.Count);

			var item = output.FirstOrDefault();
			Assert.AreEqual("hello;world;another-alias", item.Value.SemanticAliases.Value);
		}

		[TestMethod]
		public void Item_Name()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"name\" : { \"singular\" : \"object\", \"plural\" : \"objects\" } } ]"));
			Assert.AreEqual(1, output.Count);

			var item = output.FirstOrDefault();
			Assert.AreEqual("object", item.Value.ObjectType.NameSingular);
			Assert.AreEqual("objects", item.Value.ObjectType.NamePlural);
		}

		[TestMethod]
		public void Item_PropertyDefinitions()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"propertyDefinitions\" : { \"owner\" : 987, \"default\" : 654 } } ]"));
			Assert.AreEqual(1, output.Count);

			var item = output.FirstOrDefault();
			Assert.AreEqual(987, item.Value.ObjectType.OwnerPropertyDef);
			Assert.AreEqual(654, item.Value.ObjectType.DefaultPropertyDef);
		}

		[TestMethod]
		public void Item_Owner()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"owner\" : { \"hasOwner\" : true, \"ownerType\" : 101 } } ]"));
			Assert.AreEqual(1, output.Count);

			var item = output.FirstOrDefault();
			Assert.AreEqual(true, item.Value.ObjectType.HasOwnerType);
			Assert.AreEqual(101, item.Value.ObjectType.OwnerType);
		}

		[TestMethod]
		public void Item_Real_True()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"real\" : true} ]"));
			Assert.AreEqual(1, output.Count);

			var item = output.FirstOrDefault();
			Assert.AreEqual(true, item.Value.ObjectType.RealObjectType);
		}

		[TestMethod]
		public void Item_Real_False()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"real\" : false} ]"));
			Assert.AreEqual(1, output.Count);

			var item = output.FirstOrDefault();
			Assert.AreEqual(false, item.Value.ObjectType.RealObjectType);
		}

		[TestMethod]
		public void Item_CanHaveFiles_True()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"canHaveFiles\" : true} ]"));
			Assert.AreEqual(1, output.Count);

			var item = output.FirstOrDefault();
			Assert.AreEqual(true, item.Value.ObjectType.CanHaveFiles);
		}

		[TestMethod]
		public void Item_CanHaveFiles_False()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"canHaveFiles\" : false} ]"));
			Assert.AreEqual(1, output.Count);

			var item = output.FirstOrDefault();
			Assert.AreEqual(false, item.Value.ObjectType.CanHaveFiles);
		}

		[TestMethod]
		public void Item_AllowAdding_True()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"allowAdding\" : true} ]"));
			Assert.AreEqual(1, output.Count);

			var item = output.FirstOrDefault();
			Assert.AreEqual(true, item.Value.ObjectType.AllowAdding);
		}

		[TestMethod]
		public void Item_AllowAdding_False()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"allowAdding\" : false} ]"));
			Assert.AreEqual(1, output.Count);

			var item = output.FirstOrDefault();
			Assert.AreEqual(false, item.Value.ObjectType.AllowAdding);
		}
	}
}
