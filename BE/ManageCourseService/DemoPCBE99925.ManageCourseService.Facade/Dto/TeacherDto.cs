using System;
using Arc4u.Data;

namespace EG.DemoPCBE99925.ManageCourseService.Facade.Dtos;

/// <summary>
/// Teacher
/// </summary>
public class TeacherDto
{
    /// <summary>
    /// Id of Teacher.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Define what we do with the object => delete, update, insert?
    /// </summary>
    public PersistChange PersistChange { get; set; }
}