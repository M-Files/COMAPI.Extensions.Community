using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MFilesAPI.Fakes.Tests.InMemory
{
	public abstract class RepositoryBaseTests<TA, TB, TC, TD>
		where TA : Fakes.InMemory.RepositoryBase<TB, TC>, TD
	{
		public abstract TA CreateRepository();
		public abstract TB CreateItem();

		[TestMethod]
		public void EmptyByDefault()
		{
			Assert.AreEqual(0, this.CreateRepository().Count);
		}
		[TestMethod]
		public void AddIncreasesCount()
		{
			var repository = this.CreateRepository();
			var item = this.CreateItem();
			repository.Add(item);
			Assert.AreEqual(1, repository.Count);
			Assert.AreEqual(item, repository[1]);
		}
	}
}
