using System;

namespace MFilesAPI.Extensions
{
	public static partial class VaultClassOperationsExtensionMethods
	{
		/// <summary>
		/// Attempts to get the workflow details (id and whether it's forced) of a <see cref="ObjectClass"/>
		/// with the given <paramref name="classId"/>.
		/// </summary>
		/// <param name="classOperations">The <see cref="VaultClassOperations"/> instance to use to communicate with the vault.</param>
		/// <param name="classId">The Id of the class to attempt to read.</param>
		/// <param name="workflowId">The default workflow of the class, or zero if there is no default workflow.</param>
		/// <param name="forced">Whether the use of the specified workflow is forced.</param>
		/// <returns>true if the class could be found, false otherwise.</returns>
		public static bool TryGetObjectClassWorkflowDetails
		(
			this VaultClassOperations classOperations,
			int classId,
			out int workflowId,
			out bool forced
		)
		{
			// Sanity.
			if (null == classOperations)
				throw new ArgumentNullException(nameof(classOperations));

			// Default.
			workflowId = 0;
			forced = false;

			// Attempt to get from the underlying data.
			try
			{
				var oc = classOperations
					.GetObjectClass(classId);
				workflowId = oc.Workflow;
				forced = oc.ForceWorkflow;
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
