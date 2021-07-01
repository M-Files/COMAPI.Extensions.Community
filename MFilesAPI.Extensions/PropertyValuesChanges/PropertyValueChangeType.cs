using System;

namespace MFilesAPI.Extensions
{
	/// <summary>
	/// Indicates the type of property change between versions of an object.
	/// </summary>
	public enum PropertyValueChangeType
	{
		/// <summary>
		/// No property change.
		/// </summary>
		None = 0,

		/// <summary>
		/// The property value was added.
		/// </summary>
		Added,

		/// <summary>
		/// The property value was modified.
		/// </summary>
		Modified,

		/// <summary>
		/// The property value was removed.
		/// </summary>
		Removed
	}

	/// <summary>
	/// Defines a changed property value between object versions.
	/// </summary>
	public class PropertyValueChange
	{
		/// <summary>
		/// The previous version's property value (if there was one).
		/// </summary>
		public PropertyValue OldValue { get; private set; }

		/// <summary>
		/// The current version's property value (if there is one).
		/// </summary>
		public PropertyValue NewValue { get; private set; }

		/// <summary>
		/// The PropertyDef ID of the property Value(s).
		/// </summary>
		public int PropertyDef => this.NewValue != null
			? NewValue.PropertyDef
			: OldValue.PropertyDef;

		/// <summary>
		/// The data type of the propertyValue(s)
		/// </summary>
		public MFDataType DataType => this.NewValue != null
			? NewValue.Value.DataType
			: OldValue.Value.DataType;

		/// <summary>
		/// The type of property change between versions.
		/// </summary>
		public PropertyValueChangeType ChangeType
		{
			get
			{
				// If there was no previous property, it was added.
				if (this.OldValue == null)
					return PropertyValueChangeType.Added;

				// If there is no current property, it's been removed.
				if (this.NewValue == null)
					return PropertyValueChangeType.Removed;

				// If the values don't match, it's been modifed. 
				if (this.OldValue.Value.CompareTo(this.NewValue.Value) != 0)
					return PropertyValueChangeType.Modified;

				// If we get here, nothing has changed.
				return PropertyValueChangeType.None;
			}

		}

		/// <summary>
		/// Constructor. Creates a new instance of this class.
		/// </summary>
		/// <param name="oldValue">The previous property value, if any.</param>
		/// <param name="newValue">The current value, if any.</param>
		public PropertyValueChange(PropertyValue oldValue, PropertyValue newValue)
		{
			if (oldValue == null && newValue == null)
				throw new ArgumentException("Cannot pass null values for both old and new values.");

			this.OldValue = oldValue;
			this.NewValue = newValue;
		}

	}
}
