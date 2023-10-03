using System;
using Arc4u.Data;

namespace EG.DemoPCBE99925.ManageCourseService.Domain;

/// <summary>
/// Participant
/// </summary>
public class Participant : IPersistEntity
{
    #region Audio Properties
    public Guid Id { get; set; }
	public PersistChange PersistChange { get; set; }
	public string AuditedBy { get; set; }
	public DateTime AuditedOn { get; set; }
    #endregion Audi Properties

    #region Navigations
    public Guid StudentId { get; set; }
    public Student Student { get; set; }


    /// <summary>
    /// The course that the student participate
    /// </summary>
    public Guid CoursePersonId { get; set; }

    public CoursePerson CoursePerson { get; set; }
    #endregion Navigations
}
