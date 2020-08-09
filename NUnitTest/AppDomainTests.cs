using System;
using NUnit.Framework;

namespace NUnitTest
{
    public class AppDomainTests
    {
        [Test]
        public void RunsInDefaultAppDomain()
        {
            Console.WriteLine(AppDomain.CurrentDomain.FriendlyName);
            Assert.True(AppDomain.CurrentDomain.IsDefaultAppDomain());
        }
    }
}
