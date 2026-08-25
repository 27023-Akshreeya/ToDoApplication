using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApplication.Service
{
    public static class UserAuthenticatorService
    {
        private static Guid CurrentUser { get; set; }
        public static void SetCurrentUser(Guid userID)
        {
            CurrentUser = userID;
        }

        public static Guid GetCurrentUser() => CurrentUser;
    }
}
