using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAPI.Extensions.Tests.ExtensionMethods.PropertyValues
{
	[TestClass]
	public class Exists
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsWithNullPropertyValues()
		{
			((MFilesAPI.PropertyValues)null).Exists(1);
		}

		[TestMethod]
		public void ReturnsFalseIfDoesNotExist()
		{
			Assert.IsFalse(new MFilesAPI.PropertyValues().Exists(123));
		}

		[TestMethod]
		public void ReturnsTrueIfExists()
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
			
			Assert.IsTrue(propertyValues.Exists(123));
		}
	}
}
