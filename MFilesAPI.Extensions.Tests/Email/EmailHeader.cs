using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MFilesAPI.Extensions.Tests.Email
{
	[TestClass]
	public class EmailHeader
	{
		[TestMethod]
		public void ConstructorWithNoParametersDoesNotThrow()
		{
			new Extensions.Email.EmailHeader();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void NullNameNullValueThrowsAsExpected()
		{
			new Extensions.Email.EmailHeader(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EmptyNameNullValueThrowsAsExpected()
		{
			new Extensions.Email.EmailHeader(string.Empty, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void NullNameEmptyValueThrowsAsExpected()
		{
			new Extensions.Email.EmailHeader(null, string.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EmptyNameEmptyValueThrowsAsExpected()
		{
			new Extensions.Email.EmailHeader(string.Empty, string.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void NullNameThrowsAsExpected()
		{
			new Extensions.Email.EmailHeader(
				null,
				"yes"
			);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EmptyNameThrowsAsExpected()
		{
			new Extensions.Email.EmailHeader(
				string.Empty,
				"yes"
			);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void NullValueThrowsAsExpected()
		{
			new Extensions.Email.EmailHeader(
				"X-Sophos-SPX-Encrypt",
				null
			);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EmptyValueThrowsAsExpected()
		{
			new Extensions.Email.EmailHeader(
				"X-Sophos-SPX-Encrypt",
				string.Empty
			);
		}

		[TestMethod]
		public void ValidNameValidValueDoesNotThrowAndIsReturnedAsExpected()
		{
			var emailHeader = new Extensions.Email.EmailHeader
			(
				"X-Sophos-SPX-Encrypt",
				"yes"
			);
			Assert.AreEqual("X-Sophos-SPX-Encrypt", emailHeader.Name);
			Assert.AreEqual("yes", emailHeader.Value);
		}
	}
}
