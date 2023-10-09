using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.ManageCourse.Web.Proxies;
using EG.DemoPCBE99925.ManageCourse.Web.ViewModels.Courses;
using EG.DemoPCBE99925.ManageCourseService.Facade.Sdk;
using Microsoft.AspNetCore.Components;

namespace EG.DemoPCBE99925.ManageCourse.Web.States;

[Export, Scoped]
public class CourseState: BaseState
{

    private readonly DemoPCBE99925ManageCourseServiceCourseFacade _facade;
    private readonly SwitchUserState _userState;
    private readonly DemoPCBE99925ManageCourseServiceStudentFacade _studentFacade;
    private readonly DemoPCBE99925ManageCourseServiceTeacherFacade _teacherFacade;
    private readonly NavigationManager _navigationManager;
    #region Course properties list
    public IList<CourseDto> Courses { get; set; }

    private bool _loadingRefresh = false;
    public bool LoadingRefresh
    {
        get => _loadingRefresh;
        set => SetProperty(ref _loadingRefresh, value);
    }

    #endregion Course properties list

    #region Course properties form (Edit / Create)

    public IList<PersonDto> Owners { get; set; }
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

    public CourseFormViewModel Model { get; set; }
    #endregion Course properties form (Edit / Create)

    #region Constructor
    public CourseState(
        DemoPCBE99925ManageCourseServiceStudentFacade studentFacade,
        DemoPCBE99925ManageCourseServiceTeacherFacade teacherFacade,
        DemoPCBE99925ManageCourseServiceCourseFacade facade,
        SwitchUserState userState,
        NavigationManager navigationManager)
    {
        _facade = facade;
        _userState = userState;
        _studentFacade = studentFacade;
        _teacherFacade = teacherFacade;
        _navigationManager = navigationManager;
        ResetForm();
    }
    #endregion Constructor

    #region Public Methods
    public async Task RefreshList()
    {
        LoadingRefresh = true;
        Courses = await _facade.Proxy.GetAllAsync().ConfigureAwait(false);
        LoadingRefresh = false;
    }

    public async Task RefreshOwners()
    {
        if(_userState.CurrentRole == Utils.Enums.UserTypeEnum.Student)
        {
            var list = await _studentFacade.Proxy.GetAllAsync().ConfigureAwait(false);
            Owners = list?.Select(s => s as PersonDto).ToList();
        }
        else if (_userState.CurrentRole == Utils.Enums.UserTypeEnum.Teacher)
        {
            var list = await _teacherFacade.Proxy.GetAllAsync().ConfigureAwait(false);
            Owners = list?.Select(t => t as PersonDto).ToList(); 
        }
        else
        {
            Owners = new List<PersonDto>();

        }

        RaisePropertyChanged();
    }

    public async Task Submit()
    {
        SuccessSave = false;
        ErrorServer = null;
        this.Loading = true;
        var Course = ConvertCourseViewModelToDto();

        try
        {
            await _facade.Proxy.SaveAsync(Course).ConfigureAwait(false);
            SuccessSave = true;
            UpdateList();
        }
        catch (Exception e)
        {
            ErrorServer = $"The error occurence in identifier: {Course.Id}. Contact admin@elia.be";
        }

        Loading = false;
        RaisePropertyChanged();
    }

    public void ResetForm()
    {
        Model = new CourseFormViewModel();
    }


    public void OpenConfirmDelete(CourseDto CourseDto, Action action)
    {
        Loading = false;
        ErrorServer = null;
        SuccessSave = false;
        if (CourseDto != null)
        {
            ConvertCourseDtoToViewModel(CourseDto);
            PersistChange = PersistChange.Delete;
            action?.Invoke();
        }
    }

    public void OpenForm(CourseDto? CourseDto, bool isUpdate = true)
    {
        Loading = false;
        ErrorServer = null;
        SuccessSave = false;
        // state is created
        if (CourseDto == null)
        {
            ResetForm();
            PersistChange = PersistChange.Insert;
        }
        else  // state is updated or detail
        {
            ConvertCourseDtoToViewModel(CourseDto);

            if (isUpdate)
            {
                PersistChange = PersistChange.Update;
            }
            else
            {
                PersistChange = PersistChange.None;
            }
        }

        _navigationManager.NavigateTo(_userState.GetRoute(Utils.Enums.AppRouteEnum.Courses_Form));

    }
    #endregion Public method

    #region Private Methods

    private CourseDto ConvertCourseViewModelToDto()
    {
        var Course = new CourseDto()
        {
            Name = Model.Name,
            Description = Model.Description,
            Unity = Model.Coefficient,
            Id = Guid.Parse(Model.Id),
            OwnerId = Guid.Parse(Model.OwnerId),
            PersistChange = PersistChange
        };

        return Course;
    }

    private void ConvertCourseDtoToViewModel(CourseDto courseDto)
    {
        Model = new CourseFormViewModel()
        {
            Name = courseDto.Name,
            Description = courseDto.Description,
            Coefficient = courseDto.Unity,
            Id = courseDto.Id.ToString(),
            OwnerId = courseDto.OwnerId.ToString(),
           // Owner = courseDto.Owner
        };
    }

    private void UpdateList()
    {
        switch (PersistChange)
        {

            case PersistChange.Insert:
                Courses.Insert(0, ConvertCourseViewModelToDto());
                ResetForm();
                break;

            case PersistChange.Update:
                Courses = Courses.Select(t =>
                {
                    if (t.Id == Guid.Parse(Model.Id))
                    {
                        t = ConvertCourseViewModelToDto();
                    }
                    return t;
                }).ToList();
                break;

            case PersistChange.Delete:
                Courses = Courses.Where(t => t.Id != Guid.Parse(Model.Id)).ToList();
                break;
        }

    }


    #endregion
}
