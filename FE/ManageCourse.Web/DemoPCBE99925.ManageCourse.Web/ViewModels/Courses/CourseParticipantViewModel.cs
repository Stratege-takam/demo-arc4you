using System.ComponentModel.DataAnnotations;
using EG.DemoPCBE99925.ManageCourseService.Facade.Sdk;

namespace EG.DemoPCBE99925.ManageCourse.Web.ViewModels.Courses;

public class CourseParticipantViewModel : NotifyChangeProperty
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

    private string _studentId;
    [Required]
    public string StudentId { get => _studentId; set => SetProperty(ref _studentId, value); }


    private string _coursePersonId;
    [Required]
    public string CoursePersonId { get => _coursePersonId; set => SetProperty(ref _coursePersonId, value); }

    #endregion Navigation
}
