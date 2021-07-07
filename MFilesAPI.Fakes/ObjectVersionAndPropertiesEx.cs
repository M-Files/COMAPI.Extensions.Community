namespace MFilesAPI.Fakes
{
	public class ObjectVersionAndPropertiesEx
		: ObjectVersionAndProperties
	{
		public ObjectVersionAndPropertiesEx CreateNewVersion(int? newVersionNumber = null)
		{
			var clone = this.Clone();
			clone.ObjVer.Version = newVersionNumber ?? (clone.ObjVer.Version + 1);
			clone.VersionData.ObjVer = clone.ObjVer;
			return clone;
		}

		public ObjectVersionAndPropertiesEx Clone()
		{
			// Clone the properties individually.
			return new ObjectVersionAndPropertiesEx()
			{
				ObjVer = this.ObjVer.Clone(),
				VersionData = this.VersionData.Clone(),
				Properties = this.Properties.Clone(),
				Vault = this.Vault
			};
		}

		public ObjVer ObjVer { get; set; }

		public ObjectVersionEx VersionData { get; set; }

		public PropertyValues Properties { get; set; }

		public MFilesAPI.Vault Vault { get; set; }

		ObjectVersionAndProperties IObjectVersionAndProperties.Clone() => this.Clone();

		ObjectVersion IObjectVersionAndProperties.VersionData => this.VersionData;
	}
}
