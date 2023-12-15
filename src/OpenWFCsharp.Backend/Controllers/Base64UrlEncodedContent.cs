namespace OpenWFCsharp.Backend.Controllers;

using OpenWFCsharp.Backend.Security;

using System.Text;

/// <summary>
/// Content encoding for form URL-encoded with base64 encoded values.
/// </summary>
public static class Base64UrlEncodedContent
{
    /// <summary>
    /// Encode the parameters from the dictionary into a string.
    /// </summary>
    /// <param name="data">The parameters to encode.</param>
    /// <returns>Encoded string.</returns>
    public static string Encode(IReadOnlyDictionary<string, string> data)
    {
        var builder = new StringBuilder();
        foreach (var entry in data) {
            if (builder.Length > 0) {
                builder.Append('&');
            }

            builder.Append(entry.Key)
                .Append('=')
                .Append(NBase64Encoding.Encode(entry.Value));
        }

        return builder.ToString();
    }

    /// <summary>
    /// Decode with NBase64 the values from the parameters.
    /// </summary>
    /// <param name="encoded">The encoded data.</param>
    /// <returns>New dictionary with decoded values.</returns>
    public static Dictionary<string, string> Decode(IReadOnlyDictionary<string, string> encoded)
    {
        return encoded.ToDictionary(
            i => i.Key,
            i => NBase64Encoding.Decode(i.Value));
    }
}
