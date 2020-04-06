using System.Runtime.Serialization;

namespace MFilesAPI.Extensions.Email
{
	/// <summary>
	/// Configuration settings for authentication to the Smtp server.
	/// </summary>
	[DataContract]
	public class Credentials
	{
		/// <summary>
		/// The account name to connect as.
		/// </summary>
		[DataMember]
		public virtual string AccountName { get; set; }

		/// <summary>
		/// The password for the account.
		/// </summary>
		[DataMember]
		public virtual string Password { get; set; }
	}
}