using System;

namespace MFilesAPI.Fakes.ExtensionMethods
{
	public static class ObjectVersionExtensionMethods
	{
		public static void EnsureCheckedOut(this ObjectVersion objectVersion)
		{
			if (null == objectVersion)
				throw new ArgumentNullException(nameof(objectVersion));
			if (false == objectVersion.ObjectCheckedOut)
				throw new InvalidOperationException("Object is not checked out");
		}
	}
}
