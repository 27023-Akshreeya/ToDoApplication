using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    }
}
