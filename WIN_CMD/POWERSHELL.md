```powershell

# DATETIME CHANGE:
(Get-Item "C:\Users\...path...\SomeFile.txt").LastWriteTime=("12/31/2000 12:01:42")

# MARKDOWN TO HTML:
# ConvertFrom-Markdown - needs because by default will output it to a MarkdownInfo object
(Get-Item .\SomeFile.md | ConvertFrom-Markdown).Html

```