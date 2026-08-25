using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.Model;
using ToDoApplication.Model.Enums;
using ToDoApplication.Repository;

namespace ToDoApplication.Service
{
    public class TaskService
    {
        private readonly TasksRepository _tasksRepository;

        public TaskService(TasksRepository tasksRepository)
        {
            this._tasksRepository = tasksRepository;
        }

        public bool AddNewTask(List<ToDoTask> task)
        {
            if(task is null)
            {
                return false;
            }

            this._tasksRepository.AddTask(task);
            return true;
        }

        internal IEnumerable<ToDoTask> GetRecentToDo()
        {
            var userTodo = GetUserToDo();
            return userTodo.OrderByDescending(u => u.CurrentDate).Take(2);
        }

        internal IEnumerable<ToDoTask> GetUserToDo()
        {
            return this._tasksRepository.GetTasks().Where(u => u.UserID.Equals(UserAuthenticatorService.GetCurrentUser()));
        }

        internal void UpdateTask((string, UpdateOperation, Guid) details)
        {
            this._tasksRepository.Update(details);
        }
    }
}
