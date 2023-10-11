
using Arc4u.Data;

namespace EG.DemoPCBE99925.ManageCourseService.Facade.Dtos;

/// <summary>
/// Student
/// </summary>
public class PersonDto
{
    /// <summary>
    /// Id of Student.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Define what we do with the object => delete, update, insert?
    /// </summary>
    public PersistChange PersistChange { get; set; }

    #region Properties 
    public string FirstName { get; set; }

    public string LastName { get; set; }

    #endregion Properties 

    
}
