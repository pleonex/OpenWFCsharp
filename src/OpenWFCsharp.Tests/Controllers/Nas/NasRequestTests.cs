namespace OpenWFCsharp.Tests.Controllers.Nas;

using System;
using System.Collections.Generic;
using OpenWFCsharp.Backend.Controllers.Nas;

[TestFixture]
public class NasRequestTests
{
    [Test]
    public void DeserializeFieldsFromDictionary()
    {
        var parameters = new Dictionary<string, string?> {
            ["action"] = "login",
            ["passwd"] = "297",
            ["gamecd"] = "B2KJ",
            ["makercd"] = "HF",
        };

        var request = new NasRequest(parameters);

        Assert.Multiple(() => {
            Assert.That(request.Action, Is.EqualTo("login"));
            Assert.That(request.GameCode, Is.EqualTo("B2KJ"));
            Assert.That(request.Password, Is.EqualTo("297"));
            Assert.That(request.MakerCode, Is.EqualTo("HF"));
        });
    }

    [Test]
    public void SerializeFields()
    {
        var request = new ParametrizedNasRequest() {
            Action = "login",
            Password = "297",
            GameCode = "B2KJ",
            MakerCode = "HF",
        };

        Dictionary<string, string?> parameters = request.Parameters;
        Assert.Multiple(() => {
            Assert.That(parameters, Contains.Key("action").WithValue("login"));
            Assert.That(parameters, Contains.Key("passwd").WithValue("297"));
            Assert.That(parameters, Contains.Key("gamecd").WithValue("B2KJ"));
            Assert.That(parameters, Contains.Key("makercd").WithValue("HF"));
        });
    }

    [Test]
    public void DeserializeUserInfoFromDictionary()
    {
        var parameters = new Dictionary<string, string?> {
            ["sdkver"] = "005003",
            ["userid"] = "7881986029631",
            ["bssid"] = "00f01a2b3c4d",
            ["apinfo"] = "02:0000000 - 00",
            ["unitcd"] = "0",
            ["macadr"] = "0016568331f9",
            ["lang"] = "05",
            ["birth"] = "070c",
            ["devtime"] = "140429131005",
            ["devname"] = "pleonex",
        };

        var request = new NasRequest(parameters);
        var userInfo = request.UserInfo;

        Assert.Multiple(() => {
            Assert.That(userInfo.SdkVersion!.Major, Is.EqualTo(5));
            Assert.That(userInfo.SdkVersion.Minor, Is.EqualTo(3));
            Assert.That(userInfo.UserId, Is.EqualTo(7881986029631));
            Assert.That(userInfo.Bssid, Is.EqualTo(0xf01a2b3c4d));
            Assert.That(userInfo.AccessPointInfo, Is.EqualTo("02:0000000 - 00"));
            Assert.That(userInfo.UnitCode, Is.EqualTo(0));
            Assert.That(userInfo.MacAddress, Is.EqualTo(0x16568331f9));
            Assert.That(userInfo.Language, Is.EqualTo(5));
            Assert.That(userInfo.BirthDay.Month, Is.EqualTo(7));
            Assert.That(userInfo.BirthDay.Day, Is.EqualTo(12));
            Assert.That(userInfo.Time, Is.EqualTo(DateTimeOffset.FromUnixTimeMilliseconds(140429131005)));
            Assert.That(userInfo.Name, Is.EqualTo("pleonex"));
        });
    }

    [Test]
    public void SerializeUserInfoFields()
    {
        var request = new ParametrizedNasRequest();

        request.UserInfo.SdkVersion = new Version(5, 3);
        request.UserInfo.UserId = 7881986029631;
        request.UserInfo.Bssid = 0xf01a2b3c4d;
        request.UserInfo.AccessPointInfo = "02:0000000 - 00";
        request.UserInfo.UnitCode = 0;
        request.UserInfo.MacAddress = 0x16568331f9;
        request.UserInfo.Language = 5;
        request.UserInfo.BirthDay = new DateTime(2023, 7, 12, 0, 0, 0, DateTimeKind.Utc);
        request.UserInfo.Time = DateTimeOffset.FromUnixTimeMilliseconds(140429131005);
        request.UserInfo.Name = "pleonex";

        Dictionary<string, string?> parameters = request.Parameters;
        Assert.Multiple(() => {
            Assert.That(parameters, Contains.Key("sdkver").WithValue("005003"));
            Assert.That(parameters, Contains.Key("userid").WithValue("7881986029631"));
            Assert.That(parameters, Contains.Key("bssid").WithValue("00f01a2b3c4d"));
            Assert.That(parameters, Contains.Key("apinfo").WithValue("02:0000000 - 00"));
            Assert.That(parameters, Contains.Key("unitcd").WithValue("0"));
            Assert.That(parameters, Contains.Key("macadr").WithValue("0016568331f9"));
            Assert.That(parameters, Contains.Key("lang").WithValue("05"));
            Assert.That(parameters, Contains.Key("birth").WithValue("070c"));
            Assert.That(parameters, Contains.Key("devtime").WithValue("140429131005"));
            Assert.That(parameters, Contains.Key("devname").WithValue("pleonex"));
        });
    }

    [Test]
    public void DeserializeSvcLocFromDictionary()
    {
        var parameters = new Dictionary<string, string?> {
            ["svc"] = "9000",
        };

        var request = new NasRequest(parameters);

        Assert.That(request.ServiceLocation.Service, Is.EqualTo(9000));
    }

    [Test]
    public void SerializeSvcLocFields()
    {
        var request = new ParametrizedNasRequest();
        request.ServiceLocation.Service = 9000;

        Dictionary<string, string?> parameters = request.Parameters;
        Assert.Multiple(() => {
            Assert.That(parameters, Contains.Key("svc").WithValue("9000"));
        });
    }

    [Test]
    public void DeserializeLoginFromDictionary()
    {
        var parameters = new Dictionary<string, string?> {
            ["gsbrcd"] = "YW2J3q5ue10",
        };

        var request = new NasRequest(parameters);

        Assert.That(request.Login.GsbrCode, Is.EqualTo("YW2J3q5ue10"));
    }

    [Test]
    public void SerializeLoginFields()
    {
        var request = new ParametrizedNasRequest();
        request.Login.GsbrCode = "YW2J3q5ue10";

        Dictionary<string, string?> parameters = request.Parameters;
        Assert.Multiple(() => {
            Assert.That(parameters, Contains.Key("gsbrcd").WithValue("YW2J3q5ue10"));
        });
    }

    private sealed record ParametrizedNasRequest : NasRequest
    {
        public new Dictionary<string, string?> Parameters => base.Parameters;
    }
}
