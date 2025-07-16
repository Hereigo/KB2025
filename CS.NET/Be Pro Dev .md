```csharp

// REDUCE NESTING!

if(model is not null){
    var isNotOld = model.CreatedDate < DateTime.Now.AddMonths(-2);
    if(isNotOld)
    {
        foreach(var i in model.SomeFields)
        {
            // to do smth...
        }
    }
}
// Code above should be replaced with next: ======================

if(model is null || model.CreatedDate >= DateTime.Now.AddMonths(-2))
{
    return;
}

foreach(var i in model.SomeFields)
{
    // to do smth...
}

```