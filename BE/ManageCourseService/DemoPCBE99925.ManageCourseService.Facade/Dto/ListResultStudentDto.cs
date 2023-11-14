
namespace EG.DemoPCBE99925.ManageCourseService.Facade.Dtos;

/// <summary>
/// Student
/// </summary>
public class ListResultStudentDto
{
    #region Properties
    public int Count { get; set; }
    public IEnumerable<StudentDto> Results { get; set; }
    #endregion Properties

}
