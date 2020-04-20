using System;
using System.Net;
using System.Net.Mail;
using MFilesAPI.Extensions.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MFilesAPI.Extensions.Tests.Email.Configuration
{
	[TestClass]
	public class SmtpConfiguration
	{

		#region Network delivery configuration

		[TestMethod]
		public void NetworkDeliveryConfigurationValid()
		{
			// Create and configure our template.
			var smtpClient = new SmtpClient();
			var configuration = new Extensions.Email.SmtpConfiguration()
			{
				Credentials = new Credentials()
				{
					AccountName = "test",
					Password = "test password"
				},
				DefaultSender = new Extensions.Email.EmailAddress("devsupport@m-files.com"),
				UseEncryptedConnection = true,
				UseLocalPickupFolder = false,
				RequiresAuthentication = true,
				ServerAddress = "mail.mycompany.com"
			};

			// Push configuration to SmtpClient.
			configuration.ApplyConfigurationTo(smtpClient);
			
			// Ensure valid.
			Assert.AreEqual(configuration.UseEncryptedConnection, smtpClient.EnableSsl);
			Assert.AreEqual(SmtpDeliveryMethod.Network, smtpClient.DeliveryMethod);
			Assert.IsNotNull(smtpClient.Credentials, "Credentials not set");
			Assert.AreEqual
			(
				configuration.Credentials.AccountName,
				(smtpClient.Credentials as NetworkCredential)?.UserName
			);
			Assert.AreEqual
			(
				configuration.Credentials.Password,
				(smtpClient.Credentials as NetworkCredential)?.Password
			);
			Assert.AreEqual("mail.mycompany.com", smtpClient.Host);
			Assert.AreEqual(Extensions.Email.SmtpConfiguration.DefaultPort, smtpClient.Port);
		}

		[TestMethod]
		public void CustomPortPersisted()
		{
			// Create and configure our template.
			var smtpClient = new SmtpClient();
			var configuration = new Extensions.Email.SmtpConfiguration()
			{
				RequiresAuthentication = false,
				ServerAddress = "mail.mycompany.com",
				Port = 123
			};

			// Push configuration to SmtpClient.
			configuration.ApplyConfigurationTo(smtpClient);
			
			// Ensure valid.
			Assert.AreEqual(123, smtpClient.Port);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ZeroCustomPortThrows()
		{
			var configuration = new Extensions.Email.SmtpConfiguration();
			configuration.Port = 0;
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void NegativeCustomPortThrows()
		{
			var configuration = new Extensions.Email.SmtpConfiguration();
			configuration.Port = -1;
		}

		#endregion

		#region Local pickup folder configuration
		
		[TestMethod]
		public void PickupConfigurationValid()
		{
			// Create and configure our template.
			var smtpClient = new SmtpClient();
			var configuration = new Extensions.Email.SmtpConfiguration()
			{
				UseLocalPickupFolder = true,
				LocalPickupFolder = @"C:\"
			};

			// Push configuration to SmtpClient.
			configuration.ApplyConfigurationTo(smtpClient);
			
			// Ensure valid.
			Assert.AreEqual(SmtpDeliveryMethod.SpecifiedPickupDirectory, smtpClient.DeliveryMethod);
			Assert.AreEqual(@"C:\", smtpClient.PickupDirectoryLocation);
		}
		
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void EmptyPickupFolderThrows()
		{
			// Create and configure our template.
			var smtpClient = new SmtpClient();
			var configuration = new Extensions.Email.SmtpConfiguration()
			{
				UseLocalPickupFolder = true,
				LocalPickupFolder = string.Empty
			};

			// Push configuration to SmtpClient.
			configuration.ApplyConfigurationTo(smtpClient);
		}
		
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void InvalidFolderStringThrows()
		{
			// Create and configure our template.
			var smtpClient = new SmtpClient();
			var configuration = new Extensions.Email.SmtpConfiguration()
			{
				UseLocalPickupFolder = true,
				LocalPickupFolder = @"ABC:DEF\Something"
			};

			// Push configuration to SmtpClient.
			configuration.ApplyConfigurationTo(smtpClient);
		}

		#endregion
	}
}
