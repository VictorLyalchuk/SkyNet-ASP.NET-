using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SkyNet.Core.Entities.User;
using System.ComponentModel.DataAnnotations;
using SkyNet.Core.Validation.User;
using SkyNet.Core.Services;

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
                if(result.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.AuthError = result.Message;
                return View(model);
            }
            ViewBag.AuthError = validationResult.Errors[0];
            return View(model);

        }
    }
}
