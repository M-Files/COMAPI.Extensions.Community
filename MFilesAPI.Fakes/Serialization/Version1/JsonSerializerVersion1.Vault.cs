using MFilesAPI.Fakes.ExtensionMethods;
using Newtonsoft.Json.Linq;
using System;

namespace MFilesAPI.Fakes.Serialization.Version1
{
	public partial class JsonSerializer
	{
		public override MFilesAPI.Vault Deserialize(JToken input)
		{
			// Create the vault.
			var vault = this.FakeFactory?.Instantiate<IVaultEx>()
				?? new Vault();

			// Cannot populate from a null reference.
			if (null == input)
				return vault;
			if (false == (input is JObject))
				throw new ArgumentException("Vault can only be deserialised from a JObject", nameof(input));
			var jObject = input as JObject;

			// Populate properties.
			foreach (var p in jObject.Properties())
			{
				if (null == p)
					continue;
				if (null == p.Value)
					continue;
				switch (p.Name?.ToLower()?.Trim())
				{
					case "guid":
						vault.Guid = p.Value.AsGuid();
						break;
					case "name":
						vault.Name = p.Value?.ToString();
						break;
					case "objecttypes":
						vault.ObjectTypeOperations = this.DeserializeVaultObjectTypeOperations(p.Value);
						break;
						//case "classes":
						//	vault.ClassOperations?.PopulateFromJToken(p.Value);
						//	break;
						//case "propertydefinitions":
						//	vault.PropertyDefOperations?.PopulateFromJToken(p.Value);
						//	break;
				}
			}

			return vault;
		}

		public override JToken Serialize(Vault input)
		{
			if (null == input) { return new JObject(); }
			return new JObject
			{
				new JProperty("name", input.Name),
				new JProperty("guid", input.Guid.ToString("B")),
				new JProperty("objectTypes", this.Serialize(input.ObjectTypeOperations)),
				//new JProperty("propertyDefinitions", input.PropertyDefOperations?.Serialize()),
				//new JProperty("classes", input.ClassOperations?.Serialize())
			};
		}
	}
}
