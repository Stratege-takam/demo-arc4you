using System.ComponentModel.DataAnnotations;

namespace EG.DemoPCBE99925.ManageCourse.Web.ViewModels.Students;

public class StudentFormViewModel: NotifyChangeProperty
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
    private string _matricule = GenerateMatricule();
    [Required]
    public string Matricule { get => _matricule; set => SetProperty(ref _matricule, value); }
    #endregion


    #region Method Helpers
    public static string GenerateMatricule()
    {
        Random rand = new Random(100);
        int digit = rand.Next(00000, 99999);

        return $"P{digit}";
    }
    #endregion Method Helpers
}
