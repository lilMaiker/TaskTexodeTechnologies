using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace TrackerApplication.Helpers.Exports
{
    internal class ExcelExport
    {
        public static bool Export(DataGrid dataGrid, string excelTitle)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                if (dataGrid.Columns[i].Visibility == System.Windows.Visibility.Visible)//Export only visible columns  
                {
                    dt.Columns.Add(dataGrid.Columns[i].Header.ToString());//Build the header  
                }
            }

            foreach (var t1 in dataGrid.Items)
            {
                int columnsIndex = 0;
                System.Data.DataRow row = dt.NewRow();
                foreach (var t in dataGrid.Columns)
                    if (t.Visibility == System.Windows.Visibility.Visible)
                    {
                        if (t1 != null && (t.GetCellContent(t1) as TextBlock) != null)//Fill the visible column data  
                            row[columnsIndex] = (t.GetCellContent(t1) as TextBlock).Text.ToString();
                        else row[columnsIndex] = "";
                        columnsIndex++;
                    }

                dt.Rows.Add(row);
            }

            return ExportExcel(dt, excelTitle) != null;
        }

        public static string ExportExcel(System.Data.DataTable DT, string title)
        {
            try
            {
                //Create Excel
                Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook ExcelBook = ExcelApp.Workbooks.Add(System.Type.Missing);
                //Create a worksheet (that is, a sub-sheet in Excel) 1 means export data in the sub-sheet sheet1
                Microsoft.Office.Interop.Excel.Worksheet ExcelSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelBook.Worksheets[1];

                //If there is a number type in the data, it can be displayed in text format
                ExcelSheet.Cells.NumberFormat = "@";

                //Set the name of the worksheet
                ExcelSheet.Name = title;

                //Set the Sheet title
                string start = "A1";
                string end = (DT.Columns.Count) + "1";

                Microsoft.Office.Interop.Excel.Range _Range = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.get_Range(start, end);
                _Range.Merge(0);                     //Cell merge action (to be designed with the get_Range() above)
                _Range = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.get_Range(start, end);
                _Range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                _Range.Font.Size = 22; //Set font size
                _Range.Font.Name = "Song Ti"; //Set the type of font 
                ExcelSheet.Cells[1, 1] = title;    //Excel cell assignment
                _Range.EntireColumn.AutoFit(); //Automatically adjust column width

                //Write the header
                for (int m = 1; m <= DT.Columns.Count; m++)
                {
                    ExcelSheet.Cells[2, m] = DT.Columns[m - 1].ColumnName.ToString();

                    start = "A2";
                    end = (DT.Columns.Count) + "2";

                    _Range = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.get_Range(start, end);
                    _Range.Font.Size = 15; //Set font size
                    _Range.Font.Bold = true;//Bold
                    _Range.Font.Name = "Song Ti"; //Set the type of font  
                    _Range.EntireColumn.AutoFit(); //Automatically adjust column width 
                    _Range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                }

                //Write data
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    for (int j = 1; j <= DT.Columns.Count; j++)
                    {
                        //Excel cell first starts at index 1
                        // if (j == 0) j = 1;
                        ExcelSheet.Cells[i + 3, j] = DT.Rows[i][j - 1].ToString();
                    }
                }

                //Table attribute settings
                for (int n = 0; n < DT.Rows.Count + 1; n++)
                {
                    start = "A" + (n + 3).ToString();
                    end = (DT.Columns.Count) + (n + 3).ToString();

                    //Get multiple cell ranges in Excel
                    _Range = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.get_Range(start, end);

                    _Range.Font.Size = 12; //Set font size
                    _Range.Font.Name = "Song Ti"; //Set the type of font

                    _Range.EntireColumn.AutoFit(); //Automatically adjust column width
                    _Range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter; //Set the font alignment method in the cell _Range.EntireColumn.AutoFit(); //Automatically adjust the column width 
                }

                ExcelApp.DisplayAlerts = false; //When saving Excel, save directly without popping up a window whether to save 

                //Pop up the save dialog box and save the file
                Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
                sfd.DefaultExt = ".xlsx";
                sfd.Filter = "Office 2007 File|*.xlsx|Office 2000-2003 File|*.xls|All files|*.*";
                if (sfd.ShowDialog() == true)
                {
                    if (sfd.FileName != "")
                    {
                        ExcelBook.SaveAs(sfd.FileName);  //Save it to the specified path
                                                         // MessageBox.Show("The exported file has been stored as: "+ sfd.FileName, "Warm reminder");
                    }
                }

                //Release the process that may not be released yet
                ExcelBook.Close();
                ExcelApp.Quit();
                // PubHelper.Instance.KillAllExcel(ExcelApp);

                return sfd.FileName;
            }
            catch
            {
                // MessageBox.Show("Failed to save the exported file!", "Warning!");
                return null;
            }
        }
    }
}
