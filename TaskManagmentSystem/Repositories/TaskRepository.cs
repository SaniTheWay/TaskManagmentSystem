using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Constants;
using TaskManagmentSystem.Data;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Repositories
{
    public interface ITaskRepository
    {
        List<Tasks> GetUsersTask(int userId);
        List<Tasks> GetTeamsTask(int userId);
        Task<List<Teams>> GetListofTeams();
        Task<List<TeamMembers>> GetListofTeamsMembers(int teamId);
        Task<List<User>> GetListofNewTeamsMembers(int teamId);
        Task<Tasks> AddTask(Tasks task);
        Task<bool> AddAttachment(Attachments document);
        Task<Tasks> GetTaskDetail(int id);
        Task<List<Attachments>> GetAttachmentsDetail(int id);
        Task<List<Notes>> GetNotesDetail(int id);
        Task<Attachments> GetAttachmentById(int id);
        Task<bool> AddNotes(Notes note);
        Task<TaskDetailViewModel> GetTaskDetailViewData(int id);
        Task<bool> UpdateStatus(int id, string status);
        Task<bool> UpdateTask(Tasks task);
        Task<AdminViewData> GetAdminViewData(int userId);
        Task<Teams> AddTeam(string TeamName);
        Task<Teams> GetTeam(int id);
        Task<bool> AddTeamMember(Teams team, List<int> members);
    }

    public class TaskRepository : ITaskRepository
    {
        private readonly ThinkBridge_Task_DBContext _dbContext;
        public TaskRepository(ThinkBridge_Task_DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Tasks> GetUsersTask(int userId)
        {
            var tasks = _dbContext.Tasks.Where(x => x.AssignedTo == userId).ToList();

            return tasks;
        }

        public List<Tasks> GetTeamsTask(int userId)
        {
            var tasks = new List<Tasks>();
            var userTeams = _dbContext.TeamMembers.Where(x => x.UserId == userId).ToList();
            if (userTeams.Count != 0)
            {
                var teams = userTeams.Select(x => x.TeamId).ToList();
                tasks = _dbContext.Tasks.Where(x => teams.Contains(x.TeamId) && x.AssignedTo != userId).ToList();
            }

            return tasks;
        }

        public async Task<List<Teams>> GetListofTeams()
        {
            return await _dbContext.Teams.ToListAsync();
        }

        public async Task<List<TeamMembers>> GetListofTeamsMembers(int teamId)
        {
            return await _dbContext.TeamMembers.Where(x => x.TeamId == teamId).ToListAsync();
        }

        //get new member to add into teams
        public async Task<List<User>> GetListofNewTeamsMembers(int teamId)
        {
            var existingMember = await _dbContext.TeamMembers.Where(x => x.TeamId == teamId).Select(x => x.UserId).ToListAsync();
            var users = await _dbContext.Users.Where(x => !existingMember.Contains(x.UserId)).ToListAsync();
            return users;
        }

        public async Task<Tasks> AddTask(Tasks task)
        {
            try
            {
                await _dbContext.Tasks.AddAsync(task);
                await _dbContext.SaveChangesAsync();
                return task;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<bool> AddAttachment(Attachments document)
        {
            try
            {
                await _dbContext.Attachments.AddAsync(document);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<Tasks> GetTaskDetail(int id)
        {
            try
            {
                var taskDetails = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.TaskId == id);
                if (taskDetails is null)
                    return null!;
                return taskDetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }
        public async Task<List<Attachments>> GetAttachmentsDetail(int id)
        {
            try
            {
                var attachments = await _dbContext.Attachments.Where(x => x.TaskId == id).ToListAsync();
                if (attachments is null || attachments.Count == 0)
                    return [];
                return attachments;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }
        }

        public async Task<List<Notes>> GetNotesDetail(int id)
        {
            try
            {
                var notes = await _dbContext.Notes.Where(x => x.TaskId == id).ToListAsync();
                if (notes is null || notes.Count == 0)
                    return [];
                return [.. notes.OrderByDescending(x => x.CreatedDate)];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }
        }

        public async Task<Attachments> GetAttachmentById(int id)
        {
            try
            {
                var attachments = await _dbContext.Attachments.FirstOrDefaultAsync(x => x.AttachmentId == id);
                if (attachments is null)
                    return null!;
                return attachments;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<bool> AddNotes(Notes note)
        {
            try
            {
                await _dbContext.Notes.AddAsync(note);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<TaskDetailViewModel> GetTaskDetailViewData(int id)
        {
            var task = await GetTaskDetail(id);
            var teamMember = await GetListofTeamsMembers(task.TeamId);
            var taskDetails = new TaskDetailViewModel()
            {
                TaskId = task.TaskId,
                Title = task.Title,
                Description = task.Description,
                AssignedTo = task.AssignedTo,
                TeamId = task.TeamId,
                AssignedUser = task.AssignedUser,
                CreatedBy = task.CreatedBy,
                CreatedUser = task.CreatedUser,
                DueDate = task.DueDate,
                Status = task.Status,
                Attachments = await GetAttachmentsDetail(id),
                Notes = await GetNotesDetail(id),
                TeamMembers = teamMember
            };
            return taskDetails;
        }

        public async Task<bool> UpdateStatus(int id, string status)
        {
            try
            {
                var task = await GetTaskDetail(id);
                if (task != null)
                {
                    task.Status = status;
                    _dbContext.Update(task);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateTask(Tasks task)
        {
            try
            {
                if (task != null)
                {
                    _dbContext.Update(task);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<AdminViewData> GetAdminViewData(int userId)
        {
            try
            {
                var tasks = await _dbContext.Tasks.ToListAsync();
                var usr = _dbContext.Users.First(x => x.UserId == userId);
                if (usr is not null)
                {
                    var adminViewData = new AdminViewData()
                    {
                        Teams = await GetListofTeams(),
                        TotalTask = tasks.Count,
                        ToDo = tasks.Count(x => x.Status == TasksStatus.ToDo),
                        Inprogress = tasks.Count(x => x.Status == TasksStatus.InProgress),
                        QA = tasks.Count(x => x.Status == TasksStatus.QA),
                        Blocked = tasks.Count(x => x.Status == TasksStatus.Blocked),
                        Completed = tasks.Count(x => x.Status == TasksStatus.Done),
                        UserName = usr.FirstName + " " + usr.LastName,
                        AssignedToMe = tasks.Where(x => x.AssignedTo == userId).ToList(),
                        AllTasks = [.. tasks.OrderBy(x => x.DueDate)],
                        Done = tasks.Where(x => x.Status == TasksStatus.Done).ToList(),
                        EarlyDue = tasks.Where(x => x.DueDate >= DateTime.Now && x.DueDate <= DateTime.Now.AddDays(7)).ToList(),
                        LateDue = tasks.Where(x => x.DueDate > DateTime.Now.AddDays(7)).ToList(),
                        ExceededDueDate = tasks.Where(x => x.DueDate < DateTime.Now).ToList(),
                    };
                    return adminViewData;
                }
                throw new Exception("User Not found as Admin.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new AdminViewData();
            }
        }
        public async Task<Teams> AddTeam(string TeamName)
        {
            try
            {
                if (!string.IsNullOrEmpty(TeamName))
                {
                    Teams team = new()
                    {
                        TeamName = TeamName
                    };
                    await _dbContext.Teams.AddAsync(team);
                    await _dbContext.SaveChangesAsync();
                    return team;
                }

                return new Teams();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Teams();
            }
        }

        public async Task<Teams> GetTeam(int id)
        {
            try
            {
                var team = await _dbContext.Teams.FirstOrDefaultAsync(x => x.TeamId == id);
                return team!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Teams();
            }
        }

        public async Task<bool> AddTeamMember(Teams team, List<int> members)
        {
            try
            {
                foreach (var member in members)
                {
                    var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == member);
                    TeamMembers teamMember = new()
                    {
                        TeamId = team.TeamId,
                        TeamName = team.TeamName,
                        UserName = user!.Username,
                        UserId = user.UserId,
                    };
                    await _dbContext.TeamMembers.AddAsync(teamMember);
                    await _dbContext.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
