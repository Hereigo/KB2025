# Small JS tips.

```js
// To edit any site - type in dev.tools:

document.designMode="on"



// Filter only numbers from Mixed Array (except of zero)

const arr = [null, 3, 0, 6, 7, -8, "", false];

const truthyArr = arr.filter(Boolean);

console.log(truthyArr); // =>  [ 3, 6, 7, -8 ]

```