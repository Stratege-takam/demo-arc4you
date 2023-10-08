using Arc4u.Data;

namespace EG.DemoPCBE99925.ManageCourseService.Facade.Dtos;

/// <summary>
/// Course
/// </summary>
public class GetCourseDto: PersonDto
{
    #region Navigation Properties

    public PersonDto Owner { get; set; }

    public IList<CoursePersonDto> CoursePeople { get; set; }

    #endregion Navigation Properties

    #region Help Properties

    public bool CanDelete { get; set; }
    public bool IsTeacher { get; set; }

    #endregion  Help Properties
}
