using System;
using Arc4u.Data;
using EG.DemoPCBE99925.ManageCourseService.Domain;

namespace EG.DemoPCBE99925.ManageCourseService.Facade.Dtos;

/// <summary>
/// Student
/// </summary>
public class StudentDto
{
    /// <summary>
    /// Id of Student.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Define what we do with the object => delete, update, insert?
    /// </summary>
    public PersistChange PersistChange { get; set; }

    #region Properties Default
    public string FirstName { get; set; }

    public string LastName { get; set; }

    #endregion Properties Default

    #region Properties
    public string Matricule { get; set; }
    #endregion Properties

}
