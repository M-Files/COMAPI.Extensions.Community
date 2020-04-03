using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAPI.Extensions
{
	public static class FileTransfers
	{
		/// <summary>
		/// The block size to use for downloads, if none is explicitly specified.
		/// </summary>
		public const int DefaultBlockSize = 81920;

		/// <summary>
		/// The mininum block size to use for uploads, according to the documentation.
		/// </summary>
		/// <remarks>1KB</remarks>
		public const int MinimumUploadBlockSize = 1024;

		/// <summary>
		/// The maximum block size to use for uploads, according to the documentation.
		/// </summary>
		/// <remarks>4MB</remarks>
		public const int MaximumUploadBlockSize = 1024 * 1024 * 4;
	}
}
