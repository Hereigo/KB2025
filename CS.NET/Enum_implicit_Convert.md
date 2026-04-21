```csharp
enum MyEnum { One = 1, Two = 2 };

private static void Main(string[] args)
{
    Foo(MyEnum.One); // enum: One
    Foo(0);          // enum: 0
    Foo(1);          // object: 1
}

// zero (0) int can be implicitly converted to any enum type!

private static void Foo(MyEnum value)
{
    Console.WriteLine("enum: " + value);
}

private static void Foo(object value)
{
    Console.WriteLine("object: " + value);
}
```