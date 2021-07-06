using MFilesAPI.Fakes.ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MFilesAPI.Fakes
{
	public partial class VaultPropertyDefOperations
		: InMemory.RepositoryBase<PropertyDefAdmin, PropertyDef>, MFilesAPI.VaultPropertyDefOperations
	{
		/// <summary>
		/// Retrieves the <see cref="MFilesAPI.ObjType"/> from an <see cref="MFilesAPI.ObjTypeAdmin"/> reference.
		/// </summary>
		private static Func<PropertyDefAdmin, PropertyDef> DefaultConvert => (a) =>
		{
			return a?.PropertyDef;
		};

		public VaultPropertyDefOperations()
			: base(VaultPropertyDefOperations.DefaultConvert)
		{
		}

		#region MFilesAPI.VaultPropertyDefOperations

		PropertyDefs IVaultPropertyDefOperations.GetPropertyDefs()
		{
			var items = new PropertyDefs();
			foreach (var item in this.Select(kvp => this.Convert(kvp.Value)))
				items.Add(items.Count + 1, item);
			return items;
		}

		PropertyDef IVaultPropertyDefOperations.GetPropertyDef(int PropertyDef)
			=> this.Convert(this[PropertyDef]);

		PropertyDef IVaultPropertyDefOperations.GetBuiltInPropertyDef(MFBuiltInPropertyDef PropertyDefType)
			=> this.Convert(this[(int)PropertyDefType]);

		PropertyDefAdmin IVaultPropertyDefOperations.AddPropertyDefAdmin(PropertyDefAdmin PropertyDefAdmin, TypedValue ResetLastUsedValue)
		{
			PropertyDefAdmin.PropertyDef.ID = this.Add(PropertyDefAdmin);
			return PropertyDefAdmin;
		}

		void IVaultPropertyDefOperations.RemovePropertyDefAdmin(int PropertyDef, bool CopyToAnotherPropertyDef, int TargetPropertyDef, bool Append, bool DeleteFromClassesIfNecessary)
			=> this.TryRemove(PropertyDef, out _);

		void IVaultPropertyDefOperations.UpdatePropertyDefAdmin(PropertyDefAdmin PropertyDefAdmin, TypedValue ResetLastUsedValue)
			=> this.Update(PropertyDefAdmin.PropertyDef.ID, PropertyDefAdmin);

		MFilesAPI.PropertyDefsAdmin IVaultPropertyDefOperations.GetPropertyDefsAdmin()
		{
			var items = new PropertyDefsAdmin();
			foreach (var item in this.Values)
				items.Add(items.Count + 1, item);
			return items;
		}

		PropertyDefAdmin IVaultPropertyDefOperations.GetPropertyDefAdmin(int PropertyDef)
			=> this[PropertyDef];

		void IVaultPropertyDefOperations.UpdatePropertyDefWithAutomaticPermissionsAdmin(PropertyDefAdmin PropertyDefAdmin, TypedValue ResetLastUsedValue, bool AutomaticPermissionsForcedActive) { }

		int IVaultPropertyDefOperations.GetPropertyDefIDByAlias(string Alias)
		{
			// Get all items with the defined alias.
			var matches = this.Where(v => v.Value.SemanticAliases.GetAliasesFromValue().Any(a => a == Alias)).ToList();

			// Only one match allowed.
			return matches.Count == 1
				? matches[0].Key
				: -1;
		}

		int IVaultPropertyDefOperations.GetPropertyDefIDByGUID(string PropertyDefGUID)
		{
			// Get all items with the defined guid.
			var matches = this.Where(v => v.Value.PropertyDef.GUID == PropertyDefGUID).ToList();

			// Only one match allowed.
			return matches.Count == 1
				? matches[0].Key
				: -1;
		}

		void IVaultPropertyDefOperations.Recalculate(int PropertyDef, bool RecalculateCurrentlyEmptyValuesOnly) { }

		#endregion

		/// <summary>
		/// Simple implementation of <see cref="MFilesAPI.ObjectClassesAdmin"/>, as
		/// the default one has no "Add" method.
		/// </summary>
		protected class PropertyDefsAdmin
			: Dictionary<int, PropertyDefAdmin>, MFilesAPI.PropertyDefsAdmin, IEnumerable<PropertyDefAdmin>
		{
			IEnumerator IPropertyDefsAdmin.GetEnumerator() => this.Values.GetEnumerator();

			IEnumerator<PropertyDefAdmin> IEnumerable<PropertyDefAdmin>.GetEnumerator() => this.Values.GetEnumerator();

			int IPropertyDefsAdmin.Count => this.Count;

			PropertyDefAdmin IPropertyDefsAdmin.this[int Index] => this[Index];
		}
	}
}
