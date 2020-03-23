using System;

namespace MFilesAPI.Extensions
{
	public static partial class PropertyDefOrObjectTypesExtensionMethods
	{
		/// <summary>
		/// Adds an indirection level to the collection that represents
		/// a relationship using a specific property Id.
		/// </summary>
		/// <param name="indirectionLevels">The collection to add to.</param>
		/// <param name="propertyId">The Id of the property.</param>
		/// <returns>The <paramref name="indirectionLevels"/>, for chaining.</returns>
		public static PropertyDefOrObjectTypes AddPropertyDefIndirectionLevel
		(
			this PropertyDefOrObjectTypes indirectionLevels,
			int propertyId
		)
		{
			// Sanity.
			if (null == indirectionLevels)
				throw new ArgumentNullException(nameof(indirectionLevels));

			// Ensure that the property is >= 0.
			if (propertyId < 0)
				throw new ArgumentOutOfRangeException(nameof(propertyId));

			// Add the property definition indirection level.
			indirectionLevels.Add(-1, new PropertyDefOrObjectType()
			{
				ID = propertyId,
				PropertyDef = true
			});

			// Return the indirection levels for chaining.
			return indirectionLevels;
		}
	}
}
