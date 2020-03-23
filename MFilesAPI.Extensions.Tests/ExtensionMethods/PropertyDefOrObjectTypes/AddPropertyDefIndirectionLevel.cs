using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MFilesAPI.Extensions.Tests.ExtensionMethods.PropertyDefOrObjectTypes
{
	[TestClass]
	public class AddPropertyDefIndirectionLevel
	{
		/// <summary>
		/// Ensures that a null <see cref="MFilesAPI.PropertyDefOrObjectTypes"/> reference throws an <see cref="ArgumentNullException"/>.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddPropertyDefIndirectionLevel_ThrowsIfNullPropertyDefOrObjectTypes()
		{
			((MFilesAPI.PropertyDefOrObjectTypes) null).AddPropertyDefIndirectionLevel(123);
		}
		
		/// <summary>
		/// Ensures that a property ID under zero throws an <see cref="ArgumentOutOfRangeException"/>.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void AddPropertyDefIndirectionLevel_ThrowsIfNullPropertyDefIsLessThanZero()
		{
			new MFilesAPI.PropertyDefOrObjectTypes().AddPropertyDefIndirectionLevel(-1);
		}

		/// <summary>
		/// Ensures that the call to <see cref="MFilesAPI.Extensions.PropertyDefOrObjectTypesExtensionMethods.AddPropertyDefIndirectionLevel"/> adds an item to the <see cref="MFilesAPI.PropertyDefOrObjectTypes"/> collection.
		/// </summary>
		[TestMethod]
		public void AddPropertyDefIndirectionLevel_AddsToCollection()
		{
			// Create the collection and ensure it's blank.
			var collection = new MFilesAPI.PropertyDefOrObjectTypes();
			Assert.AreEqual(0, collection.Count);

			// Add the item.
			collection.AddPropertyDefIndirectionLevel(123);

			// Ensure the collection increased in size.
			Assert.AreEqual(1, collection.Count);
		}
		
		/// <summary>
		/// Ensures that the call to <see cref="MFilesAPI.Extensions.PropertyDefOrObjectTypesExtensionMethods.AddPropertyDefIndirectionLevel"/> adds a correctly-configured item to the collection.
		/// </summary>
		[TestMethod]
		public void AddPropertyDefIndirectionLevel_ValueCorrect()
		{
			// Create the collection and ensure it's blank.
			var collection = new MFilesAPI.PropertyDefOrObjectTypes();
			Assert.AreEqual(0, collection.Count);

			// Add the item.
			collection.AddPropertyDefIndirectionLevel(123);

			// Check the added item.
			var item = collection[1];
			Assert.IsNotNull(item);
			Assert.AreEqual(123, item.ID);
			Assert.IsTrue(item.PropertyDef);
		}

	}
}
