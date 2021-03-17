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
        private Dictionary<string, decimal> result;
        public IEnumerable<Defaulter> Defaulters { get; set; } 

        public PrintService()
        {
            result = new Dictionary<string, decimal>()
            {
                {"ReestrCount", 0m },
                {"ReestrTOV", 0m },
                {"ReestrRZP", 0m },

                {"VidklCount", 0m },
                {"VidklTOV", 0m },
                {"VidklRZP", 0m },

                {"OplTOVCount", 0m },
                {"OplRZPCount", 0m },
                {"OplTOV", 0m },
                {"OplRZP", 0m },

                {"NdOplTOVCount", 0m },
                {"NdOplRZPCount", 0m },
                {"NdOplTOVplan", 0m },
                {"NdOplRZPplan", 0m },
                {"NdOplTOV", 0m },
                {"NdOplRZP", 0m },
            };
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
        public void PrintReportToday(string path, DateTime? _date)
        {
            Excel.Application ExcelApp = new Excel.Application();
            Excel.Workbook ExcelWorkBook;
            try
            {
                ExcelWorkBook = ExcelApp.Workbooks.Open(Environment.CurrentDirectory + path);   //Вказуємо шлях до шаблону                               
                
                IEnumerable<Defaulter> temps;
                DateTime date = (DateTime)((_date != null) ? (_date) : (DateTime.MinValue));
                if (date == DateTime.MinValue)
                {                    
                    temps = Defaulters.ToList();
                    ExcelApp.Cells[3, 7] = temps.FirstOrDefault().Date.Month.ToString() + "  " +"місяць";
                }
                else
                {
                    ExcelApp.Cells[3, 7] = date.ToShortDateString();
                    temps = Defaulters.Where(x => x.DateResult == date);
                }                
                
                foreach (var item in temps)
                {
                    if (item.IsDisabled)
                    {
                        result["VidklCount"]++;
                        result["VidklTOV"] += item.DebtTOV;
                        result["VidklRZP"] += item.DebtRZP;
                    }
                    else
                    {                        
                        // Повна оплата ТОВ
                        if ((item.DebtTOV) != 0m && (item.DebtTOV - item.PaymentTOVResult) <= 0m)
                        {
                            result["OplTOVCount"]++;
                            result["OplTOV"] += item.DebtTOV;
                        }
                        // Повна оплата РЗП
                        if ((item.DebtRZP) != 0m && (item.DebtRZP - item.PaymentRZPResult) <= 0m)
                        {
                            result["OplRZPCount"]++;
                            result["OplRZP"] += item.DebtRZP;
                        }

                        // Часткова оплата ТОВ
                        if ((item.DebtTOV) != 0m && ((item.DebtTOV - item.PaymentTOVResult) < item.DebtTOV) && ((item.DebtTOV - item.PaymentTOVResult) > 0m))
                        {
                            result["NdOplTOVCount"]++;
                            result["NdOplTOVplan"] += item.DebtTOV;
                            result["NdOplTOV"] += item.PaymentTOVResult;
                        }
                        // Часткова оплата ТОВ
                        if ((item.DebtRZP) != 0m && ((item.DebtRZP - item.PaymentRZPResult) < item.DebtRZP) && ((item.DebtRZP - item.PaymentRZPResult) > 0m))
                        {
                            result["NdOplRZPCount"]++;
                            result["NdOplRZPplan"] += item.DebtRZP;
                            result["NdOplRZP"] += item.PaymentRZPResult;
                        }
                    }
                }


                ExcelApp.Cells[8, 1] = Defaulters?.Count() ?? 0;
                ExcelApp.Cells[8, 2] = Defaulters?.Select(x => x.DebtTOV)?.Sum() ?? 0m;
                ExcelApp.Cells[8, 3] = Defaulters?.Select(x => x.DebtRZP)?.Sum() ?? 0m;

                ExcelApp.Cells[8, 4] = decimal.Round(result["VidklCount"], 0);
                ExcelApp.Cells[8, 5] = decimal.Round(result["VidklTOV"], 2);
                ExcelApp.Cells[8, 6] = decimal.Round(result["VidklRZP"], 2);

                ExcelApp.Cells[8, 7] =  decimal.Round(result["NdOplTOVCount"], 0);
                ExcelApp.Cells[8, 8] = decimal.Round(result["NdOplTOVplan"], 2);
                ExcelApp.Cells[8, 9] = decimal.Round(result["NdOplRZPplan"], 2);
                ExcelApp.Cells[8, 10] = decimal.Round(result["NdOplTOV"], 2);
                ExcelApp.Cells[8, 11] = decimal.Round(result["NdOplRZP"], 2);

                ExcelApp.Cells[8, 12] = decimal.Round(result["OplTOVCount"], 0);
                ExcelApp.Cells[8, 13] = decimal.Round(result["OplTOV"], 2);
                ExcelApp.Cells[8, 14] = decimal.Round(result["OplRZP"], 2);               

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
        public void PrintReportTelegram(string path, DateTime? _date)
        {
            Excel.Application ExcelApp = new Excel.Application();
            Excel.Workbook ExcelWorkBook;
            try
            {
                ExcelWorkBook = ExcelApp.Workbooks.Open(Environment.CurrentDirectory + path);   //Вказуємо шлях до шаблону  
                
                IEnumerable<Defaulter> temps;
                DateTime date = (DateTime)((_date != null) ? (_date) : (DateTime.MinValue));
                if (date == DateTime.MinValue)
                {
                    temps = Defaulters.ToList();
                    ExcelApp.Cells[4, 1] = temps.FirstOrDefault().Date.Month.ToString() + "  " +"місяць";
                }
                else
                {
                    ExcelApp.Cells[4, 1] = date.ToShortDateString();
                    temps = Defaulters.Where(x => x.DateResult == date);
                }
                

                
                foreach (var item in temps)
                {
                    if (item.IsDisabled)
                    {
                        result["VidklCount"]++;
                        result["VidklTOV"] += item.DebtTOV;
                        result["VidklRZP"] += item.DebtRZP;
                    }
                    else
                    {
                        // Повна оплата ТОВ
                        if ((item.DebtTOV) != 0m && (item.DebtTOV - item.PaymentTOVResult) <= 0m)
                        {
                            result["OplTOVCount"]++;
                            result["OplTOV"] += item.DebtTOV;
                        }
                        // Повна оплата РЗП
                        if ((item.DebtRZP) != 0m && (item.DebtRZP - item.PaymentRZPResult) <= 0m)
                        {
                            result["OplRZPCount"]++;
                            result["OplRZP"] += item.DebtRZP;
                        }

                        // Часткова оплата ТОВ
                        if ((item.DebtTOV) != 0m && ((item.DebtTOV - item.PaymentTOVResult) < item.DebtTOV) && ((item.DebtTOV - item.PaymentTOVResult) > 0m))
                        {
                            result["NdOplTOVCount"]++;
                            result["NdOplTOVplan"] += item.DebtTOV;
                            result["NdOplTOV"] += item.PaymentTOVResult;
                        }
                        // Часткова оплата РЗП
                        if ((item.DebtRZP) != 0m && ((item.DebtRZP - item.PaymentRZPResult) < item.DebtRZP) && ((item.DebtRZP - item.PaymentRZPResult) > 0m))
                        {
                            result["NdOplRZPCount"]++;
                            result["NdOplRZPplan"] += item.DebtRZP;
                            result["NdOplRZP"] += item.PaymentRZPResult;
                        }
                    }
                }

                ExcelApp.Cells[4, 2] = decimal.Round(result["OplRZPCount"] + result["NdOplRZPCount"], 0);
                ExcelApp.Cells[4, 3] = decimal.Round(result["OplRZP"] + result["NdOplRZP"], 2);
                ExcelApp.Cells[4, 4] = decimal.Round(result["OplTOVCount"] + result["NdOplTOVCount"], 0);
                ExcelApp.Cells[4, 5] = decimal.Round(result["OplTOV"] + result["NdOplTOV"], 2);

                ExcelApp.Cells[4, 6] = decimal.Round(result["VidklCount"], 0);
                ExcelApp.Cells[4, 7] = decimal.Round(result["VidklRZP"], 2);
                ExcelApp.Cells[4, 8] = decimal.Round(result["VidklTOV"], 2);                
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
