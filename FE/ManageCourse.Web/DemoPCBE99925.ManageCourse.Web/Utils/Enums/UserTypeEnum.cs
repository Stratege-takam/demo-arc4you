using System.ComponentModel;

namespace EG.DemoPCBE99925.ManageCourse.Web.Utils.Enums;

public enum UserTypeEnum: ushort
{
    [Description("NONE")]
    None = 0,
    [Description("STUDENT")] 
    Student = 1,
    [Description("TEACHER")]
    Teacher = 2
}
