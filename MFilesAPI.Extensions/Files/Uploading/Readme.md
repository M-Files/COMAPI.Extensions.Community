# File upload helpers

## FileUploadStream

Allows easy writing of file data within the M-Files vault from a .NET `Stream` structure:

```csharp
// Get details of the file to overwrite.
FileVer fileVer = ...;
ObjID objId = ...;

// Assume that "sourceStream" contains the new file contents:
using (var sourceStream = ...)
{
	// Write the stream to the file on the server.
	using (var uploadStream = new FileUploadStream(fileVer, objId, this.Vault))
	{
		// Copy the source stream data to the upload stream.
		sourceStream.CopyTo(uploadStream);
	}
}
```
