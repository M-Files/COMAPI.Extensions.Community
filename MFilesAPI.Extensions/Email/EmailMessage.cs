using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
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
		private MailMessage mailMessage = new MailMessage();

		public EmailMessage(SmtpConfiguration configuration)
			: base(configuration)
		{
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
			var encoding = Encoding.GetEncoding(view.ContentType.CharSet);
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
				.Add(AlternateView.CreateAlternateViewFromString(content, new ContentType(mediaType)));
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
			return EmailMessage.ConvertToEmailAddress(this.mailMessage.From);
		}

		/// <inheritdoc />
		public override void SetSender(EmailAddress sender)
		{
			// Sanity.
			if (null == sender)
				throw new ArgumentNullException(nameof(sender));
			this.mailMessage.From = EmailMessage.ConvertToMailAddress(sender);
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
			// Set up the SmtpClient.
			using (var client = new SmtpClient())
			{
				// Should we use the local pickup folder?
				if (this.Configuration.UseLocalPickupFolder)
				{
					// We will write the item to disk for subsequent pickup.
					if (string.IsNullOrWhiteSpace(this.Configuration.LocalPickupFolder))
						throw new InvalidOperationException(
							"The local pickup folder is not set, so cannot be used to send emails.");

					// Validate the folder.
					DirectoryInfo folder;
					try
					{
						folder = new DirectoryInfo(this.Configuration.LocalPickupFolder);

						// If the folder does not exist then try and create it.
						if (false == folder.Exists)
						{
							try
							{
								// Attempt to create the folder.
								folder.Create();

								// Refresh the folder data.
								folder = new DirectoryInfo(folder.FullName);
							}
							catch (Exception e)
							{
								// Could not create.  Possibly security exception, possibly reasonable-looking-folder-string, but not actually usable.
								throw new InvalidOperationException
								(
									$"The local pickup folder ({this.Configuration.LocalPickupFolder}) does not exist and could not be created, so cannot be used to send emails.",
									e
								);
							}
						}
					}
					catch (Exception e)
					{
						// We couldn't work with the folder string at all.
						throw new InvalidOperationException
						(
							$"The local pickup folder is of an invalid format ({this.Configuration.LocalPickupFolder}), so cannot be used to send emails.",
							e
						);
					}

					// Set up the delivery data.
					client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
					client.PickupDirectoryLocation = folder.FullName;

					// There was a bug in System.Net.Mail.MailMessage.Send at one point, where a null host
					// would throw an exception, so let's set it to the configured data, or localhost.
					client.Host = string.IsNullOrWhiteSpace(this.Configuration.ServerAddress)
						? "localhost"
						: this.Configuration.ServerAddress;
					client.Port = this.Configuration.Port;
				}
				else
				{
					// We are going to send directly.
					// Set up the delivery data.
					client.DeliveryMethod = SmtpDeliveryMethod.Network;
					client.Host = this.Configuration.ServerAddress;
					client.Port = this.Configuration.Port;

					// If we need to use an encrypted connection then configure that.
					client.EnableSsl = this.Configuration.UseEncryptedConnection;

					// If we need to use authentication then configure that.
					if (this.Configuration.RequiresAuthentication)
					{
						client.Credentials = new System.Net.NetworkCredential
						(
							this.Configuration.Credentials.AccountName,
							this.Configuration.Credentials.Password
						);
					}
				}

				// Send the message.
				client.Send(this.mailMessage);
			}
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