using System.ComponentModel.DataAnnotations;
using EG.DemoPCBE99925.ManageCourseService.Facade.Sdk;

namespace EG.DemoPCBE99925.ManageCourse.Web.ViewModels.Courses;

public class CourseFormViewModel : NotifyChangeProperty
{
    #region Properties 
    private string _id = Guid.NewGuid().ToString();
    [Required]
    public string Id { get => _id; set => SetProperty(ref _id, value); }


    private string _name;

    public string Name { get => _name; set => SetProperty(ref _name, value); }

    private string _description;
    [Required]
    public string Description { get => _description; set => SetProperty(ref _description, value); }


    private double _unity;
    [Required, Range(0.1, 1000)]
    public double Unity { get => _unity; set => SetProperty(ref _unity, value); }

    private string _ownerId;
    [Required]
    public string OwnerId { get => _ownerId; set => SetProperty(ref _ownerId, value); }

    #endregion Properties 

    #region Navigation properties
    public PersonDto Owner { get; set; }
    public IList<CoursePersonDto> CoursePeople { get; set; }
    #endregion Navigation properties
}
