using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;


namespace VFCMotor.Services
{
    static class ExcelExport
    {

     

        static void CreateChart(Excel.Range rg, Excel.Range data, Excel.Worksheet sheet, Object type = null, Excel.XlRowCol xlrc = Excel.XlRowCol.xlColumns, string title = null, string CategoryTitle = null, string ValueTitle = null)
        {
            Excel.ChartObjects charts = (Excel.ChartObjects)sheet.ChartObjects(Type.Missing);
            Excel.ChartObject chartObj = charts.Add(rg.Left, rg.Top, rg.Width, rg.Height);
            Excel.Chart chart = chartObj.Chart;
            chart.ChartWizard(data, type, Type.Missing, xlrc, 1, 1, true, title, CategoryTitle, ValueTitle, Type.Missing);
            chart.Legend.Position =Excel.XlLegendPosition.xlLegendPositionTop;

        }

        public static async Task GenerateExcel(DataTable dataTable, string path, IProgress<int> progress)
        {
            try
            {
                DataSet dataSet = new DataSet();
                dataSet.Tables.Add(dataTable);
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook excelWorkBook = excelApp.Workbooks.Add();
                Excel._Worksheet xlWorksheet = excelWorkBook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;
                await Task.Run(() =>
                {
                    foreach (DataTable table in dataSet.Tables)
                    {
                        Excel.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
                        excelWorkSheet.Name = dataTable.TableName;

                        // add all the columns
                        for (int i = 1; i < table.Columns.Count + 1; i++)
                        {
                            excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                        }

                        // add all the rows
                        for (int j = 0; j < table.Rows.Count; j++)
                        {
                            for (int k = 0; k < table.Columns.Count; k++)
                            {
                                excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
                            }
                            progress.Report(j * 100 / table.Rows.Count);
                        }


                    }

                    //  CreateChart(xlWorksheet.get_Range("K2:O19"), xlWorksheet.get_Range((Excel.Range)xlWorksheet.Cells[2, 2], (Excel.Range)xlWorksheet.Cells[2 + rows.Length - 1, 2 + cols.Length]), Excel.XlChartType.xlLine, Excel.XlRowCol.xlColumns, "", null, "");
                    excelWorkBook.SaveAs(path);
                    excelWorkBook.Close();
                    excelApp.Quit();
                });
            }
            catch (Exception ex)
            {

                LogService.LogMessage("Generate Excel " + ex.Message);
            }
           
        }
    }
}
