using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAPI.Extensions.Tests.ExtensionMethods.PropertyValues
{
	[TestClass]
	public class GetProperty
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsWithNullPropertyValues()
		{
			((MFilesAPI.PropertyValues)null).GetProperty(1);
		}

		[TestMethod]
		public void ReturnsNullWithNegativeProperty()
		{
			Assert.IsNull(new MFilesAPI.PropertyValues().GetProperty(-1));
		}

		[TestMethod]
		public void ReturnsNullIfValueDoesNotExist()
		{
			Assert.IsNull(new MFilesAPI.PropertyValues().GetProperty(123));
		}

		[TestMethod]
		public void ReturnsValueIfExists()
		{
			var propertyValues = new MFilesAPI.PropertyValues();
			{
				var p = new MFilesAPI.PropertyValue()
				{
					PropertyDef = 123
				};
				p.Value.SetValue(MFDataType.MFDatatypeText, "hello world");
				propertyValues.Add(-1, p);
			}
			
			var value = propertyValues.GetProperty(123);
			Assert.IsNotNull(value);
			Assert.AreEqual(123, value.PropertyDef);
			Assert.AreEqual(MFDataType.MFDatatypeText, value.Value.DataType);
			Assert.AreEqual("hello world", value.Value.Value);
		}
	}
}
