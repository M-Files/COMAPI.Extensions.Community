using MFilesAPI.Fakes.ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static MFilesAPI.Fakes.VaultClassOperations;

namespace MFilesAPI.Fakes
{
	public partial class VaultClassOperations
		: InMemory.RepositoryBase<ObjectClassAdminEx, ObjectClassEx>, MFilesAPI.VaultClassOperations
	{
		/// <summary>
		/// Retrieves the <see cref="MFilesAPI.ObjectClass"/> from an <see cref="MFilesAPI.ObjectClassAdmin"/> reference.
		/// </summary>
		private static Func<ObjectClassAdminEx, ObjectClassEx> DefaultConvert => (a) =>
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
			var item = ObjectClassAdminEx.CloneFrom(ObjectClassAdmin);
			item.ObjectClassAdmin.ID = this.Add(item);
			return item.ObjectClassAdmin;
		}

		void IVaultClassOperations.RemoveObjectClassAdmin(int ObjectClassID)
			=> this.TryRemove(ObjectClassID, out _);

		void IVaultClassOperations.UpdateObjectClassAdmin(ObjectClassAdmin ObjectClass)
			=> this.Update(ObjectClass.ID, ObjectClassAdminEx.CloneFrom(ObjectClass));

		MFilesAPI.ObjectClassesAdmin IVaultClassOperations.GetAllObjectClassesAdmin()
		{
			var objectClasses = ComInterfaceAutoImpl.GetInstanceOfCompletedType<ObjectClassesAdmin>();
			foreach (var item in this.Values)
				objectClasses.Add(objectClasses.Count + 1, item.ObjectClassAdmin);
			return objectClasses;
		}

		MFilesAPI.ObjectClassesAdmin IVaultClassOperations.GetObjectClassesAdmin(int ObjectType)
		{
			var objectClasses = ComInterfaceAutoImpl.GetInstanceOfCompletedType<ObjectClassesAdmin>();
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
	}
}
