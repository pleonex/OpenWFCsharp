namespace OpenWFCsharp.Backend.Controllers;

using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using OpenWFCsharp.Backend.Controllers.Nas;

/// <summary>
/// Formatter for data sent from DWC servers to clients.
/// </summary>
/// <remarks>
/// This is a variant of www-form-urlencoded but using custom base64 encoding
/// and without performing URL escaping.
/// </remarks>
public class DwcOutputFormatter : TextOutputFormatter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DwcOutputFormatter"/> class.
    /// </summary>
    public DwcOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MediaTypeNames.Text.Plain));

        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.ASCII);
    }

    /// <inheritdoc />
    public override bool CanWriteResult(OutputFormatterCanWriteContext context)
    {
        return typeof(NasResponse).IsAssignableFrom(context.ObjectType);
    }

    /// <inheritdoc />
    public override async Task WriteResponseBodyAsync(
        OutputFormatterWriteContext context,
        Encoding selectedEncoding)
    {
        if (context.Object is NasResponse nasResponse) {
            string data = nasResponse.Encode();
            await context.HttpContext.Response.WriteAsync(data, selectedEncoding);
        }
    }
}
