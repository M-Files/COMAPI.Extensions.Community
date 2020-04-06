using System;
using System.Runtime.CompilerServices;
using MFilesAPI.Extensions.Email.Plugins;
using SimpleInjector;

namespace MFilesAPI.Extensions.Email
{
	/// <summary>
	/// A factory for creating instances of objects used in sending email, primarily <see cref="IEmailMessage"/> via <see cref="CreateEmailMessage"/>.
	/// </summary>
	public static class EmailMessageFactory
	{
		/// <summary>
		/// The DI container.
		/// </summary>
		private static Container container = new Container();

		/// <summary>
		/// Configures the email message factory for typical use via the default plugin.
		/// </summary>
		static EmailMessageFactory()
		{
			// Allow registrations to be overridden.
			EmailMessageFactory.container.Options.AllowOverridingRegistrations = true;

			// Register the default implementations with the container.
			EmailMessageFactory.ConfigurePlugin
			(
				Plugins.DefaultPlugin.Plugin.Instance
			);
		}

		/// <summary>
		/// Registers a specific plugin to use for sending emails.
		/// </summary>
		/// <param name="plugin">The plugin to use.</param>
		/// <remarks>Calls <see cref="IPlugin.ConfigureContainer(Container)"/> to do the registration.</remarks>
		public static void ConfigurePlugin(IPlugin plugin)
		{
			// Sanity.
			if (null == plugin)
				throw new ArgumentNullException(nameof(plugin));

			lock (EmailMessageFactory.container)
			{
				// Create a new container.
				var newContainer = new Container();

				// Ask the plugin to configure the container.
				plugin.ConfigureContainer(newContainer);

				// Update references.
				EmailMessageFactory.container = newContainer;
			}
		}

		/// <summary>
		/// Creates an <see cref="IEmailMessage"/> instance that can be used to send email.
		/// </summary>
		/// <returns>The instance to use for sending email.</returns>
		/// <remarks><see cref="IEmailMessage"/> inherits from <see cref="IDisposable"/>, so ensure that you dispose of the object correctly to free resources.</remarks>
		public static IEmailMessage CreateEmailMessage(SmtpConfiguration configuration)
		{
			// Ensure that the configuration is valid.
			if (null == configuration)
				throw new ArgumentNullException(nameof(configuration));
			if(null == configuration.DefaultSender)
				throw new ArgumentException("The default sender cannot be null.");

			// Get an instance of the registered IEmailMessage instance.
			var emailMessage = EmailMessageFactory.container.GetInstance<IEmailMessage>();

			// Set the configuration.
			emailMessage.Configuration = configuration;

			// Return the email message for use.
			return emailMessage;
		}
	}
}