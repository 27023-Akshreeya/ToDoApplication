using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApplication.Model
{
    public class ToDoTask
    {
        public string? Heading { get; set; }
        public string? Desciption { get; set; }
        public DateOnly TargetDate { get; set; }
        public string? Recurrence { get; set; }
        public Guid UserID { get; set; }
        public DateTime CurrentDate { get; set; }
        public string? Status { get; set; }
        public Guid TaskId { get; set; }

        public ToDoTask(string? heading, string? desciption, DateOnly targetDate, string? recurrence, Guid userID, DateTime currentDate, string? status, Guid taskID)
        {
            Heading = heading;
            Desciption = desciption;
            TargetDate = targetDate;
            Recurrence = recurrence;
            UserID = userID;
            CurrentDate = currentDate;
            Status = status;
            TaskId = taskID;
        }
    }
}
