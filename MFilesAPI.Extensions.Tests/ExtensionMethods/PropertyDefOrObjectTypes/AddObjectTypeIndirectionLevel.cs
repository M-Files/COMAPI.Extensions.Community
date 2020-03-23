using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MFilesAPI.Extensions.Tests.ExtensionMethods.PropertyDefOrObjectTypes
{
	[TestClass]
	public class AddObjectTypeIndirectionLevel
	{
		/// <summary>
		/// Ensures that a null <see cref="MFilesAPI.PropertyDefOrObjectTypes"/> reference throws an <see cref="ArgumentNullException"/>.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddObjectTypeIndirectionLevel_ThrowsIfNullPropertyDefOrObjectTypes()
		{
			((MFilesAPI.PropertyDefOrObjectTypes) null).AddObjectTypeIndirectionLevel(123);
		}
		
		/// <summary>
		/// Ensures that an object type ID under zero throws an <see cref="ArgumentOutOfRangeException"/>.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void AddObjectTypeIndirectionLevel_ThrowsIfNullIdIsLessThanZero()
		{
			new MFilesAPI.PropertyDefOrObjectTypes().AddObjectTypeIndirectionLevel(-1);
		}

		/// <summary>
		/// Ensures that the call to <see cref="MFilesAPI.Extensions.PropertyDefOrObjectTypesExtensionMethods.AddObjectTypeIndirectionLevel"/> adds an item to the <see cref="MFilesAPI.PropertyDefOrObjectTypes"/> collection.
		/// </summary>
		[TestMethod]
		public void AddObjectTypeIndirectionLevel_AddsToCollection()
		{
			// Create the collection and ensure it's blank.
			var collection = new MFilesAPI.PropertyDefOrObjectTypes();
			Assert.AreEqual(0, collection.Count);

			// Add the item.
			collection.AddObjectTypeIndirectionLevel(123);

			// Ensure the collection increased in size.
			Assert.AreEqual(1, collection.Count);
		}
		
		/// <summary>
		/// Ensures that the call to <see cref="MFilesAPI.Extensions.PropertyDefOrObjectTypesExtensionMethods.AddObjectTypeIndirectionLevel"/> adds a correctly-configured item to the collection.
		/// </summary>
		[TestMethod]
		public void AddObjectTypeIndirectionLevel_ValueCorrect()
		{
			// Create the collection and ensure it's blank.
			var collection = new MFilesAPI.PropertyDefOrObjectTypes();
			Assert.AreEqual(0, collection.Count);

			// Add the item.
			collection.AddObjectTypeIndirectionLevel(1234);

			// Check the added item.
			var item = collection[1];
			Assert.IsNotNull(item);
			Assert.AreEqual(1234, item.ID);
			Assert.IsFalse(item.PropertyDef);
		}

	}
}
