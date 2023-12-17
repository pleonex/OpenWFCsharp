namespace OpenWFCsharp.Tests.Controllers.Nas;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using OpenWFCsharp.Backend.Controllers.Nas;

[TestFixture]
public class NAuthenticationServerControllerTests
{
    private NAuthenticationServerController controller;

    [SetUp]
    public void TestSetUp()
    {
        var nullLogger = NullLoggerFactory.Instance.CreateLogger< NAuthenticationServerController>();
        controller = new NAuthenticationServerController(nullLogger) {
            ControllerContext = new ControllerContext() {
                HttpContext = new DefaultHttpContext(),
            },
        };
    }

    [Test]
    public void ConstructorWithInvalidArguments()
    {
        Assert.That(
            () => new NAuthenticationServerController(null!),
            Throws.ArgumentNullException);
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
            Assert.That(loginRespose.DateTime.Date, Is.EqualTo(DateTime.Today));
            Assert.That(loginRespose.Locator, Is.Not.Empty);
        });
    }

    [Test]
    public void PostSvcLoc900RequestSucceeds()
    {
        controller.Request.Headers.Append("Host", "testhost");
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
            Assert.That(loginRespose.ServiceHost, Is.Not.Empty);
            Assert.That(loginRespose.DateTime.Date, Is.EqualTo(DateTime.Today));
        });
    }

    [Test]
    public void PostSvcLocRequestWithMissingServiceReturnsBadRequest()
    {
        controller.Request.Headers.Append("Host", "testhost");
        var request = new NasRequest { Action = "SVCLOC" };
        request.ServiceLocation.Service = -1;

        var result = controller.PostNasRequest(request);

        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public void PostSvcLocRequestWithUnsupportedServiceReturnsNotFound()
    {
        controller.Request.Headers.Append("Host", "testhost");
        var request = new NasRequest { Action = "SVCLOC" };
        request.ServiceLocation.Service = 8000;

        var result = controller.PostNasRequest(request);

        Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
    }
}
