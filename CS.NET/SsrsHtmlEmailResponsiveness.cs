using System.Text.RegularExpressions;
using HtmlAgilityPack;

/// <summary>
/// Minimally modifies SSRS-generated email HTML to make it responsive on mobile email clients
/// (e.g., Outlook iOS/Android, Apple Mail, Gmail app).
/// Pure inline-style approach — no embedded &lt;style&gt; block, works on any screen width.
/// </summary>
static void EmailHtmlResponsiveFixer(string htmlFilePath)
{
    if (string.IsNullOrWhiteSpace(htmlFilePath) || !File.Exists(htmlFilePath))
    {
        string html = File.ReadAllText(htmlFilePath);
        string outputPath = Path.ChangeExtension(htmlFilePath, "_responsive.html");

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        InjectViewportMeta(doc);
        FixTableStyles(doc);
        FixCellStyles(doc);
        FixDivStyles(doc);
        FixLinkStyles(doc);
        RemoveColsAttribute(doc);

        string responsiveHtml = doc.DocumentNode.OuterHtml;

        File.WriteAllText(outputPath, responsiveHtml);
    }
}

static void InjectViewportMeta(HtmlDocument doc)
{
    var head = doc.DocumentNode.SelectSingleNode("//head");
    if (head == null) return;

    if (head.SelectSingleNode("meta[@name='viewport']") != null) return;

    var meta = doc.CreateElement("meta");
    meta.SetAttributeValue("name", "viewport");
    meta.SetAttributeValue("content", "width=device-width, initial-scale=1.0");
    head.AppendChild(meta);
}

/// <summary>
/// Tables: width:100%, remove min-width, remove border-collapse fixed widths.
/// </summary>
static void FixTableStyles(HtmlDocument doc)
{
    var tables = doc.DocumentNode.SelectNodes("//table");
    if (tables == null) return;

    foreach (var table in tables)
    {
        var style = table.GetAttributeValue("style", "");
        bool empty = !HasTextContent(table);

        // Replace any fixed WIDTH in mm
        style = Regex.Replace(style,
            @"(?<!\-)WIDTH\s*:\s*[\d.]+mm",
            empty ? "width:0;max-width:0;display:none" : "width:100%",
            RegexOptions.IgnoreCase);

        // Remove min-width
        style = Regex.Replace(style,
            @"min-width\s*:\s*[\d.]+mm\s*;?",
            "",
            RegexOptions.IgnoreCase);

        // Ensure empty tables are always fully collapsed, even if they had
        // no WIDTH:mm in the style (e.g. only HTML width/height attributes)
        if (empty && style.IndexOf("display:none", StringComparison.OrdinalIgnoreCase) < 0)
        {
            style = style.TrimEnd(';') + ";width:0;max-width:0;display:none";
        }
        else if (!empty && !Regex.IsMatch(style, @"(?<!max-)width\s*:", RegexOptions.IgnoreCase))
        {
            style = style.TrimEnd(';') + ";width:100%";
        }

        style = CleanStyle(style);
        table.SetAttributeValue("style", style);

        // Remove HTML width/height attributes so they don't override inline styles
        if (table.Attributes["width"] != null) table.Attributes.Remove("width");
        //if (table.Attributes["height"] != null) table.Attributes.Remove("height");
    }
}

/// <summary>
/// TD cells: content cells get width:100%, spacer cells get width:0;padding:0;font-size:0.
/// Heights > 5 mm become auto.
/// </summary>
static void FixCellStyles(HtmlDocument doc)
{
    var cells = doc.DocumentNode.SelectNodes("//td");
    if (cells == null) return;

    foreach (var td in cells)
    {
        var style = td.GetAttributeValue("style", "");

        bool isEmpty = !HasTextContent(td);

        if (isEmpty)
        {
            // Regex-replace any fixed WIDTH in mm
            style = Regex.Replace(style,
                @"(?<!\-)WIDTH\s*:\s*[\d.]+mm",
                "width:0;max-width:0;display:none",
                RegexOptions.IgnoreCase);

            style = Regex.Replace(style,
                @"min-width\s*:\s*[\d.]+mm\s*;?",
                "",
                RegexOptions.IgnoreCase);

            // Zero out any HEIGHT in mm
            //style = Regex.Replace(style,
            //    @"HEIGHT\s*:\s*[\d.]+mm",
            //    "height:0",
            //    RegexOptions.IgnoreCase);

            // Remove padding if present
            style = Regex.Replace(style,
                @"padding[^;]*;?",
                "",
                RegexOptions.IgnoreCase);

            // Ensure collapse styles are always present, even if the cell
            // had no WIDTH:mm (e.g. only HTML width/height attributes)
            if (style.IndexOf("display:none", StringComparison.OrdinalIgnoreCase) < 0)
                style = style.TrimEnd(';') + ";width:0;max-width:0;display:none";

            style = style.TrimEnd(';') + ";padding:0;font-size:0;overflow:hidden";
        }
        else
        {
            // Content cell: make fluid
            style = Regex.Replace(style,
                @"(?<!\-)WIDTH\s*:\s*[\d.]+mm",
                "width:100%",
                RegexOptions.IgnoreCase);

            // Remove min-width
            style = Regex.Replace(style,
                @"min-width\s*:\s*[\d.]+mm\s*;?",
                "",
                RegexOptions.IgnoreCase);

            // Large heights -> auto
            // style = Regex.Replace(style,
            //     @"HEIGHT\s*:\s*(\d+\.?\d*)mm",
            //     m =>
            //     {
            //         if (double.TryParse(m.Groups[1].Value,
            //                 System.Globalization.NumberStyles.Any,
            //                 System.Globalization.CultureInfo.InvariantCulture,
            //                 out var v) && v > 5)
            //             return "height:auto";
            //         return m.Value;
            //     },
            //     RegexOptions.IgnoreCase);

            // overflow -> visible
            style = Regex.Replace(style,
                @"overflow(?:-x)?\s*:\s*(?:auto|hidden)",
                "overflow:visible",
                RegexOptions.IgnoreCase);

            // Ensure width:100% is present even if there was no WIDTH:mm
            if (!Regex.IsMatch(style, @"(?<!max-)width\s*:", RegexOptions.IgnoreCase))
                style = style.TrimEnd(';') + ";width:100%";
        }

        style = CleanStyle(style);
        td.SetAttributeValue("style", style);

        // Remove HTML width/height attributes so they don't override inline styles
        if (td.Attributes["width"] != null) td.Attributes.Remove("width");
        //if (td.Attributes["height"] != null) td.Attributes.Remove("height");
    }
}

/// <summary>
/// Divs: fixed widths become 100%, overflow becomes visible.
/// </summary>
static void FixDivStyles(HtmlDocument doc)
{
    var divs = doc.DocumentNode.SelectNodes("//div[@style]");
    if (divs == null) return;

    foreach (var div in divs)
    {
        var style = div.GetAttributeValue("style", "");

        style = Regex.Replace(style,
            @"(?<!\-)WIDTH\s*:\s*[\d.]+mm",
            "width:100%",
            RegexOptions.IgnoreCase);

        style = Regex.Replace(style,
            @"min-width\s*:\s*[\d.]+mm\s*;?",
            "",
            RegexOptions.IgnoreCase);

        style = Regex.Replace(style,
            @"overflow(?:-x)?\s*:\s*(?:auto|hidden)",
            "overflow:visible",
            RegexOptions.IgnoreCase);

        style = CleanStyle(style);
        div.SetAttributeValue("style", style);
    }
}

/// <summary>
/// Links: add word-break so long URLs wrap on narrow screens.
/// </summary>
static void FixLinkStyles(HtmlDocument doc)
{
    var links = doc.DocumentNode.SelectNodes("//a");
    if (links == null) return;

    foreach (var a in links)
    {

        if (a.GetAttributeValue("href", "").Contains("@")) continue;

        Console.WriteLine($"Fixing link style: {a.GetAttributeValue("href", "")}");

        var style = a.GetAttributeValue("style", "");
        if (style.IndexOf("word-break", StringComparison.OrdinalIgnoreCase) < 0)
        {
            style = string.IsNullOrWhiteSpace(style)
                ? "word-break:break-all"
                : style.TrimEnd(';') + ";word-break:break-all";
        }
        a.SetAttributeValue("style", style);
    }
}

static void RemoveColsAttribute(HtmlDocument doc)
{
    var tables = doc.DocumentNode.SelectNodes("//table[@cols]");
    if (tables == null) return;

    foreach (var table in tables)
        table.Attributes.Remove("cols");
}

/// <summary>
/// Returns true if the node contains any visible text content.
/// Empty divs, empty spans, whitespace, and &amp;nbsp; are treated as no content.
/// </summary>
static bool HasTextContent(HtmlNode node)
{
    // Collect inner text, then strip whitespace and non-breaking spaces
    var text = node.InnerText;
    if (string.IsNullOrEmpty(text)) return false;

    // Remove &nbsp; (\u00A0), regular spaces, tabs, newlines
    var cleaned = text
        .Replace("\u00A0", "")
        .Replace("&nbsp;", "")
        .Trim();

    return cleaned.Length > 0;
}

static string CleanStyle(string style)
{
    style = Regex.Replace(style, @";{2,}", ";");
    style = style.Trim().Trim(';').Trim();
    return style;
}