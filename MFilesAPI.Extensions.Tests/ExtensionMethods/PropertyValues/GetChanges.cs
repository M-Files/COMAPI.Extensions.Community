using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAPI.Extensions.Tests.ExtensionMethods.PropertyValues
{
	[TestClass]
	public class GetChanges
	{
		[TestMethod]
		public void DoesNotThrowWithNullNewVersion()
		{
			var changes = ((MFilesAPI.PropertyValues)null).GetChanges(new MFilesAPI.PropertyValues()).ToList();
			Assert.IsNotNull(changes);
		}

		[TestMethod]
		public void DoesNotThrowWithNullOldVersion()
		{
			var changes = new MFilesAPI.PropertyValues().GetChanges(null).ToList();
			Assert.IsNotNull(changes);
		}

		[TestMethod]
		public void AddedItemDetected()
		{
			var oldCollection = new MFilesAPI.PropertyValues();
			var newCollection = new MFilesAPI.PropertyValues();
			{
				var p = new MFilesAPI.PropertyValue()
				{
					PropertyDef = 123
				};
				p.Value.SetValue(MFDataType.MFDatatypeText, "hello world");
				newCollection.Add(-1, p);
			}

			var changes = newCollection.GetChanges(oldCollection, out bool changed);
			Assert.IsTrue(changed);
			Assert.AreEqual(1, changes.Count());
			Assert.AreEqual(PropertyValueChangeType.Added, changes.ElementAt(0).ChangeType);
			Assert.AreEqual(123, changes.ElementAt(0).PropertyDef);
		}

		[TestMethod]
		public void RemovedItemDetected()
		{
			var oldCollection = new MFilesAPI.PropertyValues();
			{
				var p = new MFilesAPI.PropertyValue()
				{
					PropertyDef = 123
				};
				p.Value.SetValue(MFDataType.MFDatatypeText, "hello world");
				oldCollection.Add(-1, p);
			}
			var newCollection = new MFilesAPI.PropertyValues();

			var changes = newCollection.GetChanges(oldCollection, out bool changed);
			Assert.IsTrue(changed);
			Assert.AreEqual(1, changes.Count());
			Assert.AreEqual(PropertyValueChangeType.Removed, changes.ElementAt(0).ChangeType);
			Assert.AreEqual(123, changes.ElementAt(0).PropertyDef);
		}

		[TestMethod]
		public void ChangedItemDetected()
		{
			var oldCollection = new MFilesAPI.PropertyValues();
			{
				var p = new MFilesAPI.PropertyValue()
				{
					PropertyDef = 123
				};
				p.Value.SetValue(MFDataType.MFDatatypeText, "hello world");
				oldCollection.Add(-1, p);
			}
			var newCollection = new MFilesAPI.PropertyValues();
			{
				var p = new MFilesAPI.PropertyValue()
				{
					PropertyDef = 123
				};
				p.Value.SetValue(MFDataType.MFDatatypeText, "hello new world");
				newCollection.Add(-1, p);
			}

			var changes = newCollection.GetChanges(oldCollection, out bool changed);
			Assert.IsTrue(changed);
			Assert.AreEqual(1, changes.Count());
			Assert.AreEqual(PropertyValueChangeType.Modified, changes.ElementAt(0).ChangeType);
			Assert.AreEqual(123, changes.ElementAt(0).PropertyDef);
		}

		[TestMethod]
		public void IncludeItemsThatHaveNotChanged_False()
		{
			var oldCollection = new MFilesAPI.PropertyValues();
			{
				var p = new MFilesAPI.PropertyValue()
				{
					PropertyDef = 123
				};
				p.Value.SetValue(MFDataType.MFDatatypeText, "hello world");
				oldCollection.Add(-1, p);
			}
			var newCollection = new MFilesAPI.PropertyValues();
			{
				var p = new MFilesAPI.PropertyValue()
				{
					PropertyDef = 123
				};
				p.Value.SetValue(MFDataType.MFDatatypeText, "hello world");
				newCollection.Add(-1, p);
			}

			var changes = newCollection.GetChanges(oldCollection, out bool changed, includePropertiesThatHaveNotChanged: false);
			Assert.IsFalse(changed);
			Assert.AreEqual(0, changes.Count());
		}

		[TestMethod]
		public void IncludeItemsThatHaveNotChanged_True()
		{
			var oldCollection = new MFilesAPI.PropertyValues();
			{
				var p = new MFilesAPI.PropertyValue()
				{
					PropertyDef = 123
				};
				p.Value.SetValue(MFDataType.MFDatatypeText, "hello world");
				oldCollection.Add(-1, p);
			}
			var newCollection = new MFilesAPI.PropertyValues();
			{
				var p = new MFilesAPI.PropertyValue()
				{
					PropertyDef = 123
				};
				p.Value.SetValue(MFDataType.MFDatatypeText, "hello world");
				newCollection.Add(-1, p);
			}

			var returned = newCollection.GetChanges(oldCollection, out bool changed, includePropertiesThatHaveNotChanged: true);
			Assert.IsFalse(changed); // The collection has not changed, even though items are returned.
			Assert.AreEqual(1, returned.Count());
			Assert.AreEqual(PropertyValueChangeType.None, returned.ElementAt(0).ChangeType);
			Assert.AreEqual(123, returned.ElementAt(0).PropertyDef);
		}


	}
}
