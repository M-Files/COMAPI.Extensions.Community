using MFilesAPI.Extensions.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
	}
}
