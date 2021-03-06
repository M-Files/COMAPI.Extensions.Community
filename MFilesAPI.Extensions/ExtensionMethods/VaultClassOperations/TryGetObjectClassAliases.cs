﻿using System;
using System.Linq;

namespace MFilesAPI.Extensions
{
	public static partial class VaultClassOperationsExtensionMethods
	{
		/// <summary>
		/// Attempts to get the <see cref="ObjectClassAdmin.SemanticAliases"/> of a <see cref="ObjectClassAdmin"/>
		/// with the given <paramref name="classId"/>.
		/// </summary>
		/// <param name="classOperations">The <see cref="VaultClassOperations"/> instance to use to communicate with the vault.</param>
		/// <param name="classId">The Id of the class to attempt to read.</param>
		/// <param name="aliases">The aliases associated with the class.</param>
		/// <returns>true if the class could be found, false otherwise.</returns>
		public static bool TryGetObjectClassAliases
		(
			this VaultClassOperations classOperations,
			int classId,
			out string[] aliases
		)
		{
			// Sanity.
			if (null == classOperations)
				throw new ArgumentNullException(nameof(classOperations));

			// Attempt to get from the underlying data.
			try
			{
				aliases = classOperations
					.GetObjectClassAdmin(classId)
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
