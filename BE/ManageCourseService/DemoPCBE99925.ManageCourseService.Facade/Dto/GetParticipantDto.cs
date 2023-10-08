
using Arc4u.Data;

namespace EG.DemoPCBE99925.ManageCourseService.Facade.Dtos;

/// <summary>
/// Participant
/// </summary>
public class GetParticipantDto: ParticipantDto
{
    #region Navigations
    public StudentDto Student { get; set; }

    public CoursePersonDto CoursePerson { get; set; }
    #endregion Navigations
}
