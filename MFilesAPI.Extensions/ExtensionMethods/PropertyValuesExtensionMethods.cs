using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAPI.Extensions
{
	public static class PropertyValuesExtensionMethods
	{
		/// <summary>
		/// Returns the specified object property if found.
		/// </summary>
		/// <param name="props">The property values collection.</param>
		/// <param name="propID">The PropertyDef id of the property to look for.</param>/// <returns>Returns null if not found.</returns>
		public static PropertyValue GetProperty(this PropertyValues props, int propID)
		{
			// Sanity.
			if (null == props)
				throw new ArgumentNullException(nameof(props));

			// Return the item at the correct index.
			int i = props.IndexOf(propID);
			return (i < 0)
				? null // Not found.
				: props[i];
		}
		
		/// <summary>
		/// Checks whether an object has a specific property.
		/// </summary>
		/// <param name="props">The property values collection.</param>
		/// <param name="propID">The PropertyDef ID.</param>
		/// <returns>Returns true if the property was found.</returns>
		public static bool Exists(this PropertyValues props, int propID)
		{
			// Sanity.
			if (null == props)
				throw new ArgumentNullException(nameof(props));

			// Seek the property value and return the result.
			return props.IndexOf(propID) != -1;
		}

		/// <summary>
		/// Retrieves the changes between <paramref name="oldVersion"/> and <paramref name="newVersion"/>.
		/// <paramref name="hasChanged"/> will be true if the return value has any entries, false otherwise.
		/// </summary>
		/// <param name="newVersion">The properties associated with the new version of the object.</param>
		/// <param name="oldVersion">The properties associated with the old version of the object.</param>
		/// <param name="hasChanged">Whether there are any changes.</param>
		/// <param name="includePropertiesThatHaveNotChanged">If false, the returned collection will have unchanged items removed.</param>
		/// <returns>The changes.</returns>
		public static IEnumerable<PropertyValueChange> GetChanges
		(
			this PropertyValues newVersion,
			PropertyValues oldVersion,
			out bool hasChanged,
			bool includePropertiesThatHaveNotChanged = false
		)
		{
			// Sanity.
			newVersion = newVersion ?? new PropertyValues();
			oldVersion = oldVersion ?? new PropertyValues();

			// Will contain all properties, not just changes.
			var allProps = new List<PropertyValueChange>();

			// Loop over new properties and find new or changed values.
			foreach (PropertyValue newPropVal in newVersion)
			{
				allProps.Add
				(
					new PropertyValueChange
					(
						oldVersion?.GetProperty(newPropVal.PropertyDef), 
						newPropVal
					)
				);
			}

			// Loop over old properties to find removed values.
			if (oldVersion != null)
			{
				foreach (PropertyValue oldPropVal in oldVersion)
				{
					if (!newVersion.Exists(oldPropVal.PropertyDef))
					{
						allProps.Add(new PropertyValueChange(oldPropVal, null));
					}
				}
			}

			// Ignore anything that has not changed.
			var changedProperties = allProps.Where(p => p.ChangeType != PropertyValueChangeType.None);

			// Set the out variable.
			hasChanged = changedProperties.Any();

			// Return the changes.
			return includePropertiesThatHaveNotChanged
				? allProps
				: changedProperties;
		}

		/// <summary>
		/// Retrieves the changes between <paramref name="oldVersion"/> and <paramref name="newVersion"/>.
		/// </summary>
		/// <param name="newVersion">The properties associated with the new version of the object.</param>
		/// <param name="oldVersion">The properties associated with the old version of the object.</param>
		/// <param name="includePropertiesThatHaveNotChanged">If false, the returned collection will have unchanged items removed.</param>
		/// <returns>The changes.</returns>
		public static IEnumerable<PropertyValueChange> GetChanges
		(
			this PropertyValues newVersion,
			PropertyValues oldVersion,
			bool includePropertiesThatHaveNotChanged = false
		)
		{
			// Use the other overload and throw away the out value.
			return newVersion.GetChanges(oldVersion, out _, includePropertiesThatHaveNotChanged);
		}
	}
}
