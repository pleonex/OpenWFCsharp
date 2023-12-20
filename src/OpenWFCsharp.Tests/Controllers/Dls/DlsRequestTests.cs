namespace OpenWFCsharp.Tests.Controllers.Dls;

using System.Collections.Generic;
using OpenWFCsharp.Messages.Dls;

[TestFixture]
public class DlsRequestTests
{
    [Test]
    public void DeserializeCommonFieldsFromDictionary()
    {
        var parameters = new Dictionary<string, string?> {
            ["action"] = "count",
            ["gamecd"] = "VPYP",
            ["rhgamecd"] = "VPYT",
            ["passwd"] = "kFEapQWCMdPJmK7H",
            ["token"] = "IATU99IyDMj3ZDuRxiQfVY1NzazfKV+uBS7DZ3kDL1gxI0ZKzsBBDJ2aVeClnOtf9RAy0TZkNUzrcSW/COHLxw==",
            ["userid"] = "7881986029631",
            ["macadr"] = "0016568331f9",
            ["apinfo"] = "02:0000000-00",
            ["attr1"] = "RgBSACAARABhAHIAawByAGEAaQAgAE0AaQBzAHMAaQBvAG4A",
            ["attr2"] = "es",
            ["attr3"] = "fr",
        };

        var request = new DlsRequest(parameters);

        Assert.Multiple(() => {
            Assert.That(request.Action, Is.EqualTo("count"));
            Assert.That(request.GameCode, Is.EqualTo("VPYP"));
            Assert.That(request.RomHeaderGameCode, Is.EqualTo("VPYT"));
            Assert.That(request.Password, Is.EqualTo("kFEapQWCMdPJmK7H"));
            Assert.That(request.Token, Is.EqualTo("IATU99IyDMj3ZDuRxiQfVY1NzazfKV+uBS7DZ3kDL1gxI0ZKzsBBDJ2aVeClnOtf9RAy0TZkNUzrcSW/COHLxw=="));
            Assert.That(request.UserId, Is.EqualTo(7881986029631));
            Assert.That(request.MacAddress, Is.EqualTo(0x16568331f9));
            Assert.That(request.AccessPointInfo, Is.EqualTo("02:0000000-00"));
            Assert.That(request.FileAttribute1, Is.EqualTo("RgBSACAARABhAHIAawByAGEAaQAgAE0AaQBzAHMAaQBvAG4A"));
            Assert.That(request.FileAttribute2, Is.EqualTo("es"));
            Assert.That(request.FileAttribute3, Is.EqualTo("fr"));
        });
    }

    [Test]
    public void SerializeCommonFields()
    {
        var request = new ParametrizedDlsRequest {
            Action = "count",
            GameCode = "VPYP",
            RomHeaderGameCode = "VPYT",
            Password = "kFEapQWCMdPJmK7H",
            Token = "IATU99IyDMj3ZDuRxiQfVY1NzazfKV+uBS7DZ3kDL1gxI0ZKzsBBDJ2aVeClnOtf9RAy0TZkNUzrcSW/COHLxw==",
            UserId = 7881986029631,
            MacAddress = 0x16568331f9,
            AccessPointInfo = "02:0000000-00",
            FileAttribute1 = "RgBSACAARABhAHIAawByAGEAaQAgAE0AaQBzAHMAaQBvAG4A",
            FileAttribute2 = "es",
            FileAttribute3 = "fr",
        };

        Dictionary<string, string?> parameters = request.Parameters;
        Assert.Multiple(() => {
            Assert.That(parameters, Contains.Key("action").WithValue("count"));
            Assert.That(parameters, Contains.Key("gamecd").WithValue("VPYP"));
            Assert.That(parameters, Contains.Key("rhgamecd").WithValue("VPYT"));
            Assert.That(parameters, Contains.Key("passwd").WithValue("kFEapQWCMdPJmK7H"));
            Assert.That(parameters, Contains.Key("token").WithValue("IATU99IyDMj3ZDuRxiQfVY1NzazfKV+uBS7DZ3kDL1gxI0ZKzsBBDJ2aVeClnOtf9RAy0TZkNUzrcSW/COHLxw=="));
            Assert.That(parameters, Contains.Key("userid").WithValue("7881986029631"));
            Assert.That(parameters, Contains.Key("macadr").WithValue("0016568331f9"));
            Assert.That(parameters, Contains.Key("apinfo").WithValue("02:0000000-00"));
            Assert.That(parameters, Contains.Key("attr1").WithValue("RgBSACAARABhAHIAawByAGEAaQAgAE0AaQBzAHMAaQBvAG4A"));
            Assert.That(parameters, Contains.Key("attr2").WithValue("es"));
            Assert.That(parameters, Contains.Key("attr3").WithValue("fr"));
        });
    }

    [Test]
    public void DeserializeListFieldsFromDictionary()
    {
        var parameters = new Dictionary<string, string?> {
            ["offset"] = "10",
            ["num"] = "12",
        };

        var request = new DlsRequest(parameters);

        Assert.Multiple(() => {
            Assert.That(request.List.Offset, Is.EqualTo(10));
            Assert.That(request.List.Number, Is.EqualTo(12));
        });
    }

    [Test]
    public void SerializeListFields()
    {
        var request = new ParametrizedDlsRequest();
        request.List.Offset = 10;
        request.List.Number = 12;

        Dictionary<string, string?> parameters = request.Parameters;
        Assert.Multiple(() => {
            Assert.That(parameters, Contains.Key("offset").WithValue("10"));
            Assert.That(parameters, Contains.Key("num").WithValue("12"));
        });
    }

    [Test]
    public void DeserializeContentsFieldsFromDictionary()
    {
        var parameters = new Dictionary<string, string?> {
            ["contents"] = "PSL_DLC_SECURE.csv",
        };

        var request = new DlsRequest(parameters);

        Assert.That(request.Contents.Name, Is.EqualTo("PSL_DLC_SECURE.csv"));
    }

    [Test]
    public void SerializeContentsFields()
    {
        var request = new ParametrizedDlsRequest();
        request.Contents.Name = "PSL_DLC_SECURE.csv";

        Dictionary<string, string?> parameters = request.Parameters;
        Assert.Multiple(() => {
            Assert.That(parameters, Contains.Key("contents").WithValue("PSL_DLC_SECURE.csv"));
        });
    }

    private sealed record ParametrizedDlsRequest : DlsRequest
    {
        public new Dictionary<string, string?> Parameters => base.Parameters;
    }
}
