# MD View on Web page

● I built the page: index.html in the repo root, with a sidebar menu that loads Markdown files from a new docs/ folder and displays them on the page. I checked that all the
  libraries it loads from the CDN are reachable and that a local server serves the Markdown files. I haven't opened it in a browser.

  Files
  - index.html: the sidebar and the content area, all in one file.
  - docs/introduction.md, planning.md, development.md, testing.md, deployment.md: sample pages about the MD View on Web page. They include headings, tables, checklists, quotes and
    code blocks, so you can see how each one displays.

  How it works
  - Clicking a menu item loads that Markdown file and shows it styled like GitHub's Markdown, with colour highlighting in code blocks.
  - The selected menu item is highlighted. Each page gets its own address (e.g. #docs/testing.md), so the browser's back/forward buttons and bookmarks work.
  - A link inside one Markdown file to another (like [Testing](testing.md)) opens inside the page too. Relative image paths also work.
  - On phones, the menu folds into a ☰ button.

  Adding a page: put the .md file in docs/ and add a line to the PAGES list at the top of the script in index.html:
  { title: "My Page", file: "docs/my-page.md" },

  Running it: browsers won't let the page load the .md files if you just double-click index.html. Run it through a small local server:
  
  ```sh
  python3 -m http.server
  ```

  Then open http://localhost:8000. It also works as-is on GitHub Pages.

  Nothing has been committed yet.
