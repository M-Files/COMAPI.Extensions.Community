using System;
using System.Collections.Generic;
using System.Linq;

namespace MFilesAPI.Fakes
{
	public partial class VaultObjectOperations
		: MFilesAPI.VaultObjectOperations
	{

		protected virtual ObjectVersionAndProperties GetObjectVersionAndProperties(ObjVer ObjVer, bool UpdateFromServer)
		{
			if (null == ObjVer)
				throw new ArgumentNullException(nameof(ObjVer));
			return (this.ContainsKey(ObjVer?.ObjID)
				? this[ObjVer.ObjID].FirstOrDefault(v => v.ObjVer.Version == ObjVer.Version)
				: null)
				?? throw new ArgumentException("Object not found", nameof(ObjVer));
		}
		protected virtual ObjectVersionAndProperties GetLatestObjectVersionAndProperties(ObjID ObjID, bool AllowCheckedOut, bool UpdateFromServer)
		{
			if (null == ObjID)
				throw new ArgumentNullException(nameof(ObjID));
			return (this.ContainsKey(ObjID)
				? this[ObjID]
					.Where(v => AllowCheckedOut || v.VersionData.ObjectCheckedOut == false)
					.OrderByDescending(v => v.ObjVer.Version)
					.FirstOrDefault()
				: null)
				?? throw new ArgumentException("Object not found", nameof(ObjID));
		}

		#region MFilesAPI.VaultObjectOperations

		ObjectVersionAndProperties IVaultObjectOperations.CreateNewObject(int ObjectType, PropertyValues PropertyValues, SourceObjectFiles SourceObjectFiles, AccessControlList AccessControlList)
		{
			if (null == PropertyValues)
				throw new ArgumentNullException(nameof(PropertyValues));
			if (null == SourceObjectFiles)
				throw new ArgumentNullException(nameof(SourceObjectFiles));
			return ((IVaultObjectOperations)this)
				.CreateNewObjectEx
				(
					ObjectType, 
					PropertyValues, 
					SourceObjectFiles, 
					SFD: (ObjectType == (int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument && SourceObjectFiles.Count == 1),
					CheckIn: true,
					AccessControlList: AccessControlList
				);
		}

		ObjectVersion IVaultObjectOperations.CheckIn(ObjVer ObjVer)
		{
			throw new NotImplementedException();
		}

		ObjectVersion IVaultObjectOperations.CheckOut(ObjID ObjID)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectOperations.GetObjectVersionAndProperties(ObjVer ObjVer, bool UpdateFromServer)
			=> this.GetObjectVersionAndProperties(ObjVer, UpdateFromServer);

		ObjectVersionAndProperties IVaultObjectOperations.GetLatestObjectVersionAndProperties(ObjID ObjID, bool AllowCheckedOut, bool UpdateFromServer)
			=> this.GetLatestObjectVersionAndProperties(ObjID, AllowCheckedOut, UpdateFromServer);

		bool IVaultObjectOperations.IsCheckedOut(ObjID ObjID, bool UpdateFromServer)
		{
			return this.GetLatestObjectVersionAndProperties(ObjID, true, UpdateFromServer)?
				.VersionData?
				.ObjectCheckedOut
				?? throw new ArgumentException("Object not found", nameof(ObjID));
		}

		bool IVaultObjectOperations.IsSingleFileObject(ObjVer ObjVer)
		{
			return this.GetObjectVersionAndProperties(ObjVer, true)?
				.VersionData?
				.SingleFile
				?? throw new ArgumentException("Object not found", nameof(ObjVer));
		}

		void IVaultObjectOperations.SetSingleFileObject(ObjVer ObjVer, bool SingleFile)
		{
			if (null == ObjVer)
				throw new ArgumentNullException(nameof(ObjVer));

			// This has to be case because otherwise we can't set SingleFile below.
			var version = this.GetObjectVersionAndProperties(ObjVer, true) as ObjectVersionAndPropertiesEx
				?? throw new ArgumentException("Object not found", nameof(ObjVer));

			// Must be checked out.
			if (false == version.VersionData.ObjectCheckedOut)
				throw new ArgumentException("Object not checked out", nameof(ObjVer));
			version.VersionData.SingleFile = SingleFile;
		}

		void IVaultObjectOperations.DestroyObject(ObjID ObjID, bool DestroyAllVersions, int ObjectVersion)
		{
			if (null == ObjID)
				throw new ArgumentNullException(nameof(ObjID));
			if (false == this.ContainsKey(ObjID))
				throw new ArgumentException("Object ID not found", nameof(ObjID));

			// All versions is easy.
			if (DestroyAllVersions)
			{
				this.Remove(ObjID);
				return;
			}

			// Remove just a specific version.
			var allversions = this[ObjID];
			allversions.RemoveAll(v => v.ObjVer.Version == ObjectVersion);
		}

		ObjectVersion IVaultObjectOperations.Rollback(ObjID ObjID, int RollbackToVersion)
		{
			// Get all the versions.
			var objectVersions = this.ContainsKey(ObjID)
				? this[ObjID]
				: throw new ArgumentException("Object not found", nameof(ObjID));

			// Get the version to roll back to.
			var rollbackTarget = objectVersions.FirstOrDefault(v => v.ObjVer.Version == RollbackToVersion);
			if (null == rollbackTarget)
				throw new ArgumentException("Cannot find that object version", nameof(RollbackToVersion));

			// Create new copy and add it.
			var clone = rollbackTarget.CreateNewVersion(objectVersions.Max(v => v.ObjVer.Version) + 1);
			objectVersions.Add(clone);

			return clone.VersionData;

		}

		ObjectVersion IVaultObjectOperations.UndeleteObject(ObjID ObjID)
		{
			// Get all the versions.
			var objectVersions = this.ContainsKey(ObjID)
				? this[ObjID]
				: throw new ArgumentException("Object not found", nameof(ObjID));

			// Get latest version.
			var latestVersion = objectVersions
				.OrderByDescending(v => v.ObjVer.Version)
				.FirstOrDefault();
			if (null == latestVersion)
				throw new ArgumentException("Cannot find any object versions", nameof(ObjID));

			// The latest version should be deleted.
			if (false == latestVersion.VersionData.Deleted)
				throw new ArgumentException("Object is not deleted", nameof(ObjID));

			latestVersion.VersionData.Deleted = false;
			return latestVersion.VersionData;
		}

		ObjectVersion IVaultObjectOperations.UndoCheckout(ObjVer ObjVer)
		{
			// Get all the versions.
			var objectVersions = this.ContainsKey(ObjVer.ObjID)
				? this[ObjVer.ObjID]
				: throw new ArgumentException("Object not found", nameof(ObjVer));

			// Get latest version.
			var latestVersion = objectVersions
				.OrderByDescending(v => v.ObjVer.Version)
				.FirstOrDefault();
			if (null == latestVersion)
				throw new ArgumentException("Cannot find any object versions", nameof(ObjVer));
			if (latestVersion.ObjVer.Version != ObjVer.Version)
				throw new ArgumentException("Object version mismatch", nameof(ObjVer));

			// The latest version should be checked out.
			if (false == latestVersion.VersionData.ObjectCheckedOut)
				throw new ArgumentException("Object is not checked out", nameof(ObjVer));

			// Remove the checked out version.
			objectVersions.Remove(latestVersion);

			return latestVersion.VersionData;
		}

		ObjectVersions IVaultObjectOperations.GetHistory(ObjID ObjID)
		{
			// Get all the versions.
			var objectVersions = this.ContainsKey(ObjID)
				? this[ObjID]
				: throw new ArgumentException("Object not found", nameof(ObjID));

			// Add to the expected collection.
			var x = new ObjectVersions();
			foreach (var v in objectVersions)
				x.Add(x.Count, v.VersionData);
			return x;
		}

		ObjectVersion IVaultObjectOperations.GetObjectInfo(ObjVer ObjVer, bool LatestVersion, bool UpdateFromServer)
		{
			if (null == ObjVer)
				throw new ArgumentNullException(nameof(ObjVer));

			// Get all the versions.
			var versions = this.ContainsKey(ObjVer.ObjID)
				? this[ObjVer.ObjID]
				: null;
			if (null == versions)
				throw new ArgumentException("Object not found", nameof(ObjVer));

			// Get the requested version.
			var exactVersion = LatestVersion
				? versions.OrderByDescending(v => v.ObjVer.Version).FirstOrDefault()
				: versions.FirstOrDefault(v => v.ObjVer.Version == ObjVer.Version);
			if (null == exactVersion)
				throw new ArgumentException("Object version not found", nameof(ObjVer));
			return exactVersion.VersionData;
		}

		ObjectVersionAndProperties IVaultObjectOperations.CreateNewObjectEx(int ObjectType, PropertyValues Properties, SourceObjectFiles SourceFiles, bool SFD, bool CheckIn, AccessControlList AccessControlList)
		{
			// Validate and extract.
			if (ObjectType < 0)
				throw new ArgumentException(nameof(ObjectType));
			if (null == Properties)
				throw new ArgumentNullException(nameof(Properties));
			SourceFiles = SourceFiles ?? new SourceObjectFiles();
			int classId = Properties.Cast<PropertyValue>()
				.FirstOrDefault(pv => pv.PropertyDef == (int)MFBuiltInPropertyDef.MFBuiltInPropertyDefClass)?
				.Value?
				.GetLookupID()
				?? Int32.MinValue;
			if (classId == Int32.MinValue)
				throw new ArgumentException("Properties does not contain a class", nameof(Properties));
			string title = Properties.Cast<PropertyValue>()
				.FirstOrDefault(pv => pv.PropertyDef == (int)MFBuiltInPropertyDef.MFBuiltInPropertyDefNameOrTitle)?
				.Value?
				.DisplayValue;
			if (string.IsNullOrWhiteSpace(title))
				throw new ArgumentException("Properties does not contain a title", nameof(Properties));

			// Create the object version details.
			var objVer = new ObjVer();
			objVer.SetIDs(ObjectType, this.CreateNewObjectId(ObjectType), 1);
			var objectVersion = new ObjectVersionEx()
			{
				Class = classId,
				SingleFile = SFD,
				ObjVer = objVer,
				ObjectGUID = Guid.NewGuid().ToString("B"),
				Title = title
			};

			// Create the new object version and properties.
			var objectVersionAndProperties = new ObjectVersionAndPropertiesEx()
			{
				ObjVer = objVer,
				Properties = Properties.Clone(),
				Vault = this.Vault,
				VersionData = objectVersion
			};

			// Add the item to the dictionary and return the correct data.
			this.Add
			(
				objVer.ObjID.ToJSON(),
				new List<ObjectVersionAndPropertiesEx>()
				{
					objectVersionAndProperties
				}
			);
			return objectVersionAndProperties;
		}

		ObjVer IVaultObjectOperations.GetLatestObjVer(ObjID ObjID, bool AllowCheckedOut, bool UpdateFromServer)
		{
			if (null == ObjID)
				throw new ArgumentNullException(nameof(ObjID));
			return this[ObjID]?
				.Where(v => AllowCheckedOut || v.VersionData.ThisVersionCheckedOut == false)
				.OrderByDescending(v => v.ObjVer.Version)
				.FirstOrDefault()?
				.ObjVer
				?? throw new ArgumentException($"Object not found with ID {ObjID.Type}-{ObjID.ID}");
		}

		int IVaultObjectOperations.CreateNewObjectExQuick(int ObjectType, PropertyValues Properties, SourceObjectFiles SourceFiles, bool SFD, bool CheckIn, AccessControlList AccessControlList)
		{
			// Use the other overload.
			return ((IVaultObjectOperations)this)
				.CreateNewObjectEx(ObjectType, Properties, SourceFiles, SFD, CheckIn, AccessControlList).ObjVer.ID;
		}

		ObjectVersion IVaultObjectOperations.ForceUndoCheckout(ObjVer ObjVer)
		{
			return ((IVaultObjectOperations)this).UndoCheckout(ObjVer);
		}

		ObjectVersionAndProperties IVaultObjectOperations.CreateNewSFDObject(int ObjectType, PropertyValues Properties, SourceObjectFile SourceFile, bool CheckIn, AccessControlList AccessControlList)
		{
			// Create the source files.
			var sourceFiles = new SourceObjectFiles();
			sourceFiles.Add(-1, SourceFile);

			// Use the other overload.
			return ((IVaultObjectOperations)this)
				.CreateNewObjectEx(ObjectType, Properties, sourceFiles, true, CheckIn, AccessControlList);
		}

		int IVaultObjectOperations.CreateNewSFDObjectQuick(int ObjectType, PropertyValues Properties, SourceObjectFile SourceFile, bool CheckIn, AccessControlList AccessControlList)
		{
			// Use the other overload.
			return ((IVaultObjectOperations)this)
				.CreateNewSFDObject(ObjectType, Properties, SourceFile, CheckIn, AccessControlList).ObjVer.ID;
		}

		ObjectVersionAndProperties IVaultObjectOperations.CreateNewEmptySingleFileDocument(PropertyValues PropertyValues, string Title, string Extension, AccessControlList AccessControlList)
		{
			// Use the other overload.
			return ((IVaultObjectOperations)this)
				.CreateNewObjectEx
				(
					(int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument,
					PropertyValues,
					null,
					true, 
					true, 
					AccessControlList
				);
		}

		ObjectVersion IVaultObjectOperations.DeleteObject(ObjID ObjID)
		{
			if (null == ObjID)
				throw new ArgumentNullException(nameof(ObjID));

			// Get the details.
			var item = this[ObjID]
				?.OrderByDescending(v => v.ObjVer.Version)
				?.FirstOrDefault()
				?.VersionData;

			// Remove it.
			this.Remove(ObjID);

			// Return the details.
			return item;
		}

		void IVaultObjectOperations.SetExternalID(ObjID ObjID, string ExtID)
		{
			if (null == ObjID)
				throw new ArgumentNullException(nameof(ObjID));
			var versions = this.ContainsKey(ObjID)
				? this[ObjID]
				: null;
			if (null == versions)
				throw new ArgumentException("Object ID not found", nameof(ObjID));

			// Is it unique?
			if (this.Any(v => v.Value.OrderByDescending(a => a.ObjVer.Version).FirstOrDefault().VersionData.DisplayID == ExtID))
				throw new ArgumentException("Object ID already exists");

			// Create a new version.
			var version = versions.OrderByDescending(v => v.ObjVer.Version).FirstOrDefault().CreateNewVersion();
			version.VersionData.DisplayID = ExtID;
			versions.Add(version);
		}

		void IVaultObjectOperations.DestroyObjectEx(ObjID ObjID, bool DestroyAllVersions, ObjVerVersion ObjectVersion)
		{
			if (null == ObjectVersion)
				throw new ArgumentNullException(nameof(ObjectVersion));
			((IVaultObjectOperations)this).DestroyObject(ObjID, DestroyAllVersions, ObjectVersion.Version);
		}

		ObjectVersion IVaultObjectOperations.RollbackEx(ObjID ObjID, ObjVerVersion RollbackToVersion)
		{
			throw new NotImplementedException();
		}

		ObjectVersions IVaultObjectOperations.GetHistoryEx(ObjID ObjID, MFHistoryRetrievalMode HistoryRetrievalMode)
		{
			var versions = this.ContainsKey(ObjID)
				? this[ObjID]
				: null;
			if (null == versions)
				throw new ArgumentException("Object ID not found", nameof(ObjID));

			// Sort the loaded versions.
			var objectVersions = new ObjectVersions();
			switch (HistoryRetrievalMode)
			{
				case MFHistoryRetrievalMode.MFHistoryRetrievalAscending:
					{
						foreach (var v in versions.OrderBy(v => v.ObjVer.Version))
							objectVersions.Add(objectVersions.Count + 1, v.VersionData);
					}
					break;
				case MFHistoryRetrievalMode.MFHistoryRetrievalRelaxed:
				case MFHistoryRetrievalMode.MFHistoryRetrievalDescending:
					{
						foreach (var v in versions.OrderByDescending(v => v.ObjVer.Version))
							objectVersions.Add(objectVersions.Count + 1, v.VersionData);
					}
					break;
			}
			return objectVersions;
		}

		#region Not implemented

		ObjectVersions IVaultObjectOperations.GetRelationships(ObjVer ObjVer, MFRelationshipsMode Mode)
		{
			throw new NotImplementedException();
		}

		ObjectVersions IVaultObjectOperations.GetCollectionMembers(ObjVer ObjVer)
		{
			throw new NotImplementedException();
		}

		ObjectWindowResult IVaultObjectOperations.ShowBasicNewObjectWindow(IntPtr ParentWindow, ObjType ObjectType)
		{
			throw new NotImplementedException();
		}

		ObjectWindowResult IVaultObjectOperations.ShowNewObjectWindow(IntPtr ParentWindow, MFObjectWindowMode Mode, ObjectCreationInfo ObjectCreationInfo)
		{
			throw new NotImplementedException();
		}

		ObjectWindowResult IVaultObjectOperations.ShowBasicEditObjectWindow(IntPtr ParentWindow, ObjVer ObjVer)
		{
			throw new NotImplementedException();
		}

		ObjectWindowResult IVaultObjectOperations.ShowEditObjectWindow(IntPtr ParentWindow, MFObjectWindowMode Mode, ObjVer ObjVer)
		{
			throw new NotImplementedException();
		}

		ObjectVersion IVaultObjectOperations.ShowCheckoutPrompt(IntPtr ParentWindow, string Message, ObjID ObjID, bool ShowCancel, bool AutoRejectConsequentPrompts)
		{
			throw new NotImplementedException();
		}

		ObjectVersion IVaultObjectOperations.ChangePermissionsToNamedACL(ObjVer ObjVer, int NamedACL, bool ChangeAllVersions)
		{
			throw new NotImplementedException();
		}

		ObjectVersion IVaultObjectOperations.ChangePermissionsToACL(ObjVer ObjVer, AccessControlList AccessControlList, bool ChangeAllVersions)
		{
			throw new NotImplementedException();
		}

		byte[] IVaultObjectOperations.GetThumbnailAsBytes(ObjVer ObjVer, FileVer FileVer, int Width, int Height, bool GetFileIconThumbnailIfRealThumbnailNotAvailable)
		{
			throw new NotImplementedException();
		}

		ObjectVersionPermissions IVaultObjectOperations.GetObjectPermissions(ObjVer ObjVer)
		{
			throw new NotImplementedException();
		}

		ObjectVersion IVaultObjectOperations.RemoveObject(ObjID ObjID)
		{
			throw new NotImplementedException();
		}

		string IVaultObjectOperations.GetMFilesURLForObject(ObjID ObjID, int TargetVersion, bool SpecificVersion, MFilesURLType URLType)
		{
			throw new NotImplementedException();
		}

		ObjectWindowResult IVaultObjectOperations.ShowPrefilledNewObjectWindow(IntPtr ParentWindow, MFObjectWindowMode Mode, ObjectCreationInfo ObjectCreationInfo, PropertyValues PrefilledPropertyValues, AccessControlList AccessControlList)
		{
			throw new NotImplementedException();
		}

		void IVaultObjectOperations.SetOfflineAvailability(ObjID ObjID, bool AvailableInOfflineMode)
		{
			throw new NotImplementedException();
		}

		Strings IVaultObjectOperations.GetObjectLocationsInView(int View, MFLatestSpecificBehavior LatestSpecificBehavior, ObjVer ObjectVersion)
		{
			throw new NotImplementedException();
		}

		void IVaultObjectOperations.ShowRelatedObjects(IntPtr ParentWindow, ObjID SourceObject, ObjectTypeTargetForBrowsing ObjectTypeTargetForBrowsing, string ViewSelectionDialogInfoText)
		{
			throw new NotImplementedException();
		}

		void IVaultObjectOperations.ShowRelationshipsDialog(IntPtr ParentWindow, ObjVer ObjectVersion, bool Modeless)
		{
			throw new NotImplementedException();
		}

		void IVaultObjectOperations.ShowCollectionMembersDialog(IntPtr ParentWindow, ObjVer ObjectVersion, bool Modeless)
		{
			throw new NotImplementedException();
		}

		void IVaultObjectOperations.ShowSubObjectsDialogModal(IntPtr ParentWindow, ObjVer ObjectVersion)
		{
			throw new NotImplementedException();
		}

		void IVaultObjectOperations.ShowHistoryDialogModal(IntPtr ParentWindow, ObjID ObjectID)
		{
			throw new NotImplementedException();
		}

		ObjOrFileVer IVaultObjectOperations.ShowSelectObjectHistoryDialogModal(IntPtr ParentWindow, ObjID ObjectID, string WindowTitle)
		{
			throw new NotImplementedException();
		}

		void IVaultObjectOperations.ShowCommentsDialogModal(IntPtr ParentWindow, ObjID ObjectID)
		{
			throw new NotImplementedException();
		}

		ObjectVersion IVaultObjectOperations.ShowCheckInReminder(IntPtr ParentWindow, ObjVer ObjVer, bool Asynchronous)
		{
			throw new NotImplementedException();
		}

		string IVaultObjectOperations.GetMFilesURLForObjectOrFile(ObjID ObjID, int TargetVersion, bool SpecificVersion, int File, MFilesURLType URLType)
		{
			throw new NotImplementedException();
		}

		void IVaultObjectOperations.RejectCheckInReminder(ObjVer ObjVer)
		{
			throw new NotImplementedException();
		}

		void IVaultObjectOperations.DelayedCheckIn(ObjVer ObjVer)
		{
			throw new NotImplementedException();
		}

		bool IVaultObjectOperations.ShowCheckInReminderDialogModal(IntPtr ParentWindow, ObjVer ObjVer, bool ApplyEnvironmentConditions)
		{
			throw new NotImplementedException();
		}

		bool IVaultObjectOperations.ProposeCheckOutForFile(IntPtr ParentWindow, ObjectVersionFile ObjVersionFile, bool CanCancel)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndPropertiesOfMultipleObjects IVaultObjectOperations.AddFavorites(ObjIDs ObjIDs)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectOperations.AddFavorite(ObjID ObjID)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndPropertiesOfMultipleObjects IVaultObjectOperations.RemoveFavorites(ObjIDs ObjIDs)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectOperations.RemoveFavorite(ObjID ObjID)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectOperations.NotifyObjectAccess(ObjID ObjID)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndProperties IVaultObjectOperations.CreateNewAssignment(string AssignmentName, string AssignmentDescription, TypedValue AssignedToUser, TypedValue Deadline, AccessControlList AccessControlList)
		{
			throw new NotImplementedException();
		}

		string IVaultObjectOperations.GetObjectGUID(ObjID ObjID)
		{
			throw new NotImplementedException();
		}

		void IVaultObjectOperations.SetObjectGUID(ObjID ObjID, string ObjectGUID)
		{
			throw new NotImplementedException();
		}

		ObjID IVaultObjectOperations.GetObjIDByGUID(string ObjectGUID)
		{
			throw new NotImplementedException();
		}

		void IVaultObjectOperations.DestroyObjects(ObjIDs ObjIDs)
		{
			throw new NotImplementedException();
		}

		ObjectVersions IVaultObjectOperations.CheckOutMultipleObjects(ObjIDs ObjIDs)
		{
			throw new NotImplementedException();
		}

		ObjectVersions IVaultObjectOperations.CheckInMultipleObjects(ObjVers ObjVers)
		{
			throw new NotImplementedException();
		}

		ObjID IVaultObjectOperations.GetObjIDByOriginalObjID(string OriginalVaultGUID, ObjID OriginalObjID)
		{
			throw new NotImplementedException();
		}

		ObjectVersions IVaultObjectOperations.ResolveConflict(ObjID ParticipantObjID, bool KeepThis)
		{
			throw new NotImplementedException();
		}

		ObjectVersion IVaultObjectOperations.GetObjectInfoEx(ObjVer ObjVer, bool LatestVersion, bool UpdateFromServer, bool NotifyViews)
		{
			throw new NotImplementedException();
		}

		ObjVer IVaultObjectOperations.GetLatestObjVerEx(ObjID ObjID, bool AllowCheckedOut, bool UpdateFromServer, bool NotifyViews)
		{
			throw new NotImplementedException();
		}

		ObjectVersions IVaultObjectOperations.UndoCheckoutMultipleObjects(ObjVers ObjVers)
		{
			throw new NotImplementedException();
		}

		void IVaultObjectOperations.ShowChangeStateDialogModal(IntPtr ParentWindow, ObjID ObjectID)
		{
			throw new NotImplementedException();
		}

		ObjectVersionAndPropertiesOfMultipleObjects IVaultObjectOperations.GetObjectVersionAndPropertiesOfMultipleObjects(ObjVers ObjVers, bool LatestVersions, bool AllowCheckedOut, bool AllowMissingObjectVersions, bool UpdateFromServer)
		{
			throw new NotImplementedException();
		}

		bool IVaultObjectOperations.IsObjectCheckedOut(ObjID ObjID, bool UpdateFromServer)
		{
			throw new NotImplementedException();
		}

		bool IVaultObjectOperations.IsObjectCheckedOutToThisUserOnThisComputer(ObjID ObjID, bool UpdateFromServer)
		{
			throw new NotImplementedException();
		}

		void IVaultObjectOperations.FollowObject(ObjID ObjID, bool Follow)
		{
			throw new NotImplementedException();
		}

		bool IVaultObjectOperations.IsObjectFollowed(ObjID ObjID)
		{
			throw new NotImplementedException();
		}

		bool IVaultObjectOperations.MatchesSearchConditions(ObjVer pIObjVer, SearchConditions pISearchConditions)
		{
			throw new NotImplementedException();
		}

		bool IVaultObjectOperations.MatchesSearchConditionsEx(ObjectVersion pIObjectVersion, SearchConditions pISearchConditions, PropertyValues pIPropertyValues, ObjectVersionAndPropertiesOfMultipleObjects pIObjectVersionAndPropertiesOfMultipleObjects)
		{
			throw new NotImplementedException();
		}

		bool IVaultObjectOperations.GetOfflineAvailability(ObjID ObjID)
		{
			throw new NotImplementedException();
		}

		ObjIDs IVaultObjectOperations.GetOfflineObjIDs()
		{
			throw new NotImplementedException();
		}

		string IVaultObjectOperations.GetMFilesURLForObjectEx(ObjVer ObjVer, bool SpecificVersion, MFilesURLType URLType)
		{
			throw new NotImplementedException();
		}

		string IVaultObjectOperations.GetMFilesURLForObjectOrFileEx(ObjVer ObjVer, FileVer FileVer, bool SpecificVersion, MFilesURLType URLType)
		{
			throw new NotImplementedException();
		}

		ObjectVersion IVaultObjectOperations.GetRelatedObjectInfo(Lookup Lookup, bool UpdateFromServer)
		{
			throw new NotImplementedException();
		}

		ObjectVersions IVaultObjectOperations.GetRelationshipsEx(ObjVer ObjVer, MFRelationshipsMode Mode, bool SearchInEachObjectType)
		{
			throw new NotImplementedException();
		}

		ObjectVersionDataResults IVaultObjectOperations.GetObjectDataOfMultipleObjects(ObjVers IObjVers, ObjectVersionDataRequest ObjectDataRequest)
		{
			throw new NotImplementedException();
		}

		void IVaultObjectOperations.CompareWithPreviousVersion(IntPtr ParentWindow, ObjVer ObjVer)
		{
			throw new NotImplementedException();
		}

		void IVaultObjectOperations.CompareWithAnotherVersion(IntPtr ParentWindow, ObjVer ObjVer)
		{
			throw new NotImplementedException();
		}

		void IVaultObjectOperations.CompareWithAnotherDocument(IntPtr ParentWindow, ObjVer ObjVer, string FileTitleAndExtension)
		{
			throw new NotImplementedException();
		}

		string IVaultObjectOperations.GetShortObjectPath(ObjVer ObjVer)
		{
			throw new NotImplementedException();
		}

		void IVaultObjectOperations.CompareWithPreviousVersionEx(IntPtr ParentWindow, ObjVer ObjVer, string FileTitleAndExtension)
		{
			throw new NotImplementedException();
		}

		void IVaultObjectOperations.CompareWithAnotherVersionEx(IntPtr ParentWindow, ObjVer ObjVer, string FileTitleAndExtension)
		{
			throw new NotImplementedException();
		}

		ObjectVersion IVaultObjectOperations.CopyConflictObjectAsNewVersionToBaseObject(ObjVer BaseObjectObjVer, ObjVer ConflictObjectObjVer)
		{
			throw new NotImplementedException();
		}

		#endregion

		#endregion

	}
}
