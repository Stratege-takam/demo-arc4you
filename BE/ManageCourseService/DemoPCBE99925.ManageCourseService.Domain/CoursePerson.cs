using Arc4u.Data;

namespace EG.DemoPCBE99925.ManageCourseService.Domain;

/// <summary>
/// CoursePerson
/// </summary>
public class CoursePerson : IPersistEntity
{
    #region Audi Properties
    public Guid Id { get; set; }
	public PersistChange PersistChange { get; set; }
	public string AuditedBy { get; set; }
	public DateTime AuditedOn { get; set; }
    #endregion Audi Properties

    #region Properties
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    #endregion Properties

    #region Navigation
    public ICollection<Participant> Participants { get; set; }
    public Guid LeadId { get; set; }
    public Teacher Lead { get; set; }

    #endregion Navigation
}
