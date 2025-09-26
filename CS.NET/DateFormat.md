
### Date Format like - 15th November 2001

```csharp

string REZULT_DATE = FormatDateWithSuffix(date);

static string FormatDateWithSuffix(DateTime date)
{
    int day = date.Day;
    string suffix = GetDaySuffix(day);
    return $"{day}{suffix} {date:MMMM yyyy}";
}

static string GetDaySuffix(int day)
{
    if (day >= 11 && day <= 13)
        return "th";
    switch (day % 10)
    {
        case 1: return "st";
        case 2: return "nd";
        case 3: return "rd";
        default: return "th";
    }
}
```