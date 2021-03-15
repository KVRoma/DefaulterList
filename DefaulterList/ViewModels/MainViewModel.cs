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
        private string searchText;
        private decimal firstField;
        private decimal secondaryField;
        private string firstComboSelect;        
        private string secondaryComboSelect;
        private List<string> textComboBox;
        private DateTime dateResult;
        private bool isCheckedFinish;

        private string info;
        private string description;
        private bool isDisabled;
        private decimal payTOV;
        private decimal payRZP;

        private string teamSaveValue;  // контейнер для вибраної бригади

        public string TitleView { get; } = "Defaulter List - 2021 (Робота з боржниками)";
        public string Author { get; } = "© 'Kuchinik & Co.' Версія 1.0.2021  ";
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
                if (TeamSelect != null)
                {
                    teamSaveValue = TeamSelect.NameTeam;
                }
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
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
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
        public DateTime DateResult
        {
            get { return dateResult; }
            set
            {
                dateResult = value;
                OnPropertyChanged(nameof(DateResult));
            }
        }
        public bool IsCheckedFinish
        {
            get { return isCheckedFinish; }
            set
            {
                isCheckedFinish = value;
                OnPropertyChanged(nameof(IsCheckedFinish));
            }
        }

        public string Info
        {
            get { return info; }
            set
            {
                info = value;
                OnPropertyChanged(nameof(Info));
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        public bool IsDisabled
        {
            get { return isDisabled; }
            set
            {
                isDisabled = value;
                OnPropertyChanged(nameof(IsDisabled));
            }
        }
        public decimal PayTOV
        {
            get { return payTOV; }
            set
            {
                payTOV = value;
                OnPropertyChanged(nameof(PayTOV));
            }
        }
        public decimal PayRZP
        {
            get { return payRZP; }
            set
            {
                payRZP = value;
                OnPropertyChanged(nameof(PayRZP));
            }
        }
        //****************************************************************************
        private Command _getTotalList;
        private Command _getDefaulter;
        private Command _exitApp;

        private Command _addTeam;
        private Command _delTeam;
        private Command _addWorkerTeam;
        private Command _addWorker;
        private Command _delWorker;

        private Command _visibleClear;
        private Command _clearTeam;
        private Command _clearWorker;
        private Command _clearDefaulter;

        private Command _search;

        private Command _taskView;
        private Command _teamView;

        private Command _addTeamForGrid;
        private Command _delTeamForGrid;
        private Command _filterTeamForGrid;
        private Command _addResult;
        private Command _saveResult;

        private Command _printGrid;
        private Command _printReportToday;
        private Command _printReportTelegram;
        //****************************************************************************
        public Command GetTotalList => _getTotalList ?? (_getTotalList = new Command(async obj=> 
        {
            StartProgressBar();
            await Task.Run(()=> 
            { 
                LoadService service = new LoadService();
                service.LoadTotalListCSV();
                db.Defaulters.RemoveRange(db.Defaulters);
                db.TotalLists.RemoveRange(db.TotalLists);
                db.TotalLists.AddRange(service.TotalLists);
                db.SaveChanges();
                LoadTotalList();
                LoadDefaulters();
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
        public Command ExitApp => _exitApp ?? (_exitApp = new Command(obj=> 
        {
            ExitApplication();
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
                newTeam.NameTeam = (Teams.Count() <= 0) ? ("Бригада-1") : ("Бригада-" + (Teams.Max(x=>x.Id) + 1).ToString());
                newTeam.Descriptions = item;
               
                db.Teams.Add(newTeam);
                db.SaveChanges();
                LoadTeam();
                
                TeamFilter = "";                   
            }
        }));
        public Command DelTeam => _delTeam ?? (_delTeam = new Command(obj=> 
        {
            var temp = db.Defaulters.Where(x => x.NameTeam == TeamSelect.NameTeam).FirstOrDefault();
            if (temp == null)
            {
                db.Teams.Remove(TeamSelect);
                db.SaveChanges();
                LoadTeam();
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
        public Command DelWorker => _delWorker ?? (_delWorker = new Command(obj=> 
        {
            db.Workers.Remove(WorkerSelect);
            db.SaveChanges();
            LoadWorker();
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
        public Command ClearDefaulter => _clearDefaulter ?? (_clearDefaulter = new Command(obj=> 
        {
            db.Defaulters.RemoveRange(db.Defaulters);
            db.SaveChanges();
            LoadDefaulters();
        }));

        public Command Search => _search ?? (_search = new Command(obj=> 
        {
            string item = obj.ToString();            
            LoadDefaulters();
            if (!string.IsNullOrWhiteSpace(item))
            {
                Defaulters = Defaulters.Where(x => x.Search.ToUpper().Contains(item.ToUpper()));
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
            if (!IsCheckedFinish)
            {
                Defaulters = Defaulters.Where(x => x.Color == "White");
                CountItem = Defaulters?.Count() ?? 0;
            }
        }));

        public Command TaskView => _taskView ?? (_taskView = new Command(obj=> 
        {
            IsVisibility["Grid"] = Visibility.Visible;
            IsVisibility["RightPanelGrid"] = Visibility.Visible;
            IsVisibility["LeftPanel"] = Visibility.Collapsed;
            IsVisibility["RightPanel"] = Visibility.Collapsed;
            OnPropertyChanged(nameof(IsVisibility));
        }));
        public Command TeamView => _teamView ?? (_teamView = new Command(obj=> 
        {
            IsVisibility["Grid"] = Visibility.Collapsed;
            IsVisibility["RightPanelGrid"] = Visibility.Collapsed;
            IsVisibility["LeftPanel"] = Visibility.Visible;
            IsVisibility["RightPanel"] = Visibility.Visible;
            OnPropertyChanged(nameof(IsVisibility));
        }));

        public Command AddTeamForGrid => _addTeamForGrid ?? (_addTeamForGrid = new Command(obj=> 
        {
            if (TeamSelect != null)
            {
                foreach (var item in DefaultersSelect)
                {
                    item.DateResult = DateResult;
                    item.NameTeam = TeamSelect.NameTeam;
                    item.Descriptions = TeamSelect.Descriptions;
                    db.Entry(item).State = EntityState.Modified;
                }
                db.SaveChanges();
                Search.Execute(SearchText);                
                
            }
        }));
        public Command DelTeamForGrid => _delTeamForGrid ?? (_delTeamForGrid = new Command(obj=> 
        {
            
            foreach (var item in DefaultersSelect)
            {
                if (IsResultNull(item))
                {
                    item.DateResult = null;
                    item.NameTeam = "";
                    item.Descriptions = "";
                    db.Entry(item).State = EntityState.Modified;
                }
            }
            db.SaveChanges();
            Search.Execute(SearchText);           
           

        }));
        public Command FilterTeamForGrid => _filterTeamForGrid ?? (_filterTeamForGrid = new Command(obj=> 
        {
            LoadDefaulters();
            Defaulters = Defaulters.Where(x => x.DateResult == DateResult &&
                                                  x.NameTeam == TeamSelect.NameTeam)
                                      .OrderBy(x => x.TotalList.Address);
            CountItem = Defaulters?.Count() ?? 0;
        }));
        public Command AddResult => _addResult ?? (_addResult = new Command(obj=> 
        {
            var item = DefaultersSelect.FirstOrDefault();
            if (item.NameTeam != "")
            {                
                Info = item.FullNameItem;
                IsDisabled = item.IsDisabled;
                Description = item.DescriptionResult;
                PayTOV = item.PaymentTOVResult;
                PayRZP = item.PaymentRZPResult;
                IsVisibility["Footer"] = Visibility.Visible;
                OnPropertyChanged(nameof(IsVisibility));
            }
        }));
        public Command SaveResult => _saveResult ?? (_saveResult = new Command(obj=>
        {            
            var item = DefaultersSelect.FirstOrDefault();
            item.IsDisabled = IsDisabled;
            item.DescriptionResult = Description;
            item.PaymentTOVResult = PayTOV;
            item.PaymentRZPResult = PayRZP;
            item.Color = "White";
            if (PayTOV >= item.DebtTOV)
            {
                item.Color = "Green";
            }
            if (PayTOV > 0m && PayTOV < item.DebtTOV)
            {
                item.Color = "Yellow";
            }
            if (IsDisabled)
            {
                item.Color = "Red";
            }
            
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            
            IsVisibility["Footer"] = Visibility.Collapsed;
            OnPropertyChanged(nameof(IsVisibility));
            TeamSelect = Teams.FirstOrDefault(x => x.NameTeam == teamSaveValue);
            FilterTeamForGrid.Execute("");
            
        }));

        public Command PrintGrid => _printGrid ?? (_printGrid = new Command(async obj=> 
        {
            StartProgressBar();
            await Task.Run(()=> 
            {            
                if (Defaulters.Count() > 0)
                {
                    PrintService service = new PrintService();
                    service.Defaulters = Defaulters;
                    service.PrintList("\\Blanks\\Reestr");
                }
            });
            StopProgressBar();
        }));
        public Command PrintReportToday => _printReportToday ?? (_printReportToday = new Command(async obj=> 
        {
            LoadDefaulters();
            StartProgressBar();
            await Task.Run(() =>
            {
                if (Defaulters.Count() > 0)
                {
                    PrintService service = new PrintService();
                    service.Defaulters = Defaulters;
                    service.PrintReportToday("\\Blanks\\ReportToday", DateResult);
                }
            });
            StopProgressBar();
        }));
        public Command PrintReportTelegram => _printReportTelegram ?? (_printReportTelegram = new Command(async obj=> 
        {
            LoadDefaulters();
            StartProgressBar();
            await Task.Run(() =>
            {
                if (Defaulters.Count() > 0)
                {
                    PrintService service = new PrintService();
                    service.Defaulters = Defaulters;
                    service.PrintReportTelegram("\\Blanks\\ReportTelegram", DateResult);
                }
            });
            StopProgressBar();
        }));




        public MainViewModel()
        {
            InitializeVisibility();
            InitializedDB();
            LoadComboBox();
            DefaultersSelect = new List<Defaulter>();
            DateResult = DateTime.Today;
            SearchText = "";
            IsCheckedFinish = true;
            
        }

        private void InitializedDB()
        {            
            db = new ContextDefaulter();
            db.Defaulters.Include(x => x.TotalList).Load();                     
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
            if (TotalLists?.Count() <= 0)
            {
                IsVisibility["db"] = Visibility.Collapsed;
                IsVisibility["MenuClear"] = Visibility.Visible;
                OnPropertyChanged(nameof(IsVisibility));
            }
            else
            {
                IsVisibility["db"] = Visibility.Visible;
                IsVisibility["MenuClear"] = Visibility.Collapsed;
                OnPropertyChanged(nameof(IsVisibility));
            }
        }
        private void LoadDefaulters()
        {            
            dateLoad = db.Dictionaries.FirstOrDefault(x=>x.NameKey == "DateLoad")?.ValueKeyDate ?? DateTime.MinValue;
            Defaulters = db.Defaulters.Local.ToBindingList().Where(x=>x.Date == dateLoad).OrderBy(x=>x.TotalList.Address);
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
                { "RightPanelGrid", Visibility.Visible},
                { "LeftPanel", Visibility.Collapsed },
                { "RightPanel", Visibility.Collapsed},
                { "Menu", Visibility.Visible},
                { "MenuClear", Visibility.Collapsed},
                { "Footer", Visibility.Collapsed},
                { "ProgressBar", Visibility.Collapsed},
                { "db", Visibility.Collapsed}
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
        private  bool Operator(string logic, decimal x, decimal y)
        {
            switch (logic)
            {
                case ">": return x > y;
                case "<": return x < y;
                case "==": return x == y;
                default: throw new Exception("invalid logic");
            }
        }
        private bool IsResultNull(Defaulter item)
        {
            if ((!item.IsDisabled) && item.PaymentTOVResult <= 0m && item.PaymentRZPResult <= 0m && string.IsNullOrWhiteSpace(item.DescriptionResult))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Метод закриває программу
        /// </summary>        
        private void ExitApplication()
        {
            db.Dispose();
            Application app = Application.Current;
            app.Shutdown();
        }
    }
}
