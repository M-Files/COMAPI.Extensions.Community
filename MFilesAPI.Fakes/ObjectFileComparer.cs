using System;
using System.Collections.Generic;

namespace MFilesAPI.Fakes
{
	internal class ObjectFileComparer
		: IComparer<ObjectFile>
	{
		public IObjectFileComparer Comparer { get; set; }
		public ObjectFileComparer(IObjectFileComparer objectFileComparer)
		{
			this.Comparer = objectFileComparer
				?? throw new ArgumentNullException(nameof(objectFileComparer));
		}

		public int Compare(ObjectFile x, ObjectFile y)
		{
			return this.Comparer.Compare(x, y);
		}
	}
}
