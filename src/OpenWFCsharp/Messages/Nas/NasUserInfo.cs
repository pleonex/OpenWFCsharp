namespace OpenWFCsharp.Messages.Nas;

using System.Globalization;

/// <summary>
/// User information provided to the 'nas' server.
/// </summary>
public record NasUserInfo
{
    private readonly Dictionary<string, string?> parameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="NasSvcLocRequest"/> class.
    /// </summary>
    /// <param name="parameters">Request data.</param>
    public NasUserInfo(Dictionary<string, string?> parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        this.parameters = parameters;
    }

    /// <summary>
    /// Gets or sets the version of the WFC SDK.
    /// </summary>
    public Version? SdkVersion {
        get {
            if (!parameters.TryGetValue("sdkver", out string? value) || value is not { Length: 6 }) {
                return null;
            }

            int major = int.Parse(value[..3]);
            int minor = int.Parse(value[3..]);
            return new Version(major, minor);
        }

        set => parameters["sdkver"] = value is null ? "000000" : $"{value.Major:D3}{value.Minor:D3}";
    }

    /// <summary>
    /// Gets or sets the ID of the user.
    /// </summary>
    public long UserId {
        get => parameters.ContainsKey("userid") ? long.Parse(parameters["userid"]!) : 2;
        set => parameters["userid"] = value.ToString();
    }

    /// <summary>
    /// Gets or sets the user Wi-Fi BSSID.
    /// </summary>
    public long Bssid {
        get => parameters.ContainsKey("bssid")
            ? long.Parse(parameters["bssid"]!, NumberStyles.HexNumber)
            : -1;
        set => parameters["bssid"] = value.ToString("x12");
    }

    /// <summary>
    /// Gets or sets the information from the user Wi-Fi access-point.
    /// </summary>
    public string? AccessPointInfo {
        get => parameters.GetValueOrDefault("apinfo");
        set => parameters["apinfo"] = value;
    }

    /// <summary>
    /// Gets or sets the ID of the device.
    /// </summary>
    public int UnitCode {
        get => parameters.ContainsKey("unitcd") ? int.Parse(parameters["unitcd"]!) : -1;
        set => parameters["unitcd"] = value.ToString();
    }

    /// <summary>
    /// Gets or sets the device MAC address.
    /// </summary>
    public long MacAddress {
        get => parameters.ContainsKey("macadr")
            ? long.Parse(parameters["macadr"]!, NumberStyles.HexNumber)
            : -1;
        set => parameters["macadr"] = value.ToString("x12");
    }

    /// <summary>
    /// Gets or sets the user configured language.
    /// </summary>
    public int Language {
        get => parameters.ContainsKey("lang") ? int.Parse(parameters["lang"]!) : -1;
        set => parameters["lang"] = value.ToString("D2");
    }

    /// <summary>
    /// Gets or sets the user birthday.
    /// </summary>
    public DateTime BirthDay {
        get {
            if (!parameters.TryGetValue("birth", out string? value) || value is not { Length: 4 }) {
                return DateTime.MinValue;
            }

            int month = int.Parse(value[..2], NumberStyles.HexNumber);
            int day = int.Parse(value[2..], NumberStyles.HexNumber);
            return new DateTime(DateTime.Now.Year, month, day, 0, 0, 0, DateTimeKind.Utc);
        }

        set => parameters["birth"] = $"{value.Month:x2}{value.Day:x2}";
    }

    /// <summary>
    /// Gets or sets the user nickname.
    /// </summary>
    public string? Name {
        get => parameters.GetValueOrDefault("devname");
        set => parameters["devname"] = value;
    }


    /// <summary>
    /// Gets the current device time.
    /// </summary>
    public DateTimeOffset Time {
        get => parameters.ContainsKey("devtime")
            ? DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(parameters["devtime"]!))
            : DateTimeOffset.MinValue;
        set => parameters["devtime"] = value.ToUnixTimeMilliseconds().ToString();
    }
}
