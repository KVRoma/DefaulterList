using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DefaulterList.Models;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace DefaulterList.Services
{
    public class PrintService
    {
        public IEnumerable<Defaulter> Defaulters { get; set; } 

        public PrintService()
        {

        }

        public void PrintList(string path)
        {
            Excel.Application ExcelApp = new Excel.Application();
            Excel.Workbook ExcelWorkBook;
            try
            {                
                ExcelWorkBook = ExcelApp.Workbooks.Open(Environment.CurrentDirectory + path);   //Вказуємо шлях до шаблону

                var temp = Defaulters.FirstOrDefault();
                ExcelApp.Cells[1, 4] = temp?.Date.ToShortDateString() ?? "";
                ExcelApp.Cells[2, 4] = temp?.DateResult?.ToShortDateString() ?? "";
                ExcelApp.Cells[3, 4] = temp?.NameTeam ?? "";
                ExcelApp.Cells[3, 5] = temp?.Descriptions ?? "";
                int count = 7;
                foreach (var item in Defaulters)
                {
                    ExcelApp.Cells[count, 1] = (count - 6).ToString();
                    ExcelApp.Cells[count, 2] = item.TotalList.Number;
                    ExcelApp.Cells[count, 3] = item.TotalList.Address;
                    ExcelApp.Cells[count, 4] = item.TotalList.Name;
                    ExcelApp.Cells[count, 5] = item.DebtTOV;
                    ExcelApp.Cells[count, 6] = item.DebtRZP;
                    count++;
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error message: " + Environment.NewLine +
                                        ex.Message + Environment.NewLine + Environment.NewLine +
                                        "StackTrace message: " + Environment.NewLine +
                                        ex.StackTrace, "Warning !!!");
            }
            finally
            {
                ExcelApp.Visible = true;           // Робим книгу видимою
                ExcelApp.UserControl = true;       // Передаємо керування користувачу  
            }
            
        }
        public void PrintReportToday(string path, DateTime date)
        {
            Excel.Application ExcelApp = new Excel.Application();
            Excel.Workbook ExcelWorkBook;
            try
            {
                ExcelWorkBook = ExcelApp.Workbooks.Open(Environment.CurrentDirectory + path);   //Вказуємо шлях до шаблону                               

                ExcelApp.Cells[3, 7] = date.ToShortDateString();

                ExcelApp.Cells[8, 1] = Defaulters?.Count() ?? 0;
                ExcelApp.Cells[8, 2] = Defaulters?.Select(x => x.DebtTOV)?.Sum() ?? 0m;
                ExcelApp.Cells[8, 3] = Defaulters?.Select(x => x.DebtRZP)?.Sum() ?? 0m;

                ExcelApp.Cells[8, 4] = Defaulters?.Where(x => x.Color == "Red" && x.DateResult == date)?.Count() ?? 0;
                ExcelApp.Cells[8, 5] = Defaulters?.Where(x => x.Color == "Red" && x.DateResult == date)?.Select(x => x.DebtTOV)?.Sum() ?? 0m;
                ExcelApp.Cells[8, 6] = Defaulters?.Where(x => x.Color == "Red" && x.DateResult == date)?.Select(x => x.DebtRZP)?.Sum() ?? 0m;

                ExcelApp.Cells[8, 7] = Defaulters?.Where(x => x.Color == "Yellow" && x.DateResult == date)?.Count() ?? 0;
                ExcelApp.Cells[8, 8] = Defaulters?.Where(x => x.Color == "Yellow" && x.DateResult == date)?.Select(x => x.DebtTOV)?.Sum() ?? 0m;
                ExcelApp.Cells[8, 9] = Defaulters?.Where(x => x.Color == "Yellow" && x.DateResult == date)?.Select(x => x.DebtRZP)?.Sum() ?? 0m;
                ExcelApp.Cells[8, 10] = Defaulters?.Where(x => x.Color == "Yellow" && x.DateResult == date)?.Select(x => x.PaymentTOVResult)?.Sum() ?? 0m;
                ExcelApp.Cells[8, 11] = Defaulters?.Where(x => x.Color == "Yellow" && x.DateResult == date)?.Select(x => x.PaymentRZPResult)?.Sum() ?? 0m;

                ExcelApp.Cells[8, 12] = Defaulters?.Where(x => x.Color == "Green" && x.DateResult == date)?.Count() ?? 0;
                ExcelApp.Cells[8, 13] = Defaulters?.Where(x => x.Color == "Green" && x.DateResult == date)?.Select(x => x.DebtTOV)?.Sum() ?? 0m;
                ExcelApp.Cells[8, 14] = Defaulters?.Where(x => x.Color == "Green" && x.DateResult == date)?.Select(x => x.DebtRZP)?.Sum() ?? 0m;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error message: " + Environment.NewLine +
                                        ex.Message + Environment.NewLine + Environment.NewLine +
                                        "StackTrace message: " + Environment.NewLine +
                                        ex.StackTrace, "Warning !!!");
            }
            finally
            {
                ExcelApp.Visible = true;           // Робим книгу видимою
                ExcelApp.UserControl = true;       // Передаємо керування користувачу  
            }
        }
        public void PrintReportTelegram(string path, DateTime date)
        {
            Excel.Application ExcelApp = new Excel.Application();
            Excel.Workbook ExcelWorkBook;
            try
            {
                ExcelWorkBook = ExcelApp.Workbooks.Open(Environment.CurrentDirectory + path);   //Вказуємо шлях до шаблону                               

                ExcelApp.Cells[4, 1] = date.ToShortDateString();

                ExcelApp.Cells[4, 2] = Defaulters?.Where(x => (x.Color == "Green" || x.Color == "Yellow") && x.DateResult == date && x.PaymentRZPResult > 1m)?.Count() ?? 0;
                ExcelApp.Cells[4, 3] = Defaulters?.Where(x => (x.Color == "Green" || x.Color == "Yellow") && x.DateResult == date)?.Select(x => x.PaymentRZPResult)?.Sum() ?? 0m;
                ExcelApp.Cells[4, 4] = Defaulters?.Where(x => (x.Color == "Green" || x.Color == "Yellow") && x.DateResult == date && x.PaymentTOVResult > 1m)?.Count() ?? 0;
                ExcelApp.Cells[4, 5] = Defaulters?.Where(x => (x.Color == "Green" || x.Color == "Yellow") && x.DateResult == date)?.Select(x => x.PaymentTOVResult)?.Sum() ?? 0m;
                                
                ExcelApp.Cells[4, 6] = Defaulters?.Where(x => x.Color == "Red" && x.DateResult == date)?.Count() ?? 0;                
                ExcelApp.Cells[4, 7] = Defaulters?.Where(x => x.Color == "Red" && x.DateResult == date)?.Select(x => x.DebtRZP)?.Sum() ?? 0m;
                ExcelApp.Cells[4, 8] = Defaulters?.Where(x => x.Color == "Red" && x.DateResult == date)?.Select(x => x.DebtTOV)?.Sum() ?? 0m;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error message: " + Environment.NewLine +
                                        ex.Message + Environment.NewLine + Environment.NewLine +
                                        "StackTrace message: " + Environment.NewLine +
                                        ex.StackTrace, "Warning !!!");
            }
            finally
            {
                ExcelApp.Visible = true;           // Робим книгу видимою
                ExcelApp.UserControl = true;       // Передаємо керування користувачу  
            }
        }
    }
}
