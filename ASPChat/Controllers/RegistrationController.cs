using ASPChat.BuisnessLayer.Auth.Interfaces;
using ASPChat.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASPChat.Controllers
{
    public class RegistrationController : Controller
    {
        private IAuthentication _authentication;
        public RegistrationController(IAuthentication authentication) 
        { 
            _authentication = authentication;
        }

        [HttpGet]
        [Route("/Register")]
        public IActionResult Index()
        {
            RegistrationViewModel registrationViewModel = new RegistrationViewModel();
            return View(registrationViewModel);
        }

        [HttpPost]
        [Route("/Register")]
        public async Task<IActionResult> IndexPost(RegistrationViewModel model)
        {
            var emailValidationError = await _authentication.ValidateUserEmail(model.Email);
            if (emailValidationError != null)
            {
                ModelState.TryAddModelError("Email", emailValidationError.ErrorMessage!);
            }

            if (ModelState.IsValid) 
            {
                await _authentication.RegisterUser(model.ToUserMode());
                return Redirect("/Chat");
            }
            return View("Index", model);
        }
    }
}
