### Binary <==> Base 64

```csharp

Byte[] bytes = File.ReadAllBytes("filePath");
String base64string = Convert.ToBase64String(bytes);

// Backward:

Byte[] bytes = Convert.FromBase64String(base64string);
File.WriteAllBytes("filePath", bytes);

```