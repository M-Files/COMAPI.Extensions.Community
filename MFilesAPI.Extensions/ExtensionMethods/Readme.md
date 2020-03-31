# Extension methods

Much of the functionality within the extensions library is based around the use of .NET extension methods.  These extension methods can be used to add functionality to M-Files API objects without the need for additional classes or data structures.

To use these methods ensure that you import the `MFilesAPI.Extensions` namespace:

```csharp
using MFilesAPI.Extensions;
```

## VaultClassOperations

These vault extension methods provide easier access to information about classes within the M-Files vault.  More information is available within the [dedicated page](VaultClassOperations).

## VaultPropertyDefOperations

These vault extension methods provide easier access to information about property definitions within the M-Files vault.  More information is available within the [dedicated page](VaultPropertyDefOperations).

## ObjectFileExtensionMethods

These extension methods add functionality to the `ObjectFile` class from the M-Files COM API.

### File downloading

Files can easily be downloaded using one of the `Download` overloads:

```csharp
foreach(var file in objectVersion.Files.Cast<ObjectFile>())
{
    // By surrounding the call in a using statement, the temporary file on disk
    // will automatically be cleaned up for us when we're finished.
    var temporaryFile = @"C:\temp\file.tmp";
    using(var download = file.Download(vault, temporaryFile))
    {
        // TODO: Work with the downloaded file.
    }
}
```

### Reading as a Stream

Often it is better to read a file as a `System.IO.Stream`, rather than downloading the entire file to disk.  The `OpenRead` extension method can be used to progressively download a file as a Stream:

```csharp
foreach(var file in objectVersion.Files.Cast<ObjectFile>())
{
    using(var stream = file.OpenRead(vault))
    {
        // TODO: Access the data via the stream.
    }
}
```

## PropertyDefOrObjectTypes

These vault extension methods allow easier population of a `PropertyDefOrObjectTypes` instance (used for indirect searching).
