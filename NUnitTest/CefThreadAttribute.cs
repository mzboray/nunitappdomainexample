using System;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnitTest;
using ScreenshotLib;

[assembly: CefThreadActions]

namespace NUnitTest
{
    public class CefThreadActionsAttribute : TestActionAttribute
    {
        public override ActionTargets Targets => ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            CefThreadHolder.Instance.Start();
        }

        public override void AfterTest(ITest test)
        {
            CefThreadHolder.Instance.Stop();
        }
    }
}
