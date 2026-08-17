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

            AddStylesToWorkbook(workbookPart);

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
                        StyleIndex = 1
                    };
                    headerRow.Append(cell);
                }
                sheetData.Append(headerRow);

                foreach (DataRow dr in table.Rows)
                {
                    Row newRow = new Row();
                    foreach (DataColumn column in table.Columns)
                    {
                        Cell cell = new Cell();

                        object value = dr[column];

                        if (column.ColumnName == "Balance" && value is decimal balance && balance < 1000)
                        {
                            cell.StyleIndex = 3; // red style
                        }
                        else if (column.ColumnName == "Age")
                        {
                            cell.StyleIndex = 4; // green style
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

                // Set column width:
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

    private static void AddStylesToWorkbook(WorkbookPart workbookPart)
    {
        WorkbookStylesPart stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();

        stylesPart.Stylesheet = new Stylesheet(
            new Fonts(
                new Font(),
                new Font(new Bold()), // Bold
                new Font(new Color { Rgb = "FFFF0000" }), // Red
                new Font(new Bold(), new Color { Rgb = "FFFF0000" }) // 3: Bold Red
            ),
            new Fills(
                new Fill(new PatternFill() { PatternType = PatternValues.None }), // 0: default
                new Fill(new PatternFill(new ForegroundColor { Rgb = "00000000" }) { PatternType = PatternValues.Solid }), // 1: Black background
                new Fill(new PatternFill(new ForegroundColor { Rgb = "FFFFFF00" }) { PatternType = PatternValues.Solid }), // 2: Yellow background
                new Fill(new PatternFill(new ForegroundColor { Rgb = "FFDDDDDD" }) { PatternType = PatternValues.Solid }), // 3: Light Gray background
                new Fill(new PatternFill(new ForegroundColor { Rgb = "FFFF0000" }) { PatternType = PatternValues.Solid }), // 4: Red fill
                new Fill(new PatternFill(new ForegroundColor { Rgb = "FF00FF00" }) { PatternType = PatternValues.Solid })  // 5: Green fill
            ),
            new Borders(
                new Border(), // 0: default
                new Border(   // 1: thin border
                    new LeftBorder(new Color { Auto = true }) { Style = BorderStyleValues.Thin },
                    new RightBorder(new Color { Auto = true }) { Style = BorderStyleValues.Thin },
                    new TopBorder(new Color { Auto = true }) { Style = BorderStyleValues.Thin },
                    new BottomBorder(new Color { Auto = true }) { Style = BorderStyleValues.Thin },
                    new DiagonalBorder()
                )
            ),
            new CellFormats(
                new CellFormat(), // 0: default

                // 1: HeaderStyle (bold white text on black background, with border)
                new CellFormat { FontId = 3, FillId = 1, BorderId = 1, ApplyFont = true, ApplyFill = true, ApplyBorder = true },

                // 2: WarningStyle (red text on yellow background)
                new CellFormat { FontId = 2, FillId = 2, BorderId = 1, ApplyFont = true, ApplyFill = true, ApplyBorder = true },

                // 3: CurrencyStyle (bold text, light gray background, number format)
                new CellFormat { FontId = 1, FillId = 3, BorderId = 1, NumberFormatId = 4, ApplyFont = true, ApplyFill = true, ApplyBorder = true, ApplyNumberFormat = true },

                // 4: AgeStyle (bold text, green background, number format)
                new CellFormat { FillId = 5, ApplyFill = true, NumberFormatId = 1, ApplyNumberFormat = true }
            )
        );

        stylesPart.Stylesheet.Save();
    }
}