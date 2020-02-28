using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace MFilesAPI.Extensions.Tests.ExtensionMethods.VaultPropertyDefOperations
{
	public abstract class VaultPropertyDefOperationsTestBase
	{
		protected virtual Mock<MFilesAPI.VaultPropertyDefOperations> GetVaultPropertyDefOperationsMock()
		{
			return new Mock<MFilesAPI.VaultPropertyDefOperations>();
		}

	}
}
