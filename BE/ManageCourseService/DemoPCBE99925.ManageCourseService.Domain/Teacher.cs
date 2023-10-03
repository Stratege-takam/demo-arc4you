
namespace EG.DemoPCBE99925.ManageCourseService.Domain;

/// <summary>
/// Teacher
/// </summary>
public class Teacher : Person
{

    #region Properties
    public double Salary { get; set; }

    public DateTime HireDate { get; set; }

    #endregion

    #region Navigations
    public ICollection<CoursePerson> LeadCourses { get; set; }
    #endregion Navigations
}
