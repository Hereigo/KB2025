using System.Data;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ConsoleApp1_Test;

internal static class Text_Xlsx
{
    internal static void SaveDatasetToXlsx(string filePath, DataSet dataSet)
    {
        using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
        {
            WorkbookPart workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            XlsxStyles styles = new XlsxStyles();
            uint headerStyle = styles.AddStyle(bold: true, fontColor: "FFFFFFFF", backgroundColor: "FF000000", border: true, horizontalAlignment: HorizontalAlignmentValues.Center);
            uint currencyStyle = styles.AddStyle(bold: true, fontColor: "FFFF0000", backgroundColor: "FFDDDDDD", border: true, numberFormatId: 4);
            uint ageStyle = styles.AddStyle(backgroundColor: "FF00FF00", numberFormatId: 1);
            styles.ApplyTo(workbookPart);

            Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

            uint sheetId = 1;

            foreach (DataTable table in dataSet.Tables)
            {
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                SheetData sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                // Add sheet to workbook
                sheets.Append(new Sheet()
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = sheetId++,
                    Name = table.TableName
                });

                Row headerRow = new Row();
                foreach (DataColumn column in table.Columns)
                {
                    Cell cell = new Cell
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(column.ColumnName),
                        StyleIndex = headerStyle
                    };
                    headerRow.Append(cell);
                }
                sheetData.Append(headerRow);

                // int rowsCounter = 0;

                foreach (DataRow dr in table.Rows)
                {
                    Row newRow = new Row();
                    foreach (DataColumn column in table.Columns)
                    {
                        Cell cell = new Cell();

                        object value = dr[column];

                        if (column.ColumnName == "Balance" && value is decimal balance && balance < 1000)
                        {
                            cell.StyleIndex = currencyStyle;
                        }
                        else if (column.ColumnName == "Age")
                        {
                            cell.StyleIndex = ageStyle;
                        }

                        if (value is string)
                        {
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(value.ToString());
                        }
                        else if (value is int or decimal or double or float)
                        {
                            cell.DataType = CellValues.Number;
                            cell.CellValue = new CellValue(Convert.ToString(value));
                        }
                        else
                        {
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(value?.ToString() ?? string.Empty);
                        }

                        newRow.Append(cell);
                    }
                    sheetData.Append(newRow);
                }

                // Example: Set column width
                Columns columns = new Columns(
                    new Column { Min = 1, Max = (uint)table.Columns.Count, Width = 20, CustomWidth = true }
                );
                worksheetPart.Worksheet.InsertAt(columns, 0);


                // Add Formulas:
                int rowFrom = 2; // Assuming the first data row is at index 2 (after header)
                int rowTo = table.Rows.Count + 1; // Last data row index (data rows + header rows count)

                Row formulaRow = new Row();
                Cell formulaCell = new Cell
                {
                    CellFormula = new CellFormula($"SUM(A{rowFrom}:A{rowTo})"),
                    DataType = CellValues.Number
                };
                formulaRow.Append(formulaCell);
                sheetData.Append(formulaRow);
            }

            workbookPart.Workbook.Save();
        }
    }

}

/// <summary>
/// Builds a stylesheet from simple parameters; <see cref="AddStyle"/> returns the
/// index to assign to <see cref="Cell.StyleIndex"/>.
/// </summary>
internal sealed class XlsxStyles
{
    private readonly List<Font> _fonts = new() { new Font() };

    // Excel reserves fill 0 (none) and fill 1 (gray125).
    private readonly List<Fill> _fills = new()
    {
        new Fill(new PatternFill { PatternType = PatternValues.None }),
        new Fill(new PatternFill { PatternType = PatternValues.Gray125 })
    };

    private readonly List<Border> _borders = new() { new Border() };
    private readonly List<CellFormat> _cellFormats = new() { new CellFormat() };

    internal uint AddStyle(
        bool bold = false,
        bool italic = false,
        double? fontSize = null,
        string? fontName = null,
        string? fontColor = null,
        string? backgroundColor = null,
        bool border = false,
        uint numberFormatId = 0,
        HorizontalAlignmentValues? horizontalAlignment = null,
        bool wrapText = false)
    {
        uint fontId = AddFont(bold, italic, fontSize, fontName, fontColor);
        uint fillId = AddFill(backgroundColor);
        uint borderId = border ? AddThinBorder() : 0;

        CellFormat format = new CellFormat
        {
            FontId = fontId,
            FillId = fillId,
            BorderId = borderId,
            NumberFormatId = numberFormatId,
            ApplyFont = fontId != 0,
            ApplyFill = fillId != 0,
            ApplyBorder = borderId != 0,
            ApplyNumberFormat = numberFormatId != 0
        };

        if (horizontalAlignment.HasValue || wrapText)
        {
            Alignment alignment = new Alignment();
            if (horizontalAlignment.HasValue)
            {
                alignment.Horizontal = horizontalAlignment.Value;
            }
            if (wrapText)
            {
                alignment.WrapText = true;
            }
            format.Append(alignment);
            format.ApplyAlignment = true;
        }

        return Register(_cellFormats, format);
    }

    internal void ApplyTo(WorkbookPart workbookPart)
    {
        WorkbookStylesPart stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();

        stylesPart.Stylesheet = new Stylesheet(
            new Fonts(_fonts.Select(f => f.CloneNode(true))) { Count = (uint)_fonts.Count },
            new Fills(_fills.Select(f => f.CloneNode(true))) { Count = (uint)_fills.Count },
            new Borders(_borders.Select(b => b.CloneNode(true))) { Count = (uint)_borders.Count },
            new CellFormats(_cellFormats.Select(c => c.CloneNode(true))) { Count = (uint)_cellFormats.Count }
        );

        stylesPart.Stylesheet.Save();
    }

    private uint AddFont(bool bold, bool italic, double? fontSize, string? fontName, string? fontColor)
    {
        if (!bold && !italic && fontSize is null && fontName is null && fontColor is null)
        {
            return 0;
        }

        Font font = new Font();
        if (bold)
        {
            font.Append(new Bold());
        }
        if (italic)
        {
            font.Append(new Italic());
        }
        if (fontSize.HasValue)
        {
            font.Append(new FontSize { Val = fontSize.Value });
        }
        if (fontColor is not null)
        {
            font.Append(new Color { Rgb = fontColor });
        }
        if (fontName is not null)
        {
            font.Append(new FontName { Val = fontName });
        }

        return Register(_fonts, font);
    }

    private uint AddFill(string? backgroundColor)
    {
        if (backgroundColor is null)
        {
            return 0;
        }

        Fill fill = new Fill(
            new PatternFill(new ForegroundColor { Rgb = backgroundColor })
            {
                PatternType = PatternValues.Solid
            });

        return Register(_fills, fill);
    }

    private uint AddThinBorder()
    {
        Border border = new Border(
            new LeftBorder(new Color { Auto = true }) { Style = BorderStyleValues.Thin },
            new RightBorder(new Color { Auto = true }) { Style = BorderStyleValues.Thin },
            new TopBorder(new Color { Auto = true }) { Style = BorderStyleValues.Thin },
            new BottomBorder(new Color { Auto = true }) { Style = BorderStyleValues.Thin },
            new DiagonalBorder());

        return Register(_borders, border);
    }

    private static uint Register<T>(List<T> items, T element) where T : OpenXmlElement
    {
        string xml = element.OuterXml;
        int existing = items.FindIndex(i => i.OuterXml == xml);
        if (existing >= 0)
        {
            return (uint)existing;
        }

        items.Add(element);
        return (uint)(items.Count - 1);
    }
}