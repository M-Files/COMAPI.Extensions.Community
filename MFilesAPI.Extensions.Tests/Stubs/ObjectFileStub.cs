using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFilesAPI.Extensions.Tests.Stubs
{
	internal class ObjectFileStub
		: ObjectFile
	{
		#region Implementation of IObjectFile

		/// <inheritdoc />
		public string GetNameForFileSystem()
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public string ToJSON()
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public void FromJSON(string Json)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public int ID { get; set; }

		/// <inheritdoc />
		public int Version { get; set; }

		/// <inheritdoc />
		public string Title { get; set; }

		/// <inheritdoc />
		public string Extension { get; set; }

		/// <inheritdoc />
		public long LogicalSize { get; set; }

		/// <inheritdoc />
		public int LogicalSize_32bit { get; set; }

		/// <inheritdoc />
		public DateTime CreationTimeUtc { get; set; }

		/// <inheritdoc />
		public DateTime LastAccessTimeUtc { get; set; }

		/// <inheritdoc />
		public DateTime LastWriteTimeUtc { get; set; }

		/// <inheritdoc />
		public DateTime ChangeTimeUtc { get; set; }

		/// <inheritdoc />
		public FileVer FileVer { get; set; }

		/// <inheritdoc />
		public string FileGUID { get; set; }

		/// <inheritdoc />
		public string ExternalRepositoryFileID { get; set; }

		/// <inheritdoc />
		public string ExternalRepositoryFileVersionID { get; set; }

		/// <inheritdoc />
		public FileSize LogicalSizeEx { get; set; }

		/// <inheritdoc />
		public bool VolatileContent { get; set; }

		#endregion
	}
}