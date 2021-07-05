using MFilesAPI.Fakes.ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MFilesAPI.Fakes
{
	/// <summary>
	/// In-memory implementation of <see cref="MFilesAPI.VaultObjectTypeOperations"/>.
	/// </summary>
	public class VaultObjectTypeOperations
		: InMemory.RepositoryBase<ObjTypeAdmin, ObjType>, MFilesAPI.VaultObjectTypeOperations
	{
		/// <summary>
		/// Retrieves the <see cref="MFilesAPI.ObjType"/> from an <see cref="MFilesAPI.ObjTypeAdmin"/> reference.
		/// </summary>
		private static Func<ObjTypeAdmin, ObjType> DefaultConvert => (a) =>
		{
			return a?.ObjectType;
		};

		public VaultObjectTypeOperations()
			: base(VaultObjectTypeOperations.DefaultConvert)
		{
		}

		#region IVaultObjectTypeOperations

		ObjType IVaultObjectTypeOperations.GetBuiltInObjectType(MFBuiltInObjectType ObjectType)
			=> this.GetStandardInstance((int)ObjectType);

		ObjTypes IVaultObjectTypeOperations.GetObjectTypes()
		{
			var objectTypes = new ObjTypes();
			foreach (var objType in this.Select(kvp => this.Convert(kvp.Value)))
				objectTypes.Add(objectTypes.Count + 1, objType);
			return objectTypes;
		}

		ObjType IVaultObjectTypeOperations.GetObjectType(int ObjectType)
			=> this.GetStandardInstance(ObjectType);

		void IVaultObjectTypeOperations.RefreshExternalObjectType(int ObjectType, MFExternalDBRefreshType RefreshType) { }

		ObjTypeAdmin IVaultObjectTypeOperations.AddObjectTypeAdmin(ObjTypeAdmin ObjectType)
		{
			ObjectType.ObjectType.ID = this.Add(ObjectType);
			return ObjectType;
		}

		void IVaultObjectTypeOperations.RemoveObjectTypeAdmin(int ObjectTypeID)
		{
			this.TryRemove(ObjectTypeID, out _);
		}

		ObjTypeAdmin IVaultObjectTypeOperations.UpdateObjectTypeAdmin(ObjTypeAdmin ObjectType)
		{
			this.Update(ObjectType.ObjectType.ID, ObjectType);
			return ObjectType;
		}

		ObjTypesAdmin IVaultObjectTypeOperations.GetObjectTypesAdmin()
		{
			var objectTypes = new ObjectTypesAdmin();
			foreach (var objType in this.Values)
				objectTypes.Add(objectTypes.Count + 1, objType);
			return objectTypes;
		}

		ObjTypeAdmin IVaultObjectTypeOperations.GetObjectTypeAdmin(int ObjectType)
		{
			return this[ObjectType];
		}

		ObjTypeAdmin IVaultObjectTypeOperations.UpdateObjectTypeWithAutomaticPermissionsAdmin(ObjTypeAdmin ObjectType, bool AutomaticPermissionsForcedActive)
		{
			this.Update(ObjectType.ObjectType.ID, ObjectType);
			return ObjectType;
		}

		int IVaultObjectTypeOperations.GetObjectTypeIDByAlias(string Alias)
		{
			// Get all items with the defined alias.
			var matches = this.Where(v => v.Value.SemanticAliases.GetAliasesFromValue().Any(a => a == Alias)).ToList();

			// Only one match allowed.
			return matches.Count == 1
				? matches[0].Key
				: -1;
		}

		int IVaultObjectTypeOperations.GetObjectTypeIDByGUID(string ObjectTypeGUID)
		{
			// Get all items with the defined guid.
			var matches = this.Where(v => v.Value.ObjectType.GUID == ObjectTypeGUID).ToList();

			// Only one match allowed.
			return matches.Count == 1
				? matches[0].Key
				: -1;
		}

		ObjTypeAdmin IVaultObjectTypeOperations.RefreshExternalObjectTypeColumnMappingsAdmin(ObjTypeAdmin ObjectType)
		{
			return ObjectType;
		}

		#endregion

		/// <summary>
		/// Simple implementation of <see cref="MFilesAPI.ObjTypesAdmin"/>, as
		/// the default one has no "Add" method.
		/// </summary>
		protected class ObjectTypesAdmin
			: Dictionary<int, ObjTypeAdmin>, MFilesAPI.IObjectTypesAdmin, MFilesAPI.ObjTypesAdmin
		{
			IEnumerator IObjectTypesAdmin.GetEnumerator() => this.GetEnumerator();
		}
	}
}
