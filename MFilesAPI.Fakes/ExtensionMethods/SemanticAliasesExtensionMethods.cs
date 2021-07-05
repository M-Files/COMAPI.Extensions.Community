using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAPI.Fakes.ExtensionMethods
{
	public static class SemanticAliasesExtensionMethods
	{
		/// <summary>
		/// Parses individual aliases from the semantic alias string value.
		/// </summary>
		/// <param name="semanticAliases">The aliases defined on a vault structural element.</param>
		/// <returns>Any and all aliases.</returns>
		public static IEnumerable<string> GetAliasesFromValue(this SemanticAliases semanticAliases)
		{
			// Sanity.
			if (null == semanticAliases?.Value)
				yield break;

			// Return each one.
			foreach (var alias in semanticAliases.Value.Split(";".ToCharArray()))
			{
				yield return alias.Trim();
			}
		}
	}
}
