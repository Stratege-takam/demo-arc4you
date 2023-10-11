using Arc4u.Data;

namespace EG.DemoPCBE99925.ManageCourseService.Facade.Dtos;

/// <summary>
/// Course
/// </summary>
public class GetCourseDto: CourseDto
{
    #region Navigation Properties

    public PersonDto? Owner { get; set; }

    public IList<GetCoursePersonDto> CoursePeople { get; set; }

    #endregion Navigation Properties
}
