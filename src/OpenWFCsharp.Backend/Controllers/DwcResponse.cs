namespace OpenWFCsharp.Backend.Controllers;

using OpenWFCsharp.Backend.Security;

using System.Text;

/// <summary>
/// Abstract type for responses from DWC servers.
/// </summary>
public abstract record DwcResponse
{
    /// <summary>
    /// Gets the dictionary containing the parameters for the response.
    /// </summary>
    public Dictionary<string, string> Parameters { get; } = [];
}
