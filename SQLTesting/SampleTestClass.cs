using NUnit.Framework;

namespace SQLTesting
{
    [TestFixture]
    public class SampleTestClass
    {
        [Test, Category("Sample Tests")]
        public void SampleTest()
        {
            int a = 1;
            Assert.That(a, Is.EqualTo(1));
        }
    }
}