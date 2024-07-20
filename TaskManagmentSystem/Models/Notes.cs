
namespace TaskManagmentSystem.Models
{
    public class Notes
    {
        public int NoteId { get; set; }
        public int TaskId { get; set; }
        public string NoteText { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
