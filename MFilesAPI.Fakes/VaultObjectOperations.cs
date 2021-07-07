using System;
using System.Collections;
using System.Collections.Generic;

namespace MFilesAPI.Fakes
{
	public partial class VaultObjectOperations
		: Dictionary<ObjIDEx, List<ObjectVersionAndPropertiesEx>>, IRequiresVaultInstance
	{
		private object _lock = new object();
		protected Dictionary<int, int> IDCounters { get; } = new Dictionary<int, int>();
		protected int CreateNewObjectId(int objectType)
		{
			lock (_lock)
			{
				if (false == this.IDCounters.ContainsKey(objectType))
					this.IDCounters.Add(objectType, 0);
				var newId = this.IDCounters[objectType] + 1;
				this.IDCounters[objectType] = newId;
				return newId;
			}
		}
		public List<ObjectVersionAndPropertiesEx> this[ObjID objID]
		{
			get => this[new ObjIDEx(objID)];
			set => this[new ObjIDEx(objID)] = value;
		}
		public bool ContainsKey(ObjID objID)
		{
			return this.ContainsKey(new ObjIDEx(objID));
		}
		public List<ObjectVersionAndPropertiesEx> Remove(ObjID objID)
		{
			return this.Remove(new ObjIDEx(objID));
		}
		public Vault Vault { get; set; }
		public VaultObjectOperations()
			: base()
		{
		}
	}
}
