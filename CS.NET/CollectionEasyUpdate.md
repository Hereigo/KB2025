### Easy LINQ update of collection member.

```csharp

List<MyObj> myList = [ new MyObj {/*...*/}, new MyObj {/*...*/} ];

myList.Where(w => w.Field1 == "NEEDED_TO_UPDATE").ToList().ForEach(w => w.Field2 = "NEW_VALUE");

// here MyList is Modified.

```