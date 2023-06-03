using Application.Authentication.Commands.Register;
using Application.Shared.Errors;
using Application.Shared.Interfaces.Authentication;
using Application.Shared.Interfaces.Persistance;
using Application.Shared.Interfaces.Utilities;
using Domain.Entities;
using Moq;

namespace UnitTest.Application.Authentication.Commands
{
    [TestFixture]
    internal class RegisterCommandHandlerTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IJwtTokenService> _jwtTokenServiceMock;
        private Mock<IPasswordHashService> _passwordHashServiceMock;

        private IList<User> _users;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new ();
            _jwtTokenServiceMock = new();
            _passwordHashServiceMock = new();

            _users = new List<User> ();
        }

        [Test]
        public void Handle_Should_ThrowDuplicateUsernameException_WhenUsernameAlreadyExists()
        {
            // Arrange
            _users.Add(new User
            {
                FirstName = "John",
                LastName = "Doe",
                Username = "J.Doe",
                Password = "Pwd12345."
            });

            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>())).Returns((string username) => _users.SingleOrDefault(user => user.Username == username))
                .Verifiable();

            var command = new RegisterCommand("John", "Doe", "J.Doe", "Pwd12345.");
            var handler = new RegisterCommandHandler(_userRepositoryMock.Object, _jwtTokenServiceMock.Object, _passwordHashServiceMock.Object);

            // Act & Assert
            Assert.That(() => handler.Handle(command, default), Throws.Exception.TypeOf<DuplicateUsernameException>());
            _userRepositoryMock.Verify(ur => ur.GetUserByUsername(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Handle_Should_HashPasswordOnNewUser_WhenNoUserExists()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.AddUser(
                It.IsAny<User>())).Callback((User user) =>
                {
                    _users.Add(user);
                });

            _passwordHashServiceMock.Setup(ps => ps.HashPassword(
                It.IsAny<string>())).Returns((string password) => 
                BCrypt.Net.BCrypt.HashPassword(password))
                .Verifiable();

            var command = new RegisterCommand("John", "Doe", "J.Doe", "Pwd12345.");
            var handler = new RegisterCommandHandler(_userRepositoryMock.Object, _jwtTokenServiceMock.Object, _passwordHashServiceMock.Object);

            // Act
            var result = handler.Handle(command, default);

            // Assert
            Assert.That(result.Result.User.Password, Is.Not.EqualTo("Pwd12345."));
            _passwordHashServiceMock.Verify(ps => ps.HashPassword(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Handle_Should_CreateNewUser_WhenNoUserExists()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.AddUser(
                It.IsAny<User>())).Callback((User user) =>
                {
                    _users.Add(user);
                })
                .Verifiable();

            var command = new RegisterCommand("John", "Doe", "J.Doe", "Pwd12345.");
            var handler = new RegisterCommandHandler(_userRepositoryMock.Object, _jwtTokenServiceMock.Object, _passwordHashServiceMock.Object);

            // Act
            var result = handler.Handle(command, default);

            // Assert
            Assert.That(result.Result.User.Username, Is.EqualTo("J.Doe"));
            Assert.That(_users.Count, Is.EqualTo(1));
            _userRepositoryMock.Verify(ur => ur.AddUser(It.IsAny<User>()), Times.Once);
        }
    }
}
