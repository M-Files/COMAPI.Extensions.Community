using System;

namespace MFilesAPI.Fakes
{
	public class SessionInfo
		: MFilesAPI.SessionInfo
	{
		public int UserID { get; set; }
		public bool InternalUser { get; set; } = true;
		public MFProductMode ProductMode { get; set; } = MFProductMode.MFProductModeFull;
		public string LocalComputerName { get; set; } = "Testing";
		public string VaultGUID { get; set; }
		public string AccountName { get; set; }
		public MFAuthType AuthenticationType { get; set; } = MFAuthType.MFAuthTypeLoggedOnWindowsUser;
		public UserOrUserGroupIDs UserAndSubstitutedByMe { get; set; } = new UserOrUserGroupIDs();
		public UserOrUserGroupIDs UserAndGroupMemberships { get; set; } = new UserOrUserGroupIDs();
		public bool CanManageCommonViews { get; set; } = true;

		public bool CanManageCommonUISettings { get; set; } = true;

		public bool CanManageTraditionalFolders { get; set; } = true;

		public bool CanCreateObjects { get; set; } = true;

		public bool CanSeeAllObjects { get; set; } = true;

		public bool CanSeeDeletedObjects { get; set; } = true;

		public bool LicenseAllowsModifications { get; set; } = true;

		public bool CanForceUndoCheckout { get; set; } = true;

		public TimeZoneInformation TimeZoneInfo { get; set; } = new TimeZoneInformation();

		public bool CanMaterializeViews { get; set; }

		public MFACLMode ACLMode { get; set; } = MFACLMode.MFACLModeAutomaticPermissionsWithComponents;

		public bool UseSetBasedNotEqualComparisons { get; set; }

		public string ClientCulture { get; set; } = "en-us";

		public bool IsSharingPublicLinksAllowed { get; set; } = true;

		public bool IsSharingPublicLinksToLatestVersionAllowed { get; set; } = true;

		public bool AutomaticMetadataEnabled { get; set; } = true;

		public ServerVaultCapabilities ServerVaultCapabilities { get; set; }

		public string LoginHint { get; set; }

		#region MFilesAPI.SessionInfo

		#region Not implemented yet

		bool ISessionInfo.CheckPropertyDefAccess(AccessControlList PropertyDefAccessControlList, MFPropertyDefAccess DesiredPropertyDefAccess)
		{
			throw new NotImplementedException();
		}

		bool ISessionInfo.CheckObjectTypeAccess(AccessControlList ObjectTypeAccessControlList, MFObjectTypeAccess DesiredObjectTypeAccess)
		{
			throw new NotImplementedException();
		}

		bool ISessionInfo.IsLoggedOnUserSubstituteOfUser(int UserID)
		{
			throw new NotImplementedException();
		}

		bool ISessionInfo.CheckObjectAccess(AccessControlList ObjectAccessControlList, MFObjectAccess DesiredObjectAccess)
		{
			throw new NotImplementedException();
		}

		bool ISessionInfo.CheckVaultAccess(MFVaultAccess DesiredVaultAccess)
		{
			throw new NotImplementedException();
		}

		void ISessionInfo.CloneFrom(MFilesAPI.SessionInfo SessionInfo)
		{
			throw new NotImplementedException();
		}

		#endregion

		int ISessionInfo.UserID => this.UserID;

		bool ISessionInfo.InternalUser => this.InternalUser;

		MFProductMode ISessionInfo.ProductMode => this.ProductMode;

		string ISessionInfo.LocalComputerName => this.LocalComputerName;

		string ISessionInfo.VaultGUID => this.VaultGUID;

		string ISessionInfo.AccountName => this.AccountName;

		MFAuthType ISessionInfo.AuthenticationType => this.AuthenticationType;

		ulong ISessionInfo.ServerVersion => 0;

		short ISessionInfo.Language => 0;

		int ISessionInfo.KeepAliveIntervalInSeconds => 0;

		UserOrUserGroupIDs ISessionInfo.UserAndSubstitutedByMe => this.UserAndSubstitutedByMe;

		UserOrUserGroupIDs ISessionInfo.UserAndGroupMemberships => this.UserAndGroupMemberships;

		bool ISessionInfo.CanManageCommonViews => this.CanManageCommonViews;

		bool ISessionInfo.CanManageCommonUISettings => this.CanManageCommonUISettings;

		bool ISessionInfo.CanManageTraditionalFolders => this.CanManageTraditionalFolders;

		bool ISessionInfo.CanCreateObjects => this.CanCreateObjects;

		bool ISessionInfo.CanSeeAllObjects => this.CanSeeAllObjects;

		bool ISessionInfo.CanSeeDeletedObjects => this.CanSeeDeletedObjects;

		bool ISessionInfo.LicenseAllowsModifications => this.LicenseAllowsModifications;

		bool ISessionInfo.CanForceUndoCheckout => this.CanForceUndoCheckout;

		TimeZoneInformation ISessionInfo.TimeZoneInfo => this.TimeZoneInfo;

		bool ISessionInfo.CanMaterializeViews => this.CanMaterializeViews;

		MFACLMode ISessionInfo.ACLMode => this.ACLMode;

		bool ISessionInfo.UseSetBasedNotEqualComparisons => this.UseSetBasedNotEqualComparisons;

		string ISessionInfo.ClientCulture => this.ClientCulture;

		bool ISessionInfo.IsSharingPublicLinksAllowed => this.IsSharingPublicLinksAllowed;

		bool ISessionInfo.IsSharingPublicLinksToLatestVersionAllowed => this.IsSharingPublicLinksToLatestVersionAllowed;

		bool ISessionInfo.AutomaticMetadataEnabled => this.AutomaticMetadataEnabled;

		ServerVaultCapabilities ISessionInfo.ServerVaultCapabilities => this.ServerVaultCapabilities;

		string ISessionInfo.LoginHint => this.LoginHint;

		#endregion

		/// <summary>
		/// Creates a simple fake session.
		/// </summary>
		/// <returns></returns>
		public static SessionInfo CreateDefault() { return new SessionInfo(); }

		/// <summary>
		/// Returns <see langword="null"/> representing a logged-out session.
		/// </summary>
		/// <returns></returns>
		public static SessionInfo LoggedOut() { return null; }
	}
}
