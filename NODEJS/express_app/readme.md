
### Express server start:

```sh
node index.js
# or
npm run start:dev
# (see below)
```

### { "type": "module" } in package.json

* **"module"** means the project use by default *"Ecma-Script"* rather than *"Common Script"*.

* ( **index.js** must be renamed to **index.cjs** )


### nodemon

for the autorestart server during development files update

```sh
npm install --save-dev nodemon 
```

**package.json**

```json
  "scripts": {
    "start:dev": "nodemon index.cjs",
    "test": "echo \"Error: no test specified\" && exit 1"
  },
```
