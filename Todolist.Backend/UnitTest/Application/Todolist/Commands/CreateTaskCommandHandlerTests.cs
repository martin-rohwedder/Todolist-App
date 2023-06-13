using Application.Shared.Errors;
using Application.Shared.Interfaces.Persistance;
using Application.Todolist.Commands.CreateTask;
using Domain.Entities;
using Moq;

namespace UnitTest.Application.Todolist.Commands
{
    [TestFixture]
    internal class CreateTaskCommandHandlerTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ITodoTaskRepository> _todoTaskRepositoryMock;

        private IList<User> _users;
        private IList<TodoTask> _todoTasks;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new();
            _todoTaskRepositoryMock = new();

            _users = new List<User>
            {
                new User
                {
                    Username = "J.Doe"
                }
            };
            _todoTasks = new List<TodoTask>();
        }

        [Test]
        public void Handle_Should_ThrowTodoTaskEmptyMessageException_WhenMessageIsEmptyOrNull()
        {
            // Arrange
            var command = new CreateTaskCommand(string.Empty, "J.Doe");
            var handler = new CreateTaskCommandHandler(_userRepositoryMock.Object, _todoTaskRepositoryMock.Object);

            // Act & Assert
            Assert.That(() => handler.Handle(command, default), Throws.Exception.TypeOf<TodoTaskEmptyMessageException>());
        }

        [Test]
        public void Handle_Should_ThrowUserNotFoundException_WhenUserRequestedIsNotFound()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>()))
                .Returns((string username) =>
                _users.SingleOrDefault(user => user.Username == username))
                .Verifiable();

            var command = new CreateTaskCommand("My TodoTask Message", "J.Doe2");
            var handler = new CreateTaskCommandHandler(_userRepositoryMock.Object, _todoTaskRepositoryMock.Object);

            // Act & Assert
            Assert.That(() => handler.Handle(command, default), Throws.Exception.TypeOf<UserNotFoundException>());
            _userRepositoryMock.Verify(ur => ur.GetUserByUsername(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void Handle_Should_CreateNewTodoTask_WhenCreateCommandArgumentsIsValid()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>()))
                .Returns((string username) =>
                _users.SingleOrDefault(user => user.Username == username))
                .Verifiable();

            _todoTaskRepositoryMock.Setup(tr => tr.AddTodoTask(
                It.IsAny<TodoTask>()))
                .Callback((TodoTask todoTask) =>
                {
                    _todoTasks.Add(todoTask);
                })
                .Verifiable();

            var message = "My TodoTask Message";
            var command = new CreateTaskCommand(message, "J.Doe");
            var handler = new CreateTaskCommandHandler(_userRepositoryMock.Object, _todoTaskRepositoryMock.Object);

            // Act
            var todoTaskResult = handler.Handle(command, default);

            // Assert
            Assert.That(todoTaskResult.Result.TodoTask.Message, Is.EqualTo(message));
            Assert.That(todoTaskResult.Result.TodoTask.User, Is.Not.Null);
            _userRepositoryMock.Verify(ur => ur.GetUserByUsername(It.IsAny<string>()), Times.Once());
            _todoTaskRepositoryMock.Verify(tr => tr.AddTodoTask(It.IsAny<TodoTask>()), Times.Once());
        }
    }
}
