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
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using EG.DemoPCBE99925.ManageCourse.WPF.Controls;
using System.IO;
using System.Text;
using System.Windows;
using System.Reflection;

namespace EG.DemoPCBE99925.ManageCourse.WPF.ViewModels;

[Export, Shared]
public class StudentPageVM : VmBase<StudentRes, StudentPageVM>
{

    #region Properties
    private ObservableCollection<StudentItemVM> _students;

    public ObservableCollection<StudentItemVM> Students
    {
        get { return _students; }
        set { SetProperty(ref _students, value); }
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
    private void InitCommand()
    {
        DeletingCommand = new DelegateCommand<GridViewDeletingEventArgs>(OnDeletingCommandExecuted);
        ExportCommand = new DelegateCommand<GridViewElementExportedEventArgs>(OnExportedCommandExecuted);
        RowEditEndedCommand = new DelegateCommand<GridViewRowEditEndedEventArgs>(OnRowEditEndedCommandExecuted);
        AddingNewDataCommand = new DelegateCommand<GridViewAddingNewEventArgs>(OnAddingNewDataCommandExecuted);
        DistinctValuesLoadingCommand = new DelegateCommand<GridViewDistinctValuesLoadingEventArgs>(OnDistinctValuesLoadingCommandExecuted);
        ImportClickedCommand = new DelegateCommand<object>(ImportClickedCommandCommandExecuted);
        SaveCommand = new Prism.Commands.DelegateCommand( async() => await onSaveCommandExecuted());
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
    private void OnDeletingCommandExecuted(GridViewDeletingEventArgs e)
    {
        var items = e.Items;
        if (items != null)
        {
            RadWindow.Confirm("Are you sure?", OnRadWindowClosed);
        }
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
            e.ItemsSource = new List<string>()
            {
                "Brad",
                "Nell",
                "Will",
                "Camila",
                "Lionel"
            };
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
        IsDataChanged = false;
    }

    private void OnAddingNewDataCommandExecuted(GridViewAddingNewEventArgs args)
    {
        args.NewObject = new StudentItemVM();
        IsDataChanged = true;
    }

    private async Task onSaveCommandExecuted()
    {
        try
        {
            foreach (var item in Students.ToList().Where(s => s.PersistChange != PersistChange.None))
            {
               await _facade.Proxy.SaveAsync(new StudentDto()
                {
                    FirstName = item.Firstname,
                    LastName = item.Lastname,
                    PersistChange = item.PersistChange,
                    Matricule = item.Matricule,
                    Id = item.Id
                }).ConfigureAwait(false);
            }
            IsDataChanged = false;
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
           /* foreach (var club in itemsToBeDeleted)
            {
                gridView.Items.Remove(club);
            }*/
        }
    }
    #endregion

    #region Methods

    private async Task LoadDataAsync()
    {
        var students = await _facade.Proxy.GetAllAsync().ConfigureAwait(false);
        Students = new ObservableCollection<StudentItemVM>(students.Select((s, index) => new StudentItemVM
        {
            Firstname = s.FirstName,
            Lastname = s.LastName,
            Id = s.Id,
            Index = index + 1,
            Matricule = s.Matricule
        }));
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

    public class StudentItemVM: BindableBase
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

        private Guid _id;

        public Guid Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

    }
}
