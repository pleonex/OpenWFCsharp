namespace OpenWFCsharp.Backend.Controllers.Nas;

using System.Globalization;

/// <summary>
/// Response from the authentication 'nas' server for service location requests.
/// </summary>
public record NasSvcLocResponse : NasResponse
{
    /// <summary>
    /// Gets or sets the token to use with the requested service.
    /// </summary>
    public required string ServiceToken {
        get => Parameters["servicetoken"];
        init => Parameters["servicetoken"] = value;
    }

    /// <summary>
    /// Gets or sets a value indicating the status of the service.
    /// </summary>
    public required bool Status {
        get => Parameters["statusdata"] == "Y";
        init => Parameters["statusdata"] = value ? "Y" : "N";
    }

    /// <summary>
    /// Gets or sets the hostname of the requested service.
    /// </summary>
    public required string ServiceHost {
        get => Parameters["svchost"];
        init => Parameters["svchost"] = value;
    }

    /// <summary>
    /// Gets or sets the current date and time for the operation.
    /// </summary>
    public required DateTime DateTime {
        get => DateTime.ParseExact(Parameters["datetime"], "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
        init => Parameters["datetime"] = value.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
    }
}
