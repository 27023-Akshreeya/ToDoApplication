using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.Model;
using ToDoApplication.Repository;

namespace ToDoApplication.Service
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        public bool DoesUserExists(Guid userID)
        {
            var users = this._userRepository.GetUsers();
            return users.Any(u => u.UserID.Equals(userID));
        }

        public bool IsPasswordMatch(string loginPassword, Guid currentUserID)
        {
            var users = this._userRepository.GetUsers();
            return users.Single(u => u.UserID.Equals(currentUserID)).Password.Equals(loginPassword);
        }

        public bool AddNewUser(User user)
        {
            if (user is null)
            {
                return false;
            }

            this._userRepository.AddUser(user);
            return true;
        }

        public User GetUserDetails(Guid guid)
        {
            var users = _userRepository.GetUsers();
            return users.First(u => u.UserID.Equals(guid));
        }
    }
}
