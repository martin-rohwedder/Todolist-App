using Application.Shared.Errors;
using Application.Shared.Interfaces.Persistance;
using Application.Todolist.Queries.GetTasks;
using Domain.Entities;
using Moq;

namespace UnitTest.Application.Todolist.Queries
{
    [TestFixture]
    internal class GetTasksQueryHandlerTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ITodoTaskRepository> _todoTaskRepositoryMock;

        private IList<User> _users;
        private IList<TodoTask> _tasks;

        [SetUp] public void SetUp()
        {
            _userRepositoryMock = new();
            _todoTaskRepositoryMock = new();

            _tasks = new List<TodoTask>
            {
                new TodoTask
                {
                    Message = "Task 1"
                },
                new TodoTask
                {
                    Message = "Task 2"
                }
            };

            _users = new List<User>
            {
                new User
                {
                    Username = "J.Doe",
                    TodoTasks = _tasks
                }
            };

            foreach (var task in _tasks)
            {
                task.User = _users[0];
            }
        }

        [Test]
        public void Handle_Should_ThrowUserNotFoundException_WhenUserIsNotFound()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>()))
                .Returns((string username) => _users.SingleOrDefault(user => user.Username == username))
                .Verifiable();

            var query = new GetTasksQuery("J.Doe2");
            var handler = new GetTasksQueryHandler(_userRepositoryMock.Object, _todoTaskRepositoryMock.Object);

            // Act & Assert
            Assert.That(() => handler.Handle(query, default), Throws.Exception.TypeOf<UserNotFoundException>());
            _userRepositoryMock.Verify(ur => ur.GetUserByUsername(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void Handle_Should_ReturnListOfTasks_WhenNoErrorsHadOccured()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.GetUserByUsername(
                It.IsAny<string>()))
                .Returns((string username) => _users.SingleOrDefault(user => user.Username == username))
                .Verifiable();

            _todoTaskRepositoryMock.Setup(tr => tr.GetAllTasksFromUser(
                It.IsAny<string>()))
                .Returns((string username) => _tasks.Where(task => task.User!.Username == username).ToList())
                .Verifiable();

            var query = new GetTasksQuery("J.Doe");
            var handler = new GetTasksQueryHandler(_userRepositoryMock.Object, _todoTaskRepositoryMock.Object);

            // Act
            var result = handler.Handle(query, default);

            // Assert
            Assert.That(result.Result.Count, Is.EqualTo(2));
            Assert.That(result.Result[0].Message, Is.EqualTo("Task 1"));
            Assert.That(result.Result[1].Message, Is.EqualTo("Task 2"));
            Assert.That(result.Result[0].User, Is.Not.Null);
            Assert.That(result.Result[1].User, Is.Not.Null);
            _userRepositoryMock.Verify(ur => ur.GetUserByUsername(It.IsAny<string>()), Times.Once());
            _todoTaskRepositoryMock.Verify(tr => tr.GetAllTasksFromUser(It.IsAny<string>()), Times.Once());
        }
    }
}
