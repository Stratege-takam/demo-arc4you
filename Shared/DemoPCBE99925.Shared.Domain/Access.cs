using Arc4u.Security.Principal;

namespace EG.DemoPCBE99925.Rights;

/// <summary>
/// Defined the operations available to secure the application.
/// </summary>
public enum Access : int
{
    AccessApplication = 1,
    CanSeeSwaggerFacadeApi = 2,
}

public static class Operations
{
    public static readonly Operation[] Values = Enum.GetValues<Access>().Select(o => new Operation { Name = o.ToString(), ID = (int)o }).ToArray();

    public static readonly string[] Scopes = Array.Empty<string>();
}

