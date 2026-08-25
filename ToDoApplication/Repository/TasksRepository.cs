using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoApplication.Model;
using ToDoApplication.Model.Enums;
using ToDoApplication.Service;

namespace ToDoApplication.Repository
{
    public class TasksRepository
    {
        private string _filePath;
        public TasksRepository(string filePath)
        {
            _filePath = filePath ?? string.Empty;
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(this._filePath, "[]");
            }
        }

        public void AddTask(List<ToDoTask> task)
        {
            var tasks = Enumerable.ToList(this.GetTasks());
            tasks.AddRange(task);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string newTask = JsonSerializer.Serialize(tasks, options);
            File.WriteAllText(_filePath, newTask);
        }

        public IEnumerable<ToDoTask> GetTasks()
        {
            try
            {
                var tasks = File.ReadAllText(this._filePath);
                if (string.IsNullOrEmpty(tasks))
                {
                    return new List<ToDoTask>();
                }

                return JsonSerializer.Deserialize<List<ToDoTask>>(tasks) ?? new List<ToDoTask>();
            }
            catch
            {
                return new List<ToDoTask>();
            }
        }

        internal void DeleteTask(Guid taskID)
        {
            var tasks = this.GetTasks().ToList();
            var updatetask = tasks.First(u => u.TaskId == taskID);
            tasks.Remove(updatetask);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string updatedTask = JsonSerializer.Serialize(tasks, options);
            File.WriteAllText(_filePath, updatedTask);
        }

        internal void Update((string, UpdateOperation, Guid) details)
        {
            var tasks = this.GetTasks().ToList();
            var updatetask = tasks.First(u => u.TaskId == details.Item3);
            switch (details.Item2)
            {
                case (UpdateOperation.status):
                    updatetask.Status = details.Item1;
                    break;
                case UpdateOperation.Heading:
                    updatetask.Heading = details.Item1;
                    break;
                case UpdateOperation.description:
                    updatetask.Desciption = details.Item1;
                    break;
                case UpdateOperation.targetdate:
                    updatetask.TargetDate = DateOnly.Parse(details.Item1);
                    break;
            }
            var options = new JsonSerializerOptions { WriteIndented = true };
            string updatedTask = JsonSerializer.Serialize(tasks, options);
            File.WriteAllText(_filePath, updatedTask);
        }
    }
}
