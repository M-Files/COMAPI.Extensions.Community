using MFilesAPI.Extensions.Tests.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace MFilesAPI.Extensions.Tests.Email
{
	public abstract class EmailMessageBase
		: TestBaseWithVaultMock
	{
		protected virtual MFilesAPI.Extensions.Email.EmailMessageBase CreateEmailMessage()
		{
			return this.CreateEmailMessage
			(
				this.GetValidDefaultConfiguration()
			);
		}

		protected abstract MFilesAPI.Extensions.Email.EmailMessageBase CreateEmailMessage
		(
			MFilesAPI.Extensions.Email.SmtpConfiguration configuration
		);

		protected virtual MFilesAPI.Extensions.Email.SmtpConfiguration GetValidDefaultConfiguration()
		{
			return new Extensions.Email.SmtpConfiguration()
			{
				DefaultSender = new Extensions.Email.EmailAddress()
				{
					Address = "test@test.com",
					DisplayName = "Test McTest"
				}
			};
		}

		#region Constructor

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullConfigurationConstructorThrows()
		{
			this.CreateEmailMessage(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullConfigurationSetterThrows()
		{
			var emailMessage = this.CreateEmailMessage();
			emailMessage.Configuration = null;
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void DefaultConfigurationSetterThrowsAsDefaultSenderIsNull()
		{
			var configuration = this.GetValidDefaultConfiguration();
			configuration.DefaultSender = null;
			var emailMessage = this.CreateEmailMessage(configuration);
		}

		[TestMethod]
		public void DefaultConfigurationSetterOkay()
		{
			this.CreateEmailMessage();
		}

		#endregion

		#region AddFile

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddFileNullFileInfoThrows()
		{
			var emailMessage = this.CreateEmailMessage();
			emailMessage.AddFile((FileInfo) null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void AddFileFileDoesNotExistThrows()
		{
			var emailMessage = this.CreateEmailMessage();
			var fileInfo = new FileInfo
			(
				System.IO.Path.Combine
				(
					System.IO.Path.GetTempPath(),
					System.IO.Path.GetTempFileName() + ".tmp"
				)
			);
			int sanity = 0;
			while (fileInfo.Exists && sanity < 10)
			{
				fileInfo = new FileInfo
				(
					System.IO.Path.Combine
					(
						System.IO.Path.GetTempPath(),
						System.IO.Path.GetTempFileName() + ".tmp"
					)
				);
				sanity++;
			}

			if (fileInfo.Exists)
			{
				Assert.Inconclusive("Could not generate a file name.");
				return;
			}

			emailMessage.AddFile(fileInfo);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddFileThrowsForNullObjectFile()
		{
			var emailMessage = this.CreateEmailMessage();
			emailMessage.AddFile
			(
				(ObjectFile) null,
				this.GetVaultMock().Object
			);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddFileThrowsForNullVault()
		{
			var emailMessage = this.CreateEmailMessage();
			emailMessage.AddFile
			(
				new ObjectFileStub(),
				(Vault)null
			);
		}

		#endregion

		#region HTML body

		[TestMethod]
		public void HtmlBodyIsNull()
		{
			var emailMessage = this.CreateEmailMessage();
			Assert.IsNull(emailMessage.HtmlBody);
		}

		[TestMethod]
		public void SetHtmlBodyToNullDoesNotThrow()
		{
			var emailMessage = this.CreateEmailMessage();
			emailMessage.HtmlBody = null;
		}

		[TestMethod]
		public void SetHtmlBodyToEmptyStringDoesNotThrow()
		{
			var emailMessage = this.CreateEmailMessage();
			emailMessage.HtmlBody = string.Empty;
		}

		[TestMethod]
		public void SetHtmlBodyToStringPersists()
		{
			var data = Guid.NewGuid().ToString();
			var emailMessage = this.CreateEmailMessage();
			emailMessage.HtmlBody = data;
			Assert.AreEqual(data, emailMessage.HtmlBody);
		}

		#endregion

		#region Text body

		[TestMethod]
		public void TextBodyIsEmptyString()
		{
			var emailMessage = this.CreateEmailMessage();
			Assert.IsNull(emailMessage.TextBody);
		}

		[TestMethod]
		public void SetTextBodyToNullDoesNotThrow()
		{
			var emailMessage = this.CreateEmailMessage();
			emailMessage.TextBody = null;
		}

		[TestMethod]
		public void SetTextBodyToEmptyStringDoesNotThrow()
		{
			var emailMessage = this.CreateEmailMessage();
			emailMessage.TextBody = string.Empty;
		}

		[TestMethod]
		public void SetTextBodyToStringPersists()
		{
			var data = Guid.NewGuid().ToString();
			var emailMessage = this.CreateEmailMessage();
			emailMessage.TextBody = data;
			Assert.AreEqual(data, emailMessage.TextBody);
		}

		#endregion

		#region Subject

		[TestMethod]
		public void SubjectIsEmptyString()
		{
			var emailMessage = this.CreateEmailMessage();
			Assert.AreEqual(string.Empty, emailMessage.Subject);
		}

		[TestMethod]
		public void SetSubjectToNullDoesNotThrow()
		{
			var emailMessage = this.CreateEmailMessage();
			emailMessage.Subject = null;
		}

		[TestMethod]
		public void SetSubjectToEmptyStringDoesNotThrow()
		{
			var emailMessage = this.CreateEmailMessage();
			emailMessage.Subject = string.Empty;
		}

		[TestMethod]
		public void SetSubjectToStringPersists()
		{
			var data = Guid.NewGuid().ToString();
			var emailMessage = this.CreateEmailMessage();
			emailMessage.Subject = data;
			Assert.AreEqual(data, emailMessage.Subject);
		}

		[TestMethod]
		public void SubjectRemovesCarriageReturns()
		{
			var emailMessage = this.CreateEmailMessage();
			emailMessage.Subject = "hello\rworld";
			Assert.AreEqual("hello world", emailMessage.Subject);
		}

		[TestMethod]
		public void SubjectRemovesNewLines()
		{
			var emailMessage = this.CreateEmailMessage();
			emailMessage.Subject = "hello\nworld";
			Assert.AreEqual("hello world", emailMessage.Subject);
		}

		[TestMethod]
		public void SubjectRemovesNewLinesAndCarriageReturns()
		{
			var emailMessage = this.CreateEmailMessage();
			emailMessage.Subject = "hello\r\nworld";
			// Note there should be one space here, not two.
			Assert.AreEqual("hello world", emailMessage.Subject);
		}

		#endregion

		#region Sender

		[TestMethod]
		public void GetSender()
		{
			// Create the email message.
			var configuration = this.GetValidDefaultConfiguration();
			var emailMessage = this.CreateEmailMessage(configuration);

			// Ensure the sender is as specified in the configuration.
			var sender = emailMessage.GetSender();
			Assert.IsNotNull(sender);
			Assert.AreEqual(configuration.DefaultSender.Address, sender.Address);
			Assert.AreEqual(configuration.DefaultSender.DisplayName, sender.DisplayName);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetSenderThrowsWithNullSender()
		{
			var emailMessage = this.CreateEmailMessage();
			emailMessage.SetSender(null);
		}

		[TestMethod]
		public void SetSender()
		{
			// Create the email message.
			var emailMessage = this.CreateEmailMessage();

			// Create the sender.
			var address = new MFilesAPI.Extensions.Email.EmailAddress("devsupport@m-files.com", "M-Files Developer Support");
			emailMessage.SetSender(address);

			// Ensure the sender is as specified in the configuration.
			var sender = emailMessage.GetSender();
			Assert.IsNotNull(sender);
			Assert.AreEqual(address.Address, sender.Address);
			Assert.AreEqual(address.DisplayName, sender.DisplayName);
		}

		#endregion

		#region From

		[TestMethod]
		public void GetFrom()
		{
			// Create the email message.
			var configuration = this.GetValidDefaultConfiguration();
			var emailMessage = this.CreateEmailMessage(configuration);
			var address = new MFilesAPI.Extensions.Email.EmailAddress("devsupport@m-files.com", "M-Files Developer Support");
			emailMessage.SetFrom(address);

			// Ensure the from is as specified.
			var from = emailMessage.GetFrom();
			Assert.IsNotNull(from);
			Assert.AreEqual(address.Address, from.Address);
			Assert.AreEqual(address.DisplayName, from.DisplayName);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetFromThrowsWithNullSender()
		{
			var emailMessage = this.CreateEmailMessage();
			emailMessage.SetFrom(null);
		}

		#endregion

		#region Reply to list

		[TestMethod]
		public void ReplyToList()
		{
			// Create the email message.
			var configuration = this.GetValidDefaultConfiguration();
			var emailMessage = this.CreateEmailMessage(configuration);
			var address1 = new MFilesAPI.Extensions.Email.EmailAddress("devsupport1@m-files.com", "M-Files Developer Support");
			var address2 = new MFilesAPI.Extensions.Email.EmailAddress("devsupport2@m-files.com", "M-Files Developer Support");
			emailMessage.SetReplyToList(new List<Extensions.Email.EmailAddress>()
			{
				address1,
				address2
			});

			// Ensure the list is as specified.
			var list = emailMessage.GetReplyToList();
			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(address1.Address, list[0].Address);
			Assert.AreEqual(address1.DisplayName, list[0].DisplayName);
			Assert.AreEqual(address2.Address, list[1].Address);
			Assert.AreEqual(address2.DisplayName, list[1].DisplayName);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetReplyToListThrowsWithNullSender()
		{
			var emailMessage = this.CreateEmailMessage();
			emailMessage.SetReplyToList(null);
		}

		#endregion
	}
}
