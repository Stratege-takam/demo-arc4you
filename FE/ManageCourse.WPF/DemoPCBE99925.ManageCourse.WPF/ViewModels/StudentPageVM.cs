using Arc4u.Dependency;
using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.ManageCourse.WPF.Views.Resources;
using EG.DemoPCBE99925.ManageCourse.WPF.Common;
using Microsoft.Extensions.Logging;
using EG.DemoPCBE99925.ManageCourse.WPF.Proxies;
using System.Collections.ObjectModel;
using EG.DemoPCBE99925.ManageCourseService.Facade.Sdk;
using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;
using Prism.Commands;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using EG.DemoPCBE99925.ManageCourse.WPF.Controls;
using System.IO;
using System.Text;
using System.Windows;
using System.Reflection;
using Telerik.Windows.Data;
using EG.DemoPCBE99925.ManageCourse.WPF.Extensions;

namespace EG.DemoPCBE99925.ManageCourse.WPF.ViewModels;

[Export, Shared]
public class StudentPageVM : VmBase<StudentRes, StudentPageVM>
{

    #region Private
    private GridViewDeletingEventArgs CurrentDeleteEvent;
    private List<StudentItemVM> StudentsReseted = new List<StudentItemVM>();
    #endregion

    #region Properties

    public const int TAKE = 300;
    private ObservableCollection<StudentItemVM> _students;

    public ObservableCollection<StudentItemVM> Students
    {
        get { return _students; }
        set { SetProperty(ref _students, value); }
    }

    private int _count;

    public int Count
    {
        get { return _count; }
        set { SetProperty(ref _count, value); }
    }

    private int _currentCount;

    public int CurrentCount
    {
        get { return _currentCount; }
        set {
            SetProperty(ref _currentCount, value);
            if (value == Count)
            {
                LoadingText = $"{value}/{Count}";
            }
            else
            {
                LoadingText = $"Loading {value}/{Count}";
            }
        }
    }

    private string _loadingText = "Loading...";

    public string LoadingText
    {
        get { return _loadingText; }
        set { SetProperty(ref _loadingText, value); }
    }

    private VirtualQueryableCollectionView _studentsVqc;

    public VirtualQueryableCollectionView StudentsVqc
    {
        get { return _studentsVqc; }
        set { SetProperty(ref _studentsVqc, value); }
    }

    private bool _isDataChanged;

    public bool IsDataChanged
    {
        get { return _isDataChanged; }
        set { SetProperty(ref _isDataChanged, value); }
    }

    #endregion Properties

    #region Commands
    public ICommand DeletingCommand { get; set; }
    public ICommand ExportCommand { get; set; }
    public ICommand SaveCommand { get; set; }
    public ICommand RowEditEndedCommand { get; set; }
    public ICommand AddingNewDataCommand { get; set; }
    public ICommand DistinctValuesLoadingCommand { get; set; }
    public ICommand ImportClickedCommand { get; set; }
    public ICommand ResetListCommand { get; set; }
    public ICommand CellValidatedCommand { get; set; }
    private void InitCommand()
    {
        DeletingCommand = new DelegateCommand<GridViewDeletingEventArgs>(OnDeletingCommandExecuted);
        ExportCommand = new DelegateCommand<GridViewElementExportedEventArgs>(OnExportedCommandExecuted);
        RowEditEndedCommand = new DelegateCommand<GridViewRowEditEndedEventArgs>(OnRowEditEndedCommandExecuted);
        CellValidatedCommand = new DelegateCommand<GridViewCellValidatedEventArgs>(OnCellValidatedCommandExecuted);
        AddingNewDataCommand = new DelegateCommand<GridViewAddingNewEventArgs>(OnAddingNewDataCommandExecuted);
        DistinctValuesLoadingCommand = new DelegateCommand<GridViewDistinctValuesLoadingEventArgs>(OnDistinctValuesLoadingCommandExecuted);
        ImportClickedCommand = new DelegateCommand<object>(ImportClickedCommandCommandExecuted);
        SaveCommand = new Prism.Commands.DelegateCommand( async() => await OnSaveCommandExecuted());
        ResetListCommand = new Prism.Commands.DelegateCommand(OnResetListCommandExecuted);
    }

   
    #endregion Commands

    #region Constructor

    public readonly DemoPCBE99925ManageCourseServiceStudentFacade _facade;
    public StudentPageVM(IContainerResolve container,
        ILogger<StudentPageVM> logger,
        DemoPCBE99925ManageCourseServiceStudentFacade facade) : base(new StudentRes(), container, logger)
    {
        _facade = facade;

        LoadDataAsync().ConfigureAwait(false);

        InitCommand();
    }

    #endregion Constructor


    #region Methods bind command

    private void OnCellValidatedCommandExecuted(GridViewCellValidatedEventArgs e)
    {
        if (e.ValidationResult.ErrorMessage != null)
        {
            RadWindow.Confirm($" {e.ValidationResult.ErrorMessage} Are you sure to Cancel editing?", OnConfirmCancelEditRadWindowClosed);
        }
    }

    private void OnConfirmCancelEditRadWindowClosed(object sender, WindowClosedEventArgs e)
    {
        //check whether the user confirmed 
        bool confirm = e.DialogResult.HasValue ? e.DialogResult.Value : false;
        if (confirm)
        {
           // gridViewStudent.CancelEdit();
        }
    }

    private void OnDeletingCommandExecuted(GridViewDeletingEventArgs e)
    {
        var items = e.Items;
        CurrentDeleteEvent = e;
        if (items != null)
        {
            RadWindow.Confirm("Are you sure?", OnRadWindowClosed);
        }
    }

    private void OnResetListCommandExecuted()
    {
        BindStudents();
        IsDataChanged = false;
    }

    private void ImportClickedCommandCommandExecuted(object e)
    {
       var importArgs = e as ImportClickedEventArgs;
        if (importArgs != null)
        {
            if (!string.IsNullOrEmpty(importArgs.Filename))
            {
               ImportFile(importArgs.Filename).ConfigureAwait(false);
            }
        }
    }

    private void OnDistinctValuesLoadingCommandExecuted(GridViewDistinctValuesLoadingEventArgs e)
    {
      //  var gridView = (RadGridView)sender;
        bool shouldIncludeFilteredOutItems = false;

        // this call will return the first 15 distinct values regardless of their actual count in the data source 
        if (e.Column.UniqueName == "Firstname")
        {
           /* e.ItemsSource = new List<string>()
            {
                "Brad",
                "Nell",
                "Will",
                "Camila",
                "Lionel"
            }; */
        }

        //gridView.GetDistinctValues(e.Column, shouldIncludeFilteredOutItems, 15);
    }

    private void OnExportedCommandExecuted(GridViewElementExportedEventArgs e)
    {
        var element = e.Element;
        if (element != ExportElement.Row)
        {
            StudentItemVM obj = e.Context as StudentItemVM;
            if (obj != null)
            {
                e.Writer.Write(String.Format(@"<tr><td style=""background-color:#CCC;"" colspan=""{0}"">",4));
                e.Writer.Write(String.Format(@"<b>Id:</b> {0} <br />", obj.Id));
                e.Writer.Write(String.Format(@"<b>Firstname:</b> {0} <br />", obj.Firstname));
                e.Writer.Write(String.Format(@"<b>Lastname:</b> {0} <br />", obj.Lastname));
                e.Writer.Write(String.Format(@"<b>Matricule:</b> {0} <br />", obj.Matricule));
                e.Writer.Write("</td></tr>");
            }
        }
    }

    private void OnRowEditEndedCommandExecuted(GridViewRowEditEndedEventArgs e)
    {
        var obj = e.NewData as StudentItemVM;

        if (obj != null  && obj.PersistChange == PersistChange.None)
        {
            var values = e.OldValues?.Values;

            var old = new StudentItemVM()
            {
                Firstname = values != null ? values.ElementAt(1).ToString() : "",
                Lastname = values != null ? values.ElementAt(2).ToString() : "",
                Matricule = values != null ? values.ElementAt(3).ToString() : ""
            };

            if (old.Firstname != obj.Firstname || old.Lastname != obj.Lastname || old.Matricule != obj.Matricule)
            {
                obj.PersistChange = PersistChange.Update;
                e.Row.DataContext = obj;
                IsDataChanged = true;
            }
        }
        else if(obj != null &&  obj.PersistChange == PersistChange.Insert)
        {
            CurrentCount++;
            IsDataChanged = true;
        }
    }

    private void OnAddingNewDataCommandExecuted(GridViewAddingNewEventArgs args)
    {
        var obj = new StudentItemVM();
        obj.PersistChange = PersistChange.Insert;
        obj.Index = Students.Max(s => s.Index) + 1;
        args.NewObject = obj;
    }

    private async Task OnSaveCommandExecuted()
    {
        try
        {
            var students = Students.Where(s => s.PersistChange != PersistChange.None).ToList();
            var toDelete = StudentsReseted.Where(s => !Students.Any(t => s.Id == t.Id)).Select(s =>
            {
                s.PersistChange = PersistChange.Delete;
                return s;
            }).ToList();

            if (toDelete.Any())
            {
                Students.AddRange(toDelete);
            }

            await _facade.Proxy.SaveManyAsync(students.Select(item => new StudentDto()
            {
                FirstName = item.Firstname,
                LastName = item.Lastname,
                PersistChange = item.PersistChange,
                Matricule = item.Matricule,
                Id = item.Id
            }).ToList()).ConfigureAwait(true);
            IsDataChanged = false;

            UpdateList();
        }
        catch (Exception ex)
        {

            MessageBox.Show(ex.Message,
                        (Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute)).SingleOrDefault() as AssemblyTitleAttribute)?.Title,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
        }


    }

    private void OnRadWindowClosed(object sender, WindowClosedEventArgs e)
    {
        //check whether the user confirmed 
        bool shouldDelete = e.DialogResult.HasValue ? e.DialogResult.Value : false;
        if (shouldDelete)
        {
            Count = Students.Count;
            CurrentCount--;
            IsDataChanged = true;
        }
        else
        {
            CurrentDeleteEvent.Cancel = true;
        }
    }
    #endregion

    #region Methods

    private async Task LoadDataAsync()
    {
        int page = 0;
        var result = new List<StudentItemVM>();
        do
        {
            var skip = TAKE * page;

            var response = await _facade.Proxy.GetAllLazyAsync(TAKE, skip).ConfigureAwait(false);
            Count = response.Count;

            result = response.Results?.Select((s, index) => new StudentItemVM
            {
                Firstname = s.FirstName,
                Lastname = s.LastName,
                Id = s.Id,
                Index = (Students!= null ? Students.Max(s => s.Index) : 0) + index + 1,
                Matricule = s.Matricule
            }).ToList();

            if (result != null && result.Any())
            {
                page++;

                StudentsReseted.AddRange(result);

                BindStudents();

                CurrentCount = Students.Count;
            }
           Thread.Sleep(500);
        } while (result != null && result.Any());
        
      //  StudentsVqc = new VirtualQueryableCollectionView(query) { LoadSize = 5 };
    }

    private void BindStudents()
    {
        Students = new ObservableCollection<StudentItemVM>(StudentsReseted.Clone());
    }
    private async Task<bool> ImportFile(string fileName)
    {
        string line;

        var students = new List<StudentItemVM>();
        var hasErrors = false;
        try
        {
            if (File.Exists(fileName))
            {
                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (var streamReader = new StreamReader(fs, Encoding.Default, true))
                        while ((line = await streamReader.ReadLineAsync()) != null)
                        {
                            if (!string.IsNullOrEmpty(line))
                            {
                                var values = line.Split(';');
                                if (values.Length >= 2)
                                    students.Add(new StudentItemVM { Firstname = values[0],
                                        Lastname = values[1],
                                        Matricule = values.Length > 2 ? values[2] : GenerateMatricule(5).Replace("P", "I"),
                                        PersistChange = PersistChange.Insert });
                            }
                        }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message,
                        (Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute)).SingleOrDefault() as AssemblyTitleAttribute)?.Title,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
   
            hasErrors = true;
        }

        if (!hasErrors)
        {
            var all =  students.Concat(Students.ToList()).Select((s, i) =>
            {
                s.Index = i + 1;
                return s;
            }).ToList();



            Students = new ObservableCollection<StudentItemVM>(all);
            IsDataChanged = true;
            return true;
        }
        return false;
    }

    private void UpdateList()
    {
        var toDelete = StudentsReseted.Where(s => !Students.Any(t => s.Id == t.Id)).ToList();
        if (toDelete.Any())
        {
            StudentsReseted = StudentsReseted.Where(s => !toDelete.Any(t => t.Id == s.Id)).ToList();
        }

        var toInsert = Students.Where(s => s.PersistChange == PersistChange.Insert).ToList();
        if (toInsert.Any())
        {
            StudentsReseted.AddRange(toInsert);
        }

        var toUpdate = Students.Where(s => s.PersistChange == PersistChange.Update).ToList();
        if (toUpdate.Any())
        {
            foreach (var item in toUpdate)
            {
                var index = StudentsReseted.IndexOf(StudentsReseted.FirstOrDefault(s => s.Id == item.Id));
                StudentsReseted[index] = item;
            }
        }

        StudentsReseted = StudentsReseted.Select((s, index) =>
        {
            s.PersistChange = PersistChange.None;
            s.Index = index + 1;
            return s;
        }).ToList();

        BindStudents();
    }
    #endregion

    #region Method Helpers
    public static string GenerateMatricule(int length)
    {
        const string chars = "0123456789";
        Random rand = new Random();
        string digit = new string(Enumerable.Repeat(chars, length)
        .Select(s => s[rand.Next(s.Length)]).ToArray());

        return $"P{digit}";
    }
    #endregion Method Helpers

    public class StudentItemVM: BindableBase, ICloneable
    {
        private int _index;

        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }

        private string _firstname;

        public string Firstname
        {
            get { return _firstname; }
            set { SetProperty(ref _firstname, value); }
        }

        private string _lastname;

        [Required, MinLength(2)]
        public string Lastname
        {
            get { return _lastname; }
            set { SetProperty(ref _lastname, value); }
        }

        private string _matricule = GenerateMatricule(5);

        [Required, MinLength(4)]
        public string Matricule
        {
            get { return _matricule; }
            set { SetProperty(ref _matricule, value); }
        }

        private PersistChange _persistChange;

        public PersistChange PersistChange
        {
            get { return _persistChange; }
            set { SetProperty(ref _persistChange, value); }
        }

        private Guid _id = Guid.NewGuid();

        public Guid Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public object? Clone()
        {
            return MemberwiseClone() as StudentItemVM;
        }
    }
}
