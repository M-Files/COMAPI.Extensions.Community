using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MFilesAPI.Extensions.Tests.ExtensionMethods.VaultPropertyDefOperations
{
	[TestClass]
	public class TryGetPropertyDefDataType
		: VaultPropertyDefOperationsTestBase
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullReferenceThrows()
		{
			((MFilesAPI.VaultPropertyDefOperations) null).TryGetPropertyDefDataType(1, out _);
		}

		[TestMethod]
		public void KnownPropertyDefReturnsCorrectData()
		{
			// Details about the property.
			int propertyDefId = 1234;
			MFDataType dataType = MFDataType.MFDatatypeText;

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
			Assert.IsTrue(mock.Object.TryGetPropertyDefDataType(propertyDefId, out MFDataType output));
			Assert.AreEqual(dataType, output);
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
			Assert.IsFalse(mock.Object.TryGetPropertyDefDataType(propertyDefId, out MFDataType output));
		}
	}
}
