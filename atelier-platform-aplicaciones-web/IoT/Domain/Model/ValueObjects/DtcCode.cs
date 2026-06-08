using System.Text.RegularExpressions;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;

public record DtcCode
{
    public string Value { get; init; }

    private static readonly Regex DtcRegex = new(@"^[PBCU][0-9]{4}$", RegexOptions.Compiled);

    public DtcCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("iot.error.dtcCode.required");
        }
        if (!DtcRegex.IsMatch(value))
        {
            throw new ArgumentException("iot.error.dtcCode.invalidFormat");
        }
        Value = value;
    }
}
