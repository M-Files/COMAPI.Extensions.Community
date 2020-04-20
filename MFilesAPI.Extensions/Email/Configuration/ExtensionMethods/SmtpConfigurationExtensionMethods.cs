using System;
using System.IO;
using System.Net.Mail;

namespace MFilesAPI.Extensions.Email
{
	/// <summary>
	/// Extension methods for working with <see cref="SmtpConfiguration"/> instances.
	/// </summary>
	public static class SmtpConfigurationExtensionMethods
	{
		/// <summary>
		/// Applies the configuration from <paramref name="configuration"/> 
		/// to <paramref name="client"/>.
		/// </summary>
		/// <param name="configuration">The configuration to use.</param>
		/// <param name="client">The <see cref="SmtpClient"/> to configure.</param>
		public static void ApplyConfigurationTo
		(
			this SmtpConfiguration configuration, 
			SmtpClient client
		)
		{
			// Sanity.
			if (null == client)
				throw new ArgumentNullException(nameof(client));
			if (null == configuration)
				throw new ArgumentNullException(nameof(configuration));

			// Should we use the local pickup folder?
			if (configuration.UseLocalPickupFolder)
			{
				// We will write the item to disk for subsequent pickup.
				if (string.IsNullOrWhiteSpace(configuration.LocalPickupFolder))
					throw new InvalidOperationException(
						"The local pickup folder is not set, so cannot be used to send emails.");

				// Validate the folder.
				DirectoryInfo folder;
				try
				{
					folder = new DirectoryInfo(configuration.LocalPickupFolder);

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
								$"The local pickup folder ({configuration.LocalPickupFolder}) does not exist and could not be created, so cannot be used to send emails.",
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
						$"The local pickup folder is of an invalid format ({configuration.LocalPickupFolder}), so cannot be used to send emails.",
						e
					);
				}

				// Set up the delivery data.
				client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
				client.PickupDirectoryLocation = folder.FullName;

				// There was a bug in System.Net.Mail.MailMessage.Send at one point, where a null host
				// would throw an exception, so let's set it to the configured data, or localhost.
				client.Host = string.IsNullOrWhiteSpace(configuration.ServerAddress)
					? "localhost"
					: configuration.ServerAddress;
				client.Port = configuration.Port;
			}
			else
			{
				// We are going to send directly.
				// Set up the delivery data.
				client.DeliveryMethod = SmtpDeliveryMethod.Network;
				client.Host = configuration.ServerAddress;
				client.Port = configuration.Port;

				// If we need to use an encrypted connection then configure that.
				client.EnableSsl = configuration.UseEncryptedConnection;

				// If we need to use authentication then configure that.
				if (configuration.RequiresAuthentication)
				{
					client.Credentials = new System.Net.NetworkCredential
					(
						configuration.Credentials.AccountName,
						configuration.Credentials.Password
					);
				}
			}
			
		}
	}
}