### Parse string json into Object with Enum field.

```csharp
using Newtonsoft.Json;
// System.Text.Json - can't parse Enums in simple way.

public enum Status
{
    Unknown = 0,
    Active = 1,
    Inactive = 2
}

public class MyObject
{
    public string Name { get; set; }
    public Status Status { get; set; }
}

static void Main()
{
    string json = 
    @"{
        'Name': 'Example',
        'Status': 1
    }";

    var obj = JsonConvert.DeserializeObject<MyObject>(json);
}
```