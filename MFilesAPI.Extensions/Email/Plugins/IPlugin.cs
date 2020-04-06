using SimpleInjector;

namespace MFilesAPI.Extensions.Email.Plugins
{
	/// <summary>
	/// Represents a plugin that is able to send email.
	/// In most situations the <see cref="DefaultPlugin"/>, which uses <see cref="System.Net.Mail.SmtpClient"/> will suffice and will be automatically used.
	/// </summary>
	public interface IPlugin
	{
		/// <summary>
		/// Configures the dependency injection container.
		/// </summary>
		/// <param name="container">The container that should be populated.</param>
		void ConfigureContainer(Container container);
	}
}