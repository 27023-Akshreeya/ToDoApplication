using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
