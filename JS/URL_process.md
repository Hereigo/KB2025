## URL address processing:

```js
const url = "https://example.com/path/to/section/";
const parts = url.split('/');
const lastPart = parts.filter(Boolean).pop();

console.log(lastPart); // Output: "section"
```