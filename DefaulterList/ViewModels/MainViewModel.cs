using DefaulterList.Commands;
using DefaulterList.Models;
using DefaulterList.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DefaulterList.ViewModels
{
    public class MainViewModel : ViewModel
    {
        ContextDefaulter db;
        private DateTime dateLoad;
        private double opacityProgressBar;
        private Dictionary<string, Visibility> isVisibility;
        private Team teamSelect;
        private IEnumerable<Team> teams;
        private string teamFilter;
        private Worker workerSelect;
        private IEnumerable<Worker> workers;
        private string workerFilter;
        private IEnumerable<TotalList> totalLists;
        private IList<Defaulter> defaultersSelect;
        private IEnumerable<Defaulter> defaulters;
        private int countItem;        
        private decimal firstField;
        private decimal secondaryField;
        private string firstComboSelect;        
        private string secondaryComboSelect;
        private List<string> textComboBox;


        public string TitleView { get; } = "Defaulter List - 2021";
        public double OpacityProgressBar
        {
            get { return opacityProgressBar; }
            set
            {
                opacityProgressBar = value;
                OnPropertyChanged(nameof(OpacityProgressBar));
            }
        }
        public Dictionary<string, Visibility> IsVisibility
        {
            get { return isVisibility; }
            set
            {
                isVisibility = value;
                OnPropertyChanged(nameof(IsVisibility));
            }
        }
        public Team TeamSelect
        {
            get { return teamSelect; }
            set
            {
                teamSelect = value;
                OnPropertyChanged(nameof(TeamSelect));
            }
        }
        public IEnumerable<Team> Teams
        {
            get { return teams; }
            set
            {
                teams = value;
                OnPropertyChanged(nameof(Teams));
            }
        }
        public string TeamFilter
        {
            get { return teamFilter; }
            set
            {
                teamFilter = value;
                OnPropertyChanged(nameof(TeamFilter));
            }
        }
        public Worker WorkerSelect
        {
            get { return workerSelect; }
            set
            {
                workerSelect = value;
                OnPropertyChanged(nameof(WorkerSelect));
            }
        }
        public IEnumerable<Worker> Workers
        {
            get { return workers; }
            set
            {
                workers = value;
                OnPropertyChanged(nameof(Workers));
            }
        }
        public string WorkerFilter
        {
            get { return workerFilter; }
            set
            {
                workerFilter = value;
                OnPropertyChanged(nameof(WorkerFilter));
            }
        }
        public IEnumerable<TotalList> TotalLists
        {
            get { return totalLists; }
            set
            {
                totalLists = value;
                OnPropertyChanged(nameof(TotalLists));
            }
        }
        public IList<Defaulter> DefaultersSelect
        {
            get { return defaultersSelect; }
            set
            {
                defaultersSelect = value;
                OnPropertyChanged(nameof(DefaultersSelect));                 
            }
        }
        public IEnumerable<Defaulter> Defaulters
        {
            get { return defaulters; }
            set
            {
                defaulters = value;
                OnPropertyChanged(nameof(Defaulters));
            }
        }
        public int CountItem
        {
            get { return countItem; }
            set
            {
                countItem = value;
                OnPropertyChanged(nameof(CountItem));
            }
        }        
        public decimal FirstField
        {
            get { return firstField; }
            set
            {
                firstField = value;
                OnPropertyChanged(nameof(FirstField));
            }
        }
        public decimal SecondaryField
        {
            get { return secondaryField; }
            set
            {
                secondaryField = value;
                OnPropertyChanged(nameof(SecondaryField));
            }
        }
        public string FirstComboSelect
        {
            get { return firstComboSelect; }
            set
            {
                firstComboSelect = value;
                OnPropertyChanged(nameof(FirstComboSelect));
                if (FirstComboSelect == "")
                {
                    FirstField = 0m;
                }
            }
        }        
        public string SecondaryComboSelect
        {
            get { return secondaryComboSelect; }
            set
            {
                secondaryComboSelect = value;
                OnPropertyChanged(nameof(SecondaryComboSelect));
                if (SecondaryComboSelect == "")
                {
                    SecondaryField = 0m;
                }
            }
        }
        public List<string> TextComboBox
        {
            get { return textComboBox; }
            set
            {
                textComboBox = value;
                OnPropertyChanged(nameof(TextComboBox));
            }
        }

        private Command _getTotalList;
        private Command _getDefaulter;
        private Command _addTeam;
        private Command _addWorkerTeam;
        private Command _addWorker;
        private Command _visibleClear;
        private Command _clearTeam;
        private Command _clearWorker;
        private Command _search;

        public Command GetTotalList => _getTotalList ?? (_getTotalList = new Command(async obj=> 
        {
            StartProgressBar();
            await Task.Run(()=> 
            { 
                LoadService service = new LoadService();
                service.LoadTotalListCSV();
                db.TotalLists.RemoveRange(db.TotalLists);
                db.TotalLists.AddRange(service.TotalLists);
                db.SaveChanges();
                LoadTotalList();
            });
            StopProgressBar();
        }));
        public Command GetDefaulter => _getDefaulter ?? (_getDefaulter = new Command(async obj=> 
        {
            DateTime date = new DateTime();
            StartProgressBar();
            await Task.Run(()=> 
            { 
                LoadService service = new LoadService(TotalLists);          
                service.LoadDefaulterCSV();
                date = service.Defaulters.FirstOrDefault().Date;
                SaveDateLoading(date);
                db.Defaulters.AddRange(service.Defaulters);               
                db.SaveChanges();
                LoadDefaulters();
            });            
            StopProgressBar();
        }));
        public Command AddTeam => _addTeam ?? (_addTeam = new Command(obj=> 
        {
            string item = obj.ToString();
            if (item == "")
            {
                return;
            }
            else
            {
                Team newTeam = new Team() { };
                newTeam.NameTeam = (Teams.Count() <= 0) ? ("Command-1") : ("Command-" + (Teams.Max(x=>x.Id) + 1).ToString());
                newTeam.Descriptions = item;
               
                db.Teams.Add(newTeam);
                db.SaveChanges();
                LoadTeam();
                
                TeamFilter = "";                   
            }
        }));
        public Command AddWorkerTeam => _addWorkerTeam ?? (_addWorkerTeam = new Command(obj=> 
        {
            if (string.IsNullOrWhiteSpace(TeamFilter))
            {
                TeamFilter = WorkerSelect.Name;
            }
            else
            {
                TeamFilter += Environment.NewLine + WorkerSelect.Name;
            }            
        }));
        public Command AddWorker => _addWorker ?? (_addWorker = new Command(obj=> 
        {
            string item = obj.ToString();
            if (item == "")
            {
                return;
            }
            else
            {
                Worker newWorker = new Worker()
                {
                    Name = item                    
                };
                db.Workers.Add(newWorker);
                db.SaveChanges();
                LoadWorker();

                WorkerFilter = "";
            }
        }));
        public Command VisibleClear => _visibleClear ?? (_visibleClear = new Command(obj=> 
        {
            IsVisibility["MenuClear"] = (IsVisibility["MenuClear"] == Visibility.Collapsed) ? (Visibility.Visible) : (Visibility.Collapsed);
            OnPropertyChanged(nameof(IsVisibility));
        }));
        public Command ClearTeam => _clearTeam ?? (_clearTeam = new Command(obj=> 
        {
            db.Teams.RemoveRange(db.Teams);
            db.SaveChanges();
            LoadTeam();
        }));
        public Command ClearWorker => _clearWorker ?? (_clearWorker = new Command(obj=> 
        {
            db.Workers.RemoveRange(db.Workers);
            db.SaveChanges();
            LoadWorker();
        }));
        public Command Search => _search ?? (_search = new Command(obj=> 
        {
            string item = obj.ToString();            
            LoadDefaulters();
            if (!string.IsNullOrWhiteSpace(item))
            {
                Defaulters = Defaulters.Where(x => x.TotalList.City.ToUpper().Contains(item.ToUpper()));
                CountItem = Defaulters?.Count() ?? 0;                
            }
            if (!string.IsNullOrWhiteSpace(FirstComboSelect))
            {
                Defaulters = Defaulters.Where(x => Operator(FirstComboSelect, x.DebtTOV, FirstField));
                CountItem = Defaulters?.Count() ?? 0;
            }
            if (!string.IsNullOrWhiteSpace(SecondaryComboSelect))
            {
                Defaulters = Defaulters.Where(x => Operator(SecondaryComboSelect, x.DebtTOV, SecondaryField));
                CountItem = Defaulters?.Count() ?? 0;
            }
        }));

        

        public MainViewModel()
        {
            InitializeVisibility();
            InitializedDB();
            LoadComboBox();
            DefaultersSelect = new List<Defaulter>();
        }

        private void InitializedDB()
        {            
            db = new ContextDefaulter();
            db.Defaulters.Include(x => x.TotalList).Load();         
            db.Results.Include(x=>x.TotalList).Include(x=>x.Team).Load();
            LoadWorker();
            LoadTeam();
            LoadTotalList();
            LoadDefaulters();
        }
        private void LoadWorker()
        {
            db.Workers.Load();
            Workers = db.Workers.Local.ToBindingList().OrderBy(x=>x.Name);
        }
        private void LoadTeam()
        {
            db.Teams.Load();
            Teams = db.Teams.Local.ToBindingList().OrderBy(x => x.Id);
        }
        private void LoadTotalList()
        {
            db.TotalLists.Load();
            TotalLists = db.TotalLists.Local.ToBindingList();
        }
        private void LoadDefaulters()
        {            
            dateLoad = db.Dictionaries.FirstOrDefault(x=>x.NameKey == "DateLoad")?.ValueKeyDate ?? DateTime.MinValue;
            Defaulters = db.Defaulters.Local.ToBindingList().Where(x=>x.Date == dateLoad);
            CountItem = Defaulters?.Count() ?? 0;
        }
        private void LoadComboBox()
        {
            TextComboBox = new List<string>() 
            {
                "",
                ">",
                "<",
                "=="
            };           
        }
        private void StartProgressBar()
        {
            OpacityProgressBar = 0;
            IsVisibility["ProgressBar"] = Visibility.Visible;
            OnPropertyChanged(nameof(IsVisibility));
        }
        private void StopProgressBar()
        {
            OpacityProgressBar = 1;
            IsVisibility["ProgressBar"] = Visibility.Collapsed;
            OnPropertyChanged(nameof(IsVisibility));
        }
        /// <summary>
        /// Метод при старті вказує, що показувати і приховати
        /// </summary>
        private void InitializeVisibility()
        {
            IsVisibility = new Dictionary<string, Visibility>
            {
                { "Grid", Visibility.Visible},
                { "LeftPanel", Visibility.Visible },
                { "RightPanel", Visibility.Visible},
                { "Menu", Visibility.Visible},
                { "MenuClear", Visibility.Collapsed},
                { "Footer", Visibility.Collapsed},
                { "ProgressBar", Visibility.Collapsed}
            };
            OpacityProgressBar = 1;
        }
        private void SaveDateLoading(DateTime date)
        {
            var temp = db.Dictionaries.FirstOrDefault(x => x.NameKey == "DateLoad");
            if (temp != null)
            {
                temp.ValueKeyDate = date;
                db.Entry(temp).State = EntityState.Modified;
            }
            else
            {
                Dictionary dic = new Dictionary()
                {
                    NameKey = "DateLoad",
                    ValueKeyDate = date,
                    ValueKeyText = ""
                };
                db.Dictionaries.Add(dic);
            }
            db.SaveChanges();            

        }
        public  bool Operator(string logic, decimal x, decimal y)
        {
            switch (logic)
            {
                case ">": return x > y;
                case "<": return x < y;
                case "==": return x == y;
                default: throw new Exception("invalid logic");
            }
        }
    }
}
