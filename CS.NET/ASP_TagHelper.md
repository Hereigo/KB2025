# ASP TagHelper

#### Index.cshtml

```xml
<my-div pageTitle="MY-HOME-PAGE"></my-div>
```

#### MyDivTagHelper.cs

```csharp
[HtmlTargetElement("my-div")] // optional → "MyDivTagHelper" is default class name for <my-div>
public class MyDivTagHelper : TagHelper
{
    // This maps to attribute: pageTitle="..."
    public string PageTitle { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div"; // render as <div>
        output.Content.SetContent($"This is page: {PageTitle}");
        output.Attributes.SetAttribute("class", "my-custom-div");
    }
}
```

#### _ViewImports.cshtml

```java
@using Microsoft.AspNetCore.Mvc.TagHelpers
@using MyAssemblyName.TagHelpers
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, MyAssemblyName // → link to assembly to use MyDivTagHelper from it.
```
