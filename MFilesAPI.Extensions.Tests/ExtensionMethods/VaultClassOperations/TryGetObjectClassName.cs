using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MFilesAPI.Extensions.Tests.ExtensionMethods.VaultClassOperations
{
	[TestClass]
	public class TryGetObjectClassName
		: VaultClassOperationsTestBase
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullReferenceThrows()
		{
			((MFilesAPI.VaultClassOperations) null).TryGetObjectClassName(1, out _);
		}

		[TestMethod]
		public void KnownObjectClassReturnsCorrectData()
		{
			// Details about the class.
			int classId = 1234;
			string className = "hello world";
			
			// Mock the class.
			var mock = this.GetVaultClassOperationsMock();
			mock
				.Setup(m => m.GetObjectClass(classId))
				.Returns((int id) =>
				{
					var propertyDef = new ObjectClass()
					{
						ID = classId,
						Name = "hello world"
					};
					return propertyDef;
				});
			
			// Run the method and check the output.
			Assert.IsTrue(mock.Object.TryGetObjectClassName(classId, out string output));
			Assert.AreEqual(className, output);
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
			Assert.IsFalse(mock.Object.TryGetObjectClassName(classId, out string output));
		}
	}
}
