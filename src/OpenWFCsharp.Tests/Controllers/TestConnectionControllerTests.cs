namespace OpenWFCsharp.Tests.Controllers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenWFCsharp.Nas.Controllers;

[TestFixture]
public class TestConnectionControllerTests
{
    private TestConnectionController controller;

    [SetUp]
    public void TestSetUp()
    {
        controller = new TestConnectionController {
            ControllerContext = new ControllerContext() {
                HttpContext = new DefaultHttpContext(),
            },
        };
    }

    [Test]
    public void PingReturns200()
    {
        var result = controller.Ping();

        Assert.That(result, Is.InstanceOf<ContentResult>());
    }

    [Test]
    public void PingSetsAdditionalHeaders()
    {
        var result = controller.Ping();
        var contentResult = result as ContentResult;

        Assert.That(result, Is.InstanceOf<ContentResult>());
        Assert.That(contentResult!.ContentType, Is.EqualTo("text/html"));
        Assert.That(controller.Response.Headers, Contains.Key("X-Organization"));
        Assert.That(controller.Response.Headers, Contains.Key("Server"));
    }
}
