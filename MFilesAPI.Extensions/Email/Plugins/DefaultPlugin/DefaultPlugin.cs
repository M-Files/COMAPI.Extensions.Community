using SimpleInjector;

namespace MFilesAPI.Extensions.Email.Plugins.DefaultPlugin
{
	/// <summary>
	/// The default plugin uses <see cref="System.Net.Mail.SmtpClient"/> to send emails.
	/// </summary>
	public sealed partial class Plugin
		: IPlugin
	{
		#region Implementation of IPlugin

		/// <inheritdoc />
		public void ConfigureContainer(Container container)
		{
			// Register the default implementations.
			container.Register<IEmailMessage, EmailMessage>(Lifestyle.Transient);
		}

		#endregion

		/// <summary>
		/// Instantiates a <see cref="Plugin"/>.
		/// Should use <see cref="Instance"/> insead of creating new versions, so marked as private.
		/// </summary>
		private Plugin()
		{
		}

		/// <summary>
		/// The instance of the <see cref="Plugin"/> for sending email.
		/// </summary>
		public static readonly Plugin Instance = new Plugin();
	}
}