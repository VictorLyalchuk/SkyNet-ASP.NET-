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

                if (result.PayLoad is UpdateUserDTO userDto)
                {
                    UpdateProfileVM profile = new UpdateProfileVM()
                    {
                        UserInfo = userDto,
                        UserPassword = new UpdatePasswordDTO()
                        {
                            ID = userDto.ID
                        }
                    };
                    return View(profile);
                }
            }
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Profile(UpdateUserDTO modelUser)
        //{
        //    var validator = new UpdateUserValidation();
        //    var validationResult = await validator.ValidateAsync(modelUser);
        //    if (validationResult.IsValid)
        //    {
        //        var user = await _userService.UpdateUserAsync(modelUser);
        //        if (user.Success)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //        ViewBag.UpdateUserError = user.PayLoad;
        //    }
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Profile(UpdatePasswordDTO model)
        //{
        //    var validator = new UpdatePasswordValidation();
        //    var validationResult = await validator.ValidateAsync(model);
        //    if (validationResult.IsValid)
        //    {
        //        var result = await _userService.UpdatePasswordAsync(model);
        //        if (result.Success)
        //        {
        //            return RedirectToAction(nameof(SignIn));
        //        }
        //        ViewBag.UpdatePasswordError = result.PayLoad;
        //    }
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UpdateProfileVM model)
        {
            if (model.UserInfo != null)
            {
                var validator = new UpdateUserValidation();
                var validationResult = await validator.ValidateAsync(model.UserInfo);
                if (validationResult.IsValid)
                {
                    ServiceResponse user = await _userService.UpdateUserAsync(model.UserInfo);
                    if (user.Success)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    ViewBag.UpdateUserError = user.PayLoad;
                }
                ViewBag.UpdateUserError = validationResult.Errors[0];
                return View();
            }
            if (model.UserPassword != null)
            {
                var validator = new UpdatePasswordValidation();
                var validationResult = await validator.ValidateAsync(model.UserPassword);
                if (validationResult.IsValid)
                {
                    ServiceResponse result = await _userService.UpdatePasswordAsync(model.UserPassword);
                    if (result.Success)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }
                    ViewBag.UpdatePasswordError = result.PayLoad;
                }
                ViewBag.UpdatePasswordError = validationResult.Errors[0];
                return RedirectToAction("Profile", "DashBoard", model.UserPassword.ID);
            }
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
                ServiceResponse res = await _userService.CreateUser(model);
                if (res.Success == true)
                {
                    ViewBag.UpdateCreateError = validationResult.Errors[0];
                    return RedirectToAction(nameof(GetAll));
                }

            }
            ViewBag.UpdateCreateError = validationResult.Errors[0];
            return View();
        }

    }
}
