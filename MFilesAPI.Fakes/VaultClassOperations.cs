using MFilesAPI.Fakes.ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static MFilesAPI.Fakes.VaultClassOperations;

namespace MFilesAPI.Fakes
{
	public partial class VaultClassOperations
		: InMemory.RepositoryBase<WrappedObjectClassAdmin, ObjectClassEx>, MFilesAPI.VaultClassOperations
	{
		/// <summary>
		/// Retrieves the <see cref="MFilesAPI.ObjectClass"/> from an <see cref="MFilesAPI.ObjectClassAdmin"/> reference.
		/// </summary>
		private static Func<WrappedObjectClassAdmin, ObjectClassEx> DefaultConvert => (a) =>
		{
			return a.ObjectClass;
		};

		public VaultClassOperations()
			: base(VaultClassOperations.DefaultConvert)
		{
		}

		#region IVaultClassOperations

		ObjectClasses IVaultClassOperations.GetObjectClasses(int ObjectType)
		{
			var objectClasses = new ObjectClasses();
			foreach (var item in this.Where(kvp => kvp.Value.ObjectClassAdmin.ObjectType == ObjectType).Select(kvp => this.Convert(kvp.Value)))
				objectClasses.Add(objectClasses.Count + 1, item);
			return objectClasses;
		}

		ObjectClass IVaultClassOperations.GetObjectClass(int ObjectClass)
			=> this.GetStandardInstance(ObjectClass);

		ObjectClasses IVaultClassOperations.GetAllObjectClasses()
		{
			var objectClasses = new ObjectClasses();
			foreach (var item in this.Select(kvp => this.Convert(kvp.Value)))
				objectClasses.Add(objectClasses.Count + 1, item);
			return objectClasses;
		}

		ObjectClassAdmin IVaultClassOperations.AddObjectClassAdmin(ObjectClassAdmin ObjectClassAdmin)
		{
			var item = new WrappedObjectClassAdmin(ObjectClassAdmin);
			item.ObjectClassAdmin.ID = this.Add(item);
			return item.ObjectClassAdmin;
		}

		void IVaultClassOperations.RemoveObjectClassAdmin(int ObjectClassID)
			=> this.TryRemove(ObjectClassID, out _);

		void IVaultClassOperations.UpdateObjectClassAdmin(ObjectClassAdmin ObjectClass)
			=> this.Update(ObjectClass.ID, new WrappedObjectClassAdmin(ObjectClass));

		MFilesAPI.ObjectClassesAdmin IVaultClassOperations.GetAllObjectClassesAdmin()
		{
			var objectClasses = new ObjectClassesAdmin();
			foreach (var item in this.Values)
				objectClasses.Add(objectClasses.Count + 1, item.ObjectClassAdmin);
			return objectClasses;
		}

		MFilesAPI.ObjectClassesAdmin IVaultClassOperations.GetObjectClassesAdmin(int ObjectType)
		{
			var objectClasses = new ObjectClassesAdmin();
			foreach (var item in this.Values.Where(item => item.ObjectClassAdmin.ObjectType == ObjectType))
				objectClasses.Add(objectClasses.Count + 1, item.ObjectClassAdmin);
			return objectClasses;
		}

		ObjectClassAdmin IVaultClassOperations.GetObjectClassAdmin(int Class)
		{
			return this[Class]?.ObjectClassAdmin;
		}

		int IVaultClassOperations.GetObjectClassIDByAlias(string Alias)
		{
			// Get all items with the defined alias.
			var matches = this.Where(v => ((ObjectClassAdmin)v.Value).SemanticAliases.GetAliasesFromValue().Any(a => a == Alias)).ToList();

			// Only one match allowed.
			return matches.Count == 1
				? matches[0].Key
				: -1;
		}

		int IVaultClassOperations.GetObjectClassIDByGUID(string ObjectClassGUID)
		{
			// GUID is not in the API.  Can't do this.
			throw new NotImplementedException();
		}

		void IVaultClassOperations.UpdateObjectNames(int ObjectClassID) { }

		#endregion

		/// <summary>
		/// Used to hold a reference to the ObjectClass, as the standard API doesn't offer it.
		/// </summary>
		public class WrappedObjectClassAdmin
		{
			public ObjectClassAdmin ObjectClassAdmin { get; set; }
			public ObjectClassEx ObjectClass { get; set; }
			public WrappedObjectClassAdmin(ObjectClassAdmin objectClassAdmin)
			{
				this.ObjectClassAdmin = objectClassAdmin 
					?? throw new ArgumentNullException(nameof(objectClassAdmin));
				this.ObjectClass = new ObjectClassEx(objectClassAdmin);
			}
		}

		/// <summary>
		/// A basic implementation of <see cref="MFilesAPI.ObjectClass"/>
		/// which defers all implementation to the provided <see cref="MFilesAPI.ObjectClassAdmin"/>.
		/// </summary>
		public class ObjectClassEx
			: MFilesAPI.ObjectClass
		{
			protected ObjectClassAdmin ObjectClassAdmin { get; set; }

			public ObjectClassEx(ObjectClassAdmin objectClassAdmin)
			{
				this.ObjectClassAdmin = objectClassAdmin
					?? throw new ArgumentNullException(nameof(objectClassAdmin));
			}

			public ObjectClass Clone() => this;

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

		/// <summary>
		/// Simple implementation of <see cref="MFilesAPI.ObjectClassesAdmin"/>, as
		/// the default one has no "Add" method.
		/// </summary>
		protected class ObjectClassesAdmin
			: Dictionary<int, ObjectClassAdmin>, MFilesAPI.ObjectClassesAdmin
		{
			IEnumerator IObjectClassesAdmin.GetEnumerator() => this.GetEnumerator();

			void IObjectClassesAdmin.Remove(int Index)
				=> this.Remove(this.Keys.ToList()[Index]);

			public MFilesAPI.ObjectClassesAdmin Clone() => this;
		}
	}
}
