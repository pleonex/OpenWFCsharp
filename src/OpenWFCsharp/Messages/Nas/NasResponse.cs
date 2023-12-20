namespace OpenWFCsharp.Messages.Nas;

/// <summary>
/// Abstract type for responses from the authorization 'nas' server.
/// </summary>
public abstract record class NasResponse : DwcResponse
{
    /// <summary>
    /// Gets a value indicating if the operation was successful.
    /// </summary>
    public bool IsSuccessful => ((NasReturnCodes)ReturnCode).IsSucessful();

    /// <summary>
    /// Gets or sets the return code for the operation.
    /// </summary>
    public required int ReturnCode {
        get => int.Parse(Parameters["returncd"]);
        init {
            Parameters["returncd"] = value.ToString("D3");
            Parameters["retry"] = ((NasReturnCodes)value).IsSucessful() ? "0" : "1";
        }
    }
}
