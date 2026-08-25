using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.Helper;
using ToDoApplication.Model;
using ToDoApplication.Model.Enums;
using ToDoApplication.Service;

namespace ToDoApplication.View
{
    public class ToDoViewer
    {
        private readonly UserService _userService;
        private readonly TaskService _taskService;

        public ToDoViewer(UserService userService, TaskService taskService)
        {
            this._userService = userService;
            this._taskService = taskService;
        }

        public void Login()
        {
            bool isLoginSuccess = false;
            while (!isLoginSuccess)
            {
                Console.WriteLine("ToDo application\nLogin Page\n");
                string inputUserId = this.GetInputWithAttempts("Enter User Id:", input => Guid.TryParse(input, out Guid _), "Invalid UserID");
                if (inputUserId.Equals(string.Empty))
                {
                    isLoginSuccess = false;
                    break;
                }

                if (Guid.TryParse(inputUserId, out Guid userID) && this._userService.DoesUserExists(userID))
                {
                    string loginPassword = this.GetInputWithAttempts("Enter login password:", Validator.IsPasswordValid, "Invalid Password");
                    if (!this._userService.IsPasswordMatch(loginPassword, userID))
                    {
                        Console.WriteLine("Password does not match!Try again with correct password");
                        isLoginSuccess = false;
                        break;
                    }

                    UserAuthenticatorService.SetCurrentUser(userID);
                    this.DisplayDashboard();
                    isLoginSuccess = true;
                    break;
                }

                string userName = this.GetInputWithAttempts("Enter User name:", input => !string.IsNullOrWhiteSpace(input), "Invalid User name");
                if (userName.Equals(string.Empty))
                {
                    return;
                }

                string password = this.GetInputWithAttempts("Enter password:", Validator.IsPasswordValid, "Invalid Password");
                if(password.Equals(string.Empty))
                {
                    return;
                }

                string employeeId = this.GetInputWithAttempts("Enter employee ID:", input => !string.IsNullOrWhiteSpace(input), "Invalid employee ID");
                if(employeeId.Equals(string.Empty))
                {
                    return;
                }
                if(this._userService.AddNewUser(new User(userName, password, Guid.NewGuid(), employeeId)))
                {
                    this.DisplaySuccess("User added successfully!");
                }

            }

        }

        private void DisplayDashboard()
        {
            bool logout = false;
            while (!logout)
            {
                Console.Clear();
                var userDetails = this._userService.GetUserDetails(UserAuthenticatorService.GetCurrentUser());
                Console.WriteLine($"User details\nUser Name:{userDetails.UserName}\nUser ID: {userDetails.UserID}\nEmployee ID:{userDetails.EmployeeId}\n");
                ViewRecentTodo();
                Console.WriteLine("\nMain Menu\n1.Add new ToDo\n2.Update Todo\n3.View All Todo\n4.View to do by time period\n5.Delete Task\n6.Logout\n");
                string choice = this.GetInputWithAttempts("Enter your choice:", input => int.TryParse(input, out int _), "Invalid Choice");
                if (choice.Equals(string.Empty))
                {
                    return;
                }
                int.TryParse(choice, out int resultChoice);
                switch ((MenuOptions)resultChoice)
                {
                    case MenuOptions.AddToDo:
                        var newTask = GetTaskDetails();
                        if (newTask is null)
                        {
                            break;
                        }

                        this._taskService.AddNewTask(newTask);
                        continue;
                    case MenuOptions.UpdateToDo:
                        var details = GetUpdateDetails();
                        if (details.Item1.Equals(string.Empty) || details.Item2 is UpdateOperation.invalid)
                        {
                            Console.WriteLine("Invalid input");
                            break;
                        }

                        this._taskService.UpdateTask(details);
                        this.DisplaySuccess("updated");
                        continue;
                    case MenuOptions.ViewByCalender:
                        this.ViewToDoByCalender();
                        continue;
                    case MenuOptions.ViewAllToDo:
                        ViewUserToDo();
                        continue;
                    case MenuOptions.DeleteToDo:
                        this.ViewUserToDo();
                        string inputID = GetInputWithAttempts("Enter task id to Delete: ", input => Guid.TryParse(input, out Guid _), "Invalid task ID");
                        if (inputID == string.Empty)
                        {
                            break;
                        }

                        var TaskID = Guid.Parse(inputID);
                        this._taskService.DeleteToDo(TaskID);
                        this.DisplaySuccess("deleted");
                        continue;
                    case MenuOptions.Logout:
                        logout = GetInputWithAttempts("Are you sure you want to log out? [y/n]:", Validator.IslogoutValid, "invalid choice").Equals("y");
                        break;
                }
            }
        }

        private void ViewToDoByCalender()
        {
            string status = GetInputWithAttempts("how do you want to view the todo\n1. by date\n2. by month\n3. By year\nEnter choice:", input => int.TryParse(input, out int _), "Invalid option");
            Console.Clear();
            var userToDos = this._taskService.GetUserToDo();
            switch (status)
            {
                case "1":
                    string date = GetInputWithAttempts("Enter date:", input => DateOnly.TryParse(input, out DateOnly _), "Invalid option");
                    foreach (var todo in userToDos)
                    {
                        if (todo.TargetDate == DateOnly.Parse(date))
                        {
                            Console.WriteLine($"task id {todo.TaskId}, task heading:{todo.Heading}, Task Description : {todo.Desciption}, task Target date: {todo.TargetDate}, task recurrence : {todo.Recurrence}, status : {todo.Status}");
                            Console.WriteLine();
                        }
                    }
                    break;
                case "2":
                    string month = GetInputWithAttempts("Enter month:", input => DateOnly.TryParse(input, out DateOnly _), "Invalid option");
                    foreach (var todo in userToDos)
                    {
                        if (todo.TargetDate.Month == DateOnly.Parse(month).Month)
                        {
                            Console.WriteLine($"task id {todo.TaskId}, task heading:{todo.Heading}, Task Description : {todo.Desciption}, task Target date: {todo.TargetDate}, task recurrence : {todo.Recurrence}, status : {todo.Status}");
                            Console.WriteLine();
                        }
                    }
                    break;
                case "3":
                    string year = GetInputWithAttempts("Enter year:", input => DateOnly.TryParse(input, out DateOnly _), "Invalid option");
                    foreach (var todo in userToDos)
                    {
                        if (todo.TargetDate.Year == DateOnly.Parse(year).Year)
                        {
                            Console.WriteLine($"task id {todo.TaskId}, task heading:{todo.Heading}, Task Description : {todo.Desciption}, task Target date: {todo.TargetDate}, task recurrence : {todo.Recurrence}, status : {todo.Status}");
                            Console.WriteLine();
                        }
                    }
                    break;
            }
            Console.WriteLine("PRESS any key to return to menu");
            Console.ReadKey();
        }

        private (string, UpdateOperation, Guid) GetUpdateDetails()
        {
            this.ViewUserToDo();
            string inputID = GetInputWithAttempts("Enter task id to update: ", input => Guid.TryParse(input, out Guid _), "Invalid task ID");
            var TaskID = Guid.Parse(inputID);

            string choice = GetInputWithAttempts("What do you want to edit?\n1. Heading\n2. Description\n3. Target date\n4. Status\nEnter your choice:", input => int.TryParse(input, out int _), "Invalid input");
            switch ((UpdateOperation)int.Parse(choice))
            {
                case UpdateOperation.Heading:
                    string heading = GetInputWithAttempts("Enter task heading:", input => !string.IsNullOrWhiteSpace(input), "Invalid heading! heading cannot be empty");
                    if (heading.Equals(string.Empty))
                    {
                        return (string.Empty, UpdateOperation.invalid, TaskID);
                    }

                    return (heading, UpdateOperation.Heading, TaskID);
                case UpdateOperation.description:
                    string description = GetInputWithAttempts("Enter task description:", input => !string.IsNullOrWhiteSpace(input), "Invalid description! description cannot be empty");
                    if (description.Equals(string.Empty))
                    {
                        return (string.Empty, UpdateOperation.invalid, TaskID);
                    }

                    return (description, UpdateOperation.description, TaskID);
                case UpdateOperation.targetdate:
                    string inputTargetDate = GetInputWithAttempts("Enter the target date (YYYY-MM-DD):", Validator.IsDateValid, "Invalid Date!");
                    if (inputTargetDate.Equals(string.Empty))
                    {
                        return (string.Empty, UpdateOperation.invalid, TaskID);
                    }
                    return (inputTargetDate, UpdateOperation.targetdate, TaskID);
                case UpdateOperation.status:
                    string status = GetInputWithAttempts("what is status of the ToDo\n1. open\n2. close\n3. Inprogress\nEnter option:", input => int.TryParse(input, out int _), "Invalid option");
                    switch ((ToDoStatus)int.Parse(status))
                    {
                        case ToDoStatus.Open:
                            status = "Open";
                            break;
                        case ToDoStatus.Close:
                            status = "close";
                            break;
                        case ToDoStatus.Inprogress:
                            status = "In Progress";
                            break;
                        default:
                            DisplayFailure("Invalid Option");
                            break;
                    }

                    return(status, UpdateOperation.status, TaskID);
                default:
                    return (string.Empty, UpdateOperation.invalid, TaskID);
            }
        }

        private void ViewRecentTodo()
        {
            var recentTodo = this._taskService.GetRecentToDo();
            if (recentTodo is null)
            {
                return;
            }

            foreach (var todo in recentTodo)
            {
                Console.WriteLine($"task id {todo.TaskId}, task heading:{todo.Heading}, Task Description : {todo.Desciption}, task Target date: {todo.TargetDate}, task recurrence : {todo.Recurrence}, status : {todo.Status}");
                Console.WriteLine();
            }
        }

        private void ViewUserToDo()
        {
            Console.Clear();
            var userToDos = this._taskService.GetUserToDo();
            foreach (var todo in userToDos)
            {
                Console.WriteLine($"task id {todo.TaskId}, task heading:{todo.Heading}, Task Description : {todo.Desciption}, task Target date: {todo.TargetDate}, task recurrence : {todo.Recurrence}, status : {todo.Status}");
                Console.WriteLine();
            }

            Console.WriteLine("PRESS any key to continue");
            Console.ReadKey();
            return;
        }

        public List<ToDoTask>? GetTaskDetails()
        {
            Console.Clear();
            string heading = GetInputWithAttempts("Enter task heading:", input => !string.IsNullOrWhiteSpace(input), "Invalid heading! heading cannot be empty");
            if (heading.Equals(string.Empty))
            {
                return null;
            }

            string description = GetInputWithAttempts("Enter task description:", input => !string.IsNullOrWhiteSpace(input), "Invalid description! description cannot be empty");
            if (description.Equals(string.Empty))
            {
                return null;
            }

            string inputTargetDate = GetInputWithAttempts("Enter the target date (YYYY-MM-DD):", Validator.IsDateValid, "Invalid Date!");
            if (inputTargetDate.Equals(string.Empty))
            {
                return null;
            }

            string status = GetInputWithAttempts("what is status of the ToDo\n1. open\n2. close\n3. Inprogress\nEnter option:", input => int.TryParse(input, out int _), "Invalid option");
            switch ((ToDoStatus)int.Parse(status))
            {
                case ToDoStatus.Open:
                    status = "Open";
                    break;
                case ToDoStatus.Close:
                    status = "close";
                    break;
                case ToDoStatus.Inprogress:
                    status = "In Progress";
                    break;
                default:
                    DisplayFailure("Invalid Option");
                    break;
            }

            string recurrence = GetInputWithAttempts("how do you want the task to recure\n1. None\n2. Daily\n3. Weekly\n4. Monthly\n5. Yearly\nEnter Your option:",
                input => int.TryParse(input, out int _), "Invalid option");
            if (recurrence.Equals(string.Empty))
            {
                return null;
            }
            var todolist = new List<ToDoTask>();
            var targetDate = DateOnly.Parse(inputTargetDate);
            switch ((TaskRecurrenceOption)int.Parse(recurrence))
            {
                case TaskRecurrenceOption.None:
                    todolist.Add(new ToDoTask(heading, description, targetDate, "none", UserAuthenticatorService.GetCurrentUser(), DateTime.Now, status, Guid.NewGuid()));
                    return todolist;
                case TaskRecurrenceOption.Daily:
                    var repeatDaily = GetInputWithAttempts("How many days do you want the task to repeat? ", input => int.TryParse(input, out int _), "Invalid times!");
                    todolist.Add(new ToDoTask(heading, description, targetDate, "daily", UserAuthenticatorService.GetCurrentUser(), DateTime.Now, status, Guid.NewGuid()));
                    for (int repeat = 1; repeat < int.Parse(repeatDaily); repeat++)
                    {
                        targetDate = targetDate.AddDays(1);
                        todolist.Add(new ToDoTask(heading, description, targetDate, "daily", UserAuthenticatorService.GetCurrentUser(), DateTime.Now, status, Guid.NewGuid()));
                    }
                    return todolist;
                case TaskRecurrenceOption.Weekly:
                    todolist.Add(new ToDoTask(heading, description, targetDate, "weekly", UserAuthenticatorService.GetCurrentUser(), DateTime.Now, status, Guid.NewGuid()));
                    var repeatMonthly = GetInputWithAttempts("How many weeks do you want the task to repeat? ", input => int.TryParse(input, out int _), "Invalid times!");
                    for (int repeat = 1; repeat < int.Parse(repeatMonthly); repeat++)
                    {
                        targetDate = targetDate.AddDays(7);
                        todolist.Add(new ToDoTask(heading, description, targetDate, "weekly", UserAuthenticatorService.GetCurrentUser(), DateTime.Now, status, Guid.NewGuid()));
                    }
                    return todolist;
                case TaskRecurrenceOption.Monthly:
                    var repeatMonths = GetInputWithAttempts("How many months do you want the task to repeat? ", input => int.TryParse(input, out int _), "Invalid times!");
                    todolist.Add(new ToDoTask(heading, description, targetDate, "Monthly", UserAuthenticatorService.GetCurrentUser(), DateTime.Now, status, Guid.NewGuid()));
                    for (int repeat = 1; repeat < int.Parse(repeatMonths); repeat++)
                    {
                        targetDate = targetDate.AddMonths(1);
                        todolist.Add(new ToDoTask(heading, description, targetDate, "Monthly", UserAuthenticatorService.GetCurrentUser(), DateTime.Now, status, Guid.NewGuid()));
                    }
                    return todolist;
                case TaskRecurrenceOption.Yearly:
                    var repeatYearly = GetInputWithAttempts("How many days do you want the task to repeat? ", input => int.TryParse(input, out int _), "Invalid times!");
                    todolist.Add(new ToDoTask(heading, description, targetDate, "yearly", UserAuthenticatorService.GetCurrentUser(), DateTime.Now, status, Guid.NewGuid()));
                    for (int repeat = 1; repeat < int.Parse(repeatYearly); repeat++)
                    {
                        targetDate = targetDate.AddYears(1);
                        todolist.Add(new ToDoTask(heading, description, targetDate, "yearly", UserAuthenticatorService.GetCurrentUser(), DateTime.Now, status, Guid.NewGuid()));
                    }
                    return todolist;
            }
            return null;
        }

        public void Menu()
        {

        }

        public string GetInputWithAttempts(string input, InputValidator validator, string invalidMessage)
        {
            for (int tries = 3; tries > 0; tries--)
            {
                Console.Write($"Attempts Remaining: {tries}\n{input}");
                string userInput = Console.ReadLine() ?? string.Empty;

                if (validator(userInput))
                {
                    return userInput;
                }

                this.DisplayFailure(invalidMessage);
            }

            return string.Empty;
        }
        public void DisplaySuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public void DisplayFailure(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
