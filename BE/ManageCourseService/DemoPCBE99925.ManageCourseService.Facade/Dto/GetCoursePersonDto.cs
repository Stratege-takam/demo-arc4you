
using Arc4u.Data;
namespace EG.DemoPCBE99925.ManageCourseService.Facade.Dtos;

/// <summary>
/// CoursePerson
/// </summary>
public class GetCoursePersonDto: CoursePersonDto
{
    #region Navigation
    public TeacherDto? Lead { get; set; }

    public CourseDto? Course { get; set; }

    public IList<GetParticipantDto> Participants { get; set; }

    #endregion Navigation
}
