using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Models;


namespace TaskManagmentSystem.Data
{
    public class ThinkBridge_Task_DBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Teams> Teams { get; set; }
        public DbSet<TeamMembers> TeamMembers { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Attachments> Attachments { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public ThinkBridge_Task_DBContext(DbContextOptions<ThinkBridge_Task_DBContext> options)
           : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attachments>()
                .HasKey(a => a.AttachmentId);

            modelBuilder.Entity<User>()
                .HasKey(a => a.UserId);

            modelBuilder.Entity<Teams>()
                .HasKey(a => a.TeamId);

            modelBuilder.Entity<Notes>()
                .HasKey(a => a.NoteId); 
            
            modelBuilder.Entity<Tasks>()
                .HasKey(a => a.TaskId);
            
            modelBuilder.Entity<TeamMembers>()
                .HasKey(a => a.Id);
        }
    }
}
