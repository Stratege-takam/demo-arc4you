
using Arc4u.Data;

namespace EG.DemoPCBE99925.ManageCourseService.Facade.Dtos;

/// <summary>
/// Teacher
/// </summary>
public class TeacherDto : PersonDto
{
    #region Properties
    public double Salary { get; set; }

    public DateTime HireDate { get; set; }

    #endregion

}
