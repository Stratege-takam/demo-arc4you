using System.ComponentModel.DataAnnotations.Schema;
using Arc4u.Data;

namespace EG.DemoPCBE99925.ManageCourseService.Domain;

/// <summary>
/// Student
/// </summary>
[Table("Students")]
public class Student :Person, IPersistEntity
{
    #region Properties
    public string Matricule { get; set; }
    #endregion Properties

    #region Navigation
    public ICollection<Participant> CourseParticipations { get; set; }
    #endregion Navigation
}
