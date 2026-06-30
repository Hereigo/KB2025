## Text Doesn't Contains tag - <main-menu />

```regex
\A(?:(?!<main-menu\s*/>).)*\z
```

#### The same but multiline search:
```regex
\A(?:(?!<main-menu\s*/>)[\s\S])*\z
```

### Explanation:

- \A — start of the string (entire file)

- (?:(?!<main-menu\s*/>)[\s\S])* — consume every character as long as the forbidden text does not start at the current position

- \z — end of the string

- The \s* allows for optional whitespace before the />, so it matches both:
```
<main-menu />
<main-menu/>
<main-menu    />
```

### Using C#:
```cs
using System.Text.RegularExpressions;

string content = File.ReadAllText(path);

bool doesNotContainMainMenu = Regex.IsMatch(
    content,
    @"\A(?:(?!<main-menu\s*/>)[\s\S])*\z");

if (doesNotContainMainMenu)
{
    // File does NOT contain <main-menu />
}
```
#### or:
```cs
bool doesNotContainMainMenu =
    !content.Contains("<main-menu />");
```
#### or:
```cs
bool doesNotContainMainMenu =
    !Regex.IsMatch(content, @"<main-menu\s*/>");
```
This second approach is what I'd recommend in C#: use a simple regex to detect the tag, then negate the result, rather than trying to make a regex that matches "everything except files containing this text." It is simpler, more readable, and more efficient.