using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MFilesAPI.Extensions.Tests.Email
{
	[TestClass]
	public class EmailAddress
	{
		[TestMethod]
		public void ConstructorWithNoParametersDoesNotThrow()
		{
			new Extensions.Email.EmailAddress();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void NullAddressThrows()
		{
			new Extensions.Email.EmailAddress(null);
		}
		
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EmptyAddressThrows()
		{
			new Extensions.Email.EmailAddress(string.Empty);
		}

		[TestMethod]
		public void ValidAddressDoesNotThrowAndIsReturnedAsExpected()
		{
			var emailAddress = new Extensions.Email.EmailAddress("devsupport@m-files.com");
			Assert.AreEqual("devsupport@m-files.com", emailAddress.Address);
			Assert.IsNull(emailAddress.DisplayName);
		}

		[TestMethod]
		public void NullDisplayNameDoesNotThrow()
		{
			var emailAddress = new Extensions.Email.EmailAddress(
				"devsupport@m-files.com", 
				null
			);
			Assert.IsNull(emailAddress.DisplayName);
		}

		[TestMethod]
		public void EmptyDisplayNameDoesNotThrow()
		{
			var emailAddress = new Extensions.Email.EmailAddress
			(
				"devsupport@m-files.com", 
				string.Empty
			);
			Assert.AreEqual(string.Empty, emailAddress.DisplayName);
		}

		[TestMethod]
		public void ValidDisplayNameDoesNotThrowAndIsReturnedAsExpected()
		{
			var emailAddress = new Extensions.Email.EmailAddress
			(
				"devsupport@m-files.com",
				"M-Files Developer Support"
			);
			Assert.AreEqual("M-Files Developer Support", emailAddress.DisplayName);
		}
	}
}
