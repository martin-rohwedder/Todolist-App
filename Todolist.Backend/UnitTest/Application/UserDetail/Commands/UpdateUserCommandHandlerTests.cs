using Application.Shared.Errors;
using Application.Shared.Interfaces.Persistance;
using Application.UserDetail.Commands.UpdateUser;
using Domain.Entities;
using Moq;

namespace UnitTest.Application.UserDetail.Commands
{
    [TestFixture]
    internal class UpdateUserCommandHandlerTests
    {
        private Mock<IUserRepository> _userRepositoryMock;

        private IList<User> _users;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new();

            _users = new List<User>
            {
                new User
                {
                    Username = "J.Doe",
                    FirstName = "John",
                    LastName = "Doe",
                    DateTimeUpdated = DateTime.UtcNow.AddDays(-1)
                }
            };
        }

        [Test]
        public void Handle_Should_ThrowUserNotFoundException_WhenUserIsNotFound()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>()))
                .Returns((string username) => _users.SingleOrDefault(user => user.Username == username))
                .Verifiable();

            var command = new UpdateUserCommand(string.Empty, string.Empty, "J.Doe2");
            var handler = new UpdateUserCommandHandler(_userRepositoryMock.Object);

            // Act & Assert
            Assert.That(() => handler.Handle(command, default), Throws.Exception.TypeOf<UserNotFoundException>());
            _userRepositoryMock.Verify(ur => ur.GetUserByUsername(It.IsAny<string>()), Times.Once());
        }

        [Test]
        [TestCase("", "LastName")]
        [TestCase("FirstName", "")]
        [TestCase("", "")]
        public void Handle_Should_ThrowUserDetailsEmptyException_WhenUserDetailsIsEmpty(string firstName, string lastName)
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>()))
                .Returns((string username) => _users.SingleOrDefault(user => user.Username == username))
                .Verifiable();

            var command = new UpdateUserCommand(firstName, lastName, "J.Doe");
            var handler = new UpdateUserCommandHandler(_userRepositoryMock.Object);

            // Act & Assert
            Assert.That(() => handler.Handle(command, default), Throws.Exception.TypeOf<UserDetailsEmptyException>());
            _userRepositoryMock.Verify(ur => ur.GetUserByUsername(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void Handle_Should_UpdateUserDetails_WhenNoErrorsHadOccured()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>()))
                .Returns((string username) => _users.SingleOrDefault(user => user.Username == username))
                .Verifiable();

            _userRepositoryMock.Setup(ur => ur.UpdateUser())
                .Callback(() =>
                {
                    // No code provided, since this should just save changes to the database
                    // Method added to verify that a call has been made to the method.
                })
                .Verifiable();

            var command = new UpdateUserCommand("FirstName", "LastName", "J.Doe");
            var handler = new UpdateUserCommandHandler(_userRepositoryMock.Object);

            var oldUser = new User
            {
                FirstName = _users[0].FirstName,
                LastName = _users[0].LastName,
                DateTimeUpdated = _users[0].DateTimeUpdated
            };

            // Act
            var userDetailResult = handler.Handle(command, default);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(userDetailResult.Result.User.FirstName, Is.Not.EqualTo(oldUser.FirstName));
                Assert.That(userDetailResult.Result.User.FirstName, Is.EqualTo("FirstName"));
                Assert.That(userDetailResult.Result.User.LastName, Is.Not.EqualTo(oldUser.LastName));
                Assert.That(userDetailResult.Result.User.LastName, Is.EqualTo("LastName"));
                Assert.That(userDetailResult.Result.User.DateTimeUpdated, Is.Not.EqualTo(oldUser.DateTimeUpdated));
            });

            _userRepositoryMock.Verify(ur => ur.GetUserByUsername(It.IsAny<string>()), Times.Once());
            _userRepositoryMock.Verify(ur => ur.UpdateUser(), Times.Once());
        }
    }
}
