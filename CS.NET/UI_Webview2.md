#### Program.cs

```csharp
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}

internal class MainForm : Form
{
    private readonly WebView2 _webView = new()
    {
        Dock = DockStyle.Fill
    };

    public MainForm()
    {
        Text = "WebView2 Minimal App";
        Width = 800;
        Height = 500;

        Controls.Add(_webView);
        Load += MainForm_Load;
    }

    private async void MainForm_Load(object? sender, EventArgs e)
    {
        await _webView.EnsureCoreWebView2Async();
        _webView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;

        var htmlPath = Path.Combine(AppContext.BaseDirectory, "index.html");
        _webView.Source = new Uri(htmlPath);
    }

    private void CoreWebView2_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        var message = e.TryGetWebMessageAsString();
        if (message == "getValue")
        {
            var valueFromCSharp = $"Value from C#: {DateTime.Now:HH:mm:ss}";
            _webView.CoreWebView2.PostWebMessageAsString(valueFromCSharp);
        }
    }
}
```
#### index.html

```html
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>WebView2 Minimal UI</title>
    <style>
        body {
            font-family: Segoe UI, Arial, sans-serif;
            margin: 24px;
        }
        .row {
            display: flex;
            gap: 8px;
            align-items: center;
        }
        input {
            width: 350px;
            padding: 6px;
        }
        button {
            padding: 6px 12px;
        }
    </style>
</head>
<body>
    <h3>WebView2 Demo</h3>
    <div class="row">
        <button id="btnGetValue">Get value from C#</button>
        <input id="result" type="text" readonly placeholder="Value from C# will appear here" />
    </div>

    <script>
        const btn = document.getElementById('btnGetValue');
        const result = document.getElementById('result');

        btn.addEventListener('click', () => {
            window.chrome.webview.postMessage('getValue');
        });

        window.chrome.webview.addEventListener('message', (event) => {
            result.value = event.data;
        });
    </script>
</body>
</html>
```