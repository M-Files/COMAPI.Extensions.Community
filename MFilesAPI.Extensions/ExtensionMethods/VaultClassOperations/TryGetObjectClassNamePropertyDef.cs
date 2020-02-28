using System;

namespace MFilesAPI.Extensions
{
	public static partial class VaultClassOperationsExtensionMethods
	{
		/// <summary>
		/// Attempts to get the ID of the <see cref="PropertyDef"/> that is defined as
		/// the "Name or Title" property of a <see cref="ObjectClass"/>
		/// with the given <paramref name="classId"/>.
		/// </summary>
		/// <param name="classOperations">The <see cref="VaultClassOperations"/> instance to use to communicate with the vault.</param>
		/// <param name="classId">The Id of the class to attempt to read.</param>
		/// <param name="nameOrTitlePropertyDef">The property of the "Name or Title" property for this class, or <see cref="MFBuiltInPropertyDef.MFBuiltInPropertyDefNameOrTitle"/> if not set.</param>
		/// <returns>true if the class could be found, false otherwise.</returns>
		public static bool TryGetObjectClassNamePropertyDef
		(
			this VaultClassOperations classOperations,
			int classId,
			out int nameOrTitlePropertyDef
		)
		{
			// Sanity.
			if (null == classOperations)
				throw new ArgumentNullException(nameof(classOperations));

			// Default.
			nameOrTitlePropertyDef = (int)MFBuiltInPropertyDef.MFBuiltInPropertyDefNameOrTitle;

			// Attempt to get from the underlying data.
			try
			{
				nameOrTitlePropertyDef = classOperations
					.GetObjectClass(classId)
					.NamePropertyDef;
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
