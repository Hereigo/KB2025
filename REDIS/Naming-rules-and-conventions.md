## Naming conventions

By convention, C# programs use PascalCase for type names, namespaces, and all public members. In addition, the dotnet/docs team uses the following conventions, adopted from the .NET Runtime team's coding style:

- Interface names start with a capital I.

- Attribute types end with the word Attribute.

- Enum types use a singular noun for nonflags, and a plural noun for flags.

- Identifiers shouldn't contain two consecutive underscore (_) characters. Those names are reserved for compiler-generated identifiers.

- Use meaningful and descriptive names for variables, methods, and classes.

- Prefer clarity over brevity.

- Use PascalCase for class names and method names.

- Use camelCase for method arguments, local variables, and private and internal non-constant fields.

- Private and internal non-constant instance fields start with an underscore (_).

- To maintain consistency across all access modifiers, use PascalCase for constant names, both fields and local constants, including private and internal constants.

- Static fields start with s_. This convention isn't the default Visual Studio behavior, nor part of the Framework design guidelines, but is configurable in editorconfig.

- Avoid using abbreviations or acronyms in names, except for widely known and accepted abbreviations.

- Use meaningful and descriptive namespaces that follow the reverse domain name notation.

- Choose assembly names that represent the primary purpose of the assembly.

- Avoid using single-letter names, except for simple loop counters. Also, syntax examples that describe the syntax of C# constructs often use the following single-letter names that match the convention used in the C# language specification.

> #### Tip :
> You can enforce naming conventions that concern capitalization, prefixes, suffixes, and word separators by using code-style naming rules.

In the following examples, guidance pertaining to elements marked public is also applicable when working with protected and protected internal elements, all of which are intended to be visible to external callers.

### Pascal case

Use pascal casing ("PascalCasing") when naming a class, interface, struct, or delegate type.

```csharp
public class DataService
{
}

public record PhysicalAddress(
    string Street,
    string City,
    string StateOrProvince,
    string ZipCode);

public struct ValueCoordinate
{
}

public delegate void DelegateType(string message);
```
When naming an interface, use pascal casing in addition to prefixing the name with an I. This prefix clearly indicates to consumers that it's an interface.

```csharp
public interface IWorkerQueue
{
}
```
When naming public members of types, such as fields, properties, events, use pascal casing. Also, use pascal casing for all methods and local functions.

```csharp
public class ExampleEvents
{
    // A public field, these should be used sparingly
    public bool IsValid;

    // An init-only property
    public IWorkerQueue WorkerQueue { get; init; }

    // An event
    public event Action EventProcessing;

    // Method
    public void StartEventProcessing()
    {
        // Local function
        static int CountQueueItems() => WorkerQueue.Count;
        // ...
    }
}
```
When writing positional records, use pascal casing for parameters as they're the public properties of the record.

```csharp
public record PhysicalAddress(
    string Street,
    string City,
    string StateOrProvince,
    string ZipCode);
```    
For more information on positional records, see Positional syntax for property definition.

### Camel case

Use camel casing ("camelCasing") when naming private or internal non-constant fields, and prefix them with _. Use camel casing when naming local variables, including instances of a delegate type.

```csharp
public class DataService
{
    private IWorkerQueue _workerQueue;
}
```
> #### Tip :
> When editing C# code that follows these naming conventions in an IDE that supports statement completion, typing _ will show all of the object-scoped members.

When working with static fields that are private or internal, use the s_ prefix and for thread static use t_.

```csharp
public class DataService
{
    private static IWorkerQueue s_workerQueue;

    [ThreadStatic]
    private static TimeSpan t_timeSpan;
}
```
When writing method parameters, use camel casing.

```csharp
public T SomeMethod<T>(int someNumber, bool isValid)
{
}
```
### Primary constructor parameters

How you name primary constructor parameters depends on the type being declared:

For class and struct types: Use camel casing, consistent with other method parameters.

```csharp
public class DataService(IWorkerQueue workerQueue, ILogger logger)
{
    public void ProcessData()
    {
        // Use the parameters directly
        logger.LogInformation("Processing data");
        workerQueue.Enqueue("data");
    }
}

public struct Point(double x, double y)
{
    public double Distance => Math.Sqrt(x * x + y * y);
}
```
For record types: Use Pascal casing, as the parameters become public properties.

```csharp
public record Person(string FirstName, string LastName);
public record Address(string Street, string City, string PostalCode);
```
For more information on primary constructors, see Primary constructors.

### Type parameter naming guidelines

The following guidelines apply to type parameters on generic type parameters. Type parameters are the placeholders for arguments in a generic type or a generic method. You can read more about generic type parameters in the C# programming guide.

Do name generic type parameters with descriptive names, unless a single letter name is completely self explanatory and a descriptive name wouldn't add value.

```csharp
public interface ISessionChannel<TSession> { /*...*/ }
public delegate TOutput Converter<TInput, TOutput>(TInput from);
public class List<T> { /*...*/ }
```
Consider using T as the type parameter name for types with one single letter type parameter.

```csharp
public int IComparer<T>() => 0;
public delegate bool Predicate<T>(T item);
public struct Nullable<T> where T : struct { /*...*/ }
```
Do prefix descriptive type parameter names with "T".

```csharp
public interface ISessionChannel<TSession>
{
    TSession Session { get; }
}
```
Consider indicating constraints placed on a type parameter in the name of parameter. For example, a parameter constrained to ISession might be called TSession.

The code analysis rule CA1715 can be used to ensure that type parameters are named appropriately.

### Extra naming conventions

Examples that don't include using directives, use namespace qualifications. If you know that a namespace is imported by default in a project, you don't have to fully qualify the names from that namespace. Qualified names can be broken after a dot (.) if they're too long for a single line, as shown in the following example.

```csharp
var currentPerformanceCounterCategory = new System.Diagnostics.
    PerformanceCounterCategory();
```
You don't have to change the names of objects that were created by using the Visual Studio designer tools to make them fit other guidelines.