using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MFilesAPI.Extensions.Tests.ExtensionMethods.VaultClassOperations
{
	[TestClass]
	public class TryGetObjectClassNamePropertyDef
		: VaultClassOperationsTestBase
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullReferenceThrows()
		{
			((MFilesAPI.VaultClassOperations) null).TryGetObjectClassNamePropertyDef(1, out _);
		}

		[TestMethod]
		[DataRow(1234, 0)]
		[DataRow(1234, 1020)]
		public void KnownObjectClassReturnsCorrectData
			(
			int classId,
			int expectedPropertyDefId
			)
		{
			// Mock the class.
			var mock = this.GetVaultClassOperationsMock();
			mock
				.Setup(m => m.GetObjectClass(classId))
				.Returns((int id) =>
				{
					var objectClass = new ObjectClass()
					{
						ID = classId,
						NamePropertyDef = expectedPropertyDefId
					};
					return objectClass;
				});
			
			// Run the method and check the output.
			Assert.IsTrue(mock.Object.TryGetObjectClassNamePropertyDef(classId, out int output));
			Assert.AreEqual(expectedPropertyDefId, output);
		}

		[TestMethod]
		public void UnknownObjectClassReturnsFalse()
		{
			// Details about the class.
			int classId = 1234;
			
			// Mock the class.
			var mock = this.GetVaultClassOperationsMock();
			mock
				.Setup(m => m.GetObjectClass(classId))
				.Returns((int id) => throw new InvalidOperationException("Sample error"));
			
			// Run the method and check the output.
			Assert.IsFalse(mock.Object.TryGetObjectClassNamePropertyDef(classId, out _));
		}
	}
}
