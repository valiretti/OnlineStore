using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Interfaces;
using OnlineStore.Web.Models;

namespace OnlineStore.Web.Controllers
{
    public class AccountController : Controller
    {

        private IUserService UserService => HttpContext.GetOwinContext().GetUserManager<IUserService>();

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                UserDto userDto = new UserDto { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await UserService.Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Wrong login or password");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                UserDto userDto = new UserDto
                {
                    Email = model.Email,
                    Password = model.Password,
                    Address = model.Address,
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    Role = "User"
                };

                string result = await UserService.Create(userDto);
                if (result == "OK")
                    return View("SuccessfullRegister");
                
                    ModelState.AddModelError("", result);
            }
            return View(model);
        }

        public ActionResult AddManager()
        {
            return View("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddManager(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                UserDto userDto = new UserDto
                {
                    Email = model.Email,
                    Password = model.Password,
                    Address = model.Address,
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    Role = "Manager"
                };
                string result = await UserService.Create(userDto);
                if (result == "OK")
                    return View("SuccessfullRegister");
                
                    ModelState.AddModelError("", result);
            }
            return View("Register", model);
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            var user = UserService.GetUserData(User.Identity.GetUserId());
            if (user != null)
            {
                var mapper = new MapperConfiguration(c => c.CreateMap<UserDto, ClientProfileViewModel>()).CreateMapper();
                var profile = mapper.Map<UserDto, ClientProfileViewModel>(user);
                return View(profile);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(ClientProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(c => c.CreateMap<ClientProfileViewModel, UserDto>()).CreateMapper();
                var userDto = mapper.Map<ClientProfileViewModel, UserDto>(model);
                UserService.EditProfile(userDto, User.Identity.GetUserId());

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult EditPassword()
        {
            var user = UserService.GetUserData(User.Identity.GetUserId());
            if (user != null)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword(PasswordChangeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(c => c.CreateMap<PasswordChangeViewModel, UserDto>()).CreateMapper();
                var userDto = mapper.Map<PasswordChangeViewModel, UserDto>(model);
                UserService.EditPassword(userDto, User.Identity.GetUserId());

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult LostPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LostPassword(LostPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserService.GetUserData(model.Email);
                if (user != null)
                {
                    var mapper = new MapperConfiguration(c => c.CreateMap<LostPasswordViewModel, UserDto>())
                        .CreateMapper();
                    var userDto = mapper.Map<LostPasswordViewModel, UserDto>(model);
                    UserService.ResetPassword(userDto, user.Email);
                }
                
                ModelState.AddModelError("", "User with this email doesn't exist");
            }

            return View(model);
        }
    }
}