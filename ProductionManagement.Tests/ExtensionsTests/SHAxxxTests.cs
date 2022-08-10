namespace ProductionManagement.Tests.ExtensionsTests
{
    using NUnit.Framework;
    using ProductionManagement.Common.Extensions;

    public class SHAxxxTests
    {
        [Test]
        public void CheckPasswordTest()
        {
            var password = "test123".ToSHA1String();

            Assert.AreEqual("test123".ToSHA1String(), password, "The passwords are the same.");
            Assert.AreNotEqual("test124".ToSHA1String(), password, "Passwords do not match.");
        }
    }
}
