using MFilesAPI.Fakes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MFilesAPI.Fakes
{
	public partial class VaultObjectOperations
		: MFilesAPI.VaultObjectPropertyOperations
	{

		#region VaultObjectPropertyOperations

		PropertyValues IVaultObjectPropertyOperations.GetProperties(ObjVer ObjVer, bool UpdateFromServer)
		{
			return this.GetObjectVersionAndProperties(ObjVer, UpdateFromServer)?
				.Properties
				?? throw new ArgumentException("Object not found", nameof(ObjVer));
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.SetProperties(ObjVer ObjVer, PropertyValues PropertyValues)
		{
			var objectVersionAndProperties = this.GetObjectVersionAndProperties(ObjVer, true)
				?? throw new ArgumentException("Object not found", nameof(ObjVer));

			objectVersionAndProperties.EnsureCheckedOut();

			if (PropertyValues != null)
			{
				foreach (var pv in PropertyValues.Cast<PropertyValue>())
				{
					var existing = objectVersionAndProperties.Properties.SearchForPropertyEx(pv.PropertyDef, true);
					if (null == existing)
					{
						// Add.
						objectVersionAndProperties.Properties.Add
						(
							objectVersionAndProperties.Properties.Count,
							pv
						);
					}
					else
					{
						// Update.
						existing.Value = pv.Value;
					}
				}
			}

			return objectVersionAndProperties;
		}

		PropertyValue IVaultObjectPropertyOperations.GetProperty(ObjVer ObjVer, int Property)
		{
			return this.GetObjectVersionAndProperties(ObjVer, true)?
				.Properties?
				.SearchForPropertyEx(Property, true)
				?? throw new ArgumentException("Object not found", nameof(ObjVer));
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.SetProperty(ObjVer ObjVer, PropertyValue PropertyValue)
		{
			var objectVersionAndProperties = this.GetObjectVersionAndProperties(ObjVer, true)
				?? throw new ArgumentException("Object not found", nameof(ObjVer));

			objectVersionAndProperties.EnsureCheckedOut();

			var existing = objectVersionAndProperties.Properties.SearchForPropertyEx(PropertyValue.PropertyDef, true);
			if (null == existing)
			{
				objectVersionAndProperties.Properties.Add
				(
					objectVersionAndProperties.Properties.Count,
					PropertyValue
				);
			}
			else
			{
				existing.Value = PropertyValue.Value;
			}
			return objectVersionAndProperties;

		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.RemoveProperty(ObjVer ObjVer, int Property)
		{
			throw new NotImplementedException();
		}

		PropertyValuesForDisplay IVaultObjectPropertyOperations.GetPropertiesForDisplay(ObjVer ObjVer, bool UpdateFromServer)
		{
			throw new NotImplementedException();
		}

		WorkflowAssignment IVaultObjectPropertyOperations.GetAssignment_DEPRECATED(ObjVer ObjVer, bool UpdateFromServer)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.SetAssignment_DEPRECATED(ObjVer ObjVer, WorkflowAssignment Assignment)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.MarkAssignmentComplete(ObjVer ObjVer)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.SetAllProperties(ObjVer ObjVer, bool AllowModifyingCheckedInObject, PropertyValues PropertyValues)
		{
			// Must be case because we need to set Properties.
			var objectVersionAndProperties = this.GetObjectVersionAndProperties(ObjVer, true) as ObjectVersionAndPropertiesEx
				?? throw new ArgumentException("Object not found", nameof(ObjVer));
			objectVersionAndProperties.Properties = PropertyValues;
			return objectVersionAndProperties;
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.SetVersionComment(ObjVer ObjVer, PropertyValue VersionComment)
		{
			throw new NotImplementedException();
		}

		VersionComment IVaultObjectPropertyOperations.GetVersionComment(ObjVer ObjVer)
		{
			throw new NotImplementedException();
		}

		VersionComments IVaultObjectPropertyOperations.GetVersionCommentHistory(ObjVer ObjVer)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.SetWorkflowState(ObjVer ObjVer, ObjectVersionWorkflowState WorkflowState)
		{
			throw new NotImplementedException();
		}

		ObjectVersionWorkflowState IVaultObjectPropertyOperations.GetWorkflowState(ObjVer ObjVer, bool UpdateFromServer)
		{
			throw new NotImplementedException();
		}

		string IVaultObjectPropertyOperations.GetPropertiesAsXML(ObjVer ObjVer, bool UpdateFromServer)
		{
			throw new NotImplementedException();
		}

		PropertyValuesOfMultipleObjects IVaultObjectPropertyOperations.GetPropertiesOfMultipleObjects(ObjVers ObjectVersions)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.SetCreationInfoAdmin(ObjVer ObjVer, bool UpdateCreatedBy, TypedValue CreatedBy, bool UpdateCreated, TypedValue CreatedUtc)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.SetLastModificationInfoAdmin(ObjVer ObjVer, bool UpdateLastModifiedBy, TypedValue LastModifiedBy, bool UpdateLastModified, TypedValue LastModifiedUtc)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndPropertiesOfMultipleObjects IVaultObjectPropertyOperations.SetPropertiesOfMultipleObjects(SetPropertiesParamsOfMultipleObjects SetPropertiesParamsOfObjects)
		{
			throw new NotImplementedException();
		}

		PropertyValuesWithIconClues IVaultObjectPropertyOperations.GetPropertiesWithIconClues(ObjVer ObjVer, bool UpdateFromServer)
		{
			throw new NotImplementedException();
		}

		PropertyValuesWithIconCluesOfMultipleObjects IVaultObjectPropertyOperations.GetPropertiesWithIconCluesOfMultipleObjects(ObjVers ObjectVersions)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.SetPropertiesWithPermissions(ObjVer ObjVer, PropertyValues PropertyValues, MFACLEnforcingMode ACLEnforcingMode, AccessControlList ACLProvided)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.SetAllPropertiesWithPermissions(ObjVer ObjVer, bool AllowModifyingCheckedInObject, PropertyValues PropertyValues, MFACLEnforcingMode ACLEnforcingMode, AccessControlList ACLProvided)
		{
			throw new NotImplementedException();
		}

		AccessControlList IVaultObjectPropertyOperations.GenerateAutomaticPermissionsFromPropertyValues(PropertyValues PropertyValues)
		{
			throw new NotImplementedException();
		}

		NamedValues IVaultObjectPropertyOperations.GetPropertiesForMetadataSync(ObjVer ObjVer, MFMetadataSyncFormat Format)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.SetWorkflowStateEx(ObjVer ObjVer, ObjectVersionWorkflowState WorkflowState, object ElectronicSignature)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.SetPropertiesWithPermissionsEx(ObjVer ObjVer, PropertyValues PropertyValues, MFACLEnforcingMode ACLEnforcingMode, AccessControlList ACLProvided, object ElectronicSignature)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.SetAllPropertiesWithPermissionsEx(ObjVer ObjVer, bool AllowModifyingCheckedInObject, PropertyValues PropertyValues, MFACLEnforcingMode ACLEnforcingMode, AccessControlList ACLProvided, object ElectronicSignature)
		{
			throw new NotImplementedException();
		}

		PropertyValues IVaultObjectPropertyOperations.CreatePropertiesFromFileInformation(FileInformation FileInformation)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.SetWorkflowStateTransition(ObjVer ObjVer, int Workflow, int lStateTransition, string lVersionComment)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.SetWorkflowStateTransitionEx(ObjVer ObjVer, int Workflow, int StateTransition, string VersionComment, object ElectronicSignature)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.MarkAssignmentCompleteByUser(ObjVer ObjVer, int UserID)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.ApproveOrRejectAssignment(ObjVer ObjVer, bool Approve)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.ApproveOrRejectAssignmentByUser(ObjVer ObjVer, bool Approve, int UserID)
		{
			throw new NotImplementedException();
		}

		PropertyValuesOfMultipleObjectsForDisplay IVaultObjectPropertyOperations.GetPropertiesOfMultipleObjectsForDisplay(ObjVers ObjectVersions)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.MarkForArchiving(ObjID ObjID)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectPropertyOperations.ClearArchivingMarker(ObjID ObjID)
		{
			throw new NotImplementedException();
		}

		EmbeddedMetadataProperties IVaultObjectPropertyOperations.GetPropertiesForMetadataSyncEx(ObjVer ObjVer)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
