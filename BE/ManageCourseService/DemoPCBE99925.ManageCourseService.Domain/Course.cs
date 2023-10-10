using System.ComponentModel.DataAnnotations.Schema;
using Arc4u.Data;

namespace EG.DemoPCBE99925.ManageCourseService.Domain;

/// <summary>
/// Course
/// </summary>
public class Course : IPersistEntity
{
    #region Audi Properties
    public Guid Id { get; set; }
	public PersistChange PersistChange { get; set; }
	public string AuditedBy { get; set; }
	public DateTime AuditedOn { get; set; }
    #endregion Audi Properties


    #region Properties
    public string Name { get; set; }
    public string Description { get; set; }
    public string Unity { get; set; }

    #endregion Properties

    #region Navigations

    public Person Owner { get; set; }

    public Guid OwnerId { get; set; }

    public ICollection<CoursePerson> CoursePeople { get; set; }

    #endregion Navigation


    #region Help Properties

    [NotMapped]
    public bool CanDelete { get; set; }

    [NotMapped]
    public bool CanLead { get; set; }

    [NotMapped]
    public bool IsTeacher { get; set; }
    [NotMapped]
    public string OwnerFullname { get; set; }

    #endregion  Help Properties
}
