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
    [Required, MinLength(2)]
    public string Name { get => _name; set => SetProperty(ref _name, value); }

    private string _description;
    [Required, MinLength(10)]
    public string Description { get => _description; set => SetProperty(ref _description, value); }

    private double _coefficient;
    [Required, Range(1, 1000)]
    public double Coefficient { get => _coefficient; set => SetProperty(ref _coefficient, value); }

    private string _ownerId;
    [Required]
    public string OwnerId { get => _ownerId; set => SetProperty(ref _ownerId, value); }

    #endregion Properties 

    #region Navigation properties
    public PersonDto Owner { get; set; }
    public IList<CoursePersonDto> CoursePeople { get; set; }
    #endregion Navigation properties
}
