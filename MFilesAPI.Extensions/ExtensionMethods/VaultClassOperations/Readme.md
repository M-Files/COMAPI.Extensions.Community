# VaultClassOperations

## TryGetObjectClassName

Attempts to retrieve the name of the class with the given ID from the vault:

```csharp
var classId = 1234;
if(vault.ClassOperations.TryGetObjectClassName(classId, out string className))
{
    Console.WriteLine($"The name of the class with ID {classId} is {className}.");
}
else
{
    Console.WriteLine($"A class with ID {classId} could not be found.");
}
```

## TryGetObjectClassAliases

Attempts to retrieve the aliases of the class with the given ID from the vault:

```csharp
var classId = 1234;
if(vault.ClassOperations.TryGetObjectClassAliases(classId, out string[] aliases))
{
    Console.WriteLine($"The class with ID {classId} has {aliases.Length} alias(es):");
    foreach(var alias in aliases)
    {
        Console.WriteLine($"\t {alias}");
    }
}
else
{
    Console.WriteLine($"A class with ID {classId} could not be found.");
}
```

## TryGetObjectClassNamePropertyDef

Attempts to retrieve the ID of the property definition associated with the class with the given ID from the vault:

```csharp
var classId = 1234;
if(vault.ClassOperations.TryGetObjectClassNamePropertyDef(classId, out int nameOrTitlePropertyDef))
{
    Console.WriteLine($"The property set as 'Name or Title' for the class with ID {classId} is {nameOrTitlePropertyDef}.");
}
else
{
    Console.WriteLine($"A class with ID {classId} could not be found.");
}
```

## TryGetObjectClassWorkflowDetails

Attempts to retrieve the workflow details (the workflow ID and whether the workflow is forced) associated with the class with the given ID from the vault:

```csharp
var classId = 1234;
if(vault.ClassOperations.TryGetObjectClassWorkflowDetails(classId, out int workflowId, out bool forced))
{
    if(workflowId <= 0)
    {
        Console.WriteLine($"The class with ID {classId} does not have a default workflow set.")
    }
    else
    {
        var forcedString = forced ? "is" : "is not";
        Console.WriteLine($"The class with ID {classId} has a default workflow set as {workflowId}.  This workflow {forcedString} forced.");
    }
}
else
{
    Console.WriteLine($"A class with ID {classId} could not be found.");
}
```

