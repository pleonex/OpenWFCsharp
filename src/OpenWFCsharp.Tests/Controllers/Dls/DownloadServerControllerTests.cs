namespace OpenWFCsharp.Tests.Controllers.Dls;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using OpenWFCsharp.Backend.Controllers.Dls;
using OpenWFCsharp.Backend.Controllers.Dls.Storage;
using OpenWFCsharp.Backend.Controllers.Nas;

[TestFixture]
public class DownloadServerControllerTests
{
    private DownloadServerController controller;
    private Mock<IContentStorage> storage;

    [SetUp]
    public void TestSetUp()
    {
        var nullLogger = NullLoggerFactory.Instance.CreateLogger<DownloadServerController>();
        storage = new Mock<IContentStorage>();

        controller = new DownloadServerController(nullLogger, storage.Object) {
            ControllerContext = new ControllerContext() {
                HttpContext = new DefaultHttpContext(),
            },
        };
    }

    [Test]
    public void PostRequestReturnsAdditionalHeaders()
    {
        var request = new DlsRequest();

        _ = controller.PostDlsRequest(request);

        Assert.That(controller.Response.Headers, Contains.Key("X-DLS-Host"));
        Assert.That(controller.Response.Headers, Contains.Key("Date"));
        Assert.That(controller.Response.Headers, Contains.Key("Server"));
    }

    [Test]
    public void PostRequestWithoutActionReturnsBadRequest()
    {
        var request = new DlsRequest { Action = null };
        var result = controller.PostDlsRequest(request);

        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public void PostRequestWithUnsupportedActionReturnsNotFound()
    {
        var request = new DlsRequest { Action = "unsupported" };
        var result = controller.PostDlsRequest(request);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public void PostCountRequest()
    {
        storage.Setup(x => x.CountFiles("VPYS", new[] { "es", "", "fr" }))
            .Returns(10)
            .Verifiable(Times.Once);

        var request = new DlsRequest {
            Action = "count",
            GameCode = "VPYS",
            FileAttribute1 = "es",
            FileAttribute2 = null,
            FileAttribute3 = "fr"
        };

        var result = controller.PostDlsRequest(request);

        storage.Verify();
        Assert.That(result, Is.InstanceOf<ContentResult>());

        var content = (result as ContentResult)!;
        Assert.That(content.Content, Is.EqualTo("10"));
    }

    [Test]
    public void PostListRequest()
    {
        const string expected = "f1\t\t\t\t\t24\r\nf2.txt\tes\tfr\tde\t\t48\r\n";
        GameFileInfo[] outputFiles = [
            null!,
            new GameFileInfo("f1", null, 24, Array.Empty<string>()),
            new GameFileInfo("f2.txt", null, 48, ["es", "fr", "de"]),
            null!,
            null!,
        ];

        storage.Setup(x => x.GetList("VPYS", new[] { "es", "", "fr" }))
            .Returns(outputFiles)
            .Verifiable(Times.Once);

        var request = new DlsRequest {
            Action = "list",
            GameCode = "VPYS",
            FileAttribute1 = "es",
            FileAttribute2 = null,
            FileAttribute3 = "fr"
        };
        request.List.Offset = 1;
        request.List.Number = 2;

        var result = controller.PostDlsRequest(request);

        storage.Verify();
        Assert.That(result, Is.InstanceOf<ContentResult>());

        var content = (result as ContentResult)!;
        Assert.That(content.Content, Is.EqualTo(expected));
    }

    [Test]
    public void PostContentsRequest()
    {
        byte[] expectedData = [0xCA, 0xFE];
        using Stream expectedStream = new MemoryStream(expectedData);

        storage.Setup(x => x.GetFile("VPYS", "f2.txt"))
            .Returns(expectedStream)
            .Verifiable(Times.Once);

        var request = new DlsRequest {
            Action = "contents",
            GameCode = "VPYS",
        };
        request.Contents.Name = "f2.txt";

        var result = controller.PostDlsRequest(request);

        storage.Verify();
        Assert.That(result, Is.InstanceOf<FileStreamResult>());

        var content = (result as FileStreamResult)!;
        Assert.Multiple(() => {
            Assert.That(content.FileDownloadName, Is.EqualTo("f2.txt"));
            Assert.That(content.ContentType, Is.EqualTo("application/x-dsdl"));
            Assert.That(content.FileStream, Is.SameAs(expectedStream));
        });
    }

    [Test]
    public void PostRequestWithoutGameCodeReturnsBadRequest()
    {
        var count = controller.PostDlsRequest(new DlsRequest { Action = "count", GameCode = null });
        var list = controller.PostDlsRequest(new DlsRequest { Action = "list", GameCode = null });
        var contents = controller.PostDlsRequest(new DlsRequest { Action = "contents", GameCode = null });

        Assert.Multiple(() => {
            Assert.That(count, Is.InstanceOf<BadRequestObjectResult>());
            Assert.That(list, Is.InstanceOf<BadRequestObjectResult>());
            Assert.That(contents, Is.InstanceOf<BadRequestObjectResult>());
        });
    }

    [Test]
    public void PostContentsRequestWithoutNameReturnsBadRequest()
    {
        var request = new DlsRequest { Action = "contents", GameCode = "VPYS" };

        var contents = controller.PostDlsRequest(request);

        Assert.That(contents, Is.InstanceOf<BadRequestObjectResult>());
    }
}
