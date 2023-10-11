
using Arc4u.Data;

namespace EG.DemoPCBE99925.ManageCourseService.Facade.Dtos;

/// <summary>
/// Teacher
/// </summary>
public class GetTeacherDto: TeacherDto
{
    #region Navigations
    public IList<CourseDto> Courses { get; set; }
    public IList<CoursePersonDto> LeadCourses { get; set; }
    #endregion Navigations
}
