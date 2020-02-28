using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace MFilesAPI.Extensions.Tests.ExtensionMethods.VaultClassOperations
{
	[TestClass]
	public class TryGetObjectClassAliases
		: VaultClassOperationsTestBase
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullReferenceThrows()
		{
			((MFilesAPI.VaultClassOperations) null).TryGetObjectClassAliases(1, out _);
		}

		[TestMethod]
		public void KnownObjectClassReturnsCorrectData()
		{
			// Details about the class.
			int classId = 1234;
			string[] aliases = new []
			{
				"alias1",
				"alias2"
			};

			// Mock the class.
			var mock = this.GetVaultClassOperationsMock();
			mock
				.Setup(m => m.GetObjectClassAdmin(classId))
				.Returns((int id) =>
				{
					var objectClassAdmin = new ObjectClassAdmin()
					{
						ID = classId,
						Name = "hello world",
						SemanticAliases = new SemanticAliases()
						{
							Value = string.Join(";", aliases)
						}
					};
					return objectClassAdmin;
				});
			
			// Run the method and check the output.
			Assert.IsTrue(mock.Object.TryGetObjectClassAliases(classId, out string[] output));
			Assert.AreEqual(aliases.Length, output.Length);
			foreach (var alias in aliases)
			{
				Assert.IsTrue(output.Contains(alias));
			}
		}

		[TestMethod]
		public void UnknownObjectClassReturnsFalse()
		{
			// Details about the class.
			int classId = 1234;
			string[] aliases = new []
			{
				"alias1",
				"alias2"
			};
			
			// Mock the class.
			var mock = this.GetVaultClassOperationsMock();
			mock
				.Setup(m => m.GetObjectClass(classId))
				.Returns((int id) => throw new InvalidOperationException("Sample error"));
			
			// Run the method and check the output.
			Assert.IsFalse(mock.Object.TryGetObjectClassAliases(classId, out string[] output));
		}
	}
}
