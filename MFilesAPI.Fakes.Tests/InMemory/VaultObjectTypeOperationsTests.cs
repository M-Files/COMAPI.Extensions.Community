using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MFilesAPI.Fakes.Tests.InMemory
{
	[TestClass]
	public class VaultObjectTypeOperationsTests
		: RepositoryBaseTests<Fakes.VaultObjectTypeOperations, ObjTypeAdmin, ObjType, MFilesAPI.VaultObjectTypeOperations>
	{
		public override Fakes.VaultObjectTypeOperations CreateRepository()
		{
			return new Fakes.VaultObjectTypeOperations();
		}

		public override ObjTypeAdmin CreateItem()
		{
			return new ObjTypeAdmin();
		}
		[TestMethod]
		public void GetObjectTypes_Empty()
		{
			var repository = ((MFilesAPI.VaultObjectTypeOperations)this.CreateRepository());
			var all = repository.GetObjectTypes();
			Assert.IsNotNull(all);
			Assert.AreEqual(0, all.Count);
		}
		[TestMethod]
		public void GetObjectTypesAdmin_Empty()
		{
			var repository = ((MFilesAPI.VaultObjectTypeOperations)this.CreateRepository());
			var all = repository.GetObjectTypes();
			Assert.IsNotNull(all);
			Assert.AreEqual(0, all.Count);
		}
		[TestMethod]
		public void GetObjectTypes_SingleItem()
		{
			var repository = ((MFilesAPI.VaultObjectTypeOperations)this.CreateRepository());
			var item = this.CreateItem();
			repository.AddObjectTypeAdmin(item);
			Assert.AreNotEqual(0, item.ObjectType.ID);
			var all = repository.GetObjectTypes();
			Assert.IsNotNull(all);
			Assert.AreEqual(1, all.Count);
			Assert.AreEqual(item.ObjectType.ID, all[1].ID);
		}
		[TestMethod]
		public void GetObjectTypesAdmin_SingleItem()
		{
			var repository = ((MFilesAPI.VaultObjectTypeOperations)this.CreateRepository());
			var item = this.CreateItem();
			repository.AddObjectTypeAdmin(item);
			Assert.AreNotEqual(0, item.ObjectType.ID);
			var all = repository.GetObjectTypesAdmin();
			Assert.IsNotNull(all);
			Assert.AreEqual(1, all.Count);
			Assert.AreEqual(item.ObjectType.ID, all[1].ObjectType.ID);
		}
	}
}
