### Easy LINQ update of collection member.

```csharp

var myList = new List<string>() { "apple", "banana", "pear" };

Console.WriteLine(myList.Except(["apple", "pear"]).Count()); // => 1

/////////////////////////////////////////////////////////////////////

List<MyObj> myList = [ new MyObj {/*...*/}, new MyObj {/*...*/} ];

myList.Where(w => w.Field1 == "NEEDED_TO_UPDATE").ToList().ForEach(w => w.Field2 = "NEW_VALUE");

// here MyList is Modified.

```