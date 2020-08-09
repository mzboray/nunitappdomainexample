using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using ScreenshotLib;
using Xunit;
using Xunit.Abstractions;

namespace XUnitTest
{
    public class ScreenshotTakerTest : IClassFixture<CefFixture>
    {
        private readonly ITestOutputHelper _output;
        private readonly CefFixture _fixture;

        public ScreenshotTakerTest(CefFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
        }

        [Fact]
        public async Task TakeScreenshotOfGithubPage()
        {
            using (var browser = new ScreenshotTaker())
            {
                using (var bitmap = await browser.LoadAndTakeScreenshot("http://github.com", 100))
                {
                    Assert.NotNull(bitmap);
                    string outputFile = Path.Combine(TestAppDomainHelper.TestDirectory, "github.png");
                    bitmap.Save(outputFile, ImageFormat.Png);
                    _output.WriteLine($"Wrote file to {outputFile}");
                }
            }
        }

        [Fact]
        public async Task TakeScreenshotOfNunitDocsPage()
        {
            using (var browser = new ScreenshotTaker())
            {
                using (var bitmap = await browser.LoadAndTakeScreenshot("https://docs.nunit.org/", 100))
                {
                    Assert.NotNull(bitmap);
                    string outputFile = Path.Combine(TestAppDomainHelper.TestDirectory, "nunitdocs.png");
                    bitmap.Save(outputFile, ImageFormat.Png);
                    _output.WriteLine($"Wrote file to {outputFile}");
                }
            }
        }
    }
}
