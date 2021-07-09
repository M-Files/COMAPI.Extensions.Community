using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace MFilesAPI.Fakes.Tests.InMemory
{
	[TestClass]
	public class VaultObjectOperationsTests
	{
		public VaultObjectOperations CreateRepository() => new VaultObjectOperations();

		[TestMethod]
		public void EmptyByDefault()
		{
			Assert.AreEqual(0, this.CreateRepository().Count);
		}
		[TestMethod]
		public void AddIncreasesCount()
		{
			var repository = this.CreateRepository();
			var item = new ObjectVersionAndPropertiesEx()
			{
				ObjVer = new ObjVer()
				{
					ID = 123,
					Type = 101,
					Version = 1
				},
				VersionData = new ObjectVersionEx()
				{
				},
				Properties = new PropertyValues()
				{
				},
				Vault = new Vault()
			};
			repository.Add(item);
			Assert.AreEqual(1, repository.Count);
		}
		[TestMethod]
		public void GetLatestObjVer_Single()
		{
			var repository = this.CreateRepository();
			var item = new ObjectVersionAndPropertiesEx()
			{
				ObjVer = new ObjVer()
				{
					ID = 123,
					Type = 101,
					Version = 1
				},
				VersionData = new ObjectVersionEx()
				{
				},
				Properties = new PropertyValues()
				{
				},
				Vault = new Vault()
			};
			repository.Add(item);

			// Check the ObjVer is correct.
			var objVer = ((MFilesAPI.VaultObjectOperations)repository)
				.GetLatestObjVer(new ObjID() { ID = 123, Type = 101 }, true);
			Assert.IsNotNull(objVer);
			Assert.AreEqual(item.ObjVer.ID, objVer.ID);
			Assert.AreEqual(item.ObjVer.Type, objVer.Type);
			Assert.AreEqual(item.ObjVer.Version, objVer.Version);
		}
		[TestMethod]
		public void GetLatestObjectVersionAndProperties_Single()
		{
			var repository = this.CreateRepository();
			var item = new ObjectVersionAndPropertiesEx()
			{
				ObjVer = new ObjVer()
				{
					ID = 123,
					Type = 101,
					Version = 1
				},
				VersionData = new ObjectVersionEx()
				{
				},
				Properties = new PropertyValues()
				{
				},
				Vault = new Vault()
			};
			repository.Add(item);

			// Check that the object version is correct.
			var objectVersionAndProperties = ((MFilesAPI.VaultObjectOperations)repository)
				.GetLatestObjectVersionAndProperties(new ObjID() { ID = 123, Type = 101 }, true);
			Assert.IsNotNull(objectVersionAndProperties);
			Assert.AreEqual(item.ObjVer.ID, objectVersionAndProperties.ObjVer.ID);
			Assert.AreEqual(item.ObjVer.Type, objectVersionAndProperties.ObjVer.Type);
			Assert.AreEqual(item.ObjVer.Version, objectVersionAndProperties.ObjVer.Version);
		}
	}
}
