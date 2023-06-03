using Application.Shared.Interfaces.Utilities;
using Moq;

namespace UnitTest.Application.Services.Utilities
{
    [TestFixture]
    internal class PasswordHashServiceTests
    {
        private Mock<IPasswordHashService> _passwordHashServiceMock;

        [SetUp]
        public void SetUp()
        {
            _passwordHashServiceMock = new();
        }

        [Test]
        public void HashPassword_Should_ReturnAHashedPassword_WhenGivenANonHashedPassword()
        {
            // Arrange
            _passwordHashServiceMock.Setup(ps => ps.HashPassword(
                It.IsAny<string>())).Returns((string passwordHash) => 
                BCrypt.Net.BCrypt.HashPassword(passwordHash))
                .Verifiable();

            // Act
            var result = _passwordHashServiceMock.Object.HashPassword("Pwd12345.");

            // Assert
            Assert.That(result, Is.Not.EqualTo("Pwd12345."));
            _passwordHashServiceMock.Verify(ps => ps.HashPassword(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void VerifyPassword_Should_ReturnTrue_WhenGivenPasswordIsVerified()
        {
            // Arrange
            _passwordHashServiceMock.Setup(ps => ps.HashPassword(
                It.IsAny<string>())).Returns((string passwordHash) =>
                BCrypt.Net.BCrypt.HashPassword(passwordHash))
                .Verifiable();

            _passwordHashServiceMock.Setup(ps => ps.VerifyPassword(
                It.IsAny<string>(), It.IsAny<string>())).Returns((string password, string passwordHash) =>
                BCrypt.Net.BCrypt.Verify(password, passwordHash))
                .Verifiable();

            // Act
            var passwordHash = _passwordHashServiceMock.Object.HashPassword("Pwd12345.");
            var result = _passwordHashServiceMock.Object.VerifyPassword("Pwd12345.", passwordHash);

            // Assert
            Assert.That(result, Is.True);
            _passwordHashServiceMock.Verify(ps => ps.HashPassword(It.IsAny<string>()), Times.Once);
            _passwordHashServiceMock.Verify(ps => ps.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
