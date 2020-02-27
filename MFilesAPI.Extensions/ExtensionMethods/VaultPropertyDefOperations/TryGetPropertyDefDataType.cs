using System;

namespace MFilesAPI.Extensions
{
	public static partial class VaultPropertyDefOperationsExtensionMethods
	{
		/// <summary>
		/// Attempts to get the <see cref="PropertyDef.DataType"/> of a <see cref="PropertyDef"/>
		/// with the given <paramref name="propertyDefId"/>.
		/// </summary>
		/// <param name="propertyDefOperations">The <see cref="VaultPropertyDefOperations"/> instance to use to communicate with the vault.</param>
		/// <param name="propertyDefId">The Id of the property definition to attempt to read.</param>
		/// <param name="dataType">The data type of the property definition, or <see cref="MFDataType.MFDatatypeUninitialized"/> if not found.</param>
		/// <returns>true if the property definition could be found, false otherwise.</returns>
		public static bool TryGetPropertyDefDataType
		(
			this VaultPropertyDefOperations propertyDefOperations,
			int propertyDefId,
			out MFDataType dataType
		)
		{
			// Sanity.
			if (null == propertyDefOperations)
				throw new ArgumentNullException(nameof(propertyDefOperations));

			// Default.
			dataType = MFDataType.MFDatatypeUninitialized;

			// Attempt to get from the underlying data.
			try
			{
				dataType = propertyDefOperations
					.GetPropertyDef(propertyDefId)
					.DataType;
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
