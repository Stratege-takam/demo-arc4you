using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.ManageCourse.Web.Proxies;
using EG.DemoPCBE99925.ManageCourse.Web.ViewModels.Teachers;
using EG.DemoPCBE99925.ManageCourseService.Facade.Sdk;

namespace EG.DemoPCBE99925.ManageCourse.Web.States;

[Export, Shared]
public class TeacherState: BaseState
{
    private readonly DemoPCBE99925ManageCourseServiceTeacherFacade _facade;

    #region Teacher properties list
    public IList<TeacherDto> Teachers { get; set; }

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

    #endregion Teacher properties list

    #region Teacher properties form (Edit / Create)
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
    #endregion Teacher properties form (Edit / Create)

    #region Constructor
    public TeacherState(DemoPCBE99925ManageCourseServiceTeacherFacade facade)
    {
        _facade = facade;
        ResetForm();
    }
    #endregion Constructor

    #region Public Methods
    public async Task RefreshList()
    {
        LoadingRefresh = true;
        Teachers = await _facade.Proxy.GetAllAsync().ConfigureAwait(false);
        LoadingRefresh = false;
        DataIsAlreadyInitialize = true;
    }

    public async Task Submit()
    {
        SuccessSave = false;
        this.Loading = true;
        var teacher = ConvertTeacherViewModelToDto();

        try
        {
            await _facade.Proxy.SaveAsync(teacher).ConfigureAwait(false);
            SuccessSave = true;
            UpdateList();
        }
        catch (Exception e)
        {
            ErrorServer = $"The error occurence in identifier: {teacher.Id}. Contact admin@elia.be";
        }

        Loading = false;
    }

    public void ResetForm()
    {
        Model = new StudentFormViewModel();
    }


    public void OpenConfirmDelete(TeacherDto teacherDto, Action action)
    {
        Loading = false;
        ErrorServer = null;
        SuccessSave = false;
        if(teacherDto != null)
        {
            ConvertTeacherDtoToViewModel(teacherDto);
            PersistChange = PersistChange.Delete;
            action?.Invoke();
        }
    }

    public void OpenForm(TeacherDto? teacherDto, Action action, bool isUpdate = true)
    {
        Loading = false;
        ErrorServer = null;
        SuccessSave = false;
        // state is created
        if(teacherDto == null)
        {
            ResetForm();
            PersistChange = PersistChange.Insert;
        }
        else  // state is updated or detail
        {
            ConvertTeacherDtoToViewModel(teacherDto);

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

    private TeacherDto ConvertTeacherViewModelToDto()
    {
        var teacher = new TeacherDto()
        {
            FirstName = Model.FirstName,
            HireDate = Model.HireDate.GetValueOrDefault(),
            LastName = Model.LastName,
            Id = Guid.Parse(Model.Id),
            Salary = Model.Salary,
            PersistChange = PersistChange
        };

        return teacher;
    }

    private void ConvertTeacherDtoToViewModel( TeacherDto teacherDto)
    {
        Model = new StudentFormViewModel()
        {
            FirstName = teacherDto.FirstName,
            HireDate = teacherDto.HireDate,
            LastName = teacherDto.LastName,
            Id = teacherDto.Id.ToString(),
            Salary = teacherDto.Salary
        };
    }

    private void UpdateList()
    {
        switch (PersistChange)
        {

            case PersistChange.Insert:
                Teachers.Insert(0, ConvertTeacherViewModelToDto());
                ResetForm();
                break;

            case PersistChange.Update:
                Teachers = Teachers.Select(t =>
                {
                    if (t.Id == Guid.Parse(Model.Id))
                    {
                        t = ConvertTeacherViewModelToDto();
                    }
                    return t;
                }).ToList();
                break;

            case PersistChange.Delete:
                Teachers = Teachers.Where(t => t.Id != Guid.Parse(Model.Id)).ToList();
                break;
        }

    }


    #endregion
}
