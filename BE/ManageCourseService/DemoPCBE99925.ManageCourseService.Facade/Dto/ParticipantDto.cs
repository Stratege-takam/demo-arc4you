
using Arc4u.Data;

namespace EG.DemoPCBE99925.ManageCourseService.Facade.Dtos;

/// <summary>
/// Participant
/// </summary>
public class ParticipantDto
{
    /// <summary>
    /// Id of Participant.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Define what we do with the object => delete, update, insert?
    /// </summary>
    public PersistChange PersistChange { get; set; }

    #region Properties
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    #endregion

    #region Navigations
    public Guid StudentId { get; set; }

    /// <summary>
    /// The course that the student participate
    /// </summary>
    public Guid CoursePersonId { get; set; }

    #endregion Navigations
}
