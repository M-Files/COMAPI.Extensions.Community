using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MFilesAPI.Fakes.Tests.InMemory
{
	[TestClass]
	public class VaultClassOperationsTests
		: RepositoryBaseTests<Fakes.VaultClassOperations, ObjectClassAdminEx, ObjectClassEx, MFilesAPI.VaultClassOperations>
	{
		public override Fakes.VaultClassOperations CreateRepository()
		{
			return new Fakes.VaultClassOperations();
		}

		public override ObjectClassAdminEx CreateItem()
		{
			return new ObjectClassAdminEx(new ObjectClassAdmin());
		}
	}
}
