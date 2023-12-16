namespace OpenWFCsharp.Backend.Controllers.Nas;

/// <summary>
/// Codes returned by the 'nas' server on valid requests.
/// </summary>
public enum NasReturnCodes
{
    /// <summary>
    /// Login was successful.
    /// </summary>
    LoginSuccess = 1,

    /// <summary>
    /// Account creation successful.
    /// </summary>
    AccountCreated = 2,

    /// <summary>
    /// Service found and redirection provided.
    /// </summary>
    Redirect = 7,

    /// <summary>
    /// Account creation denied. User is banned.
    /// </summary>
    AccountCreateDeniedUserBanned = 3913,

    /// <summary>
    /// Login denied. User is banned.
    /// </summary>
    LoginDeniedUserBanned = 3914,

    /// <summary>
    /// Request denied. Device is pending for admin approval.
    /// </summary>
    DevicePendingApproval = 3921,

    /// <summary>
    /// Request denied. Device is not in the allowed list.
    /// </summary>
    DeviceNotInAllowList = 3888,
}

/// <summary>
/// Extension methods for <see cref="NasReturnCodes"/>.
/// </summary>
public static class NasReturnCodesExtensions
{
    /// <summary>
    /// Gets a value indicating if the return code is for a successful operation.
    /// </summary>
    /// <param name="code">Code to evaluate.</param>
    /// <returns>Value indicating if the operation was successful.</returns>
    public static bool IsSucessful(this NasReturnCodes code) =>
        (int)code is < 10;
}
