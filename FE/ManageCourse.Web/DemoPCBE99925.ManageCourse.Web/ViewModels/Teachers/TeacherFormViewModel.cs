using System.ComponentModel.DataAnnotations;

namespace EG.DemoPCBE99925.ManageCourse.Web.ViewModels.Teachers;

public class TeacherFormViewModel: NotifyChangeProperty
{
    #region Properties Default
    private string _id =  Guid.NewGuid().ToString();
    [Required]
    public string Id { get => _id; set => SetProperty(ref _id, value); }


    private string _firstName;

    public string FirstName { get => _firstName; set =>  SetProperty(ref _firstName, value); }

    private string _lastName;
    [Required]
    public string LastName { get => _lastName; set => SetProperty(ref _lastName, value); }

    #endregion Properties Default


    #region Properties
    private double _salary;
    [Range(1,int.MaxValue)]
    public double Salary { get => _salary; set => SetProperty(ref _salary, value); }

    private DateTime? _hireDate = null;
    [Required]
    public DateTime? HireDate { get => _hireDate; set => SetProperty(ref _hireDate, value); }
    #endregion
}
