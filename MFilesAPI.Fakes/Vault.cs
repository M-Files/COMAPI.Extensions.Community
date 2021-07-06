using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAPI.Fakes
{
	public interface IVaultEx
		: MFilesAPI.Vault
	{
		Guid Guid { get; set; }
		new string Name { get; set; }
	}
	public partial class Vault
		: IVaultEx
	{
		public Guid Guid { get; set; }
		public MFilesVersion ServerVersion { get; set; }
		public string MFilesUrlForVaultRoot { get; set; }
		public int MetadataStructureVersion { get; set; } = 1;
		public string Translations { get; set; }
		public LicenseStatus LicenseStatus { get; set; }
		public string SessionId { get; set; } = Guid.NewGuid().ToString("N");
		public VaultServerAttachments VaultServerAttachments { get; set; }
		public string Name { get; set; }
		public MFilesAPI.SessionInfo SessionInfo { get; set; }
		public bool KeepAlive { get; set; } = true;
		public MFilesAPI.VaultObjectTypeOperations ObjectTypeOperations { get; set; }
		public MFilesAPI.VaultClassOperations ClassOperations { get; set; }
		public MFilesAPI.VaultPropertyDefOperations PropertyDefOperations { get; set; }

		public Vault()
			: this(FakeFactory.Default)
		{
		}
		public Vault(FakeFactory factory)
		{
			// Ensure we have a factory.
			if (null == factory)
				throw new ArgumentNullException(nameof(factory));

			// Ensure we have types registered.
			if (false == factory.HasRegistration<MFilesAPI.VaultObjectTypeOperations>())
				throw new ArgumentException("Factory does not have a registration for MFilesAPI.VaultObjectTypeOperations.", nameof(factory));
			if (false == factory.HasRegistration<MFilesAPI.VaultClassOperations>())
				throw new ArgumentException("Factory does not have a registration for MFilesAPI.VaultClassOperations.", nameof(factory));
			if (false == factory.HasRegistration<MFilesAPI.VaultPropertyDefOperations>())
				throw new ArgumentException("Factory does not have a registration for MFilesAPI.VaultPropertyDefOperations.", nameof(factory));
			if (false == factory.HasRegistration<MFilesAPI.SessionInfo>())
				throw new ArgumentException("Factory does not have a registration for MFilesAPI.SessionInfo.", nameof(factory));

			// Create instances.
			this.ObjectTypeOperations = factory.Instantiate<MFilesAPI.VaultObjectTypeOperations>();
			this.ClassOperations = factory.Instantiate<MFilesAPI.VaultClassOperations>();
			this.PropertyDefOperations = factory.Instantiate<MFilesAPI.VaultPropertyDefOperations>();
			this.SessionInfo = factory.Instantiate<MFilesAPI.SessionInfo>();

			// These are optional.
			{
				this.VaultServerAttachments = new VaultServerAttachments();
				if (factory.TryInstantiate<MFilesAPI.VaultServerAttachments>(out VaultServerAttachments x))
				{
					this.VaultServerAttachments = x;
				}
			}
		}

		#region MFilesAPI.Vault

		bool IVault.LogOutWithDialogs(IntPtr ParentWindow) => true;

		string IVault.GetGUID() => null == this.Guid ? "" : this.Guid.ToString("B");

		void IVault.LogOutSilent() { }

		void IVault.ChangePassword(string OldPassword, string NewPassword) { }

		MFilesVersion IVault.GetServerVersionOfVault() => this.ServerVersion;

		void IVault.TestConnectionToServer() { }

		void IVault.TestConnectionToVault() { }

		string IVault.GetMFilesURLForVaultRoot() => this.MFilesUrlForVaultRoot;

		void IVault.TestConnectionToVaultWithTimeout(int TimeoutInMilliseconds) { }

		void IVault.CloneFrom(MFilesAPI.Vault SourceVault) { }

		MFilesAPI.Vault IVault.CloneForParallelActivity() => this;

		int IVault.GetMetadataStructureVersionID() => this.MetadataStructureVersion;

		string IVault.GetAllTranslations() => this.Translations;

		LicenseStatus IVault.GetServerLicenseStatus() => this.LicenseStatus;

		void IVault.DisconnectDevice(string DeviceToken) { }

		string IVault.DetachSession() => this.SessionId;

		void IVault.Restart() { }

		VaultServerAttachments IVault.GetVaultServerAttachments() => this.VaultServerAttachments;

		bool IVault.ReadOnlyAccess => !(this.SessionInfo?.LicenseAllowsModifications ?? false);

		bool IVault.LoggedIn => this.SessionInfo != null;

		MFilesAPI.SessionInfo IVault.SessionInfo => this.SessionInfo;

		int IVault.CurrentLoggedInUserID => this.SessionInfo?.UserID ?? -1;

		string IVault.Name => this.Name;

		object IVault.Async => this;

		bool IVault.KeepAlive
		{
			get => this.KeepAlive;
			set => this.KeepAlive = value;
		}

		int IVault.GetMetadataStructureItemIDByAlias(MFMetadataStructureItem MetadataStructureItemType, string Alias, bool Unused)
		{
			throw new NotImplementedException();
		}

		MFilesAPI.VaultObjectTypeOperations IVault.ObjectTypeOperations => this.ObjectTypeOperations;

		MFilesAPI.VaultClassOperations IVault.ClassOperations => this.ClassOperations;

		MFilesAPI.VaultPropertyDefOperations IVault.PropertyDefOperations => this.PropertyDefOperations;

		VaultObjectOperations IVault.ObjectOperations => throw new NotImplementedException();

		VaultObjectPropertyOperations IVault.ObjectPropertyOperations => throw new NotImplementedException();

		VaultObjectFileOperations IVault.ObjectFileOperations => throw new NotImplementedException();

		VaultValueListOperations IVault.ValueListOperations => throw new NotImplementedException();

		VaultValueListItemOperations IVault.ValueListItemOperations => throw new NotImplementedException();

		VaultClassGroupOperations IVault.ClassGroupOperations => throw new NotImplementedException();

		VaultWorkflowOperations IVault.WorkflowOperations => throw new NotImplementedException();

		VaultViewOperations IVault.ViewOperations => throw new NotImplementedException();

		VaultUserOperations IVault.UserOperations => throw new NotImplementedException();

		VaultUserGroupOperations IVault.UserGroupOperations => throw new NotImplementedException();

		VaultNamedACLOperations IVault.NamedACLOperations => throw new NotImplementedException();

		VaultTraditionalFolderOperations IVault.TraditionalFolderOperations => throw new NotImplementedException();

		VaultClientOperations IVault.ClientOperations => throw new NotImplementedException();

		VaultObjectSearchOperations IVault.ObjectSearchOperations => throw new NotImplementedException();

		VaultManagementOperations IVault.ManagementOperations => throw new NotImplementedException();

		VaultUserSettingOperations IVault.UserSettingOperations => throw new NotImplementedException();

		Languages IVault.VaultLanguages => throw new NotImplementedException();

		VaultNamedValueStorageOperations IVault.NamedValueStorageOperations => throw new NotImplementedException();

		VaultDataSetOperations IVault.DataSetOperations => throw new NotImplementedException();

		VaultEventLogOperations IVault.EventLogOperations => throw new NotImplementedException();

		VaultElectronicSignatureOperations IVault.ElectronicSignatureOperations => throw new NotImplementedException();

		VaultScheduledJobManagementOperations IVault.ScheduledJobManagementOperations => throw new NotImplementedException();

		VaultCustomApplicationManagementOperations IVault.CustomApplicationManagementOperations => throw new NotImplementedException();

		VaultExtensionMethodOperations IVault.ExtensionMethodOperations => throw new NotImplementedException();

		VaultNotificationOperations IVault.NotificationOperations => throw new NotImplementedException();

		VaultServerDataPushOperations IVault.ServerDataPushOperations => throw new NotImplementedException();

		string IVault.LoginSessionID => this.SessionId;

		VaultLoginAccountOperations IVault.LoginAccountOperations => throw new NotImplementedException();

		VaultAutomaticMetadataOperations IVault.AutomaticMetadataOperations => throw new NotImplementedException();

		VaultSharedLinkOperations IVault.SharedLinkOperations => throw new NotImplementedException();

		VaultExternalObjectOperations IVault.ExternalObjectOperations => throw new NotImplementedException();

		VaultExtensionAuthenticationOperations IVault.ExtensionAuthenticationOperations => throw new NotImplementedException();

		VaultUserOperationsEx IVault.UserOperationsEx => throw new NotImplementedException();

		VaultApplicationTaskOperations IVault.ApplicationTaskOperations => throw new NotImplementedException();

		VaultDocumentComparisonOperations IVault.DocumentComparisonOperations => throw new NotImplementedException();

		VaultWOPIOperations IVault.VaultWOPIOperations => throw new NotImplementedException();

		MetadataStructureAliases IVault.GetMetadataStructureItemIDsByAliases(MFMetadataStructureItem MetadataStructureItemType, bool bUnambiguousAliasesOnly)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
