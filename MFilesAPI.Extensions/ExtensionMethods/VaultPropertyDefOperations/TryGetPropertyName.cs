using System;

namespace MFilesAPI.Extensions
{
	public static partial class VaultPropertyDefOperationsExtensionMethods
	{
		/// <summary>
		/// Attempts to get the name of a <see cref="PropertyDef"/>
		/// with the given <paramref name="propertyDefId"/>.
		/// </summary>
		/// <param name="propertyDefOperations">The <see cref="VaultPropertyDefOperations"/> instance to use to communicate with the vault.</param>
		/// <param name="propertyDefId">The Id of the property definition to attempt to read.</param>
		/// <param name="propertyDefName">The name of the property definition, or null if not found.</param>
		/// <returns>true if the property definition could be found, false otherwise.</returns>
		public static bool TryGetPropertyDefName
		(
			this VaultPropertyDefOperations propertyDefOperations,
			int propertyDefId,
			out string propertyDefName
		)
		{
			// Sanity.
			if (null == propertyDefOperations)
				throw new ArgumentNullException(nameof(propertyDefOperations));

			// Default.
			propertyDefName = null;

			// Attempt to get from the underlying data.
			try
			{
				propertyDefName = propertyDefOperations
					.GetPropertyDef(propertyDefId)
					.Name;
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
