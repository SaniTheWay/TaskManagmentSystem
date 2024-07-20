using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using TaskManagmentSystem.Constants;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Repositories;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Controllers
{
    public class TaskManagementController : Controller
    {
        private readonly ITaskRepository taskRepo;
        private readonly IAccountRepository accountRepo;
        private readonly INotyfService toastNotification;
        public TaskManagementController(ITaskRepository taskRepo, IAccountRepository accountRepo, INotyfService toastNotification)
        {
            this.taskRepo = taskRepo;
            this.accountRepo = accountRepo;
            this.toastNotification = toastNotification;
        }

        /// <summary>
        /// Index Page Controller
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                toastNotification.Warning("Session Expire, Try Login Again!");
                return View("~/Views/AuthView/Login.cshtml");
            }
            var tasks = new TaskListViewModel()
            {
                Members = await accountRepo.GetAllUsers(),
                Teams = await taskRepo.GetListofTeams(),
                AssignedToMe = taskRepo.GetUsersTask(userId.Value),
                TeamMatesTasks = taskRepo.GetTeamsTask(userId.Value)
            };
            return View("~/Views/Dashboard/IndexDashboard.cshtml", tasks);
        }

        /// <summary>
        /// Task Details Page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> TaskDetailView(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                toastNotification.Warning("Session Expired, Login Again!");
                return View("~/Views/AuthView/Login.cshtml");
            }
            var user = accountRepo.GetUserByUserId(userId.Value);
            var taskDetails = await taskRepo.GetTaskDetailViewData(id);
            taskDetails.UserRole = user.UserRole;
            return View("~/Views/Tasks/TaskView.cshtml", taskDetails);
        }

        /// <summary>
        /// Admin View dashboard
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> AdminView()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                toastNotification.Warning("Session Expire, Login Again !!!");
                return View("~/Views/AuthView/Login.cshtml");
            }
            var admindata = await taskRepo.GetAdminViewData(userId.Value);
            admindata.Members = await accountRepo.GetAllUsers();
            return View("~/Views/Dashboard/AdminDashboard.cshtml", admindata);
        }

        /// <summary>
        /// Create/Generate New Task
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GenerateTask()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                toastNotification.Warning("User session expired, Login Again.");
                return View("~/Views/AuthView/Login.cshtml");
            }
            var user = accountRepo.GetUserByUserId(userId.Value);
            var teamViewModel = new TeamViewModel
            {
                UserRole = user.UserRole,
                Teams = await taskRepo.GetListofTeams(),
                TeamMembers = new List<TeamMembers>()
            };
            return View("~/Views/Dashboard/AddTask.cshtml", teamViewModel);
        }

        /// <summary>
        /// Add Tasks
        /// </summary>
        /// <param name="file"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddTask(IFormFile file, Tasks task)
        {
            var assingedTo = accountRepo.GetUserByUserId(task.AssignedTo);

            //fetching creator id from session
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                toastNotification.Warning("Your session is being expired, Login Again!");
                return View("~/Views/AuthView/Login.cshtml");
            }
            var createdBy = accountRepo.GetUserByUserId(userId.Value);

            task.CreatedUser = createdBy.Username;
            task.CreatedBy = createdBy.UserId;
            task.AssignedUser = assingedTo.Username;

            task.Status = TasksStatus.ToDo;

            var result = await taskRepo.AddTask(task);
            //uploading attachment
            if(file != null)
              await UploadAssignment(file, task.TaskId);
            if (result != null)
            {
                toastNotification.Success("Task Created Successfully !!");
                return RedirectToAction("Index");
            }
            else
            {
                toastNotification.Error("Something went wrong !! Try Again");
                return View("~/Views/Dashboard/AddTask.cshtml");
            }

        }

        /// <summary>
        /// Add Notes to the task
        /// </summary>
        /// <param name="TaskId"></param>
        /// <param name="NoteText"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddTaskNotes(int TaskId, string NoteText)
        {

            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                toastNotification.Warning("Session Expire, Login Again !!!");
                return View("~/Views/AuthView/Login.cshtml");
            }
            var user = accountRepo.GetUserByUserId(userId.Value);

            var noteDetail = new Notes()
            {
                NoteText = NoteText,
                TaskId = TaskId,
                CreatedUser = user.Username,
                CreatedBy = user.UserId,
                CreatedDate = DateTime.Now,
            };

            var result = await taskRepo.AddNotes(noteDetail);
            if (result)
                toastNotification.Success("Note Added Successfully !!");
            else
                toastNotification.Error("Something went wrong !! Try Again");

            var taskDetails = await taskRepo.GetTaskDetailViewData(TaskId);
            taskDetails.UserRole = user.UserRole;
            return View("~/Views/Tasks/TaskView.cshtml", taskDetails);
        }

        /// <summary>
        /// Update Task Status
        /// </summary>
        /// <param name="TaskId"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public async Task<IActionResult> TaskUpdateStatus(int TaskId, string Status)
        {
            var result = await taskRepo.UpdateStatus(TaskId, Status);
            if (result)
                return Ok();

            return BadRequest();
        }

        /// <summary>
        /// Update the task details
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateUserTask(Tasks tasks)
        {
            var taskDetail = await taskRepo.GetTaskDetail(tasks.TaskId);
            taskDetail.Title = tasks.Title;
            taskDetail.Description = tasks.Description;
            taskDetail.DueDate = tasks.DueDate;
            if (taskDetail.AssignedTo != tasks.AssignedTo)
            {
                var assingedTo = accountRepo.GetUserByUserId(tasks.AssignedTo);
                taskDetail.AssignedTo = assingedTo.UserId;
                taskDetail.AssignedUser = assingedTo.Username;
            }

            var result = await taskRepo.UpdateTask(taskDetail);
            if (result)
            {
                toastNotification.Success("Task Updated SucessFully !!");
            }
            else
            {
                toastNotification.Success("An Error Occurred while Updating !!");
            }

            return RedirectToAction("TaskDetailView", new { id = tasks.TaskId });
        }

        /// <summary>
        /// Add New Attachments to the task
        /// </summary>
        /// <param name="file"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddNewAttachments(IFormFile file, int taskId)
        {
            await UploadAssignment(file, taskId);
            return RedirectToAction("TaskDetailView", new { id = taskId });
        }

        /// <summary>
        /// Add new team with members and team Id
        /// </summary>
        /// <param name="SelectedMemberIds"></param>
        /// <param name="teamName"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddTeam(List<int> SelectedMemberIds, string teamName)
        {
            if (SelectedMemberIds != null && SelectedMemberIds.Count > 0)
            {
                var team = await taskRepo.AddTeam(teamName);
                var result = false;
                if (team != null)
                {
                    result = await taskRepo.AddTeamMember(team, SelectedMemberIds);
                    if (result)
                    {
                        toastNotification.Success("Team Added Successfully");
                    }
                }
                if (team == null || !result)
                {
                    toastNotification.Error("An Error Occurred !! Try Again");
                }
            }
            else
            {
                toastNotification.Warning("Please Select a Team Member");
            }
            string role = HttpContext.Session.GetString("Role")!;

            if (string.IsNullOrEmpty(role))
            {
                toastNotification.Warning("Session Expire, Login Again !!!");
                return View("~/Views/AuthView/Login.cshtml");
            }
            if (role != "CompanyAdmin")
                return RedirectToAction("Index");
            else
                return RedirectToAction("AdminView");
        }

        /// <summary>
        /// Add Members in Team
        /// </summary>
        /// <param name="SelectedMemberIds"></param>
        /// <param name="TeamId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddMemberInTeam(List<int> SelectedMemberIds, int TeamId)
        {
            if (SelectedMemberIds != null && SelectedMemberIds.Count > 0)
            {
                var team = await taskRepo.GetTeam(TeamId);
                var result = false;
                if (team != null)
                {
                    result = await taskRepo.AddTeamMember(team, SelectedMemberIds);
                    if (result)
                    {
                        toastNotification.Success("Members Added Successfully");
                    }
                }
                if (team == null || !result)
                {
                    toastNotification.Error("An Error Occurred !! Try Again");
                }
            }
            else
            {
                toastNotification.Warning("Please Select a Team Member");
            }
            string role = HttpContext.Session.GetString("Role")!;

            if (string.IsNullOrEmpty(role))
            {
                toastNotification.Warning("Session Expire, Login Again !!!");
                return View("~/Views/AuthView/Login.cshtml");
            }
            if (role != "CompanyAdmin")
                return RedirectToAction("Index");
            else
                return RedirectToAction("AdminView");
        }

        /// <summary>
        /// Get existing team members
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetTeamMembers(int teamId)
        {
            var teamMembers = await taskRepo.GetListofTeamsMembers(teamId);
            return Json(teamMembers);
        }

        /// <summary>
        /// Get the new members of the team with team id
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetNewMember(int teamId)
        {
            var teamMembers = await taskRepo.GetListofNewTeamsMembers(teamId);
            return Json(teamMembers);
        }

        /// <summary>
        /// Upload the attachments to the task
        /// </summary>
        /// <param name="file"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<IActionResult> UploadAssignment(IFormFile file, int taskId)
        {
            //fetching creator id from session
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                toastNotification.Warning("Session Expire, Login Again !!!");
                return View("~/Views/AuthView/Login.cshtml");
            }
            var createdBy = accountRepo.GetUserByUserId(userId.Value);
            var result = false;
            if (file != null && file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    var document = new Attachments
                    {
                        TaskId = taskId,
                        DocData = memoryStream.ToArray(),
                        DocType = file.ContentType,
                        FileName = file.FileName,
                        UploadedBy = createdBy.UserId,
                        UploadedUser = createdBy.Username,
                        UploadDate = DateTime.Now
                    };
                    result = await taskRepo.AddAttachment(document);
                }
            }
            if (!result)
            {
                toastNotification.Error("Error while Uploading Attachment !!");
                return BadRequest();
            }
            toastNotification.Success("Attachment Uploaded Successfully !!");
            return Ok();
        }
    }
}
