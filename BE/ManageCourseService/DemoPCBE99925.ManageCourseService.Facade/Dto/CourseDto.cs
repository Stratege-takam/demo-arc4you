using System.ComponentModel.DataAnnotations.Schema;
using Arc4u.Data;

namespace EG.DemoPCBE99925.ManageCourseService.Facade.Dtos;

/// <summary>
/// Course
/// </summary>
public class CourseDto
{
    /// <summary>
    /// Id of Course.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Define what we do with the object => delete, update, insert?
    /// </summary>
    public PersistChange PersistChange { get; set; }


    #region Properties
    public string Name { get; set; }
    public string Description { get; set; }
    public double Unity { get; set; }
    public Guid OwnerId { get; set; }

    #endregion Properties

    #region Help Properties

    public bool CanDelete { get; set; }
    public bool CanLead { get; set; }
    public bool IsTeacher { get; set; }
    public string? OwnerFullname { get; set; }
    #endregion  Help Properties
}
