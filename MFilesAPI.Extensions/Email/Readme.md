# Email Extensions

*Note: This is a very early version of a potential addition to the above library.  This content is only valid for the versions being provided here and may not be applicable in the future.*

Sending email containing information from the M-Files vault is a common request.  The libary aims to make this approach aims to promote best practice and to make implementations consistent.

## Configuration

The EmailMessage object requires an instance of `MFilesAPI.extensions.Email.Configuration.SmtpConfiguration` to understand how to send email.  Email can be sent either via network (`SmtpConfiguration.UseLocalPickupFolder` set to `false`; the default), or written to a local pickup folder for another process to send (`SmtpConfiguration.UseLocalPickupFolder` set to `true`):

### Sending via the network

```csharp
var smptConfiguration = new SmtpConfiguration()
{
	// UseLocalPickupFolder = false (default)
	ServerAddress = "mail.mycompany.com",
	RequiresAuthentication = true,
	Credentials = new Credentials()
	{
		AccountName = "accountName",
		Password = "Password123"
	},
	DefaultSender = new EmailAddress()
	{
		Address = "myapp.mycompany.com",
		DisplayName = "My App"
	}
};
```

### Using the local pickup folder

```csharp
var smptConfiguration = new SmtpConfiguration()
{
	UseLocalPickupFolder = true,
	LocalPickupFolder = @"C:\temp\pickup\"
};
```

## Sending email

```csharp
// Assume the configuration is set.
SmtpConfiguration configuration = ...;

// Assume Vault reference retrieved/populated from somewhere.
Vault vault = ...;

// Assume ObjectVersion retrieved from somewhere...
ObjectVersion objectVersion = ...;

// Create a message.
using (var emailMessage = new EmailMessage(configuration))
{
	// To.
	emailMessage.AddRecipient(AddressType.To, "devsupport@m-files.com");

	// Configure the message metadata.
	emailMessage.Subject = "hello world";
	emailMessage.HtmlBody = $"This is a <b>HTML</b> for document {objectVersion.Title}.";

	// Add all files from the current object.
	emailMessage.AddAllFiles(objectVersion, vault, MFFileFormat.MFFileFormatPDF);
				
	// Send the message.
	emailMessage.Send();
}
```