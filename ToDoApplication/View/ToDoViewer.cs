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
            Console.Clear();
            var userDetails = this._userService.GetUserDetails(UserAuthenticatorService.GetCurrentUser());
            Console.WriteLine($"User details\nUser Name:{userDetails.UserName}\nUser ID: {userDetails.UserID}\nEmployee ID:{userDetails.EmployeeId}");
            Console.WriteLine("Recent todo:");
            Console.WriteLine("Main Menu\n1.Add new ToDo\n2.Update Todo\n3.View All Todo\n4.Logout\nEnter your choice:");
            string choice = this.GetInputWithAttempts("Enter your choice:", input => int.TryParse(input, out int _), "Invalid Choice");
            if (choice.Equals(string.Empty))
            {
                return;
            }
            int.TryParse(choice, out int resultChoice);
            switch ((MenuOptions)resultChoice)
            {
                case MenuOptions.AddToDo:
                    break;
                case MenuOptions.UpdateToDo: break;
                case MenuOptions.ViewAllToDo: break;
                case MenuOptions.Logout: break;
            }
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
