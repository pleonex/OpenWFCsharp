namespace OpenWFCsharp.Tests.Controllers.Nas;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using OpenWFCsharp.Messages.Nas;
using OpenWFCsharp.Nas;
using OpenWFCsharp.Nas.Controllers;

[TestFixture]
public class NAuthenticationServerControllerTests
{
    private NAuthenticationServerController controller;
    private Mock<IOptions<NAuthenticationServerOptions>> options;

    [SetUp]
    public void TestSetUp()
    {
        var nullLogger = NullLoggerFactory.Instance.CreateLogger< NAuthenticationServerController>();
        options = new Mock<IOptions<NAuthenticationServerOptions>>();

        controller = new NAuthenticationServerController(nullLogger, options.Object) {
            ControllerContext = new ControllerContext() {
                HttpContext = new DefaultHttpContext(),
            },
        };
    }

    [Test]
    public void PostRequestReturnsAdditionalHeaders()
    {
        var request = new NasRequest { Action = "login" };
        controller.PostNasRequest(request);

        Assert.That(controller.Response.Headers, Contains.Key("NODE"));
        Assert.That(controller.Response.Headers, Contains.Key("Date"));
        Assert.That(controller.Response.Headers, Contains.Key("Server"));
    }

    [Test]
    public void PostRequestWithoutActionReturnsBadRequest()
    {
        var request = new NasRequest { Action = null };
        var result = controller.PostNasRequest(request);

        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public void PostRequestWithUnsupportedActionReturnsNotFound()
    {
        var request = new NasRequest { Action = "unsupported" };
        var result = controller.PostNasRequest(request);

        Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public void PostAcctCreateRequestSucceeds()
    {
        var request = new NasRequest { Action = "acctcreate" };
        request.UserInfo.UserId = 42;

        var result = controller.PostNasRequest(request);
        var response = (result.Value as NasAcctCreateResponse)!;

        Assert.That(result.Result, Is.Null);
        Assert.That(result.Value, Is.InstanceOf<NasAcctCreateResponse>());
        Assert.Multiple(() => {
            Assert.That(response.ReturnCode, Is.EqualTo((int)NasReturnCodes.AccountCreated));
            Assert.That(response.IsSuccessful, Is.True);
            Assert.That(response.UserId, Is.EqualTo(42.ToString().PadLeft(13, '0')));
        });
    }

    [Test]
    public void PostAcctCreateRequestWithoutUserIdReturnsUser2()
    {
        var request = new NasRequest { Action = "acctcreate" };

        var result = controller.PostNasRequest(request);
        var response = (result.Value as NasAcctCreateResponse)!;

        Assert.That(result.Result, Is.Null);
        Assert.That(result.Value, Is.InstanceOf<NasAcctCreateResponse>());
        Assert.Multiple(() => {
            Assert.That(response.ReturnCode, Is.EqualTo((int)NasReturnCodes.AccountCreated));
            Assert.That(response.IsSuccessful, Is.True);
            Assert.That(response.UserId, Is.EqualTo(2.ToString().PadLeft(13, '0')));
        });
    }

    [Test]
    public void PostLoginRequestSucceeds()
    {
        var request = new NasRequest { Action = "login" };

        var result = controller.PostNasRequest(request);
        var loginRespose = (result.Value as NasLoginResponse)!;

        Assert.That(result.Result, Is.Null);
        Assert.That(result.Value, Is.InstanceOf<NasLoginResponse>());
        Assert.Multiple(() => {
            Assert.That(loginRespose.ReturnCode, Is.EqualTo((int)NasReturnCodes.LoginSuccess));
            Assert.That(loginRespose.IsSuccessful, Is.True);
            Assert.That(loginRespose.Token, Is.Not.Empty);
            Assert.That(loginRespose.Challenge, Is.Not.Empty);
            Assert.That(loginRespose.DateTime.Date, Is.EqualTo(DateTime.UtcNow.Date));
            Assert.That(loginRespose.Locator, Is.Not.Empty);
        });
    }

    [Test]
    public void PostSvcLoc900RequestSucceeds()
    {
        var serverOpts = new NAuthenticationServerOptions();
        serverOpts.Services[9000] = "myService";
        options.SetupGet(x => x.Value).Returns(serverOpts);

        var request = new NasRequest { Action = "SVCLOC" };
        request.ServiceLocation.Service = 9000;

        var result = controller.PostNasRequest(request);
        var loginRespose = (result.Value as NasSvcLocResponse)!;

        Assert.That(result.Result, Is.Null);
        Assert.That(result.Value, Is.InstanceOf<NasSvcLocResponse>());
        Assert.Multiple(() => {
            Assert.That(loginRespose.ReturnCode, Is.EqualTo((int)NasReturnCodes.Redirect));
            Assert.That(loginRespose.IsSuccessful, Is.True);
            Assert.That(loginRespose.Status, Is.True);
            Assert.That(loginRespose.ServiceToken, Is.Not.Empty);
            Assert.That(loginRespose.ServiceHost, Is.EqualTo("myService"));
            Assert.That(loginRespose.DateTime.Date, Is.EqualTo(DateTime.UtcNow.Date));
        });
    }

    [Test]
    public void PostSvcLocRequestWithMissingServiceReturnsBadRequest()
    {
        var request = new NasRequest { Action = "SVCLOC" };
        request.ServiceLocation.Service = -1;

        var result = controller.PostNasRequest(request);

        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public void PostSvcLocRequestWithUnsupportedServiceReturnsNotFound()
    {
        var serverOpts = new NAuthenticationServerOptions();
        serverOpts.Services[9000] = "myService";
        options.SetupGet(x => x.Value).Returns(serverOpts);

        var request = new NasRequest { Action = "SVCLOC" };
        request.ServiceLocation.Service = 8000;

        var result = controller.PostNasRequest(request);

        Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
    }
}
