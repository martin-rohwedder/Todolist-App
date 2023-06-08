using Application.Authentication.Queries.RefreshToken;
using Application.Shared.Errors;
using Application.Shared.Interfaces.Authentication;
using Application.Shared.Interfaces.Persistance;
using Domain.Entities;
using Moq;

namespace UnitTest.Application.Authentication.Queries
{
    [TestFixture]
    internal class RefreshTokenQueryHandlerTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IJwtTokenService> _jwtTokenServiceMock;

        private IList<User> _users;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new();
            _jwtTokenServiceMock = new();

            _users = new List<User>
            {
                new User
                {
                    Username = "J.Doe",
                    RefreshToken = "12345",
                    RefreshTokenExpires = DateTime.UtcNow.AddDays(1)
                }
            };
        }

        [Test]
        public void Handle_Should_ThrowRefreshTokenInvalidException_WhenRefreshTokenIsNotValid()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>()))
                .Returns((string username) => _users
                .Where(user => user.Username.Equals(username)).SingleOrDefault())
                .Verifiable();

            _jwtTokenServiceMock.Setup(js => js.GetRefreshTokenCookie())
                .Returns("1234")
                .Verifiable();

            var query = new RefreshTokenQuery("J.Doe");
            var handler = new RefreshTokenQueryHandler(_userRepositoryMock.Object, _jwtTokenServiceMock.Object);

            // Act & Assert
            Assert.That(() => handler.Handle(query, default), Throws.Exception.TypeOf<RefreshTokenInvalidException>());
            _userRepositoryMock.Verify(ur => ur.GetUserByUsername(It.IsAny<string>()), Times.Once());
            _jwtTokenServiceMock.Verify(js => js.GetRefreshTokenCookie(), Times.Once());
        }

        [Test]
        public void Handle_Should_ThrowRefreshTokenExpiredException_WhenRefreshTokenHasExpired()
        {
            // Arrange
            _users.First().RefreshTokenExpires = DateTime.UtcNow.AddDays(-1);

            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>()))
                .Returns((string username) => _users
                .Where(user => user.Username.Equals(username)).SingleOrDefault())
                .Verifiable();

            _jwtTokenServiceMock.Setup(js => js.GetRefreshTokenCookie())
                .Returns("12345")
                .Verifiable();

            var query = new RefreshTokenQuery("J.Doe");
            var handler = new RefreshTokenQueryHandler(_userRepositoryMock.Object, _jwtTokenServiceMock.Object);

            // Act & Assert
            Assert.That(() => handler.Handle(query, default), Throws.Exception.TypeOf<RefreshTokenExpiredException>());
            _userRepositoryMock.Verify(ur => ur.GetUserByUsername(It.IsAny<string>()), Times.Once());
            _jwtTokenServiceMock.Verify(js => js.GetRefreshTokenCookie(), Times.Once());
        }

        [Test]
        public void Handle_Should_ReturnNewToken_WhenNewRefreshTokenHasBeenCreatedAndSet()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>()))
                .Returns((string username) => _users
                .Where(user => user.Username.Equals(username)).SingleOrDefault());

            _jwtTokenServiceMock.Setup(js => js.GetRefreshTokenCookie())
                .Returns("12345");

            _jwtTokenServiceMock.Setup(js => js.GenerateToken(
                It.IsAny<User>()))
                .Returns("new-token")
                .Verifiable();

            _jwtTokenServiceMock.Setup(js => js.GenerateRefreshToken())
                .Returns(new RefreshToken { Token = "6789" })
                .Verifiable();

            _jwtTokenServiceMock.Setup(js => js.SetRefreshToken(
                It.IsAny<RefreshToken>(), It.IsAny<User>()))
                .Callback((RefreshToken refreshToken, User user) =>
                {
                    //
                })
                .Verifiable();

            var query = new RefreshTokenQuery("J.Doe");
            var handler = new RefreshTokenQueryHandler(_userRepositoryMock.Object, _jwtTokenServiceMock.Object);

            // Act
            var tokenResult = handler.Handle(query, default);

            // Assert
            Assert.That(tokenResult.Result, Is.EqualTo("new-token"));
            _jwtTokenServiceMock.Verify(js => js.GenerateToken(It.IsAny<User>()), Times.Once());
            _jwtTokenServiceMock.Verify(js => js.GenerateRefreshToken(), Times.Once());
            _jwtTokenServiceMock.Verify(js => js.SetRefreshToken(It.IsAny<RefreshToken>(), It.IsAny<User>()), Times.Once());
        }
    }
}
