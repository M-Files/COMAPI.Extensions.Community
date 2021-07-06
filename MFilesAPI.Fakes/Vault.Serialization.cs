using MFilesAPI.Fakes.Exceptions;
using MFilesAPI.Fakes.ExtensionMethods;
using MFilesAPI.Fakes.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAPI.Fakes
{
	public partial class Vault
		: ISerializableToJson, IDeserializableFromJson
	{
		void IDeserializableFromJson.PopulateFromJToken(JToken token)
		{
			// Cannot populate from a null reference.
			if (null == token)
				return;
			if(false == (token is JObject))
				throw new ArgumentException("Vault can only be deserialised from a JObject", nameof(token));
			var jObject = token as JObject;

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
						this.Guid = p.Value.AsGuid();
						break;
					case "name":
						this.Name = p.Value?.ToString();
						break;
					case "objecttypes":
						this.ObjectTypeOperations?.PopulateFromJToken(p.Value);
						break;
					case "classes":
						this.ClassOperations?.PopulateFromJToken(p.Value);
						break;
					case "propertydefinitions":
						this.PropertyDefOperations?.PopulateFromJToken(p.Value);
						break;
				}
			}
		}

		JToken ISerializableToJson.ToJToken()
		{

			return new JObject
			{
				new JProperty("name", this.Name),
				new JProperty("guid", this.Guid.ToString("B")),
				new JProperty("objectTypes", this.ObjectTypeOperations?.ToJToken()),
				new JProperty("propertyDefinitions", this.PropertyDefOperations?.ToJToken()),
				new JProperty("classes", this.ClassOperations?.ToJToken())
			};
		}
	}
}
