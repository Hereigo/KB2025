```cs
using System;
using System.ComponentModel;

public enum Status
{
    [Description("Pending Approval")]
    Pending,
    [Description("Approved by Admin")]
    Approved,
    [Description("Rejected by Admin")]
    Rejected
}

public class Program
{
    public static string GetEnumDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
        return attribute == null ? value.ToString() : attribute.Description;
    }

    public static void Main()
    {
        Status status = Status.Approved;
        string statusDescription = GetEnumDescription(status);

        Console.WriteLine(statusDescription);
    }
}
```