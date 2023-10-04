using System;
using Arc4u.Data;
using EG.DemoPCBE99925.ManageCourseService.Domain;

namespace EG.DemoPCBE99925.ManageCourseService.Facade.Dtos;

/// <summary>
/// CoursePerson
/// </summary>
public class CoursePersonDto
{
    /// <summary>
    /// Id of CoursePerson.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Define what we do with the object => delete, update, insert?
    /// </summary>
    public PersistChange PersistChange { get; set; }


    #region Properties
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    #endregion Properties

    #region Navigation
    public Guid LeadId { get; set; }
    public Teacher Lead { get; set; }

    public Guid CourseId { get; set; }
    public Course Course { get; set; }

    #endregion Navigation
}
