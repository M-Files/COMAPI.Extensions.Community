using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MFilesAPI.Extensions.Tests.ExtensionMethods.VaultClassOperations
{
	[TestClass]
	public class TryGetObjectClassWorkflowDetails
		: VaultClassOperationsTestBase
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullReferenceThrows()
		{
			((MFilesAPI.VaultClassOperations) null).TryGetObjectClassWorkflowDetails(1, out _, out bool _);
		}

		[TestMethod]
		[DataRow(1234, 0, false)]
		[DataRow(1234, 101, false)]
		[DataRow(1234, 101, true)]
		public void ObjectClassWithWorkflowDetailsReturnsCorrectData
			(
			int classId,
			int workflowId,
			bool forced
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
						Name = "hello world",
						Workflow = workflowId,
						ForceWorkflow = forced
					};
					return objectClass;
				});
			
			// Run the method and check the output.
			Assert.IsTrue(mock.Object.TryGetObjectClassWorkflowDetails(classId, out int a, out bool b));
			Assert.AreEqual(workflowId, a);
			Assert.AreEqual(forced, b);
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
			Assert.IsFalse(mock.Object.TryGetObjectClassWorkflowDetails(classId, out _, out _));
		}
	}
}
