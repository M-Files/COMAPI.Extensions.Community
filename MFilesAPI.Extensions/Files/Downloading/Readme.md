# File download helpers

## FileDownloadLocation

The `FileDownloadLocation.cs` class represents a physical folder on disk where temporary files are stored. The system temporary folder path is automatically used if no specific folder location is provided.

This class implements the `IDisposable` interface and can automatically delete the content of the folder it represent when the object is disposed, if the value of the `CleanDirectoryOnDisposal` property is `true`.

Usage example, using the system default temporary folder path, and the assembly name as folder name:

```csharp
using(FileDownloadLocation downloadLocation = new FileDownloadLocation())
{
    // Will use the system temporary folder path
    // and the running assembly name as folder name
    // since no specific folder has been provided in the constructor.

    // Set the location to automatically clean on disposal.
    // This is true by default, so the following line is not needed
    // unless to set it to false.
    downloadLocation.CleanDirectoryOnDisposal = true;
    // Alternatively we could manually call downloadLocation.CleanTemporaryFiles().
    
    // TODO: Add, manipulate folders, files in that location,
    // they will be automatically deleted when this object is disposed
    // if CleanDirectoryOnDisposal is true,
    // or deleted when downloadLocation.CleanTemporaryFiles() is called.
}
```



To use a specific location, simply use the constructor overload that takes a path, and a folder name:

```csharp
using (FileDownloadLocation downloadLocation = new FileDownloadLocation(@"C:\temp", "myfolder")) 
{ 
    // The folder represented by this FileDownloadLocation instance is:
    // C:\temp\myfolder
}
```



## TemporaryFileDownload

The `TemporaryFileDownload` class represents a download of a single file from the vault onto disk for temporary usage.

This class implements the `IDisposable` interface and automatically deletes the temporary file it represents when the object is disposed.

It cannot be directly instantiated using the `new` keyword.

Instances are either created using the `TemporaryFileDownload` class static method `Download`, or via the `DownloadFile` method of a `FileDownloadLocation` instance, or via extensions methods on `ObjectFile` instances.

### Using the `TemporaryFileDownload` static `Download` method

```csharp
// Download the file.
// It is assumed that objectFile and vault are already available/populated.
var physicalFile = new FileInfo(@"C:\temp\mydownload.tmp");
using(TemporaryFileDownload fileDownload = TemporaryFileDownload.Download(objectFile, vault, physicalFile))
{
    // TODO: Perform operations on physical file.
}
```

### Using a `FileDownloadLocation` instance `DownloadFile` method

```csharp
using(FileDownloadLocation downloadLocation = new FileDownloadLocation())
{    
    // Perform file downloads.
    // It is assumed that objectFile and vault are already available/populated.
    using(TemporaryFileDownload fileDownload = downloadLocation.DownloadFile(objectFile, vault))
    {
        // The downloaded file will be deleted when the TemporaryFileDownload instance is disposed.
        // TODO: Perform operations on the temporary downloaded file.
    }

	// Any other file or folder created in that folder will be deleted
    // when this FileDownloadLocation instance will be disposed.
}
```

### Using extension methods on `ObjectFile` instance

Various extension methods are enabled from the `ObjectFile` instance, and can be used for easy file access.

These methods are defined in the file MFilesAPI.Extensions/ExtensionMethods/ObjectFileExtensionMethods.cs

#### Passing a physical file path

```csharp
// It is assumed that objectFile and vault are already available/populated.
using(TemporaryFileDownload fileDownload = objectFile.Download(vault, @"C:\temp\mydownload.tmp"))
{
    // TODO: Perform operations on physical file.
    // The file will be deleted when this instance will be disposed.
}
```

#### Passing a FileInfo instance

```csharp
// It is assumed that objectFile and vault are already available/populated.
var physicalFile = new FileInfo("C:\temp\mydownload.tmp");
using(TemporaryFileDownload fileDownload = objectFile.Download(vault, physicalFile))
{
    // TODO: Perform operations on physical file.
    // The file will be deleted when this instance will be disposed.
}
```

### Controlling block size and file format

By default the file will be downloaded from the server in block sizes of 4096 bytes.  This can be overridden by providing your own `blockSize` in the `Download` methods:

```csharp
// It is assumed that objectFile and vault are already available/populated.
using(TemporaryFileDownload fileDownload = objectFile.Download(vault, @"C:\temp\mydownload.tmp", blockSize: 8192))
{
    // BlockSize will be 8192 bytes instead of the default 4096 bytes.
}
```

By default the file will be downloaded in the native format.  You can request that the server convert the file to PDF prior to conversion by setting the `fileFormat` in the `Download` methods:

```csharp
// It is assumed that objectFile and vault are already available/populated.
using(TemporaryFileDownload fileDownload = objectFile.Download(vault, @"C:\temp\mydownload.tmp", fileFormat: MFFileFormat.MFFileFormatPDF))
{
    // TODO: Perform operations on physical file, which will be a PDF.
}
```



## FileDownloadStream

Allows access to file data from the M-Files vault in a .NET `Stream` structure.

This class implements the `IDisposable` interface as it inherits from `System.IO.Stream`.

Instances are either created using the `new` keyword, or via extensions methods on `ObjectFile` instances.

### Using the new keyword

```csharp
// Read the data as a stream.
using (FileDownloadStream downloadStream = new FileDownloadStream(this.FileToDownload, this.Vault))
{
    // TODO use the stream as needed.
}
```

### Using extension methods on `ObjectFile` instance

See also the `OpenRead` and `OpenWrite` extension methods on `ObjectFile` instances.

These methods are defined in the file MFilesAPI.Extensions/ExtensionMethods/ObjectFileExtensionMethods.cs

```csharp
// It is assumed that objectFile and vault are already available/populated.
using(FileDownloadStream downloadStream = objectFile.Download(vault, @"C:\temp\mydownload.tmp"))
{
    // TODO use the stream as needed.
}
```
