using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MFilesAPI.Extensions.Tests.ExtensionMethods.VaultPropertyDefOperations
{
	[TestClass]
	public class TryGetPropertyDefName
		: VaultPropertyDefOperationsTestBase
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullReferenceThrows()
		{
			((MFilesAPI.VaultPropertyDefOperations) null).TryGetPropertyDefName(1, out _);
		}

		[TestMethod]
		public void KnownPropertyDefReturnsCorrectData()
		{
			// Details about the property.
			int propertyDefId = 1234;
			string propertyName = "hello world";

			// Mock the property definition.
			var mock = this.GetVaultPropertyDefOperationsMock();
			mock
				.Setup(m => m.GetPropertyDef(propertyDefId))
				.Returns((int id) =>
				{
					var propertyDef = new PropertyDef()
					{
						ID = propertyDefId,
						Name = "hello world",
						DataType = MFDataType.MFDatatypeText
					};
					return propertyDef;
				});
			
			// Run the method and check the output.
			Assert.IsTrue(mock.Object.TryGetPropertyDefName(propertyDefId, out string output));
			Assert.AreEqual(propertyName, output);
		}

		[TestMethod]
		public void UnknownPropertyDefReturnsFalse()
		{
			// Details about the property.
			int propertyDefId = 1234;

			// Mock the property definition.
			var mock = this.GetVaultPropertyDefOperationsMock();
			mock
				.Setup(m => m.GetPropertyDef(propertyDefId))
				.Returns((int id) => throw new InvalidOperationException("Sample error"));
			
			// Run the method and check the output.
			Assert.IsFalse(mock.Object.TryGetPropertyDefName(propertyDefId, out string output));
		}
	}
}
