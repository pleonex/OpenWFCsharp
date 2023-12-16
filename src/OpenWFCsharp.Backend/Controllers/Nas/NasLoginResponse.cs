namespace OpenWFCsharp.Backend.Controllers.Nas;

using System.Globalization;

/// <summary>
/// Response from the authentication 'nas' server for the login request.
/// </summary>
public record NasLoginResponse : NasResponse
{
    /// <summary>
    /// Gets or sets the locator. TBD the actual meaning.
    /// </summary>
    public required string Locator {
        get => Parameters["locator"];
        init => Parameters["locator"] = value;
    }

    /// <summary>
    /// Gets or sets the challenge to resolve by the client to continue with
    /// the authentication process.
    /// </summary>
    public required string Challenge {
        get => Parameters["challenge"];
        init => Parameters["challenge"] = value;
    }

    /// <summary>
    /// Gets or sets the token provided by the server for the current user session.
    /// </summary>
    public required string Token {
        get => Parameters["token"];
        init => Parameters["token"] = value;
    }

    /// <summary>
    /// Gets or sets the current date and time for the operation.
    /// </summary>
    public required DateTime DateTime {
        get => DateTime.ParseExact(Parameters["datetime"], "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
        init => Parameters["datetime"] = value.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
    }
}
