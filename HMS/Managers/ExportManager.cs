using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using HMS.DataRecords;


namespace HMS.Managers
{
	class ExportManager
	{
		public static void ExportToExcel(IList<BatchRecord> batches)
		{
			// load Excel Application
			Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
			if (excelApp == null) {
				Debug.WriteLine("Ошибка. У вас не установлен Excel");
				return;
			}

			excelApp.Visible = true;
			// Create empty workbook
			var excelWorkBook = excelApp.Workbooks.Add();
			// Create Worksheet from first sheet
			var excelWorkSheet = (Worksheet)excelWorkBook.Worksheets[1];
			//Microsoft.Office.Interop.Excel._Worksheet worksheet = excel.ActiveSheet;


			try {
				excelWorkSheet.Cells[1, "A"] = "# измерения";
				excelWorkSheet.Cells[1, "B"] = "Название артикула";
				excelWorkSheet.Cells[1, "C"] = "№ артикула";
				excelWorkSheet.Cells[1, "D"] = "Дата";
				excelWorkSheet.Cells[1, "E"] = "№";
				excelWorkSheet.Cells[1, "F"] = "Высота";
				excelWorkSheet.Cells[1, "G"] = "Время";
				excelWorkSheet.Cells[1, "H"] = "Высота закатки";
				excelWorkSheet.Cells[1, "I"] = "Время";
				excelWorkSheet.Cells[1, "J"] = "Диаметр";
				excelWorkSheet.Cells[1, "K"] = "Время";

				

				int batchRow = 2;
				int measurementRow = 3;
				foreach (BatchRecord batch in batches) {
					IList<MeasurementRecord> measurements = batch.Measurements;
					excelWorkSheet.Cells[batchRow, "A"] = batch.BatchNumber;
					excelWorkSheet.Cells[batchRow, "B"] = batch.ArticleName;
					excelWorkSheet.Cells[batchRow, "C"] = batch.ArticleNumber;
					excelWorkSheet.Cells[batchRow, "D"] = batch.Date;

					int count = 0;
					foreach (MeasurementRecord measurement in measurements) {
						count++;
						excelWorkSheet.Cells[measurementRow, "E"] = string.Format(count + ".");
						excelWorkSheet.Cells[measurementRow, "F"] = measurement.Height;
						excelWorkSheet.Cells[measurementRow, "G"] = measurement.HTime;
						excelWorkSheet.Cells[measurementRow, "H"] = measurement.SeamerHeight;
						excelWorkSheet.Cells[measurementRow, "I"] = measurement.SHTime;
						excelWorkSheet.Cells[measurementRow, "J"] = measurement.Diameter;
						excelWorkSheet.Cells[measurementRow, "K"] = measurement.DTime;

						measurementRow++;
					}
					//SetCellBorders(excelWorkSheet.get_Range("E"+(measurementRow-count-1), "K"+ (measurementRow - 1)));
					//excelWorkSheet.Range[(measurementRow - count - 1), "E"][(measurementRow - 1), "K"].AutoFormat(XlRangeAutoFormat.xlRangeAutoFormatTable1);
					excelWorkSheet.Range["A1"].AutoFormat(XlRangeAutoFormat.xlRangeAutoFormatClassic1);
					measurementRow++;
					batchRow += count + 1;
					
				
				}

				
				string fileName = string.Format(@"{0}\ExcelData.xlsx", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));

				// Save this data as a file
				//excelWorkSheet.SaveAs(fileName);

				// Display SUCCESS message
				Debug.WriteLine(string.Format("The file '{0}' is saved successfully!", fileName));
				

			}
			catch (Exception exception) {

				Debug.WriteLine(string.Format("There was a PROBLEM saving Excel file!\n"));
			}
			finally {
				// Quit Excel application
				//excelApp.Quit();

				try {
					// Release COM objects (very important!)
					if (excelApp != null)
						System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

					if (excelWorkSheet != null)
						System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorkSheet);

					// Empty variables
					excelApp = null;
					excelWorkSheet = null;
				}
				catch (Exception ex) {
					Debug.WriteLine(string.Format("There was a PROBLEM saving Excel file!\n"));
					//MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
				}
				finally {
					// Force garbage collector cleaning
					GC.Collect();
				}
			}
		}

		private static void SetCellBorders(Range cells)
		{
			cells.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlDot;   // внутренние вертикальные 
			cells.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlDot; // внутренние горизонтальные            
			cells.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlDouble; // верхняя внешняя
			cells.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlDouble; // правая внешняя
			cells.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlDouble; // левая внешняя
			cells.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDouble; // нижняя внешняя
		}
	}

}

/*	To do
*  1. Сделать оповещение пользователся о возникших ошибках
*  2. Оповещение пользователя о завершении операции
*
*
*/
