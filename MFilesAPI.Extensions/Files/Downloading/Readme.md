# File download helpers

## FileDownloadLocation

The `FileDownloadLocation.cs` class represents a physical folder on disk where temporary file downloads are stored.  This class inherits from `IDisposable` and can automatically delete temporary files when the object is disposed:

```csharp
using(var downloadLocation = new FileDownloadLocation())
{
    // Set the location to automatically clean on disposal.
    // Alternatively we could manually call downloadLocation.CleanTemporaryFiles().
    downloadLocation.CleanDirectoryOnDisposal = true;
    
    // Perform file downloads.
    // It is assumed that objectFile and vault are already available/populated.
    using(var fileDownload = downloadLocation.DownloadFile(objectFile, vault))
    {
        // TODO: Perform operations on the downloaded file.
    }
}
```


## FileDownload

The `FileDownload` class represents a download of a single file from the vault.  Files can be downloaded with or without a `FileDownloadLocation` object, as appropriate:

### Using the static method

```csharp
// Download the file.
// It is assumed that objectFile and vault are already available/populated.
var physicalFile = new FileInfo("C:\temp\mydownload.tmp");
using(var fileDownload = FileDownload.DownloadFile(objectFile, vault, physicalFile))
{
    // TODO: Perform operations on physicalFile.
}
```

### Using the instance method

```csharp
// Initialise the file download object.
// It is assumed that objectFile and vault are already available/populated.
var physicalFile = new FileInfo(@"C:\temp\mydownload.tmp");
using(var fileDownload = new FileDownload(objectFile, vault, physicalFile))
{
    // Perform the download.
    fileDownload.DownloadFile();

    // TODO: Perform operations on physicalFile.
}
```

### Using FileDownloadLocation

See example under [FileDownloadLocation](#FileDownloadLocation).

### Using extension methods

Various extension methods are enabled from the `ObjectFile` instance, and can be used for easy file access.

#### Passing a physical file path

```csharp
// It is assumed that objectFile and vault are already available/populated.
using(var fileDownload = objectFile.Download(vault, @"C:\temp\mydownload.tmp"))
{
    // TODO: Perform operations on physicalFile.
}
```

#### Passing a FileInfo instance

```csharp
// It is assumed that objectFile and vault are already available/populated.
var physicalFile = new FileInfo("C:\temp\mydownload.tmp");
using(var fileDownload = objectFile.Download(vault, physicalFile))
{
    // TODO: Perform operations on physicalFile.
}
```

#### Using a FileDownloadLocation

```csharp
using(var downloadLocation = new FileDownloadLocation())
{
    // Set the location to automatically clean on disposal.
    // Alternatively we could manually call downloadLocation.CleanTemporaryFiles().
    downloadLocation.CleanDirectoryOnDisposal = true;

    // It is assumed that objectFile and vault are already available/populated.
    // Note: the file will be assigned a guid-based filename with a suitable file extension.
    using(var fileDownload = objectFile.Download(vault, downloadLocation))
    {
        // TODO: Perform operations on physicalFile.
    }
}
```

### Controlling block size and file format

By default the file will be downloaded from the server in block sizes of 4096 bytes.  This can be overridden by providing your own `blockSize` in the `Download` methods:

```csharp
// It is assumed that objectFile and vault are already available/populated.
using(var fileDownload = objectFile.Download(vault, @"C:\temp\mydownload.tmp", blockSize: 8192))
{
    // TODO: Perform operations on physicalFile.
}
```

By default the file will be downloaded in the native format.  You can request that the server convert the file to PDF prior to conversion by setting the `fileFormat` in the `Download` methods:

```csharp
// It is assumed that objectFile and vault are already available/populated.
using(var fileDownload = objectFile.Download(vault, @"C:\temp\mydownload.tmp", fileFormat: MFFileFormat.MFFileFormatPDF))
{
    // TODO: Perform operations on physicalFile.
}
```
