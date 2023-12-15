namespace OpenWFCsharp.Backend.Security;

/// <summary>
/// Manage the tokens for authentication.
/// </summary>
public static class TokenService
{
    /// <summary>
    /// Generate a new random challenge token.
    /// </summary>
    /// <returns>Challenge token.</returns>
    public static string GenerateChallenge()
    {
        // TODO: is it really 6 bytes encoded as base64? regular base64?
        Span<byte> tokenData = stackalloc byte[6];
        Random.Shared.NextBytes(tokenData);
        return Convert.ToBase64String(tokenData);
    }

    /// <summary>
    /// Generate a new random authentication token.
    /// </summary>
    /// <returns>New token</returns>
    public static string GenerateAuthToken()
    {
        // TODO: is the token really 64 bytes random as regular base64?
        Span<byte> tokenData = stackalloc byte[64];
        Random.Shared.NextBytes(tokenData);

        return "NDS" + Convert.ToBase64String(tokenData);
    }
}
