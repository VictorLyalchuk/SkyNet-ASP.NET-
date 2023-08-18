using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SkyNet.Core.Entities.User;
using SkyNet.Core.DTOs.User;
using System.ComponentModel.DataAnnotations;
using SkyNet.Core.Validation.User;
using SkyNet.Core.Services;
using SkyNet.Web.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using IdentityResult = Microsoft.AspNet.Identity.IdentityResult;
using FluentValidation;
using FluentValidation.Results;

namespace SkyNet.Web.Controllers
{
    [Authorize]
    public class DashBoardController : Controller
    {
        private readonly UserService _userService;
        public DashBoardController(UserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous] // GET
        public IActionResult SignIn()
        {
            var user = HttpContext.User.Identity.IsAuthenticated;
            if (user)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [AllowAnonymous] // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(UserLoginDTO model)
        {
            var validator = new LoginUserValidation();
            var validationResult = validator.Validate(model);
            if (validationResult.IsValid)
            {
                ServiceResponse result = await _userService.LoginUserAsync(model);
                if (result.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.AuthError = result.Message;
                return View(model);
            }
            ViewBag.AuthError = validationResult.Errors[0];
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutUserAsync();
            return RedirectToAction(nameof(SignIn));
        }
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllAsync();
            return View(result.PayLoad);
        }
        public async Task<IActionResult> Profile(string Id)
        {
            var result = await _userService.GetUserByIdAsync(Id);
            if (result.Success)
            {

                return View(result.PayLoad);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UpdateUserDTO model)
        {
            var validator = new UpdateUserValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                ServiceResponse user = await _userService.UpdateUserAsync(model);
                if (user.Success)
                {
                    //return RedirectToAction("Profile", "DashBoard", model.ID);
                    return RedirectToAction(nameof(GetAll));
                }
                ViewBag.UpdateUserError = user.PayLoad;
            }
            ViewBag.UpdateUserError = validationResult.Errors[0];
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDTO model)
        {
            var validator = new UpdatePasswordValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                ServiceResponse result = await _userService.UpdatePasswordAsync(model);
                if (result.Success)
                {
                    return RedirectToAction(nameof(SignIn));
                }
                ViewBag.UpdatePasswordError = result.PayLoad;
            }
            ViewBag.UpdatePasswordError = validationResult.Errors[0];
            return View();
        }
        public  IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserDTO model)
        {
            var validator = new CreateUserValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                ServiceResponse res = await _userService.CreateUserAsync(model);
                if (res.Success == true)
                {
                    return RedirectToAction(nameof(GetAll));
                }
            }
            ViewBag.UpdateCreateError = validationResult.Errors[0];
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteUserDTO model)
        {
            var result = await _userService.DeleteUserAsync(model);
            if (result.Success)
            {
                return RedirectToAction(nameof(GetAll));
            }
            ViewBag.AuthError = result.Errors;
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _userService.ConfirmEmailAsync(userId, token);
            if (result.Success)
            {
                return Redirect(nameof(SignIn));
            }
            return Redirect(nameof(SignIn));
        }
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var result = await _userService.ForgotPasswordAsync(email);
            if (result.Success)
            {
                ViewBag.AuthError = "Check your email.";
                return View(nameof(SignIn));
            }
            ViewBag.AuthError = "Something went wrong.";
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            ViewData["Email"] = email;
            ViewData["Token"] = token;
            return View();
        }
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            var result = await _userService.ResetPasswordAsync(model);
            if (result.Success)
            {
                ViewBag.AuthError = result.Message;
                return View(nameof(ResetPassword));
            }
            ViewBag.AuthError = result.Errors;
            return View(nameof(SignIn));
        }
    }
}