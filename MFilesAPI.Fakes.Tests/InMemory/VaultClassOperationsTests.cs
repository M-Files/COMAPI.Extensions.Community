using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MFilesAPI.Fakes.Tests.InMemory
{
	[TestClass]
	public class VaultClassOperationsTests
		: RepositoryBaseTests<Fakes.VaultClassOperations, ObjectClassAdminEx, ObjectClassEx, MFilesAPI.VaultClassOperations>
	{
		public override Fakes.VaultClassOperations CreateRepository()
		{
			return ComInterfaceAutoImpl.GetInstanceOfCompletedType<Fakes.VaultClassOperations>();
		}

		public override ObjectClassAdminEx CreateItem()
		{
			return ObjectClassAdminEx.CloneFrom(new ObjectClassAdmin());
		}
	}
}
