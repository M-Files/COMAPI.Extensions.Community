using System;

namespace MFilesAPI.Fakes
{
	/// <summary>
	/// A basic implementation of <see cref="MFilesAPI.ObjectClass"/>
	/// which defers all implementation to the provided <see cref="MFilesAPI.ObjectClassAdmin"/>.
	/// </summary>
	public class ObjectClassEx
		: MFilesAPI.ObjectClass
	{
		public ObjectClassAdmin ObjectClassAdmin { get; set; }
		public ObjectClassEx()
		{
		}

		public ObjectClassEx(ObjectClassAdmin objectClassAdmin)
		{
			this.ObjectClassAdmin = objectClassAdmin
				?? throw new ArgumentNullException(nameof(objectClassAdmin));
		}

		public ObjectClass Clone() => CloneHelper.Clone(this);

		public int ID { get => this.ObjectClassAdmin.ID; set => this.ObjectClassAdmin.ID = value; }
		public string Name { get => this.ObjectClassAdmin.Name; set => this.ObjectClassAdmin.Name = value; }
		public AssociatedPropertyDefs AssociatedPropertyDefs { get => this.ObjectClassAdmin.AssociatedPropertyDefs; set => this.ObjectClassAdmin.AssociatedPropertyDefs = value; }
		public int Workflow { get => this.ObjectClassAdmin.Workflow; set => this.ObjectClassAdmin.Workflow = value; }
		public int ObjectType { get => this.ObjectClassAdmin.ObjectType; set => this.ObjectClassAdmin.ObjectType = value; }
		public AccessControlList ACLForObjects { get => this.ObjectClassAdmin.ACLForObjects; set => this.ObjectClassAdmin.ACLForObjects = value; }
		public int NamePropertyDef { get => this.ObjectClassAdmin.NamePropertyDef; set => this.ObjectClassAdmin.NamePropertyDef = value; }
		public AutomaticPermissions AutomaticPermissionsForObjects { get => this.ObjectClassAdmin.AutomaticPermissionsForObjects; set => this.ObjectClassAdmin.AutomaticPermissionsForObjects = value; }
		public bool ForceWorkflow { get => this.ObjectClassAdmin.ForceWorkflow; set => this.ObjectClassAdmin.ForceWorkflow = value; }
		public AccessControlList AccessControlList { get; set; }

		public AdditionalClassInfo AdditionalClassInfo => this.ObjectClassAdmin.AdditionalClassInfo;

		public bool DefaultClass { get; set; }
	}
}
