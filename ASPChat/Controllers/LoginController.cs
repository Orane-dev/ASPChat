using ASPChat.BuisnessLayer.Auth.Interfaces;
using ASPChat.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASPChat.Controllers
{
    public class LoginController : Controller
    {
        private IAuthentication _authentication;
        public LoginController(IAuthentication authentication)
        {
            _authentication = authentication;
        }

        [HttpGet]
        [Route("/Login")]
        public async Task<IActionResult> Index()
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            return View(loginViewModel);
        }

        [HttpPost]
        [Route("/Login")]

        public async Task<IActionResult> PostIndex(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isAuthSuccess = await _authentication.AuthenticateUser(model.Email, model.Password);
                if (isAuthSuccess) 
                {
                    return Redirect("/");
                }
            }
            return View("Index", model);
        }
    }
}
