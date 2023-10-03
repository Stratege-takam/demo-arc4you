namespace EG.DemoPCBE99925.ManageCourseService.Domain;

/// <summary>
/// Provide information on the running environment.
/// </summary>
public class EnvironmentInfo
{
    /// <summary>
    /// Name of the application - service.
    /// </summary>
    public string? Name { get; init; }


    /// <summary>
    /// Server name.
    /// </summary>
    public string? Server { get; init; }

    /// <summary>
    /// Database server used.
    /// </summary>
    public List<DatabaseInfo>? DatabaseInfo { get; init; }

}

/// <summary>
/// Will determine which database on which server is used by the application.
/// </summary>
public class DatabaseInfo
{
    /// <summary>
    /// Server name.
    /// </summary>
    public string? Server { get; init; }


    /// <summary>
    /// Database name.
    /// </summary>
    public string? Database { get; init; }
}