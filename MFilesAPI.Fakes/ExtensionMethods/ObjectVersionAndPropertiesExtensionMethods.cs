using System;

namespace MFilesAPI.Fakes.ExtensionMethods
{
	public static class ObjectVersionAndPropertiesExtensionMethods
	{
		public static void EnsureCheckedOut(this ObjectVersionAndProperties objectVersionAndProperties)
		{
			if (null == objectVersionAndProperties)
				throw new ArgumentNullException(nameof(objectVersionAndProperties));
			if (null == objectVersionAndProperties?.VersionData)
				throw new ArgumentException("Version data cannot be null", nameof(objectVersionAndProperties));

			// Ensure it's checked out.
			objectVersionAndProperties.VersionData.EnsureCheckedOut();
		}
	}
}
