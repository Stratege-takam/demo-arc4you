using System.ComponentModel.DataAnnotations;
using EG.DemoPCBE99925.ManageCourseService.Facade.Sdk;

namespace EG.DemoPCBE99925.ManageCourse.Web.ViewModels.Courses;

public class CoursePersonViewModel : NotifyChangeProperty
{
    #region Properties

    private string _id = Guid.NewGuid().ToString();
    [Required]
    public string Id { get => _id; set => SetProperty(ref _id, value); }

    private PersistChange _persistChange;
    [Required]
    public PersistChange PersistChange { get => _persistChange; set => SetProperty(ref _persistChange, value); }

    private DateTime? _startDate;
    [Required]
    public DateTime? StartDate { get => _startDate; set => SetProperty(ref _startDate, value); }

    private DateTime? _endDate;
    [Required]
    public DateTime? EndDate { get => _endDate; set => SetProperty(ref _endDate, value); }
    #endregion Properties

    #region Navigation

    private string _leadId = Guid.NewGuid().ToString();
    [Required]
    public string LeadId { get => _leadId; set => SetProperty(ref _leadId, value); }


    private string _courseId = Guid.NewGuid().ToString();
    [Required]
    public string CourseId { get => _courseId; set => SetProperty(ref _courseId, value); }

    #endregion Navigation
}
