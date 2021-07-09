using System;
using System.Collections;
using System.Collections.Generic;

namespace MFilesAPI.Fakes
{
	public partial class VaultObjectOperations
		: Dictionary<string, List<ObjectVersionAndPropertiesEx>>, IRequiresVaultInstance
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
		public virtual void Add(IEnumerable<ObjectVersionAndProperties> objectVersionAndProperties)
		{
			if (null == objectVersionAndProperties)
				throw new ArgumentNullException(nameof(objectVersionAndProperties));
			foreach (var item in objectVersionAndProperties)
				this.Add(item);
		}
		public virtual void Add(ObjectVersionAndProperties objectVersionAndProperties)
		{
			if (null == objectVersionAndProperties)
				throw new ArgumentNullException(nameof(objectVersionAndProperties));
			var objID = objectVersionAndProperties?.ObjVer?.ObjID;
			if (null == objectVersionAndProperties)
				throw new ArgumentException("Object version does not contain an ID.", nameof(objectVersionAndProperties));

			if (this.ContainsKey(objID))
				throw new ArgumentException("Object with that type and ID already exists.", nameof(objectVersionAndProperties));
			base.Add(objID.ToJSON(), new List<ObjectVersionAndPropertiesEx>(){
				new ObjectVersionAndPropertiesEx(objectVersionAndProperties)
			});

		}
		public List<ObjectVersionAndPropertiesEx> this[ObjID objID]
		{
			get => base[objID.ToJSON()];
			set => base[objID.ToJSON()] = value;
		}
		public bool ContainsKey(ObjID objID)
		{
			return base.ContainsKey(objID.ToJSON());
		}
		public void Remove(ObjID objID)
		{
			this.Remove(objID.ToJSON());
		}
		public Vault Vault { get; set; }
		public VaultObjectOperations()
			: base()
		{
		}
	}
}
