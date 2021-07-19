using System;
using System.Linq;

namespace MFilesAPI.Fakes
{
	public class ObjectVersionEx
		: ObjectVersion
	{
		public ObjectVersionEx()
		{
		}
		public ObjectVersionEx Clone()
		{
			return CloneHelper.Clone(this);
		}
		public static ObjectVersionEx CloneFrom(ObjectVersion toClone)
		{
			var clone = ComInterfaceAutoImpl.GetInstanceOfCompletedType<ObjectVersionEx>();
			clone.Title = toClone.Title;
			clone.SingleFile = toClone.SingleFile;
			clone.CreatedUtc = toClone.CreatedUtc;
			clone.LastModifiedUtc = toClone.LastModifiedUtc;
			clone.Deleted = toClone.Deleted;
			clone.CheckedOutVersion = toClone.CheckedOutVersion;
			clone.CheckedOutTo = toClone.CheckedOutTo;
			clone.CheckedOutToUserName = toClone.CheckedOutToUserName;
			clone.CheckedOutToHostName = toClone.CheckedOutToHostName;
			clone.CheckedOutAtUtc = toClone.CheckedOutAtUtc;
			clone.LatestCheckedInVersion = toClone.LatestCheckedInVersion;
			clone.HasRelationshipsFromThis = toClone.HasRelationshipsFromThis;
			clone.HasRelationshipsToThis = toClone.HasRelationshipsToThis;
			clone.ObjVer = toClone.ObjVer;
			clone.DisplayID = toClone.DisplayID;
			clone.ObjectCheckedOut = toClone.ObjectCheckedOut;
			clone.ObjectCheckedOutToThisUser = toClone.ObjectCheckedOutToThisUser;
			clone.ThisVersionCheckedOut = toClone.ThisVersionCheckedOut;
			clone.ThisVersionLatestToThisUser = toClone.ThisVersionLatestToThisUser;
			clone.LatestCheckedInVersionOrCheckedOutVersion = toClone.LatestCheckedInVersionOrCheckedOutVersion;
			clone.ThisVersionLatestToThisUser = toClone.ThisVersionLatestToThisUser;
			clone.VisibleAfterOperation = toClone.VisibleAfterOperation;
			clone.HasAssignments = toClone.HasAssignments;
			clone.ObjectVersionFlags = toClone.ObjectVersionFlags;
			clone.Class = toClone.Class;
			clone.ObjectGUID = toClone.ObjectGUID;
			clone.IsAccessedByMeValid = toClone.IsAccessedByMeValid;
			clone.AccessedByMeUtc = toClone.AccessedByMeUtc;
			clone.VersionGUID = toClone.VersionGUID;
			clone.OriginalVaultGUID = toClone.OriginalVaultGUID;
			clone.OriginalObjID = toClone.OriginalObjID;
			clone.ObjectFlags = toClone.ObjectFlags;
			clone.IsObjectShortcut = toClone.IsObjectShortcut;
			clone.IsObjectConflict = toClone.IsObjectConflict;
			clone.HasSharedFiles = toClone.HasSharedFiles;
			clone.ObjectCheckedOutToThisUserOnThisComputer = toClone.ObjectCheckedOutToThisUserOnThisComputer;
			clone.ObjectCheckedOutToThisUserOnAnyComputer = toClone.ObjectCheckedOutToThisUserOnAnyComputer;
			clone.ObjectCapabilityFlags = toClone.ObjectCapabilityFlags;
			clone.LatestCheckedInObjVerVersion = toClone.LatestCheckedInObjVerVersion;
			clone.BaseProperties = toClone.BaseProperties;
			clone.AssociatedFolderID = toClone.AssociatedFolderID;
			clone.CurrentUser = toClone.CurrentUser;
			clone.HostName = toClone.HostName;
			return clone;
		}

		public string GetNameForFileSystem(bool IncludeID = true)
		{
			throw new NotImplementedException();
		}

		public string GetNameForFileSystemEx(bool IncludeID = true, bool UseOriginalID = false)
		{
			throw new NotImplementedException();
		}

		public bool CompareObjectVersionEx(ObjectVersion pIObjectVersion)
		{
			throw new NotImplementedException();
		}

		public string Title { get; set; }

		public bool SingleFile { get; set; }

		public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

		public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;

		public int FilesCount => this.Files?.Count ?? 0;

		public ObjectFiles Files
		{
			get => this.FilesEx;
			set
			{
				var f = ComInterfaceAutoImpl.GetInstanceOfCompletedType<ObjectFilesEx>();
				if(value != null)
				{
					f.AddRange(value.Cast<ObjectFile>());
				}
				this.FilesEx = f;
			}
		}
		public ObjectFiles FilesEx { get; set; } = ComInterfaceAutoImpl.GetInstanceOfCompletedType<ObjectFilesEx>();

		public bool Deleted { get; set; }

		public int CheckedOutVersion { get; set; }

		public int CheckedOutTo { get; set; }

		public string CheckedOutToUserName { get; set; }

		public string CheckedOutToHostName { get; set; }

		public DateTime CheckedOutAtUtc { get; set; }

		public int LatestCheckedInVersion { get; set; }

		public bool HasRelationshipsFromThis { get; set; }

		public bool HasRelationshipsToThis { get; set; }

		public ObjVer ObjVer { get; set; } = new ObjVer();

		public string DisplayID { get; set; }

		public bool DisplayIDAvailable => false == string.IsNullOrWhiteSpace(this.DisplayID);

		public bool ObjectCheckedOut { get; set; }

		public bool ObjectCheckedOutToThisUser { get; set; }

		public bool ThisVersionCheckedOut { get; set; }

		public bool ThisVersionLatestToThisUser { get; set; }

		public bool LatestCheckedInVersionOrCheckedOutVersion { get; set; }

		public bool VisibleAfterOperation { get; set; }

		public bool HasAssignments { get; set; }

		public MFObjectVersionFlag ObjectVersionFlags { get; set; }
			= MFObjectVersionFlag.MFObjectVersionFlagNone;

		public int Class { get; set; }

		public bool IsStub { get; set; }

		public string ObjectGUID { get; set; } = Guid.NewGuid().ToString("B");

		public bool IsAccessedByMeValid { get; set; }

		public DateTime AccessedByMeUtc { get; set; }

		public string VersionGUID { get; set; } = Guid.NewGuid().ToString("B");

		public string OriginalVaultGUID { get; set; }

		public ObjID OriginalObjID { get; set; }

		public MFSpecialObjectFlag ObjectFlags { get; set; }
			= MFSpecialObjectFlag.MFSpecialObjectFlagNone;

		public bool IsObjectShortcut { get; set; }

		public bool IsObjectConflict { get; set; }

		public bool HasSharedFiles { get; set; }

		public bool ObjectCheckedOutToThisUserOnThisComputer { get; set; }

		public bool ObjectCheckedOutToThisUserOnAnyComputer { get; set; }

		public MFObjectCapabilityFlag ObjectCapabilityFlags { get; set; }
			= MFObjectCapabilityFlag.MFObjectCapabilityFlagAllCapabilities;

		public ObjVerVersion LatestCheckedInObjVerVersion { get; set; }

		public PropertyValues BaseProperties { get; set; } = new PropertyValues();

		public FolderID AssociatedFolderID { get; set; } = new FolderID();

		public string IconID { get; set; }

		public int CurrentUser { get; set; }

		public string HostName { get; set; }

		ObjectVersion IObjectVersion.Clone() => this.Clone();
	}
}
