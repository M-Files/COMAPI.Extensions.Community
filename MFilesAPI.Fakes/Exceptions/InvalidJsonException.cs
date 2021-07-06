using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAPI.Fakes.Exceptions
{
	public class InvalidJsonException
		: Exception
	{
		public InvalidJsonException(string message, JToken token = null)
			: base($"{message}{(token == null ? "" : $" at: {token.Path}" )}")
		{
		}
	}
}
