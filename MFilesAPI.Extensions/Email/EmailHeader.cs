using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAPI.Extensions.Email
{
	/// <summary>
	/// Represents an email header.
	/// </summary>
	[DataContract]
	public class EmailHeader
	{
		/// <summary>
		/// The name of the header.
		/// </summary>
		[DataMember]
		public string Name { get; set; }

		/// <summary>
		/// The value of the header.
		/// </summary>
		[DataMember]
		public string Value { get; set; }

		/// <summary>
		/// Creates an header with no <see cref="Name"/> and <see cref="Value"/>.
		/// </summary>
		public EmailHeader()
		{
		}

		/// <summary>
		/// Creates an <see cref="EmailHeader"/> instance with <see cref="Name"/> and <see cref="Value"/>
		/// </summary>
		/// <param name="name">The name of the header</param>
		public EmailHeader(string name, string value)
		{
			// Sanity.
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("The header name cannot be null or whitespace", nameof(name));
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentException($"The header value cannot be null or whitespace for header name '{name}'", nameof(value));
			this.Name = name;
			this.Value = value;
		}
	}
}
