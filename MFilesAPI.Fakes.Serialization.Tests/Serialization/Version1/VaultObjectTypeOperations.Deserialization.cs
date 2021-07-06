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
			var items = output.GetObjectTypesAdmin();
			Assert.IsNotNull(items);
			Assert.AreEqual(1, items.Count);

			var item = items[1];
			Assert.AreEqual(1234, item.ObjectType.ID);
		}

		[TestMethod]
		public void Item_SemanticAliases_Single()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"aliases\" : [\"alias1\"]} ]"));
			var items = output.GetObjectTypesAdmin();
			Assert.IsNotNull(items);
			Assert.AreEqual(1, items.Count);

			var item = items[1];
			Assert.AreEqual("alias1", item.SemanticAliases.Value);
		}

		[TestMethod]
		public void Item_SemanticAliases_Multiple()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"aliases\" : [\"hello\",\"world\",\"another-alias\"]} ]"));
			var items = output.GetObjectTypesAdmin();
			Assert.IsNotNull(items);
			Assert.AreEqual(1, items.Count);

			var item = items[1];
			Assert.AreEqual("hello;world;another-alias", item.SemanticAliases.Value);
		}

		[TestMethod]
		public void Item_Name()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"name\" : { \"singular\" : \"object\", \"plural\" : \"objects\" } } ]"));
			var items = output.GetObjectTypesAdmin();
			Assert.IsNotNull(items);
			Assert.AreEqual(1, items.Count);

			var item = items[1];
			Assert.AreEqual("object", item.ObjectType.NameSingular);
			Assert.AreEqual("objects", item.ObjectType.NamePlural);
		}

		[TestMethod]
		public void Item_PropertyDefinitions()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"propertyDefinitions\" : { \"owner\" : 987, \"default\" : 654 } } ]"));
			var items = output.GetObjectTypesAdmin();
			Assert.IsNotNull(items);
			Assert.AreEqual(1, items.Count);

			var item = items[1];
			Assert.AreEqual(987, item.ObjectType.OwnerPropertyDef);
			Assert.AreEqual(654, item.ObjectType.DefaultPropertyDef);
		}

		[TestMethod]
		public void Item_Owner()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"owner\" : { \"hasOwner\" : true, \"ownerType\" : 101 } } ]"));
			var items = output.GetObjectTypesAdmin();
			Assert.IsNotNull(items);
			Assert.AreEqual(1, items.Count);

			var item = items[1];
			Assert.AreEqual(true, item.ObjectType.HasOwnerType);
			Assert.AreEqual(101, item.ObjectType.OwnerType);
		}

		[TestMethod]
		public void Item_Real_True()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"real\" : true} ]"));
			var items = output.GetObjectTypesAdmin();
			Assert.IsNotNull(items);
			Assert.AreEqual(1, items.Count);

			var item = items[1];
			Assert.AreEqual(true, item.ObjectType.RealObjectType);
		}

		[TestMethod]
		public void Item_Real_False()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"real\" : false} ]"));
			var items = output.GetObjectTypesAdmin();
			Assert.IsNotNull(items);
			Assert.AreEqual(1, items.Count);

			var item = items[1];
			Assert.AreEqual(false, item.ObjectType.RealObjectType);
		}

		[TestMethod]
		public void Item_CanHaveFiles_True()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"canHaveFiles\" : true} ]"));
			var items = output.GetObjectTypesAdmin();
			Assert.IsNotNull(items);
			Assert.AreEqual(1, items.Count);

			var item = items[1];
			Assert.AreEqual(true, item.ObjectType.CanHaveFiles);
		}

		[TestMethod]
		public void Item_CanHaveFiles_False()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"canHaveFiles\" : false} ]"));
			var items = output.GetObjectTypesAdmin();
			Assert.IsNotNull(items);
			Assert.AreEqual(1, items.Count);

			var item = items[1];
			Assert.AreEqual(false, item.ObjectType.CanHaveFiles);
		}

		[TestMethod]
		public void Item_AllowAdding_True()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"allowAdding\" : true} ]"));
			var items = output.GetObjectTypesAdmin();
			Assert.IsNotNull(items);
			Assert.AreEqual(1, items.Count);

			var item = items[1];
			Assert.AreEqual(true, item.ObjectType.AllowAdding);
		}

		[TestMethod]
		public void Item_AllowAdding_False()
		{
			var serializer = new MFilesAPI.Fakes.Serialization.Version1.JsonSerializer();
			var output = serializer.DeserializeVaultObjectTypeOperations(JToken.Parse("[ { \"allowAdding\" : false} ]"));
			var items = output.GetObjectTypesAdmin();
			Assert.IsNotNull(items);
			Assert.AreEqual(1, items.Count);

			var item = items[1];
			Assert.AreEqual(false, item.ObjectType.AllowAdding);
		}
	}
}
