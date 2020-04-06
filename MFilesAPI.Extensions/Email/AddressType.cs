namespace MFilesAPI.Extensions.Email
{
	/// <summary>
	/// The type of the email address.
	/// </summary>
	public enum AddressType
	{
		/// <summary>
		/// The email should be sent to this address.
		/// </summary>
		To = 0,

		/// <summary>
		/// The email should be copied to this address.
		/// </summary>
		CarbonCopy = 2,

		/// <summary>
		/// The email should be BCCed to this address (the other recipients won't see this recipient in the list).
		/// </summary>
		BlindCarbonCopy = 3
	}
}
