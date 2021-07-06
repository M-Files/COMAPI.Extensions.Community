using MFilesAPI.Fakes.Exceptions;
using MFilesAPI.Fakes.ExtensionMethods;
using Newtonsoft.Json.Linq;
using System;

namespace MFilesAPI.Fakes.Serialization.Version1
{
	public partial class JsonSerializer
	{
		public override VaultObjectTypeOperations DeserializeVaultObjectTypeOperations(JToken input)
		{
			// Create the vaultObjectTypeOperations.
			var vaultObjectTypeOperations = this.FakeFactory?.Instantiate<VaultObjectTypeOperations>()
				?? new VaultObjectTypeOperations();

			// Cannot populate from a null reference.
			if (null == input)
				return vaultObjectTypeOperations;
			if (false == (input is JArray))
				throw new ArgumentException("VaultObjectTypeOperations can only be deserialised from a JArray", nameof(input));
			var jArray = input as JArray;

			// Add each item in turn.
			foreach (var item in jArray)
			{
				if (item.Type != JTokenType.Object)
					continue;
				var jObject = item as JObject;
				var objectTypeAdmin = new ObjTypeAdmin();
				foreach (var p in jObject.Properties())
				{
					if (null == p.Value)
						continue;
					switch (p.Name?.ToLower()?.Trim())
					{
						case "id":
							objectTypeAdmin.ObjectType.ID = p.Value.AsInteger();
							break;
						//case "guid":
						//	//TODO: Cannot be assigned - would need to implement it myself.
						//	objectTypeAdmin.ObjectType.GUID = p.Value.AsGuid();
						//	break;
						case "aliases":
							objectTypeAdmin.SemanticAliases = p.Value.AsSemanticAliases();
							break;
						case "name":
							{
								if (p.Value.Type != JTokenType.Object)
									throw new InvalidJsonException("Object type name was an object", p.Value);
								var name = p.Value as JObject;
								objectTypeAdmin.ObjectType.NameSingular = name["singular"].ToString();
								objectTypeAdmin.ObjectType.NamePlural = name["plural"].ToString();
							}
							break;
						case "propertydefinitions":
							{
								if (p.Value.Type != JTokenType.Object)
									throw new InvalidJsonException("Object type property definitions was an object", p.Value);
								var name = p.Value as JObject;
								objectTypeAdmin.ObjectType.OwnerPropertyDef = name["owner"].AsInteger();
								objectTypeAdmin.ObjectType.DefaultPropertyDef = name["default"].AsInteger();
							}
							break;
						case "real":
							objectTypeAdmin.ObjectType.RealObjectType = p.Value.AsBoolean();
							break;
						case "canHaveFiles":
							objectTypeAdmin.ObjectType.CanHaveFiles = p.Value.AsBoolean();
							break;
						case "allowAdding":
							objectTypeAdmin.ObjectType.AllowAdding = p.Value.AsBoolean();
							break;
						case "owner":
							{
								if (p.Value.Type != JTokenType.Object)
									throw new InvalidJsonException("Object type owner was an object", p.Value);
								var name = p.Value as JObject;
								objectTypeAdmin.ObjectType.HasOwnerType = name["hasOwner"].AsBoolean();
								objectTypeAdmin.ObjectType.OwnerType = name["ownerType"].AsInteger();
							}
							break;
					}
				}

				// Add it to our collection.
				vaultObjectTypeOperations.Add(objectTypeAdmin);
			}

			return vaultObjectTypeOperations;

		}

		public override JToken Serialize(VaultObjectTypeOperations input)
		{
			var jArray = new JArray();
			if (null == input)
				return jArray;

			foreach (var kvp in input)
			{
				// We can add to this list as we need more data.
				jArray.Add
				(
					new JObject()
					{
						new JProperty("id", kvp.Key),
						new JProperty("guid", kvp.Value.ObjectType.GUID),
						new JProperty("aliases", new JArray(kvp.Value.SemanticAliases?.GetAliasesFromValue())),
						new JProperty("name", new JObject()
						{
							new JProperty("singular", kvp.Value.ObjectType.NameSingular),
							new JProperty("plural", kvp.Value.ObjectType.NamePlural)
						}),
						new JProperty("propertydefinitions", new JObject()
						{
							new JProperty("owner", kvp.Value.ObjectType.OwnerPropertyDef),
							new JProperty("default", kvp.Value.ObjectType.DefaultPropertyDef)
						}),
						new JProperty("real", kvp.Value.ObjectType.RealObjectType),
						new JProperty("canHaveFiles", kvp.Value.ObjectType.CanHaveFiles),
						new JProperty("allowAdding", kvp.Value.ObjectType.AllowAdding),
						new JProperty("owner", new JObject()
						{
							new JProperty("hasOwner", kvp.Value.ObjectType.HasOwnerType),
							new JProperty("ownerType", kvp.Value.ObjectType.OwnerType)
						}),
					}
				);
			}

			return jArray;
		}
	}
}
