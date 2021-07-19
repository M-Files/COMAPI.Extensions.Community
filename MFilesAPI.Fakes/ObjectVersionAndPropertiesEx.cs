using System;

namespace MFilesAPI.Fakes
{
	public class ObjectVersionAndPropertiesEx
		: ObjectVersionAndProperties
	{
		public ObjectVersionAndPropertiesEx()
		{
		}
		public ObjectVersionAndPropertiesEx(ObjectVersionAndProperties ovap)
			: this()
		{
			if (null == ovap)
				throw new ArgumentNullException(nameof(ovap));
			this.ObjVer = ovap.ObjVer;
			this.Properties = ovap.Properties;
			this.Vault = ovap.Vault;
			this.VersionData = ObjectVersionEx.CloneFrom(ovap.VersionData);
		}
		public ObjectVersionAndPropertiesEx CreateNewVersion(int? newVersionNumber = null)
		{
			var clone = this.Clone();
			clone.ObjVer.Version = newVersionNumber ?? (clone.ObjVer.Version + 1);
			clone.VersionData.ObjVer = clone.ObjVer;
			return clone;
		}

		public ObjectVersionAndPropertiesEx Clone()
		{
			return CloneHelper.Clone(this);
		}

		public ObjVer ObjVer { get; set; }

		public ObjectVersionEx VersionData { get; set; }

		public PropertyValues Properties { get; set; }

		public MFilesAPI.Vault Vault { get; set; }

		ObjectVersionAndProperties IObjectVersionAndProperties.Clone() => this.Clone();

		ObjectVersion IObjectVersionAndProperties.VersionData => this.VersionData;
	}
}
