using System.IO;

namespace XUnitTest
{
    internal static class TestAppDomainHelper
    {
        public static readonly string TestDirectory = Path.GetDirectoryName(typeof(TestAppDomainHelper).Assembly.Location);
    }
}
