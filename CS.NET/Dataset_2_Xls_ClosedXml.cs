using ClosedXML.Excel;

internal static void SaveDatasetToXlsx(string filePath, DataSet dataSet)
{
    using (var workbook = new XLWorkbook())
    {
        foreach (DataTable table in dataSet.Tables)
        {
            string sheetName = SanitizeSheetName(table.TableName);
            var ws = workbook.Worksheets.Add(sheetName);

            // Write headers manually (gives full control over header style)
            for (int col = 0; col < table.Columns.Count; col++)
            {
                ws.Cell(1, col + 1).Value = table.Columns[col].ColumnName;
            }

            // Write data rows starting at row 2
            for (int row = 0; row < table.Rows.Count; row++)
            {
                for (int col = 0; col < table.Columns.Count; col++)
                {
                    var cell = ws.Cell(row + 2, col + 1);
                    object value = table.Rows[row][col];

                    if (value == DBNull.Value || value == null)
                    {
                        cell.Value = string.Empty;
                    }
                    else
                    {
                        cell.Value = XLCellValue.FromObject(value);
                    }
                }
            }

            int lastRow = table.Rows.Count + 1;
            int lastCol = table.Columns.Count;
            var fullRange = ws.Range(1, 1, lastRow, lastCol);
            var headerRange = ws.Range(1, 1, 1, lastCol);
            var dataRange = ws.Range(2, 1, lastRow, lastCol);

            // ---------- HEADER STYLING ----------
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4");
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Row(1).Height = 22;

            // ---------- BORDERS (whole table) ----------
            fullRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            fullRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            fullRange.Style.Border.OutsideBorderColor = XLColor.Black;
            fullRange.Style.Border.InsideBorderColor = XLColor.Gray;

            // ---------- ALTERNATING ROW COLORS (zebra striping) ----------
            for (int r = 0; r < table.Rows.Count; r++)
            {
                if (r % 2 == 1) // odd data rows (2nd, 4th, ...)
                {
                    ws.Range(r + 2, 1, r + 2, lastCol).Style.Fill.BackgroundColor = XLColor.FromHtml("#F2F2F2");
                }
            }

            // ---------- COLUMN-SPECIFIC NUMBER/DATE FORMATTING ----------
            for (int col = 0; col < table.Columns.Count; col++)
            {
                var colType = table.Columns[col].DataType;
                var colRange = ws.Column(col + 1)
                                  .Column(2, lastRow); // data cells only, skip header

                if (colType == typeof(DateTime))
                {
                    colRange.Style.DateFormat.Format = "yyyy-mm-dd";
                }
                else if (colType == typeof(decimal) || colType == typeof(double) || colType == typeof(float))
                {
                    colRange.Style.NumberFormat.Format = "#,##0.00";
                }
                else if (colType == typeof(int) || colType == typeof(long))
                {
                    colRange.Style.NumberFormat.Format = "#,##0";
                }
            }

            // ---------- ROW HEIGHT / CELL PADDING FEEL ----------
            dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            // ---------- COLUMN WIDTHS ----------
            ws.Columns().AdjustToContents();
            ws.Column(2).Width = Math.Max(ws.Column(2).Width, 20); // e.g. widen "Name" column a bit

            // ---------- FREEZE HEADER + AUTOFILTER ----------
            ws.SheetView.FreezeRows(1);
            fullRange.SetAutoFilter();

            // ---------- CONDITIONAL FORMATTING EXAMPLE ----------
            // Highlight salaries above 60000 in green (assuming Salary is column 4)
            int salaryColIndex = table.Columns.IndexOf("Salary") + 1;
            if (salaryColIndex > 0)
            {
                var salaryRange = ws.Range(2, salaryColIndex, lastRow, salaryColIndex);
                salaryRange.AddConditionalFormat()
                    .WhenGreaterThan(60000)
                    .Fill.SetBackgroundColor(XLColor.LightGreen)
                    .Font.SetFontColor(XLColor.DarkGreen);
            }
        }

        workbook.SaveAs(filePath);
    }
}

static string SanitizeSheetName(string name)
{
    if (string.IsNullOrWhiteSpace(name)) return "Sheet1";
    foreach (char c in new[] { '\\', '/', '?', '*', '[', ']', ':' })
        name = name.Replace(c, '_');
    return name.Length > 31 ? name.Substring(0, 31) : name;
}
