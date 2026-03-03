- Press: ctrl+shift+p
- Search Tasks: Open User Tasks
- In the tasks.json file:

```json
// Inside "tasks": [ ... ]
{
  "label": "open file",
  "type": "shell",
  "command": "${file}",
  "presentation": {
    "reveal": "never",
  }
}
// "reveal": "never" - to Hide the task terminal from showing up
```

##### For Windows - "command": "${file}"
##### For Linux - "command": "xdg-open ${file}"

- Press: ctrl+shift+p
- Search: Keyboard shortcuts (JSON)

```json
// inside [ ... ]
{
  "key": "ctrl+alt+l",
  "command": "workbench.action.tasks.runTask",
  "args": "open file"
}
```