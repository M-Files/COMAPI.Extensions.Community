using Newtonsoft.Json.Linq;

namespace MFilesAPI.Fakes.Serialization
{
	public interface IDeserializableFromJson
	{
		void PopulateFromJToken(JToken token);
	}
}
