using MFilesAPI.Fakes.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace MFilesAPI.Fakes.ExtensionMethods
{
	public static class JTokenExtensionMethods
	{
		public static Guid AsGuid(this JToken input, bool throwExceptionIfCannotParse = true)
		{
			// Sanity.
			if (null == input)
			{
				if (throwExceptionIfCannotParse)
					throw new ArgumentNullException(nameof(input));
				return default;
			}
			if (input.Type != JTokenType.String)
				if (throwExceptionIfCannotParse)
					throw new InvalidJsonException($"GUID value was not a string", input);
				else
					return default;
			if (Guid.TryParse(input.ToString(), out Guid g))
				return g;
			else
				if (throwExceptionIfCannotParse)
				throw new InvalidJsonException($"Could not parse {input} into a GUID.");
			else
				return default;
		}
		public static int AsInteger(this JToken input, bool throwExceptionIfCannotParse = true)
		{
			// Sanity.
			if (null == input)
			{
				if (throwExceptionIfCannotParse)
					throw new ArgumentNullException(nameof(input));
				return default;
			}
			if (input.Type != JTokenType.Integer)
				if (throwExceptionIfCannotParse)
					throw new InvalidJsonException($"Value was not an integer", input);
				else
					return default;
			if (int.TryParse(input.ToString(), out int i))
				return i;
			else
				if (throwExceptionIfCannotParse)
				throw new InvalidJsonException($"Could not parse {input} into an integer.");
			else
				return default;
		}
		public static bool AsBoolean(this JToken input, bool throwExceptionIfCannotParse = true)
		{
			// Sanity.
			if (null == input)
			{
				if (throwExceptionIfCannotParse)
					throw new ArgumentNullException(nameof(input));
				return default;
			}
			if (input.Type != JTokenType.Boolean)
				if (throwExceptionIfCannotParse)
					throw new InvalidJsonException($"Value was not a boolean", input);
				else
					return default;
			if (bool.TryParse(input.ToString(), out bool b))
				return b;
			else
				if (throwExceptionIfCannotParse)
				throw new InvalidJsonException($"Could not parse {input} into a boolean.");
			else
				return default;
		}
		public static SemanticAliases AsSemanticAliases(this JToken input, bool throwExceptionIfCannotParse = true)
		{
			// Sanity.
			if (null == input)
			{
				if (throwExceptionIfCannotParse)
					throw new ArgumentNullException(nameof(input));
				return default;
			}
			if (input.Type != JTokenType.Array)
				throw new InvalidJsonException($"Aliases were not an array", input);
			return new SemanticAliases()
			{
				Value = string.Join(";", (input as JArray).Select(v => v.Value<string>()))
			};
		}
	}
}
