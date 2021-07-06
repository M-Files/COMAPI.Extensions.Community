using MFilesAPI.Fakes.ExtensionMethods;
using Newtonsoft.Json.Linq;
using System;

namespace MFilesAPI.Fakes.Serialization
{
	public interface IJsonSerializer
	{
		Vault Deserialize(JToken input);
		JToken Serialize(Vault input);
	}
	public abstract class JsonSerializerBase
		: IJsonSerializer
	{
		public abstract Vault Deserialize(JToken input);
		public abstract VaultObjectTypeOperations DeserializeVaultObjectTypeOperations(JToken input);

		public abstract JToken Serialize(Vault input);

		public abstract JToken Serialize(VaultObjectTypeOperations input);
	}
}
