# Small JS tips.

### Prevent Memory Leaks:

1- Use let and const: Avoid global variables.

2- Clean up event listeners: Always remove listeners when they’re no longer needed.

3- Break references: Set unused objects or variables to null.

4- Avoid excessive closures: Be mindful of closures, especially in loops.

5- Profile regularly: Use browser tools to monitor memory usage.


```js
// To edit any site - type in dev.tools:

document.designMode="on"



// Filter only numbers from Mixed Array (except of zero)

const arr = [null, 3, 0, 6, 7, -8, "", false];

const truthyArr = arr.filter(Boolean);

console.log(truthyArr); // =>  [ 3, 6, 7, -8 ]

```