using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace MFilesAPI.Extensions.Email
{
	/// <summary>
	/// The configuration of the SMTP server to send via.
	/// </summary>
	[DataContract]
	public class SmtpConfiguration
	{
		/// <summary>
		/// The default value for <see cref="Port"/>.
		/// </summary>
		public const int DefaultPort = 587;

		/// <summary>
		/// The remote host / server address to send through.
		/// </summary>
		[DataMember]
		public virtual string ServerAddress { get; set; }

		/// <summary>
		/// If true, will attempt to connect using an encrypted connection.
		/// </summary>
		[DataMember]
		public virtual bool UseEncryptedConnection { get; set; } = true;

		private int port = SmtpConfiguration.DefaultPort;

		/// <summary>
		/// The port to connect with.
		/// </summary>
		[DataMember]
		public virtual int Port
		{
			get => this.port;
			set
			{
				// Sanity.
				if (value <= 0)
					throw new ArgumentOutOfRangeException(nameof(value));
				this.port = value;
			}
		}

		/// <summary>
		/// If true then the remote server requires authentication to send through.
		/// </summary>
		[DataMember]
		public virtual bool RequiresAuthentication { get; set; } = true;

		/// <summary>
		/// The authentication credentials to use if <see cref="RequiresAuthentication"/> is true.
		/// </summary>
		[DataMember]
		public virtual Credentials Credentials { get; set; } = new Credentials();

		/// <summary>
		/// If true will write to a local pickup folder instead of sending via a remote host.
		/// </summary>
		[DataMember]
		public virtual bool UseLocalPickupFolder { get; set; } = false;

		/// <summary>
		/// If <see cref="UseLocalPickupFolder"/> is true, the location of the local folder to write to.
		/// </summary>
		[DataMember]
		public virtual string LocalPickupFolder { get; set; }

		/// <summary>
		/// The default sender details.
		/// </summary>
		[DataMember]
		public virtual EmailAddress DefaultSender { get; set; }

		/// <summary>
		/// The default email headers.
		/// </summary>
		[DataMember]
		public virtual List<EmailHeader> DefaultEmailHeader { get; set; } = new List<EmailHeader>();
	}
}
