using System;

namespace MFilesAPI.Extensions
{
	public static partial class PropertyDefOrObjectTypesExtensionMethods
	{
		/// <summary>
		/// Adds an indirection level to the collection that represents
		/// a relationship using any/all properties of a given type.
		/// </summary>
		/// <param name="indirectionLevels">The collection to add to.</param>
		/// <param name="objectTypeId">The Id of the object type.</param>
		/// <returns>The <paramref name="indirectionLevels"/>, for chaining.</returns>
		public static PropertyDefOrObjectTypes AddObjectTypeIndirectionLevel
		(
			this PropertyDefOrObjectTypes indirectionLevels,
			int objectTypeId
		)
		{
			// Sanity.
			if (null == indirectionLevels)
				throw new ArgumentNullException(nameof(indirectionLevels));

			// Ensure that the object type id is >= 0.
			if (objectTypeId < 0)
				throw new ArgumentOutOfRangeException(nameof(objectTypeId));

			// Add the object type indirection level.
			indirectionLevels.Add(-1, new PropertyDefOrObjectType()
			{
				ID = objectTypeId,
				PropertyDef = false
			});

			// Return the indirection levels for chaining.
			return indirectionLevels;
		}
	}
}
