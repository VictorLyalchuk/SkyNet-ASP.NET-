using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SkyNet.Core.Entities.User;
using System.ComponentModel.DataAnnotations;
using SkyNet.Core.Validation.User;

namespace SkyNet.Web.Controllers
{
    [Authorize]
    public class DashBoardController : Controller
    {
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
        public IActionResult SignIn(UserLoginDTO model)
        {
            var validator = new LoginUserValidation();
            var validationResult = validator.Validate(model);
            if (validationResult.IsValid)
            {
                return View(model);
            }
            ViewBag.AuthError = validationResult.Errors[0];
            return View(model);

        }
    }
}
