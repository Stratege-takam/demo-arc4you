
using System.ComponentModel.DataAnnotations.Schema;
using Arc4u.Data;

namespace EG.DemoPCBE99925.ManageCourseService.Domain;

/// <summary>
/// Teacher
/// </summary>
[Table("Teachers")]
public class Teacher : Person, IPersistEntity
{
    #region Properties
    public double Salary { get; set; }

    public DateTime HireDate { get; set; }

    #endregion

    #region Navigations
    public ICollection<CoursePerson> LeadCourses { get; set; }
    #endregion Navigations
}
