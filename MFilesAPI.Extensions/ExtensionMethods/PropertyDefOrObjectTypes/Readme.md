# PropertyDefOrObjectTypes

Enables easier population of a `PropertyDefOrObjectTypes` instance (used for indirect searching).

## AddObjectTypeIndirectionLevel

Adds a level of indirection by object type ID.

```csharp
// Create the indirection levels.
var indirectionLevels = new PropertyDefsOrObjectTypes()
    .AddObjectTypeIndirectionLevel(136); // The `Customer` object type Id
```

## AddPropertyDefIndirectionLevel

Adds a level of indirection by property definition ID.

```csharp
// Create the indirection levels.
var indirectionLevels = new PropertyDefsOrObjectTypes()
    .AddPropertyDefIndirectionLevel(1174); // The `Signer` property definition
```