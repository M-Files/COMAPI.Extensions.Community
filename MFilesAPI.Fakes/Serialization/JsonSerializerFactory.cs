using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAPI.Fakes.Serialization
{
	public class JsonSerializerFactory
	{
		protected Dictionary<Version, Func<IJsonSerializer>> TypeDictionary { get; }
			= new Dictionary<Version, Func<IJsonSerializer>>();

		/// <summary>
		/// Registers a converter that can convert to/from the specified <paramref name="version"/>.
		/// </summary>
		/// <typeparam name="TConverter">The converter type.</typeparam>
		/// <param name="version">The version that the converter can target.</param>
		/// <returns>The factory, for chaining.</returns>
		public JsonSerializerFactory Register<TConverter>(Version version)
			where TConverter : IJsonSerializer, new()
		{
			this.TypeDictionary.Add(version, () => new TConverter());
			return this;
		}

		/// <summary>
		/// Retrieves a converter for version <paramref name="version"/>.
		/// If <paramref name="allowHigherVersions"/> is true then higher version parsers/generators are okay.
		/// If multiple converters are found then the highest version is returned.
		/// </summary>
		/// <param name="version">The version to target.</param>
		/// <param name="allowHigherVersions"></param>
		/// <returns></returns>
		public IJsonSerializer Instantiate(Version version = null, bool allowHigherVersions = true)
		{
			// If we don't have a version then get every one.
			version = version ?? new Version(0, 0);

			// Find the appropriate converters.
			var converters = this.TypeDictionary
				.Where(kvp => allowHigherVersions
						? kvp.Key >= version
						: kvp.Key == version)
				.OrderByDescending(kvp => kvp.Key)
				.ToList();
			if(0 == converters.Count)
				throw new InvalidOperationException($"No converter found for version {version}");

			// Instantiate it.
			return this.TypeDictionary[version]();
		}

		/// <summary>
		/// A default factory containing out of the box converters.
		/// </summary>
		public static JsonSerializerFactory Default = new JsonSerializerFactory()
			.Register<JsonConverterVersion1>(new Version(1, 0));
	}
}
