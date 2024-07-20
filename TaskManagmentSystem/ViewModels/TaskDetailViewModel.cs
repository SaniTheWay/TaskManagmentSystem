using System.Collections.ObjectModel;
using TaskManagmentSystem.Constants;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.ViewModels
{
    public class TaskDetailViewModel : Tasks
    {
        public string UserRole { get; set; }
        public List<Attachments> Attachments { get; set; }
        public List<Notes> Notes { get; set; }
        public List<TeamMembers> TeamMembers{get;set;}

        public List<string> StatusList { get; set; } =
        [
            TasksStatus.ToDo,
            TasksStatus.InProgress,
            TasksStatus.Blocked,
            TasksStatus.QA,
            TasksStatus.Done
        ];

    }
}
