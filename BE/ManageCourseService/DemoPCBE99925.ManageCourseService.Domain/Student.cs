using System.ComponentModel.DataAnnotations.Schema;

namespace EG.DemoPCBE99925.ManageCourseService.Domain;

/// <summary>
/// Student
/// </summary>
[Table("Students")]
public class Student :Person
{
    #region Properties
    public string Matricule { get; set; }
    #endregion Properties

    #region Navigation
    public ICollection<Participant> CourseParticipations { get; set; }
    #endregion Navigation
}
