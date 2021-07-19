using System;

namespace MFilesAPI.Fakes
{
	/// <summary>
	/// Used to hold a reference to the ObjectClass, as the standard API doesn't offer it.
	/// </summary>
	public class ObjectClassAdminEx
	{
		public ObjectClassAdmin ObjectClassAdmin { get; set; }
		public ObjectClassEx ObjectClass { get; set; }
		public static ObjectClassAdminEx CloneFrom(ObjectClassAdmin objectClassAdmin)
		{
			var clone = ComInterfaceAutoImpl.GetInstanceOfCompletedType<ObjectClassAdminEx>();
			clone.ObjectClassAdmin = objectClassAdmin
				?? throw new ArgumentNullException(nameof(objectClassAdmin));
			clone.ObjectClass = ObjectClassEx.CloneFrom(objectClassAdmin);
			return clone;
		}
	}
}
