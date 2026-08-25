using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApplication.Service
{
    public class UserAuthenticatorService
    {
        private Guid CurrentUser { get; set; }
        public void SetCurrentUser(Guid userID)
        {
            this.CurrentUser = userID;
        }

        public Guid GetCurrentUser() => this.CurrentUser;
    }
}
