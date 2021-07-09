using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MFilesAPI.Fakes
{
	/// <summary>
	/// Simple implementation of <see cref="MFilesAPI.ObjectClassesAdmin"/>, as
	/// the default one has no "Add" method.
	/// </summary>
	public class ObjectClassesAdmin
		: Dictionary<int, ObjectClassAdmin>, MFilesAPI.ObjectClassesAdmin
	{
		IEnumerator IObjectClassesAdmin.GetEnumerator() => this.GetEnumerator();

		void IObjectClassesAdmin.Remove(int Index)
			=> this.Remove(this.Keys.ToList()[Index]);

		public MFilesAPI.ObjectClassesAdmin Clone() => CloneHelper.Clone(this);
	}
}
