using System;
using Arc4u.Data;
using EG.DemoPCBE99925.ManageCourseService.Domain;

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

    #region Properties Default

    public string FirstName { get; set; }

    public string LastName { get; set; }

    #endregion Properties Default


    #region Properties
    public double Salary { get; set; }

    public DateTime HireDate { get; set; }

    #endregion

}
