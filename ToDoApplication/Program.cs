using ToDoApplication.Repository;
using ToDoApplication.Service;
using ToDoApplication.View;

namespace ToDoApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var userRepository = new UserRepository("Users.json");
                var taskRepository = new TasksRepository("Tasks.json");
                var userService = new UserService(userRepository);
                var taskService = new TaskService(taskRepository);
                var viewer = new ToDoViewer(userService, taskService);
                viewer.Login();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
