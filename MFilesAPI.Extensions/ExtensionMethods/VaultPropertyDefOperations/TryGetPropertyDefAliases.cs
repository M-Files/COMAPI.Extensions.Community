using System;
using System.Linq;

namespace MFilesAPI.Extensions
{
	public static partial class VaultPropertyDefOperationsExtensionMethods
	{
		/// <summary>
		/// Attempts to get the <see cref="PropertyDefAdmin.SemanticAliases"/> of a <see cref="PropertyDefAdmin"/>
		/// with the given <paramref name="propertyDefId"/>.
		/// </summary>
		/// <param name="propertyDefOperations">The <see cref="VaultPropertyDefOperations"/> instance to use to communicate with the vault.</param>
		/// <param name="propertyDefId">The Id of the property definition to attempt to read.</param>
		/// <param name="aliases">The aliases associated with the property definition.</param>
		/// <returns>true if the property definition could be found, false otherwise.</returns>
		public static bool TryGetPropertyDefAliases
		(
			this VaultPropertyDefOperations propertyDefOperations,
			int propertyDefId,
			out string[] aliases
		)
		{
			// Sanity.
			if (null == propertyDefOperations)
				throw new ArgumentNullException(nameof(propertyDefOperations));

			// Attempt to get from the underlying data.
			try
			{
				aliases = propertyDefOperations
					.GetPropertyDefAdmin(propertyDefId)
					.SemanticAliases
					.Value
					.Split(";".ToCharArray())
					.Select(a => a.Trim())
					.ToArray();
				return true;
			}
#pragma warning disable CA1031 // Do not catch general exception types
			catch
			{
				// Default.
#pragma warning disable CA1825 // Avoid zero-length array allocations.
				aliases = new string[0];
#pragma warning restore CA1825 // Avoid zero-length array allocations.
				return false;
			}
#pragma warning restore CA1031 // Do not catch general exception types
		}
	}
}
