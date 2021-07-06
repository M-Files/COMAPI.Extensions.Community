using MFilesAPI.Fakes.ExtensionMethods;
using Newtonsoft.Json.Linq;
using System;

namespace MFilesAPI.Fakes.Serialization
{
	public interface IJsonSerializer
	{
		FakeFactory FakeFactory { get; set; }
		MFilesAPI.Vault Deserialize(JToken input);
		JToken Serialize(Vault input);
	}
	public abstract class JsonSerializerBase
		: IJsonSerializer
	{
		public FakeFactory FakeFactory { get; set; } = FakeFactory.Default;

		public abstract MFilesAPI.Vault Deserialize(JToken input);
		public abstract MFilesAPI.VaultObjectTypeOperations DeserializeVaultObjectTypeOperations(JToken input);

		public abstract JToken Serialize(Vault input);

		public abstract JToken Serialize(MFilesAPI.VaultObjectTypeOperations input);
	}
}
