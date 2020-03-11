# VaultPropertyDefOperations

## TryGetPropertyDefName

Attempts to retrieve the name of the property definition with the given ID from the vault:

```csharp
var propertyDefId = 1234;
if(vault.PropertyDefOperations.TryGetPropertyDefName(propertyDefId, out string propertyDefName))
{
    Console.WriteLine($"The name of the property definition with ID {propertyDefId} is {propertyDefName}.");
}
else
{
    Console.WriteLine($"A property definition with ID {propertyDefId} could not be found.");
}
```

## TryGetPropertyDefAliases

Attempts to retrieve the aliases of the property definition with the given ID from the vault:

```csharp
var propertyDefId = 1234;
if(vault.PropertyDefOperations.TryGetPropertyDefAliases(propertyDefId, out string[] aliases))
{
    Console.WriteLine($"The property definition with ID {propertyDefId} has {aliases.Length} alias(es):");
    foreach(var alias in aliases)
    {
        Console.WriteLine($"\t {alias}");
    }
}
else
{
    Console.WriteLine($"A property definition with ID {propertyDefId} could not be found.");
}
```

## TryGetPropertyDefDataType

Attempts to retrieve the data type of the property definition with the given ID from the vault:

```csharp
var propertyDefId = 1234;
if(vault.PropertyDefOperations.TryGetPropertyDefDataType(propertyDefId, out MFDataType dataType))
{
    Console.WriteLine($"The data type of the property definition with ID {propertyDefId} is {dataType}.");
}
else
{
    Console.WriteLine($"A property definition with ID {propertyDefId} could not be found.");
}
```

