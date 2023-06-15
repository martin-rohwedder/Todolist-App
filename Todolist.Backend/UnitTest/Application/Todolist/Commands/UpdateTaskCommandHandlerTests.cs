using Application.Shared.Errors;
using Application.Shared.Interfaces.Persistance;
using Application.Todolist.Commands.UpdateTask;
using Domain.Entities;
using Moq;

namespace UnitTest.Application.Todolist.Commands
{
    [TestFixture]
    internal class UpdateTaskCommandHandlerTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ITodoTaskRepository> _todoTaskRepositoryMock;

        private IList<User> _users;
        private IList<TodoTask> _tasks;

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

            _tasks = new List<TodoTask>
            {
                new TodoTask
                {
                    Uuid = new Guid(),
                    Message = "My Test Message",
                    IsCompleted = false,
                    IsArchived = false,
                    DateTimeUpdated = DateTime.UtcNow.AddDays(-1),
                    User = _users[0]
                }
            };

            _users[0].TodoTasks = _tasks;
        }

        [Test]
        public void Handle_Should_ThrowUserNotFoundException_WhenUserIsNotFound()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>()))
                .Returns((string username) => _users.SingleOrDefault(user => user.Username == username))
                .Verifiable();

            var command = new UpdateTaskCommand(new Guid(), "My Test Message Updated", false, false, "J.Doe2");
            var handler = new UpdateTaskCommandHandler(_userRepositoryMock.Object, _todoTaskRepositoryMock.Object);

            // Act & Assert
            Assert.That(() => handler.Handle(command, default), Throws.Exception.TypeOf<UserNotFoundException>());
            _userRepositoryMock.Verify(ur => ur.GetUserByUsername(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void Handle_Should_ThrowTaskNotFoundException_WhenTodoTaskIsNotFound()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>()))
                .Returns((string username) => _users.SingleOrDefault(user => user.Username == username))
                .Verifiable();

            _todoTaskRepositoryMock.Setup(tr => tr.GetTaskByUuid(
                It.IsAny<Guid>()))
                .Returns((Guid uuid) => _tasks.SingleOrDefault(task => task.Uuid.Equals(uuid))!)
                .Verifiable();

            var command = new UpdateTaskCommand(Guid.NewGuid(), "My Test Message Updated", false, false, "J.Doe");
            var handler = new UpdateTaskCommandHandler(_userRepositoryMock.Object, _todoTaskRepositoryMock.Object);


            Assert.That(() => handler.Handle(command, default), Throws.Exception.TypeOf<TaskNotFoundException>());

            _userRepositoryMock.Verify(ur => ur.GetUserByUsername(It.IsAny<string>()), Times.Once());
            _todoTaskRepositoryMock.Verify(tr => tr.GetTaskByUuid(It.IsAny<Guid>()), Times.Once());
        }

        [Test]
        public void Handle_Should_ThrowTodoTaskEmptyMessageException_WhenMessageInCommandRequestIsAnEmptyString()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>()))
                .Returns((string username) => _users.SingleOrDefault(user => user.Username == username))
                .Verifiable();

            _todoTaskRepositoryMock.Setup(tr => tr.GetTaskByUuid(
                It.IsAny<Guid>()))
                .Returns((Guid uuid) => _tasks.SingleOrDefault(task => task.Uuid.Equals(uuid))!)
                .Verifiable();

            var command = new UpdateTaskCommand(new Guid(), string.Empty, false, false, "J.Doe");
            var handler = new UpdateTaskCommandHandler(_userRepositoryMock.Object, _todoTaskRepositoryMock.Object);

            // Act & Assert
            Assert.That(command.Message, Is.Empty);
            Assert.That(() => handler.Handle(command, default), Throws.Exception.TypeOf<TodoTaskEmptyMessageException>());

            _userRepositoryMock.Verify(ur => ur.GetUserByUsername(It.IsAny<string>()), Times.Once());
            _todoTaskRepositoryMock.Verify(tr => tr.GetTaskByUuid(It.IsAny<Guid>()), Times.Once());
        }

        [Test]
        public void Handle_Should_UpdateTaskFoundWithNewData_WhenNoErrorsHasOccured()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>()))
                .Returns((string username) => _users.SingleOrDefault(user => user.Username == username))
                .Verifiable();

            _todoTaskRepositoryMock.Setup(tr => tr.GetTaskByUuid(
                It.IsAny<Guid>()))
                .Returns((Guid uuid) => _tasks.SingleOrDefault(task => task.Uuid.Equals(uuid))!)
                .Verifiable();

            _todoTaskRepositoryMock.Setup(tr => tr.UpdateTasks())
                .Callback(() =>
                {
                    // No code, since this is just saving context changes to database
                    // Method is setup so a call to it can be verified.
                })
                .Verifiable();

            var command = new UpdateTaskCommand(new Guid(), "My Test Message Updated", true, true, "J.Doe");
            var handler = new UpdateTaskCommandHandler(_userRepositoryMock.Object, _todoTaskRepositoryMock.Object);

            // Act
            var oldTask = new TodoTask
            {
                Uuid = _tasks[0].Uuid,
                Message = _tasks[0].Message,
                IsCompleted = _tasks[0].IsCompleted,
                IsArchived = _tasks[0].IsArchived,
                DateTimeUpdated = _tasks[0].DateTimeUpdated
            };

            var updatedTaskResult = handler.Handle(command, default);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(updatedTaskResult.Result.TodoTask.Message, Is.Not.EqualTo(oldTask.Message));
                Assert.That(updatedTaskResult.Result.TodoTask.IsCompleted, Is.True);
                Assert.That(updatedTaskResult.Result.TodoTask.IsArchived, Is.True);
                Assert.That(updatedTaskResult.Result.TodoTask.DateTimeUpdated, Is.Not.EqualTo(oldTask.DateTimeUpdated));
            });

            _userRepositoryMock.Verify(ur => ur.GetUserByUsername(It.IsAny<string>()), Times.Once());
            _todoTaskRepositoryMock.Verify(tr => tr.GetTaskByUuid(It.IsAny<Guid>()), Times.Once());
            _todoTaskRepositoryMock.Verify(tr => tr.UpdateTasks(), Times.Once());
        }
    }
}
