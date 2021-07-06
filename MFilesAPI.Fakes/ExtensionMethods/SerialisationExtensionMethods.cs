using MFilesAPI.Fakes.Serialization;
using Newtonsoft.Json.Linq;

namespace MFilesAPI.Fakes
{
	internal static class SerialisationExtensionMethods
	{
		public static void PopulateFromJToken(this MFilesAPI.VaultObjectTypeOperations input, JToken data)
		{
			// Does it support population?
			var supportsPopulation = input as IDeserializableFromJson;
			if (null == supportsPopulation || null == data)
				return;

			// Use the method.
			supportsPopulation.PopulateFromJToken(data);
		}
		public static void PopulateFromJToken(this MFilesAPI.VaultClassOperations input, JToken data)
		{
			// Does it support population?
			var supportsPopulation = input as IDeserializableFromJson;
			if (null == supportsPopulation || null == data)
				return;

			// Use the method.
			supportsPopulation.PopulateFromJToken(data);
		}
		public static void PopulateFromJToken(this MFilesAPI.VaultPropertyDefOperations input, JToken data)
		{
			// Does it support population?
			var supportsPopulation = input as IDeserializableFromJson;
			if (null == supportsPopulation || null == data)
				return;

			// Use the method.
			supportsPopulation.PopulateFromJToken(data);
		}
		public static JToken ToJToken(this MFilesAPI.VaultObjectTypeOperations input)
		{
			// Does it support serialisation?
			var supportsSerialization = input as ISerializableToJson;
			if (null == supportsSerialization)
				return null;

			// Use the method.
			return supportsSerialization.ToJToken();
		}
		public static JToken ToJToken(this MFilesAPI.VaultClassOperations input)
		{
			// Does it support serialisation?
			var supportsSerialization = input as ISerializableToJson;
			if (null == supportsSerialization)
				return null;

			// Use the method.
			return supportsSerialization.ToJToken();
		}
		public static JToken ToJToken(this MFilesAPI.VaultPropertyDefOperations input)
		{
			// Does it support serialisation?
			var supportsSerialization = input as ISerializableToJson;
			if (null == supportsSerialization)
				return null;

			// Use the method.
			return supportsSerialization.ToJToken();
		}
	}
}
