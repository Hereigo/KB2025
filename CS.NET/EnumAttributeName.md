### Get Attribute Name of Enum.

```csharp
public enum MyCustomEnumType
{
    [Display(Name = " Is Active")]
    Active,
    [Display(Name = " Is Not Active")]
    Inactive
}

public static class EnumExtensions
{
    public static string GetEnumDisplayName(this MyCustomEnumType enumType)
    {
        var descrip = enumType.GetType().GetMember(enumType.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()?
            .Name;
        return descrip ?? enumType.ToString();
    }
}
```