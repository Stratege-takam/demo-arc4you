using System.ComponentModel.DataAnnotations.Schema;
using Arc4u.Data;

namespace EG.DemoPCBE99925.ManageCourseService.Domain;

/// <summary>
/// Person
/// </summary>
[Table("Peoples")]
public class Person : IPersistEntity
{
    #region Audi Properties
    public Guid Id { get; set; }
	public PersistChange PersistChange { get; set; }
	public string AuditedBy { get; set; }
	public DateTime AuditedOn { get; set; }
    #endregion Audi Properties

    #region Properties
    public string FirstName { get; set; }

    public string LastName { get; set; }

    #endregion

    #region Navigations
    public ICollection<Course> Courses { get; set; }
    #endregion

}
