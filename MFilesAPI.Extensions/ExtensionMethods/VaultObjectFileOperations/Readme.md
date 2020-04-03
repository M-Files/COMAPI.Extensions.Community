# VaultObjectFileOperations

## AddFile

Adds a file to an existing M-Files object.

```csharp
// Get details of the object to add to.
ObjVer objVer = ...;

// Assume that "stream" contains the new file contents:
using (var stream = ...)
{
    this.Vault.ObjectFileOperations.AddFile
    (
        objVer,
        this.Vault,
        "My new file",
        ".pdf,
        stream
    );
}
```

*Note: the object must be already checked out and have the "Single File Document" property appropriately set prior to calling this method.*

## OpenFileForReading

Opens an existing file in the M-Files vault for reading (streams the file as a download).

```csharp
// Get details of the object to download.
ObjectFile objectFile = ...;

using(var stream = this.Vault.ObjectFileOperations.OpenFileForReading(objectFile, this.Vault))
{
    // TODO: Do something with the file.
}
```

*Note: Passing an MFFileFormat can be used to convert documents to PDF, for example, during the read process.*

## OpenFileForWriting

Opens an existing file in the M-Files vault for writing (updates the file contents from a stream).

```csharp
// Get details of the object to download.
ObjectFile objectFile = ...;

// Assume that "sourceStream" contains the new file contents:
using (var sourceStream = ...)
{
    using(var uploadStream = this.Vault.ObjectFileOperations.OpenFileForWriting(objectFile, this.Vault))
    {
        // Write the source content to the upload stream.
        sourceStream.CopyTo(uploadStream);
    }
}
```
