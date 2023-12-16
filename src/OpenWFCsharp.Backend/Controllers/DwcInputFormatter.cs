namespace OpenWFCsharp.Backend.Controllers;

using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using OpenWFCsharp.Backend.Security;

/// <summary>
/// Formatter for data sent from clients to DWC servers.
/// </summary>
/// <remarks>
/// This is a variant of www-form-urlencoded but using custom base64 encoding
/// and without performing URL escaping.
/// </remarks>
public class DwcInputFormatter : TextInputFormatter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DwcInputFormatter"/> class.
    /// </summary>
    public DwcInputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MediaTypeNames.Application.FormUrlEncoded));

        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.ASCII);
    }

    /// <inheritdoc />
    public override bool CanRead(InputFormatterContext context) =>
        typeof(DwcRequest).IsAssignableFrom(context.ModelType);

    /// <inheritdoc />
    public override async Task<InputFormatterResult> ReadRequestBodyAsync(
        InputFormatterContext context,
        Encoding encoding)
    {
        try {
            Dictionary<string, string?> parameters = Decode(context.HttpContext.Request.Form);

            object? model = Activator.CreateInstance(context.ModelType, parameters);
            return await InputFormatterResult.SuccessAsync(model).ConfigureAwait(false);
        } catch {
            return await InputFormatterResult.FailureAsync().ConfigureAwait(false);
        }
    }

    private static Dictionary<string, string?> Decode(IFormCollection content)
    {
        return content.ToDictionary(
            i => i.Key,
            i => NBase64Encoding.Decode(i.Value));
    }
}
