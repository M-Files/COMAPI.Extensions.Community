using System;

namespace MFilesAPI.Extensions
{
	public static partial class VaultObjectTypeOperationsExtensionMethods
	{
		/// <summary>
		/// Gets the "owner" property definition ID of the supplied <paramref name="objectTypeId"/>.
		/// </summary>
		/// <param name="objectTypeOperations">The <see cref="VaultObjectTypeOperations"/> instance to use to communicate with the vault.</param>
		/// <param name="propertyDefId">The Id of the property definition to attempt to read.</param>
		/// <param name="propertyDefName">The name of the property definition, or null if not found.</param>
		/// <returns>true if the property definition could be found, false otherwise.</returns>
		public static bool TryGetOwnerPropertyDefID
		(
			this VaultObjectTypeOperations objectTypeOperations,
			int objectTypeId,
			out int ownerPropertyDefID
		)
		{
			// Sanity.
			if (null == objectTypeOperations)
				throw new ArgumentNullException(nameof(objectTypeOperations));

			// Return.
			ownerPropertyDefID = -1;
			try
			{
				ownerPropertyDefID = objectTypeOperations.GetObjectType(objectTypeId)?.OwnerPropertyDef ?? -1;
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
