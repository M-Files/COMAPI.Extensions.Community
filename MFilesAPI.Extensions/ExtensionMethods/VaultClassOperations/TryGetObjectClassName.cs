using System;

namespace MFilesAPI.Extensions
{
	public static partial class VaultClassOperationsExtensionMethods
	{
		/// <summary>
		/// Attempts to get the name of a <see cref="ObjectClass"/>
		/// with the given <paramref name="classId"/>.
		/// </summary>
		/// <param name="classOperations">The <see cref="VaultClassOperations"/> instance to use to communicate with the vault.</param>
		/// <param name="classId">The Id of the class to attempt to read.</param>
		/// <param name="className">The name of the class, or null if not found.</param>
		/// <returns>true if the class could be found, false otherwise.</returns>
		public static bool TryGetObjectClassName
		(
			this VaultClassOperations classOperations,
			int classId,
			out string className
		)
		{
			// Sanity.
			if (null == classOperations)
				throw new ArgumentNullException(nameof(classOperations));

			// Default.
			className = null;

			// Attempt to get from the underlying data.
			try
			{
				className = classOperations
					.GetObjectClass(classId)
					.Name;
				return true;
			}
#pragma warning disable CA1031 // Do not catch general exception types
			catch
			{
				return false;
			}
#pragma warning restore CA1031 // Do not catch general exception types
		}
	}
}
