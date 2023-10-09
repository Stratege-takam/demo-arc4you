using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.ManageCourse.Web.Proxies;
using EG.DemoPCBE99925.ManageCourse.Web.ViewModels.Students;
using EG.DemoPCBE99925.ManageCourseService.Facade.Sdk;

namespace EG.DemoPCBE99925.ManageCourse.Web.States;

[Export, Scoped]
public class StudentState: BaseState
{
    private readonly DemoPCBE99925ManageCourseServiceStudentFacade _facade;

    #region Student properties list
    public IList<StudentDto> Students { get; set; }

    private bool _loadingRefresh = false;
    public bool LoadingRefresh
    {
        get => _loadingRefresh;
        set => SetProperty(ref _loadingRefresh, value);
    }

    private bool _dataIsAlreadyInitialize = false;
    public bool DataIsAlreadyInitialize
    {
        get => _dataIsAlreadyInitialize;
        set => SetProperty(ref _dataIsAlreadyInitialize, value);
    }

    #endregion Student properties list

    #region Student properties form (Edit / Create)
    private bool _loading = false;
    public bool Loading
    {
        get => _loading;
        set => SetProperty(ref _loading, value);
    }

    private bool _successSave = false;
    public bool SuccessSave
    {
        get => _successSave;
        set => SetProperty(ref _successSave, value);
    }




    private PersistChange _persistChange;
    public PersistChange PersistChange
    {
        get => _persistChange;
        set => SetProperty(ref _persistChange, value);
    }

    private string? _errorServer;
    public string? ErrorServer
    {
        get => _errorServer;
        set => SetProperty(ref _errorServer, value);
    }

    public StudentFormViewModel Model { get; set; }
    #endregion Student properties form (Edit / Create)

    #region Constructor
    public StudentState(DemoPCBE99925ManageCourseServiceStudentFacade facade)
    {
        _facade = facade;
        ResetForm();
    }
    #endregion Constructor

    #region Public Methods
    public async Task RefreshList()
    {
        LoadingRefresh = true;
        Students = await _facade.Proxy.GetAllAsync().ConfigureAwait(false);
        LoadingRefresh = false;
        DataIsAlreadyInitialize = true;
    }

    public async Task Submit()
    {
        SuccessSave = false;
        this.Loading = true;
        ErrorServer = null;
        var Student = ConvertStudentViewModelToDto();

        try
        {
            await _facade.Proxy.SaveAsync(Student).ConfigureAwait(false);
            SuccessSave = true;
            UpdateList();
        }
        catch (Exception e)
        {
            ErrorServer = $"The error occurence in identifier: {Student.Id}. Contact admin@elia.be";
        }

        Loading = false;
        RaisePropertyChanged();
    }

    public void ResetForm()
    {
        Model = new StudentFormViewModel();
    }


    public void OpenConfirmDelete(StudentDto StudentDto, Action action)
    {
        Loading = false;
        ErrorServer = null;
        SuccessSave = false;
        if (StudentDto != null)
        {
            ConvertStudentDtoToViewModel(StudentDto);
            PersistChange = PersistChange.Delete;
            action?.Invoke();
        }
    }

    public void OpenForm(StudentDto? StudentDto, Action action, bool isUpdate = true)
    {
        Loading = false;
        ErrorServer = null;
        SuccessSave = false;
        // state is created
        if (StudentDto == null)
        {
            ResetForm();
            PersistChange = PersistChange.Insert;
        }
        else  // state is updated or detail
        {
            ConvertStudentDtoToViewModel(StudentDto);

            if (isUpdate)
            {
                PersistChange = PersistChange.Update;
            }
            else
            {
                PersistChange = PersistChange.None;
            }
        }

        action?.Invoke();
    }
    #endregion Public method

    #region Private Methods

    private StudentDto ConvertStudentViewModelToDto()
    {
        var Student = new StudentDto()
        {
            FirstName = Model.FirstName,
            Matricule = Model.Matricule,
            LastName = Model.LastName,
            Id = Guid.Parse(Model.Id),
            PersistChange = PersistChange
        };

        return Student;
    }

    private void ConvertStudentDtoToViewModel(StudentDto StudentDto)
    {
        Model = new StudentFormViewModel()
        {
            FirstName = StudentDto.FirstName,
            Matricule = StudentDto.Matricule,
            LastName = StudentDto.LastName,
            Id = StudentDto.Id.ToString()
        };
    }

    private void UpdateList()
    {
        switch (PersistChange)
        {

            case PersistChange.Insert:
                Students.Insert(0, ConvertStudentViewModelToDto());
                ResetForm();
                break;

            case PersistChange.Update:
                Students = Students.Select(t =>
                {
                    if (t.Id == Guid.Parse(Model.Id))
                    {
                        t = ConvertStudentViewModelToDto();
                    }
                    return t;
                }).ToList();
                break;

            case PersistChange.Delete:
                Students = Students.Where(t => t.Id != Guid.Parse(Model.Id)).ToList();
                break;
        }

    }


    #endregion
}
