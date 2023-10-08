
using Arc4u.Data;
namespace EG.DemoPCBE99925.ManageCourseService.Facade.Dtos;

/// <summary>
/// Student
/// </summary>
public class GetStudentDto: StudentDto
{
    #region Navigations
    public IList<CourseDto> Courses { get; set; }
    #endregion Navigations
}
