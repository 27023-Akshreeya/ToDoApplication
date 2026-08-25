using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApplication.Model
{
    public class Task
    {
        public string? Heading { get; set; }
        public string? Desciption { get; set; }
        public DateOnly TargetDate { get; set; }
        public string? Recurrence { get; set; }
        public Guid UserID { get; set; }
        public DateTime CurrentDate { get; set; }
        public string? Status { get; set; }
    }
}
