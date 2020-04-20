using MFilesAPI.Extensions.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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

	}
}
