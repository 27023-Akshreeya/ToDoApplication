using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApplication.Model
{
    public class User
    {
        /// <summary>
        /// Denotes the name of the user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// login password for the user
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// unique UserId 
        /// </summary>
        public Guid UserID { get; set; }

        /// <summary>
        /// employee ID of the user
        /// </summary>
        public  string EmployeeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="userId"></param>
        /// <param name="employeeId"></param>
        public User(string userName, string password, Guid userId, string employeeId)
        {
            this.UserName = userName;
            this.Password = password;
            this.UserID = userId;
            this.EmployeeId = employeeId;
        }
    }
}
