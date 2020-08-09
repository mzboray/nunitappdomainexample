using System;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using ScreenshotLib;

namespace NUnitTest
{
    class ScreenshotTakerTest
    {
        [Test]
        public async Task TakeScreenshotOfGithubPage()
        {
            using (var browser = new ScreenshotTaker())
            {
                using (var bitmap = await browser.LoadAndTakeScreenshot("http://github.com", 100))
                {
                    Assert.NotNull(bitmap);
                    string outputFile = Path.Combine(TestAppDomainHelper.TestDirectory, "github.png");
                    bitmap.Save(outputFile, ImageFormat.Png);
                    TestContext.AddTestAttachment(outputFile);
                }
            }
        }

        [Test]
        public async Task TakeScreenshotOfNunitDocsPage()
        {
            using (var browser = new ScreenshotTaker())
            {
                using (var bitmap = await browser.LoadAndTakeScreenshot("https://docs.nunit.org/", 100))
                {
                    Assert.NotNull(bitmap);
                    string outputFile = Path.Combine(TestAppDomainHelper.TestDirectory, "nunitdocs.png");
                    bitmap.Save(outputFile, ImageFormat.Png);
                    TestContext.AddTestAttachment(outputFile);
                }
            }
        }
    }
}
