## Get Enum - [Display(Name="Some Enum Field")]

```html
<div class="form-group">
    <p>
        @Html.RadioButtonFor(m => m.aType, SomeType.Active)
        @Html.LabelFor(m => m.aType, SomeType.Active.GetEnumDisplayName())
    </p>
    <p>
        @Html.RadioButtonFor(m => m.aType, SomeType.Inactive)
        @Html.LabelFor(m => m.aType, SomeType.Inactive.GetEnumDisplayName())
    </p>
</div>
```
```csharp
public enum SomeType
{
    [Display(Name = "Is Active")]
    Active,
    [Display(Name = "Is Not Active")]
    Inactive
}

public class SomeEntity
{
    public int Id { get; set; }
    public SomeType aType { get; set; }
}

public static class EnumExtensions
{
    public static string GetEnumDisplayName(this SomeType enumType)
    {
        var descrip = enumType.GetType().GetMember(enumType.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()?
            .Name;
        return descrip ?? enumType.ToString();
    }
}
```