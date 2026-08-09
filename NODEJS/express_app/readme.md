
### Express server start:

```sh
node index.js
# or
npm run start:dev
# (see below)
```

### { "type": "module" } in package.json

* **"module"** means the project use by default *"Ecma-Script"* rather than *"Common Script"*.

* ( **index.js** must be renamed to **index.cjs** ) ???


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

### SQLite
```sh
npm install sqlite3
```

### VSCodium installed as Flatpak

Best solution: make VSCodium terminal use the host shell

- In VSCodium, open:
- Settings → search for terminal profiles linux
- Or edit settings.json and add:

```json
{
    "terminal.integrated.profiles.linux": {
        "Host Bash": {
            "path": "flatpak-spawn",
            "args": ["--host", "bash", "-l"]
        }
    },
    "terminal.integrated.defaultProfile.linux": "Host Bash"
}
```
Then you can:
```sh
flatpak-spawn --host npm --version
```