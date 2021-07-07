using System;

namespace MFilesAPI.Fakes
{
	public class ObjIDEx
		: ObjIDClass
	{
		public ObjIDEx() { }
		public ObjIDEx(ObjID objID)
		{
			if (null == objID)
				throw new ArgumentNullException(nameof(objID));
			this.Type = objID.Type;
			this.ID = objID.ID;
		}
		public override int GetHashCode()
		{
			// Cantor's pairing without the *.5.
			return ((this.Type + this.ID) * (this.Type + this.ID + 1)) + (this.Type * this.ID);
		}
	}
}
