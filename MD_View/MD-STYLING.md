### To modify styles of displayed in web page md files

1. The Markdown renders into `<article class="markdown-body" id="content">` (index.html:82)

2. GitHub-style comes from the github-markdown-css stylesheet loaded at index.html

3. To change it, add your own rules to the `<style>` block, scoped to .markdown-body.

```css
/* ---------- Markdown overrides ---------- */
.markdown-body {
  font-family: Georgia, serif;

  line-height: 1.7;
}
.markdown-body h1,
.markdown-body h2 {
  color: #0969da;
  border-bottom: 2px solid #0969da;
}
.markdown-body a { color: #8250df; }
.markdown-body blockquote {
  border-left-color: #0969da;

}
.markdown-body table th { background: #ddf4ff; }
.markdown-body pre { border-radius: 8px; }
.markdown-body img { max-width: 100%; border-radius: 6px; }
```

If a rule doesn't take effect, the library's selector is more specific than yours. Copy its selector from DevTools (right-click → Inspect) or make yours more specific, for example #content.markdown-body h1.

2. Change the theme variables

github-markdown-css v5 takes most of its colors from CSS variables. Overriding tnce:

```css
.markdown-body {
  --fgColor-default: #24292f;
  --fgColor-accent: #0969da;          /* links */
  --bgColor-muted: #f6f8fa;           /* code blocks, table stripes */
  --borderColor-default: #d0d7de;
}
```

3. Fix the light/dark mismatch

github-markdown.min.css switches to dark colors automatically when the OS is in dark mode. Your body and sidebar are hard-coded light (#fff, #f6f8fa), so in dark mode you get a dark article
on a light page. You can fix this either way:
- Light only: change line 7 to github-markdown-light.min.css. Line 8 already uses the light github.min.css for code highlighting.
- Support dark mode: add a @media (prefers-color-scheme: dark) block that also darkens body and .sidebar, and swap the code highlighting to github-dark.min.css.


DOMPurify keeps class and style attributes by default, so you can write inline HTML in a .md file:

```html
<div class="note">Heads up: this step needs admin rights.</div>
```
```css
.markdown-body .note { 
    background: #fff8c5; border: 1px solid #d4a72c; padding: 12px; border-radius: 6px; 
}
```

5. Change the code highlight theme

Replace the URL on line 8 with another highlight.js theme, such as atom-one-dark.min.css, vs2015.min.css or github-dark.min.css, from the same highlight.js/11.9.0/styles/ path.

To see changes, serve the folder with python3 -m http.server and refresh the pagdex.html notes, the page can't load the .md files when opened from disk. If youtell me the look you want, I can add the CSS for you.
