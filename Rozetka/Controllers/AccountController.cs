using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BLL;
using BLL.DTOs;
using Rozetka.ViewModels;

namespace Rozetka.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);


            var loginDto = new UserLoginDto
            {
                Email = model.Email,
                Password = model.Password
            };

            var success = await _userService.LoginAsync(loginDto);

            if (success)
            {
                if (await _userService.IsAdminAsync(loginDto.Email))
                {
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var registerDto = new UserRegisterDto
            {
                Email = model.Email,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address
            };
            registerDto.CardNumber = model.CardNumber;
            registerDto.CardExpiry = model.CardExpiry;
            registerDto.CardCvv = model.CardCvv;

            var success = await _userService.RegisterAsync(registerDto);

            if (success)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Registration failed. Email might be taken.");
            return View(model);
        }
    }
}