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
		public ObjectClassAdminEx(ObjectClassAdmin objectClassAdmin)
		{
			this.ObjectClassAdmin = objectClassAdmin
				?? throw new ArgumentNullException(nameof(objectClassAdmin));
			this.ObjectClass = new ObjectClassEx(objectClassAdmin);
		}
	}
}
