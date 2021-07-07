using System;
using System.Collections;
using System.Collections.Generic;

namespace MFilesAPI.Fakes
{
	public class ObjectFilesEx
		: List<ObjectFile>, ObjectFiles
	{
		IEnumerator IObjectFiles.GetEnumerator() => this.GetEnumerator();

		public void Sort(IObjectFileComparer ObjectFileComparer)
			=> base.Sort(new ObjectFileComparer(ObjectFileComparer));

		public ObjectFile GetObjectFileByNameForFileSystem(string NameForFileSystem)
		{
			throw new NotImplementedException();
		}

		public int GetObjectFileIndexByNameForFileSystem(string NameForFileSystem)
		{
			throw new NotImplementedException();
		}

		public string ToJSON()
		{
			throw new NotImplementedException();
		}

		public void FromJSON(string Json)
		{
			throw new NotImplementedException();
		}
	}
}
