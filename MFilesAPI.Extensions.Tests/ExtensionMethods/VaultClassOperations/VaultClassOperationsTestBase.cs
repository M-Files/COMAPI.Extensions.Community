using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Moq;

namespace MFilesAPI.Extensions.Tests.ExtensionMethods.VaultClassOperations
{
	public abstract class VaultClassOperationsTestBase
	{
		public Mock<MFilesAPI.VaultClassOperations> GetVaultClassOperationsMock()
		{
			return new Mock<MFilesAPI.VaultClassOperations>();
		}

	}
}
