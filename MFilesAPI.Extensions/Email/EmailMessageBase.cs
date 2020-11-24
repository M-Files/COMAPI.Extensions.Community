using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MFilesAPI.Extensions.Email
{
	/// <summary>
	/// A base implementation of <see cref="IEmailMessage"/> that plugins can extend.
	/// </summary>
	public abstract class EmailMessageBase
		: IEmailMessage
	{
		private SmtpConfiguration configuration = null;

		/// <summary>
		/// The <see cref="SmtpConfiguration"/> to use to send emails.
		/// </summary>
		public SmtpConfiguration Configuration
		{
			get => this.configuration;
			set
			{
				// Don't accept nulls.
				if (value == null)
					throw new ArgumentNullException(nameof(value));

				// Ensure we have a valid default sender.
				if (null == value.DefaultSender)
					throw new ArgumentException("Default sender cannot be null.", nameof(value));

				// Set the value.
				this.configuration = value;
				this.SetSender(this.configuration?.DefaultSender);
			}
		}

		protected EmailMessageBase(SmtpConfiguration configuration)
		{
			// Sanity.
			this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		#region Implementation of IEmailMessage

		/// <inheritdoc />
		public abstract void AddRecipient(AddressType addressType, EmailAddress emailAddress);

		/// <inheritdoc />
		public void AddRecipient(AddressType addressType, string address)
		{
			this.AddRecipient(addressType, new EmailAddress(address));
		}

		/// <inheritdoc />
		public void AddRecipient(AddressType addressType, string address, string displayName)
		{
			this.AddRecipient(addressType, new EmailAddress(address, displayName));
		}

		/// <inheritdoc />
		public abstract EmailAddress GetSender();

		/// <inheritdoc />
		public abstract void SetSender(EmailAddress sender);

		/// <inheritdoc />
		public abstract EmailAddress GetFrom();

		/// <inheritdoc />
		public abstract void SetFrom(EmailAddress from);

		/// <inheritdoc />
		public abstract List<EmailAddress> GetReplyToList();

		/// <inheritdoc />
		public abstract void SetReplyToList(List<EmailAddress> replytolist);

		/// <inheritdoc />
		public abstract string Subject { get; set; }

		/// <inheritdoc />
		public abstract string HtmlBody { get; set; }

		/// <inheritdoc />
		public abstract string TextBody { get; set; }

		/// <inheritdoc />
		public void AddFile(FileInfo fileInfo)
		{
			// Sanity.
			if (null == fileInfo)
				throw new ArgumentNullException(nameof(fileInfo));
			if (false == fileInfo.Exists)
				throw new ArgumentException($"The file {fileInfo.FullName} does not exist on disk.", nameof(fileInfo));

			// Parse data from the file info and use the other overload.
			using (var stream = fileInfo.OpenRead())
			{
				this.AddFile
				(
					fileInfo.Name,
					MimeTypeMap.GetMimeType(fileInfo.Extension),
					stream
				);
			}
		}

		/// <inheritdoc />
		public void AddFile(string filename, Stream stream)
		{
			// Use the other overload.
			this.AddFile
			(
				filename,
				MimeTypeMap.GetMimeType(System.IO.Path.GetExtension(filename)),
				stream
			);
		}

		/// <inheritdoc />
		public abstract void AddFile(string filename, string mimeType, Stream stream);

		/// <inheritdoc />
		public void AddFile(ObjectFile file, Vault vault, MFFileFormat fileFormat = MFFileFormat.MFFileFormatNative)
		{
			// Sanity.
			if (null == file)
				throw new ArgumentNullException(nameof(file));
			if (null == vault)
				throw new ArgumentNullException(nameof(vault));

			// Open the file data as a stream from the vault.
			using (var stream = file.OpenRead(vault, fileFormat))
			{
				// Get the default filename.
				string filename = file.GetNameForFileSystem();

				// If we are going to convert to PDF then we need to handle the filename.
				if (fileFormat != MFFileFormat.MFFileFormatNative)
				{
					filename = filename.Substring(0, filename.Length - ("." + file.Extension).Length) + ".pdf";
				}

				// Use the other overload.
				this.AddFile
				(
					filename,
					stream
				);
			}
		}

		/// <inheritdoc />
		public void AddAllFiles(
			ObjectVersion objectVersion,
			Vault vault,
			MFFileFormat fileFormat = MFFileFormat.MFFileFormatNative
		)
		{
			// Sanity.
			if (null == objectVersion)
				throw new ArgumentNullException(nameof(objectVersion));
			if (null == vault)
				throw new ArgumentNullException(nameof(vault));

			// Add all files.
			foreach(var file in objectVersion.Files?.Cast<ObjectFile>() ?? new List<ObjectFile>())
			{
				this.AddFile(file, vault, fileFormat);
			}
		}

		/// <inheritdoc />
		public abstract void Send();

		/// <inheritdoc />
		public abstract void AddHeader(EmailHeader emailHeader);

		/// <inheritdoc />
		public abstract void AddHeader(string name, string value);

		#endregion

		#region IDisposable

		/// <summary>
		/// Disposes of any managed or unmanaged resources.
		/// </summary>
		/// <param name="disposing">If true, <see cref="IDisposable.Dispose"/> has been called.  False if called from within a finalizer.</param>
		/// <remarks>
		/// Unmanaged resources should be disposed of regardless of <paramref name="disposing" />.
		/// Managed resources should be disposed of if <paramref name="disposing"/> is <see langword="true"/>.
		/// More information on this pattern is available onlne: https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
		/// </remarks>
		protected virtual void Dispose(bool disposing)
		{
		}

		/// <inheritdoc />
		/// <remarks>Calls <see cref="Dispose(bool)"/>, with disposing set to <see langword="true"/>.</remarks>
		public void Dispose()
		{
			// Dispose of unmanaged resources.
			this.Dispose(true);

			// Suppress finalization.
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Ensures that any resources are correctly disposed of during finalization.
		/// </summary>
		/// <remarks>Calls <see cref="Dispose(bool)"/>, with disposing set to <see langword="false"/>.</remarks>
		~EmailMessageBase()
		{
			this.Dispose(false);
		}

		#endregion

	}
}