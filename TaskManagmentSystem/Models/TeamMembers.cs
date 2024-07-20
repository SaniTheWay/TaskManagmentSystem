using System.ComponentModel.DataAnnotations;

namespace TaskManagmentSystem.Models
{
    public class TeamMembers
    {
        [Key]
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
