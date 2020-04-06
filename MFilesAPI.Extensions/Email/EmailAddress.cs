using System;
using System.Runtime.Serialization;

namespace MFilesAPI.Extensions.Email
{
	/// <summary>
	/// Represents an email address to send to, or send from.
	/// </summary>
	[DataContract]
	public class EmailAddress
	{
		/// <summary>
		/// The actual email address of the user.
		/// </summary>
		[DataMember]
		public string Address { get; set; }

		/// <summary>
		/// The display name for the user.
		/// </summary>
		[DataMember]
		public string DisplayName { get; set; }

		/// <summary>
		/// Creates an EmailAddress with no <see cref="Address"/> populated.
		/// Usage of an EmailAddress without an address will throw an exception.
		/// </summary>
		public EmailAddress()
		{
		}

		/// <summary>
		/// Creates an <see cref="EmailAddress"/> instance with just the <see cref="Address"/> property populated.
		/// </summary>
		/// <param name="address">The email address of the user.</param>
		/// <remarks>The email address will be checked to see whether it is null or whitespace on creation, but no other checks are made.  The mail library may do additional checks when the message is created.</remarks>
		public EmailAddress(string address)
			: this()
		{
			// Sanity.
			if (string.IsNullOrWhiteSpace(address))
				throw new ArgumentException("The email address cannot be null or whitespace", nameof(address));
			this.Address = address;
		}
		
		/// <summary>
		/// Creates an <see cref="EmailAddress"/> instance with just the <see cref="Address"/> property populated.
		/// </summary>
		/// <param name="address">The email address of the user.</param>
		/// <param name="displayName">The display name for the user.</param>
		/// <remarks>The email address will be checked to see whether it is null or whitespace on creation, but no other checks are made.  The mail library may do additional checks when the message is created.</remarks>
		public EmailAddress(string address, string displayName)
			: this(address)
		{
			this.DisplayName = displayName;
		}
	}
}