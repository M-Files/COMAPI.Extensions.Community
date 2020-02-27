using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace MFilesAPI.Extensions.Tests.ExtensionMethods.VaultPropertyDefOperations
{
	[TestClass]
	public class TryGetPropertyDefAliases
		: VaultPropertyDefOperationsTestBase
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullReferenceThrows()
		{
			((MFilesAPI.VaultPropertyDefOperations) null).TryGetPropertyDefAliases(1, out _);
		}

		[TestMethod]
		public void KnownPropertyDefReturnsCorrectData()
		{
			// Details about the property.
			int propertyDefId = 1234;
			string[] aliases = new []
			{
				"alias1",
				"alias2"
			};

			// Mock the property definition.
			var mock = this.GetVaultPropertyDefOperationsMock();
			mock
				.Setup(m => m.GetPropertyDefAdmin(propertyDefId))
				.Returns((int id) =>
				{
					var propertyDefAdmin = new PropertyDefAdmin()
					{
						PropertyDef = new PropertyDef()
						{
							ID = propertyDefId,
							Name = "hello world",
							DataType = MFDataType.MFDatatypeText
						},
						SemanticAliases = new SemanticAliases()
						{
							Value = string.Join(";", aliases)
						}
					};
					return propertyDefAdmin;
				});
			
			// Run the method and check the output.
			Assert.IsTrue(mock.Object.TryGetPropertyDefAliases(propertyDefId, out string[] output));
			Assert.AreEqual(aliases.Length, output.Length);
			foreach (var alias in aliases)
			{
				Assert.IsTrue(output.Contains(alias));
			}
		}

		[TestMethod]
		public void UnknownPropertyDefReturnsFalse()
		{
			// Details about the property.
			int propertyDefId = 1234;
			string[] aliases = new []
			{
				"alias1",
				"alias2"
			};

			// Mock the property definition.
			var mock = this.GetVaultPropertyDefOperationsMock();
			mock
				.Setup(m => m.GetPropertyDefAdmin(propertyDefId))
				.Returns((int id) => throw new InvalidOperationException("Sample error"));
			
			// Run the method and check the output.
			Assert.IsFalse(mock.Object.TryGetPropertyDefAliases(propertyDefId, out string[] output));
		}
	}
}
