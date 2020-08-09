using System.IO;

namespace NUnitTest
{
    internal static class TestAppDomainHelper
    {
        public static readonly string TestDirectory = Path.GetDirectoryName(typeof(TestAppDomainHelper).Assembly.Location);
    }
}
