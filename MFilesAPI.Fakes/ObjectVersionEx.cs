using System;

namespace MFilesAPI.Fakes
{
	public class ObjectVersionEx
		: ObjectVersion
	{
		public ObjectVersionEx Clone()
		{
			// TODO: This is rather important.
			throw new NotImplementedException();
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

		public ObjectFiles Files { get; set; } = new ObjectFilesEx();

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
