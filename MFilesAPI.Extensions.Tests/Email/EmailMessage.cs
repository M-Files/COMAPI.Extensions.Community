using MFilesAPI.Extensions.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net.Mail;

namespace MFilesAPI.Extensions.Tests.Email
{
	[TestClass]
	public class EmailMessage
		: EmailMessageBase
	{
		#region Overrides of EmailMessageBase

		/// <inheritdoc />
		protected override Extensions.Email.EmailMessageBase CreateEmailMessage(SmtpConfiguration configuration)
		{
			return new Extensions.Email.EmailMessage(configuration);
		}

		#endregion

		#region Convert to email address

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConvertToEmailAddressThrowsWithNullAddress()
		{
			Extensions.Email.EmailMessage.ConvertToEmailAddress(null);
		}

		[TestMethod]
		public void ConvertToEmailAddress()
		{
			var emailAddress = Extensions.Email.EmailMessage.ConvertToEmailAddress
			(
				new System.Net.Mail.MailAddress("devsupport@m-files.com", "M-Files Developer Support")
			);
			Assert.IsNotNull(emailAddress);
			Assert.AreEqual("devsupport@m-files.com", emailAddress.Address);
			Assert.AreEqual("M-Files Developer Support", emailAddress.DisplayName);
		}

		#endregion

		#region Convert to mail address

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConvertToMailAddressThrowsWithNullAddress()
		{
			Extensions.Email.EmailMessage.ConvertToMailAddress(null);
		}

		[TestMethod]
		public void ConvertToMailAddress()
		{
			var mailAddress = Extensions.Email.EmailMessage.ConvertToMailAddress
			(
				new Extensions.Email.EmailAddress("devsupport@m-files.com", "M-Files Developer Support")
			);
			Assert.IsNotNull(mailAddress);
			Assert.AreEqual("devsupport@m-files.com", mailAddress.Address);
			Assert.AreEqual("M-Files Developer Support", mailAddress.DisplayName);
		}

		#endregion

		#region Defaults

		[TestMethod]
		public void MailMessageDefaults()
		{
			var emailAddress = new EmailMessageProxy(this.GetValidDefaultConfiguration());

			// Basic content.
			Assert.AreEqual(string.Empty, emailAddress.MailMessage.Subject);
			Assert.AreEqual(null, emailAddress.HtmlBody);
			Assert.AreEqual(null, emailAddress.TextBody);

			// Recipients.
			Assert.AreEqual(0, emailAddress.MailMessage.To.Count);
			Assert.AreEqual(0, emailAddress.MailMessage.CC.Count);
			Assert.AreEqual(0, emailAddress.MailMessage.Bcc.Count);

			// Attachments.
			Assert.AreEqual(0, emailAddress.MailMessage.Attachments.Count);
		}

		#endregion

		#region AddRecipient tests

		[TestMethod]
		public void AddRecipient_AddressTypeTo()
		{
			var emailMessage = new EmailMessageProxy(this.GetValidDefaultConfiguration());
			emailMessage.AddRecipient(AddressType.To, new Extensions.Email.EmailAddress("devsupport@m-files.com"));
			Assert.AreEqual(1, emailMessage.MailMessage.To.Count);
			Assert.AreEqual("devsupport@m-files.com", emailMessage.MailMessage.To[0].Address);
		}

		[TestMethod]
		public void AddRecipient_AddressTypeCC()
		{
			var emailMessage = new EmailMessageProxy(this.GetValidDefaultConfiguration());
			emailMessage.AddRecipient(AddressType.CarbonCopy, new Extensions.Email.EmailAddress("devsupport@m-files.com"));
			Assert.AreEqual(1, emailMessage.MailMessage.CC.Count);
			Assert.AreEqual("devsupport@m-files.com", emailMessage.MailMessage.CC[0].Address);
		}

		[TestMethod]
		public void AddRecipient_AddressTypeBCC()
		{
			var emailMessage = new EmailMessageProxy(this.GetValidDefaultConfiguration());
			emailMessage.AddRecipient(AddressType.BlindCarbonCopy, new Extensions.Email.EmailAddress("devsupport@m-files.com"));
			Assert.AreEqual(1, emailMessage.MailMessage.Bcc.Count);
			Assert.AreEqual("devsupport@m-files.com", emailMessage.MailMessage.Bcc[0].Address);
		}

		#endregion

		#region AddHeader tests

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Addheader_NullNameNullValue()
		{
			var emailMessage = new EmailMessageProxy(this.GetValidDefaultConfiguration());
			emailMessage.AddHeader(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Addheader_EmptyNameNullValue()
		{
			var emailMessage = new EmailMessageProxy(this.GetValidDefaultConfiguration());
			emailMessage.AddHeader(string.Empty, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Addheader_NullNameEmptyValue()
		{
			var emailMessage = new EmailMessageProxy(this.GetValidDefaultConfiguration());
			emailMessage.AddHeader(null, string.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Addheader_EmptyNameEmptyValue()
		{
			var emailMessage = new EmailMessageProxy(this.GetValidDefaultConfiguration());
			emailMessage.AddHeader(string.Empty, string.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Addheader_NullName()
		{
			var emailMessage = new EmailMessageProxy(this.GetValidDefaultConfiguration());
			emailMessage.AddHeader(null, "yes");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Addheader_EmptyName()
		{
			var emailMessage = new EmailMessageProxy(this.GetValidDefaultConfiguration());
			emailMessage.AddHeader(string.Empty, "yes");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Addheader_NullValue()
		{
			var emailMessage = new EmailMessageProxy(this.GetValidDefaultConfiguration());
			emailMessage.AddHeader("X-Sophos-SPX-Encrypt", null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Addheader_EmptyValue()
		{
			var emailMessage = new EmailMessageProxy(this.GetValidDefaultConfiguration());
			emailMessage.AddHeader("X-Sophos-SPX-Encrypt", string.Empty);
		}

		[TestMethod]
		public void Addheader()
		{
			var emailMessage = new EmailMessageProxy(this.GetValidDefaultConfiguration());
			emailMessage.AddHeader("X-Sophos-SPX-Encrypt", "yes");
			Assert.AreEqual(1, emailMessage.MailMessage.Headers.Count);
			Assert.AreEqual("X-Sophos-SPX-Encrypt", emailMessage.MailMessage.Headers.GetKey(0));
			Assert.AreEqual("yes", emailMessage.MailMessage.Headers.Get(0));
		}

		#endregion

		public class EmailMessageProxy
			: Extensions.Email.EmailMessage
		{
			public MailMessage MailMessage
			{
				get => base.mailMessage;
				set => base.mailMessage = mailMessage;
			}

			public EmailMessageProxy(SmtpConfiguration configuration)
				: base(configuration)
			{
			}

			public EmailMessageProxy(SmtpConfiguration configuration, SmtpClient smtpClient)
				: base(configuration, smtpClient)
			{
			}
		}

	}
}
