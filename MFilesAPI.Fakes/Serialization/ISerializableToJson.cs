using Newtonsoft.Json.Linq;

namespace MFilesAPI.Fakes.Serialization
{
	public interface ISerializableToJson
	{
		JToken ToJToken();
	}
}
