## Work with cookie:
```js
// SET:

document.cookie = "theme=dark; max-age=31536000; path=/; SameSite=Lax";

// READ:

const theme = document.cookie.split('; ').find(row => row.startsWith('theme='))?.split('=')[1];

// READ ANY BY NAME:

function getCookie(name) {
    let value = `; ${document.cookie}`;
    let parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}

let theme = getCookie("theme");

// DELETE:

document.cookie = "theme=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
```