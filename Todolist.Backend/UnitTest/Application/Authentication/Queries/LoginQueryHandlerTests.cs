using Application.Authentication.Queries.Login;
using Application.Shared.Errors;
using Application.Shared.Interfaces.Authentication;
using Application.Shared.Interfaces.Persistance;
using Application.Shared.Interfaces.Utilities;
using Domain.Entities;
using Moq;

namespace UnitTest.Application.Authentication.Queries
{
    [TestFixture]
    internal class LoginQueryHandlerTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IJwtTokenService> _jwtTokenServiceMock;
        private Mock<IPasswordHashService> _passwordHashServiceMock;

        private IList<User> _users;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new();
            _jwtTokenServiceMock = new();
            _passwordHashServiceMock = new();

            _users = new List<User>();

        }

        [Test]
        public void Handle_Should_ThrowBadCredentialsException_WhenUsernameIsNotFound()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>())).Returns((string s) => _users.Where(
                x => x.Username == s).SingleOrDefault())
                .Verifiable();

            _users.Add(new User
            {
                FirstName = "John",
                LastName = "Doe",
                Username = "J.Doe",
                Password = "Pwd12345."
            });

            var query = new LoginQuery("Test", "Pwd12345.");
            var handler = new LoginQueryHandler(_userRepositoryMock.Object, _jwtTokenServiceMock.Object, _passwordHashServiceMock.Object);

            // Act & Assert
            Assert.That(() => handler.Handle(query, default), Throws.Exception.TypeOf<BadCredentialsException>());
            _userRepositoryMock.Verify(ur => ur.GetUserByUsername(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Handle_Should_ThrowBadCredentialsException_WhenPasswordIsNotValid()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>())).Returns((string s) => _users.Where(
                x => x.Username == s).SingleOrDefault());

            _passwordHashServiceMock.Setup(ps => ps.HashPassword(
                It.IsAny<string>())).Returns((string s) => 
                BCrypt.Net.BCrypt.HashPassword(s));

            // Set Verifiable since this is the only method we are interested in testing has been called
            _passwordHashServiceMock.Setup(ps => ps.VerifyPassword(
                It.IsAny<string>(), It.IsAny<string>())).Returns((string password, string passwordHash) =>
                BCrypt.Net.BCrypt.Verify(password, passwordHash))
                .Verifiable();

            _users.Add(new User
            {
                FirstName = "John",
                LastName = "Doe",
                Username = "J.Doe",
                Password = _passwordHashServiceMock.Object.HashPassword("Pwd12345.")
            });

            var query = new LoginQuery("J.Doe", "1234");
            var handler = new LoginQueryHandler(_userRepositoryMock.Object, _jwtTokenServiceMock.Object, _passwordHashServiceMock.Object);

            // Act & Assert
            Assert.That(() => handler.Handle(query, default), Throws.Exception.TypeOf<BadCredentialsException>());
            _passwordHashServiceMock.Verify(ps => ps.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Handle_Should_ReturnUserDetails_WhenLoginCredentialsIsValid()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>())).Returns((string s) => _users.Where(
                x => x.Username == s).SingleOrDefault())
                .Verifiable();

            _passwordHashServiceMock.Setup(ps => ps.HashPassword(
                It.IsAny<string>())).Returns((string s) =>
                BCrypt.Net.BCrypt.HashPassword(s));

            _passwordHashServiceMock.Setup(ps => ps.VerifyPassword(
                It.IsAny<string>(), It.IsAny<string>())).Returns((string password, string passwordHash) =>
                BCrypt.Net.BCrypt.Verify(password, passwordHash));

            _users.Add(new User
            {
                FirstName = "John",
                LastName = "Doe",
                Username = "J.Doe",
                Password = _passwordHashServiceMock.Object.HashPassword("Pwd12345.")
            });

            var query = new LoginQuery("J.Doe", "Pwd12345.");
            var handler = new LoginQueryHandler(_userRepositoryMock.Object, _jwtTokenServiceMock.Object, _passwordHashServiceMock.Object);

            // Act
            var result = handler.Handle(query, default);

            // Assert
            Assert.That(result.Result.User.Username, Is.EqualTo("J.Doe"));
            _userRepositoryMock.Verify(ur => ur.GetUserByUsername(It.IsAny<string>()), Times.Once);
        }
    }
}
