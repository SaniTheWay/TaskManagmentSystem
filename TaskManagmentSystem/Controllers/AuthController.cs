using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using TaskManagmentSystem.Constants;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Repositories;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAccountRepository _accountRepo;
        private readonly ITaskRepository _taskRepo;
        private readonly INotyfService _toastNotification;
        public AuthController(IAccountRepository accountRepo, ITaskRepository taskRepo, INotyfService toastNotification)
        {
            _accountRepo = accountRepo;
            _taskRepo = taskRepo;
            _toastNotification = toastNotification;
        }
        public IActionResult Login()
        {
            return View("~/Views/AuthView/Login.cshtml");
        }

        public IActionResult Register()
        {
            return View("~/Views/AuthView/Register.cshtml");
        }

        public async Task<IActionResult> UserAuthentication(string userName, string password)
        {
            var user = _accountRepo.GetUserByUserName(userName);
            if(user == null)
            {
                _toastNotification.Error("⚠ Invalid Login Details");
                return View("~/Views/AuthView/Login.cshtml");
            }
            if(user != null && user.Password == password)
            {
                HttpContext.Session.SetInt32("UserId", user.UserId);
                if (user.UserRole != null && user.UserRole != UserRoles.CompanyAdmin)
                {
                    HttpContext.Session.SetString("Role", "Team");
                    var tasklist = new TaskListViewModel()
                    {
                        Members = await _accountRepo.GetAllUsers(),
                        Teams = await _taskRepo.GetListofTeams(),
                        AssignedToMe = _taskRepo.GetUsersTask(user.UserId),
                        TeamMatesTasks = _taskRepo.GetTeamsTask(user.UserId)
                    };
                    return View("~/Views/Dashboard/IndexDashboard.cshtml", tasklist);
                }
                else if (user.UserRole != null && user.UserRole == UserRoles.CompanyAdmin)
                {
                    HttpContext.Session.SetString("Role", "CompanyAdmin");
                    var admindata = await _taskRepo.GetAdminViewData(user.UserId);
                    admindata.Members = await _accountRepo.GetAllUsers();
                    return View("~/Views/Dashboard/AdminDashboard.cshtml", admindata);
                }
                _toastNotification.Success("✔ Login successfully");     
            }
            else
            {
                _toastNotification.Error("⚠ Invalid UserName or Password");
            }
            return View("~/Views/AuthView/Login.cshtml");
        }

        public async Task<IActionResult> UserRegistration(User user)
        {
            var userDetails = _accountRepo.GetUserByUserName(user.Username);
            if(userDetails != null)
            {
                _toastNotification.Error("⚠ This User Name already allocated to someone else. Retry something else.");
                return View("~/Views/AuthView/Register.cshtml");
            }

            if (user != null && user.Password.Length < 5)
            {
                _toastNotification.Error("⚠ Password length must be greater than 5 characters.");
                return View("~/Views/AuthView/Register.cshtml");
            }

            user.UserRole = UserRoles.TeamMember;
            var result = await _accountRepo.UpdateUser(user);

            if (result)
            {
                _toastNotification.Success("User Account Created Successfully. 👍");
                _toastNotification.Information("You can now Login. 😃");
                return View("~/Views/AuthView/Login.cshtml");
            }
            else
                _toastNotification.Error("⚠ An error Occured. Please Try Again");
            return View("~/Views/AuthView/Register.cshtml");
        }

        public IActionResult LogOut()
        {
            _toastNotification.Information("ℹ You are now Logged our safely.");
            HttpContext.Session.Remove("UserId");
            return View("~/Views/AuthView/Login.cshtml");
        }

    }
}
