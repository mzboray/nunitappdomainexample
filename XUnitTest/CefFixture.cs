using System;
using System.Reflection;
using ScreenshotLib;

namespace XUnitTest
{
    public class CefFixture : IDisposable
    {
        public CefFixture()
        {
            CefThreadHolder.Instance.Start();
        }

        public void Dispose()
        {
            CefThreadHolder.Instance.Stop(true);
        }
    }
}
