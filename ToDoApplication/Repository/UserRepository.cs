using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoApplication.Model;

namespace ToDoApplication.Repository
{
    public class UserRepository
    {
        private string _filePath;
        public UserRepository(string filePath)
        {
            _filePath = filePath ?? string.Empty;
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(this._filePath, "[]");
            }
        }

        public void AddUser(User user)
        {
            var users = Enumerable.ToList(this.GetUsers());
            users.Add(user);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string newUser = JsonSerializer.Serialize(users, options);
            File.WriteAllText(_filePath, newUser);
        }

        public IEnumerable<User> GetUsers()
        {
            try
            {
                var users = File.ReadAllText(this._filePath);
                if (string.IsNullOrEmpty(users))
                {
                    return new List<User>();
                }

                return JsonSerializer.Deserialize<List<User>>(users) ?? new List<User>();
            }
            catch
            {
                return new List<User>();
            }
        }
    }
}
