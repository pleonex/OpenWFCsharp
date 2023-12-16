namespace OpenWFCsharp.Backend.Security;

using System.Text;

/// <summary>
/// Variant of base-64 text encoding from N.
/// </summary>
public static class NBase64Encoding
{
    private static readonly Encoding TextEncoding = Encoding.ASCII;

    /// <summary>
    /// Decode the NBase64 text into string.
    /// </summary>
    /// <param name="encoded">The encoded text.</param>
    /// <returns>Decoded text.</returns>
    public static string? Decode(string? encoded)
    {
        if (string.IsNullOrEmpty(encoded)) {
            return encoded;
        }

        string standardBase64 = new StringBuilder(encoded)
            .Replace('*', '=')
            .Replace('.', '+')
            .Replace('-', '/')
            .ToString();

        byte[] decodedData = Convert.FromBase64String(standardBase64);

        return TextEncoding.GetString(decodedData);
    }

    /// <summary>
    /// Encoded plain text into NBase64 text.
    /// </summary>
    /// <param name="text">Plain text to encode.</param>
    /// <returns>Encoded text.</returns>
    public static string Encode(string text)
    {
        byte[] data = TextEncoding.GetBytes(text);

        string standardBase64 = Convert.ToBase64String(data);

        return new StringBuilder(standardBase64)
            .Replace('=', '*')
            .Replace('+', '.')
            .Replace('/', '-')
            .ToString();
    }
}
