using System;
using System.Collections.Generic;

namespace MFilesAPI.Extensions.Email
{
	/// <summary>
	/// Represents an email message to be sent.
	/// Sent when the <see cref="Send"/> method is called.
	/// </summary>
	public interface IEmailMessage
		: IDisposable
	{
		/// <summary>
		/// The <see cref="SmtpConfiguration"/> to use to send emails.
		/// </summary>
		SmtpConfiguration Configuration { get; set; }

		/// <summary>
		/// Adds a recipient of the given type to the email message.
		/// </summary>
		/// <param name="addressType">The recipient type.</param>
		/// <param name="emailAddress">The recipient email address.</param>
		void AddRecipient(AddressType addressType, EmailAddress emailAddress);

		/// <summary>
		/// Adds a recipient of the given type to the email message.
		/// </summary>
		/// <param name="addressType">The recipient type.</param>
		/// <param name="address">The recipient email address.</param>
		void AddRecipient(AddressType addressType, string address);

		/// <summary>
		/// Adds a recipient of the given type to the email message.
		/// </summary>
		/// <param name="addressType">The recipient type.</param>
		/// <param name="address">The recipient email address.</param>
		/// <param name="displayName">The display name for the recipient.</param>
		void AddRecipient(AddressType addressType, string address, string displayName);

		/// <summary>
		/// Gets the sender ("sender") address information.
		/// </summary>
		/// <returns>The sender.</returns>
		EmailAddress GetSender();

		/// <summary>
		/// Sets the header ("sender") address information.
		/// </summary>
		/// <param name="sender">The new sender information.</param>
		void SetSender(EmailAddress sender);

		/// <summary>
		/// Gets the from ("from") address information.
		/// </summary>
		/// <returns>The from address.</returns>
		EmailAddress GetFrom();

		/// <summary>
		/// Sets the header ("from") address information.
		/// </summary>
		/// <param name="sender">The new from information.</param>
		void SetFrom(EmailAddress from);

		/// <summary>
		/// Gets the reply to list address information.
		/// </summary>
		/// <returns>The reply to list.</returns>
		List<EmailAddress> GetReplyToList();

		/// <summary>
		/// Sets the header ("reply to list") address information.
		/// </summary>
		/// <param name="replytolist">Who to reply to</param>
		void SetReplyToList(List<EmailAddress> replytolist);

		/// <summary>
		/// The email subject.
		/// </summary>
		string Subject { get;set; }

		/// <summary>
		/// The HTML body for the email (if null then text-only will be sent).
		/// </summary>
		/// <remarks>Either <see cref="HtmlBody"/> or <see cref="TextBody"/> should be set.</remarks>
		string HtmlBody { get;set; }

		/// <summary>
		/// The plain text body for the email (if null then HTML-only will be sent).
		/// </summary>
		/// <remarks>Either <see cref="HtmlBody"/> or <see cref="TextBody"/> should be set.</remarks>
		string TextBody { get; set; }

		/// <summary>
		/// Adds an attachment to the email.
		/// </summary>
		/// <param name="fileInfo">The file on disk to add.</param>
		/// <remarks>Will take the mime type and file name from the local file.</remarks>
		void AddFile(System.IO.FileInfo fileInfo);

		/// <summary>
		/// Adds an attachment to the email.
		/// </summary>
		/// <param name="filename">The full address of the local file to attach.</param>
		/// <param name="stream"></param>
		/// <remarks>Will take the mime type from the file extension.</remarks>
		void AddFile(string filename, System.IO.Stream stream);

		/// <summary>
		/// Adds an attachment to the email.
		/// </summary>
		/// <param name="filename">The full address of the local file to attach.</param>
		/// <param name="mimeType">The mime type of the file.</param>
		/// <param name="stream">A readable stream of the file contents.</param>
		void AddFile(string filename, string mimeType, System.IO.Stream stream);

		/// <summary>
		/// Adds an attachment to the email.
		/// </summary>
		/// <param name="file">The file from the M-Files vault to attach.</param>
		/// <param name="vault">The vault that the file came from.</param>
		/// <param name="fileFormat">If not <see cref="MFFileFormat.MFFileFormatNative"/> then will attempt to convert the file before attaching.</param>
		void AddFile(ObjectFile file, Vault vault, MFFileFormat fileFormat = MFFileFormat.MFFileFormatNative);

		/// <summary>
		/// Adds all files on the given object to the email.
		/// </summary>
		/// <param name="objectVersion">The object from the M-Files vault from which to find files to attach.</param>
		/// <param name="vault">The vault that the object came from.</param>
		/// <param name="fileFormat">If not <see cref="MFFileFormat.MFFileFormatNative"/> then will attempt to convert the file before attaching.</param>
		void AddAllFiles
		(
			ObjectVersion objectVersion,
			Vault vault,
			MFFileFormat fileFormat = MFFileFormat.MFFileFormatNative
		);

		/// <summary>
		/// Sends the email message.
		/// </summary>
		void Send();

		/// <summary>
		/// Adds an header to the email.
		/// </summary>
		/// <param name="emailHeader">The new header information.</param>
		void AddHeader(EmailHeader emailHeader);

		/// <summary>
		/// Adds an header to the email.
		/// </summary>
		/// <param name="name">The name of the new header information.</param>
		/// <param name="value">The value of the new header information.</param>
		void AddHeader(string name, string value);

	}
}