using System;
using Xunit;
using Xunit.Abstractions;

namespace XUnitTest
{
    public class AppDomainTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public AppDomainTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public void RunsInDefaultAppDomain()
        {
            _outputHelper.WriteLine(AppDomain.CurrentDomain.FriendlyName);
            Assert.True(AppDomain.CurrentDomain.IsDefaultAppDomain());
        }
    }
}
