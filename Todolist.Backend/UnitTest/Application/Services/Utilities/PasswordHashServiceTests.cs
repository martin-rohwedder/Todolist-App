using Application.Shared.Services.Utilities;

namespace UnitTest.Application.Services.Utilities
{
    [TestFixture]
    internal class PasswordHashServiceTests
    {
        private PasswordHashService _passwordHashService;

        [SetUp]
        public void SetUp()
        {
            _passwordHashService = new PasswordHashService();
        }

        [Test]
        public void HashPassword_Should_ReturnAHashedPassword_WhenGivenANonHashedPassword()
        {
            // Act
            var result = _passwordHashService.HashPassword("Pwd12345.");

            // Assert
            Assert.That(result, Is.Not.EqualTo("Pwd12345."));
        }

        [Test]
        public void VerifyPassword_Should_ReturnTrue_WhenGivenPasswordIsVerified()
        {
            // Act
            var passwordHash = _passwordHashService.HashPassword("Pwd12345.");
            var result = _passwordHashService.VerifyPassword("Pwd12345.", passwordHash);

            // Assert
            Assert.That(result, Is.True);
        }
    }
}
