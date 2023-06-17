using Application.Shared.Errors;
using Application.Shared.Interfaces.Persistance;
using Application.Shared.Interfaces.Utilities;
using Application.UserDetail.Commands.ChangePassword;
using Domain.Entities;
using Moq;

namespace UnitTest.Application.UserDetail.Commands
{
    [TestFixture]
    internal class ChangePasswordCommandHandlerTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IPasswordHashService> _passwordHashServiceMock;

        private IList<User> _users;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new();
            _passwordHashServiceMock = new();

            _users = new List<User>
            {
                new User
                {
                    Username = "J.Doe",
                    Password = "Pwd12345."
                }
            };
        }

        [Test]
        public void Handle_Should_ThrowChangePasswordEmptyPasswordException_WhenNewPasswordProvidedIsEmpty()
        {
            // Arrange
            var command = new ChangePasswordCommand("Pwd12345.", string.Empty, "J.Doe");
            var handler = new ChangePasswordCommandHandler(_userRepositoryMock.Object, _passwordHashServiceMock.Object);

            // Act & Assert
            Assert.That(() => handler.Handle(command, default), Throws.Exception.TypeOf<ChangePasswordEmptyPasswordException>());
        }

        [Test]
        public void Handle_Should_ThrowUserNotFoundException_WhenUserIsNotFound()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>()))
                .Returns((string username) => _users.SingleOrDefault(user => user.Username == username))
                .Verifiable();

            var command = new ChangePasswordCommand("Pwd12345.", "1234", "J.Doe2");
            var handler = new ChangePasswordCommandHandler(_userRepositoryMock.Object, _passwordHashServiceMock.Object);

            // Act & Assert
            Assert.That(() => handler.Handle(command, default), Throws.Exception.TypeOf<UserNotFoundException>());
            _userRepositoryMock.Verify(ur => ur.GetUserByUsername(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void Handle_Should_ThrowChangePasswordMismatchException_WhenOldPasswordProvidedMismatchUsersCurrentPassword()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>()))
                .Returns((string username) => _users.SingleOrDefault(user => user.Username == username))
                .Verifiable();

            _passwordHashServiceMock.Setup(ps => ps.VerifyPassword(
                It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string password, string passwordHash) =>
                BCrypt.Net.BCrypt.Verify(password, passwordHash))
                .Verifiable();

            _users[0].Password = BCrypt.Net.BCrypt.HashPassword("OldPassword");

            var command = new ChangePasswordCommand("Pwd12345.", "1234", "J.Doe");
            var handler = new ChangePasswordCommandHandler(_userRepositoryMock.Object, _passwordHashServiceMock.Object);

            // Act & Assert
            Assert.That(() => handler.Handle(command, default), Throws.Exception.TypeOf<ChangePasswordMismatchException>());

            _userRepositoryMock.Verify(ur => ur.GetUserByUsername(It.IsAny<string>()), Times.Once());
            _passwordHashServiceMock.Verify(ps => ps.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void Handle_Should_ChangeUsersPasswordWithNewPassword_WhenNoErrorsHasOccured()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>()))
                .Returns((string username) => _users.SingleOrDefault(user => user.Username == username))
                .Verifiable();

            _passwordHashServiceMock.Setup(ps => ps.VerifyPassword(
                It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string password, string passwordHash) =>
                BCrypt.Net.BCrypt.Verify(password, passwordHash))
                .Verifiable();

            _userRepositoryMock.Setup(ur => ur.UpdateUser())
                .Callback(() =>
                {
                    // No code
                    // Only made to verify that a call to the method has been made.
                })
                .Verifiable();

            _users[0].Password = BCrypt.Net.BCrypt.HashPassword("Pwd12345.");

            var command = new ChangePasswordCommand("Pwd12345.", "1234", "J.Doe");
            var handler = new ChangePasswordCommandHandler(_userRepositoryMock.Object, _passwordHashServiceMock.Object);

            // Act
            var passwordChangedResult = handler.Handle(command, default);

            // Assert
            Assert.That(passwordChangedResult.Result, Is.True);

            _userRepositoryMock.Verify(ur => ur.GetUserByUsername(It.IsAny<string>()), Times.Once());
            _userRepositoryMock.Verify(ur => ur.UpdateUser(), Times.Once());
            _passwordHashServiceMock.Verify(ps => ps.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }
    }
}
