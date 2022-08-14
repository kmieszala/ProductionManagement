namespace ProductionManagement.Tests.ExtensionsTests
{
    using NUnit.Framework;
    using ProductionManagement.Common.Extensions;

    public class SHAxxxTests
    {
        [TestCase("")]
        [TestCase("test")]
        [TestCase("!@#$%")]
        [TestCase("123asd")]
        [TestCase("123ASD!@#asd")]
        public void CheckPasswordTest(string value)
        {
            var password = value.ComputeSha256Hash();

            Assert.AreEqual(value.ComputeSha256Hash(), password, "The passwords are the same.");
            Assert.AreNotEqual($"{value}1".ComputeSha256Hash(), password, "Passwords do not match.");
        }
    }
}
