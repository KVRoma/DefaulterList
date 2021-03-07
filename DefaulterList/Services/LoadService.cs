using DefaulterList.Models;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaulterList.Services
{
    public class LoadService
    {      


        public List<TotalList> TotalLists { get; set; }
        public List<Defaulter> Defaulters { get; set; }        

        public LoadService()
        {
            TotalLists = new List<TotalList>();
            Defaulters = new List<Defaulter>();
        }
        public LoadService(IEnumerable<TotalList> totalLists)
        {
            TotalLists = new List<TotalList>();
            TotalLists = totalLists.ToList();
            Defaulters = new List<Defaulter>();
        }


        /// <summary>
        /// Зчитує всі дані з файлу CSV та заповнює в TotalList
        /// </summary>
        public void LoadTotalListCSV()
        {
            string path = OpenFile("File CSV|*.CSV;*.TXT");   // Вибираємо наш файл (метод OpenFile() описаний нижче)

            if (path == null) // Перевіряємо шлях до файлу на null
            {                
                return;
            }
            GetTotalListCSV(path);
        }
        /// <summary>
        /// Зчитує всі дані з файлу CSV та заповнює в Defaulter
        /// </summary>
        public void LoadDefaulterCSV()
        {
            string path = OpenFile("File CSV|*.CSV;*.TXT");   // Вибираємо наш файл (метод OpenFile() описаний нижче)

            if (path == null) // Перевіряємо шлях до файлу на null
            {
                return;
            }
            GetDefaulterCSV(path);
        }



        /// <summary>
        /// Зчитує всі дані з файлу CSV та заповнює в TotalList
        /// </summary>
        /// <param name="pat"></param>
        private void GetTotalListCSV(string pat)
        {
            //var path = @"C:\Person.csv"; // Habeeb, "Dubai Media City, Dubai"
            using (TextFieldParser csvParser = new TextFieldParser(pat))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { ";" });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    object[] fields = csvParser.ReadFields();
                    if (!string.IsNullOrWhiteSpace(fields[0].ToString()))
                    {
                        TotalList total = new TotalList() 
                        {                            
                            Number = fields[0].ToString(),
                            Address = fields[1].ToString(),
                            Name = fields[2].ToString(),
                            City = fields[3].ToString()
                        };
                        TotalLists.Add(total);                        
                    }
                }               
            }
        }
        /// <summary>
        /// Зчитує всі дані з файлу CSV та заповнює в TotalList
        /// </summary>
        /// <param name="pat"></param>
        private void GetDefaulterCSV(string pat)
        {            
            using (TextFieldParser csvParser = new TextFieldParser(pat))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { ";" });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    object[] fields = csvParser.ReadFields();
                    if (!string.IsNullOrWhiteSpace(fields[0].ToString()))
                    {
                        TotalList total = TotalLists.FirstOrDefault(x => x.Number == fields[0].ToString());
                        if (total != null)
                        {
                            Defaulter defaulter = new Defaulter()
                            {
                                Date = (DateTime.TryParse(fields[1].ToString(), out DateTime date)) ? (date) : (DateTime.MinValue),
                                DebtTOV = (decimal.TryParse(fields[2].ToString(), out decimal tov)) ? tov : 0m,
                                DebtRZP = (decimal.TryParse(fields[3].ToString(), out decimal rzp)) ? rzp : 0m,
                                TotalList = total
                            };
                            Defaulters.Add(defaulter);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Відкриває файл по заданій масці (один файл)
        /// </summary>
        /// <returns></returns>
        private string OpenFile(string filter)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = filter;
            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            return null;
        }
    }
}
