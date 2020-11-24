using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;

namespace MFilesAPI.Extensions.Email
{
	/// <summary>
	/// Represents an email message to be sent via the default plugin.
	/// </summary>
	public class EmailMessage
		: EmailMessageBase
	{
		/// <summary>
		/// The <see cref="MailMessage"/> that will be used to send the email.
		/// </summary>
		protected MailMessage mailMessage = new MailMessage();

		/// <summary>
		/// The <see cref="SmtpClient"/> used to send email.
		/// If null then a new instance will be created as needed.
		/// </summary>
		protected SmtpClient SmtpClient { get; private set; }

		/// <summary>
		/// Instantiates an EmailMessage for sending an email.
		/// </summary>
		/// <param name="configuration">The configuration to use.</param>
		public EmailMessage(SmtpConfiguration configuration)
			: this(configuration, null)
		{
			// Add default email header from the configuration
			if (null != configuration.DefaultEmailHeader)
				configuration.DefaultEmailHeader.ForEach(x => AddHeader(x));
		}

		/// <summary>
		/// Instantiates an EmailMessage for sending an email.
		/// </summary>
		/// <param name="configuration">The configuration to use.</param>
		/// <param name="smtpClient">
		/// The <see cref="SmtpClient"/> to send email using.
		/// Note: The client must already be correctly configured
		/// (e.g. by calling <see cref="SmtpConfigurationExtensionMethods.ApplyConfigurationTo"/>).
		/// </param>
		public EmailMessage(SmtpConfiguration configuration, SmtpClient smtpClient)
			: base(configuration)
		{
			// Set up the SmtpClient.
			this.SmtpClient = smtpClient;
		}

		/// <summary>
		/// Converts an internal <see cref="EmailAddress"/> structure to a <see cref="MailAddress"/> used by <see cref="MailMessage"/>.
		/// </summary>
		/// <param name="input">The address to convert from.</param>
		/// <returns>The converted address.</returns>
		public static MailAddress ConvertToMailAddress(EmailAddress input)
		{
			// Sanity.
			if (null == input)
				throw new ArgumentNullException(nameof(input));
			return new MailAddress(input.Address, input.DisplayName);
		}

		/// <summary>
		/// Converts a <see cref="MailAddress"/> structure to an internal <see cref="EmailAddress"/> used by <see cref="MailMessage"/>.
		/// </summary>
		/// <param name="input">The address to convert from.</param>
		/// <returns>The converted address.</returns>
		public static EmailAddress ConvertToEmailAddress(MailAddress input)
		{
			// Sanity.
			if (null == input)
				throw new ArgumentNullException(nameof(input));
			return new EmailAddress(input.Address, input.DisplayName);
		}

		/// <summary>
		/// Retrieves an <see cref="AlternateView"/> from the mail message with the given mime type.
		/// </summary>
		/// <param name="mediaType">The media/mime type for the alternate view.</param>
		/// <returns>The alternate view as a string.  If no alternate view is found then returns null.</returns>
		public string GetAlternateViewAsString(string mediaType)
		{
			// Try and get the alternate view.
			var view = this.mailMessage
				.AlternateViews
				.FirstOrDefault(v => v.ContentType?.MediaType == mediaType);

			// No alternate view then return null.
			if (null == view)
				return null;

			// Decode it.
			var dataStream = view.ContentStream;
			if (dataStream.Length == 0)
				return null;

			dataStream.Position = 0;
			byte[] byteBuffer = new byte[dataStream.Length];
			var encoding = Encoding.UTF8;
			if(false == string.IsNullOrWhiteSpace(view.ContentType.CharSet))
				encoding = Encoding.GetEncoding(view.ContentType.CharSet);
			return encoding.GetString(byteBuffer, 0,
				dataStream.Read(byteBuffer, 0, byteBuffer.Length));
		}

		/// <summary>
		/// Sets an <see cref="AlternateView"/> for the mail message with the given content and mime type.
		/// </summary>
		/// <param name="content">The content for the alternate view.</param>
		/// <param name="mediaType">The mime/media type for the alternate view.</param>
		public void SetAlternateViewFromString(string content, string mediaType)
		{
			// Remove the view if already exists.
			{
				// Do we have a view for this mime type?
				var view = this.mailMessage
					.AlternateViews
					.FirstOrDefault(v => v.ContentType?.MediaType == mediaType);

				// Remove the view if we have it.
				if (null != view)
					this.mailMessage.AlternateViews.Remove(view);
			}

			// Null/empty content = die.
			if (string.IsNullOrWhiteSpace(content))
				return;

			// Add the alternate view.
			this.mailMessage
				.AlternateViews
				.Add(AlternateView.CreateAlternateViewFromString(content, Encoding.UTF8, mediaType));
		}

		#region Overrides of EmailMessageBase

		/// <inheritdoc />
		public override void AddRecipient(AddressType addressType, EmailAddress emailAddress)
		{
			switch (addressType)
			{
				case AddressType.To:
					this.mailMessage.To.Add(EmailMessage.ConvertToMailAddress(emailAddress));
					break;
				case AddressType.CarbonCopy:
					this.mailMessage.CC.Add(EmailMessage.ConvertToMailAddress(emailAddress));
					break;
				case AddressType.BlindCarbonCopy:
					this.mailMessage.Bcc.Add(EmailMessage.ConvertToMailAddress(emailAddress));
					break;
				default:
					throw new ArgumentException($"Unhandled address type {addressType}.", nameof(addressType));
			}
		}

		/// <inheritdoc />
		public override EmailAddress GetSender()
		{
			return EmailMessage.ConvertToEmailAddress(this.mailMessage.Sender);
		}

		/// <inheritdoc />
		public override void SetSender(EmailAddress sender)
		{
			// Sanity.
			if (null == sender)
				throw new ArgumentNullException(nameof(sender));
			this.mailMessage.Sender = EmailMessage.ConvertToMailAddress(sender);
		}

		/// <inheritdoc />
		public override EmailAddress GetFrom()
		{
			return EmailMessage.ConvertToEmailAddress(this.mailMessage.From);
		}

		/// <inheritdoc />
		public override void SetFrom(EmailAddress from)
		{
			// Sanity.
			if (null == from)
				throw new ArgumentNullException(nameof(from));
			this.mailMessage.From = EmailMessage.ConvertToMailAddress(from);
		}

		/// <inheritdoc />
		public override List<EmailAddress> GetReplyToList()
		{
			var list = new List<EmailAddress>();
			if (null != this.mailMessage?.ReplyToList)
				list.AddRange
				(
					this.mailMessage
						.ReplyToList
						.Select(EmailMessage.ConvertToEmailAddress)
				);
			return list;
		}

		/// <inheritdoc />
		public override void SetReplyToList(List<EmailAddress> replytolist)
		{
			// Sanity.
			if (null == replytolist)
				throw new ArgumentNullException(nameof(replytolist));
			foreach (EmailAddress emailAddress in replytolist)
            {
				this.mailMessage.ReplyToList.Add(EmailMessage.ConvertToMailAddress(emailAddress));
			}
		}
		/// <inheritdoc />
		public override string Subject
		{
			get => this.mailMessage.Subject;
			set => this.mailMessage.Subject = value?
				.Replace("\r\n", "\n")?
				.Replace("\r", "\n")?
				.Replace("\n", " ");
		}

		/// <inheritdoc />
		public override string HtmlBody
		{
			get => this.GetAlternateViewAsString(MediaTypeNames.Text.Html);
			set => this.SetAlternateViewFromString(value, MediaTypeNames.Text.Html);
		}

		/// <inheritdoc />
		public override string TextBody
		{
			get => this.GetAlternateViewAsString(MediaTypeNames.Text.Plain);
			set => this.SetAlternateViewFromString(value, MediaTypeNames.Text.Plain);
		}

		/// <inheritdoc />
		public override void AddFile(string filename, string mimeType, Stream stream)
		{
			this.mailMessage.Attachments.Add(new Attachment(stream, filename, mimeType));
		}

		/// <inheritdoc />
		public override void Send()
		{
			// Get the provided SmtpClient.
			var client = this.SmtpClient;
			var disposeSmtpClientOnceUsed = false;

			// If we don't have a client then make one just for this call.
			if (null == client)
			{
				// Create the SmtpClient and mark it for disposal.
				client = new SmtpClient();
				disposeSmtpClientOnceUsed = true;

				// Configure the SmtpClient as per our current configuration.
				this.Configuration.ApplyConfigurationTo(client);
			}

			// Send the message.
			try
			{
				client.Send(this.mailMessage);
			}
			finally
			{
				// If we created the SmtpClient then we need to dispose of it.
				if (disposeSmtpClientOnceUsed)
				{
					client?.Dispose();
				}
			}
		}

		/// <inheritdoc />
		public override void AddHeader(EmailHeader emailHeader)
		{
			// Sanity.
			if (null == emailHeader)
				throw new ArgumentNullException(nameof(emailHeader));
			AddHeader(emailHeader.Name, emailHeader.Value);
		}

		/// <inheritdoc />
		public override void AddHeader(string name, string value)
		{
			// Sanity.
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("The header name cannot be null or whitespace", nameof(name));
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentException($"The header value cannot be null or whitespace for header name '{name}'", nameof(value));

			this.mailMessage.Headers.Add(name, value);
		}

		protected override void Dispose(bool disposing)
		{
			// Attempt to dispose the mail message.
			this.mailMessage?.Dispose();
			this.mailMessage = null;

			// Call the base implementation.
			base.Dispose(disposing);
		}

		#endregion
	}
}