using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SV20T1080063.BusinessLayers;

namespace SV20T1080063.WebMVC.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// Giao diện đăng nhập
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Login(string userName ="", string password = "")
        {
            ViewBag.UserName = userName;
            ViewBag.Password = password;

            var userAccount = UserAccountService.Authorize(userName, password, TypeOfAccount.Employee);
            //var userAccount = UserAccountService.Authorize(userName, password, TypeOfAccount.Customer);
            // Kiểm tra thông tin đăng nhập đúng hay sai
            //TODO: Kiểm tra username và password từ SQL

            if (userAccount != null)
            {
                // Đăng nhập thành công

                //1. Tạo đối tượng lưu các thông tin của phiên đăng nhập
                WebUserData userData = new WebUserData()
                {
                    UserId = userAccount.UserId, 
                    UserName = userAccount.UserName,
                    DisplayName = userAccount.FullName,
                    Email = userAccount.Email,
                    Photo = userAccount.Photo,
                    ClientIP = HttpContext.Session.Id,
                    AdditionalData = "",
                    Roles = new List<string>() { WebUserRoles.Administrator }
                };
                //2. Thiết lập (ghi nhận) phiên đăng nhập
                await HttpContext.SignInAsync(userData.CreatePrincipal());
                //3. Quay lại trang chủ của Admin
                return RedirectToAction("Index", "Dashboard", new {area = "Admin"});
            }
            else
            {
                //Đăng nhập không thành công" trả lại giao diện để đăng nhập lại
                ModelState.AddModelError("Error", "Đăng nhập thất bại");
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        public IActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePass(string oldpassword ,string newpassword ,string confirmpassword)
        {
            string userName = User?.GetUserData()?.UserName ?? "";

            // so sanh pass cu
            var userAccount = UserAccountService.Authorize(userName, oldpassword, TypeOfAccount.Employee);

            if (userAccount == null)
            {
                ModelState.AddModelError("Error", "Mật khẩu cũ bị sai");

                return View();
            }
            // Mật khẩu cũ trùng mật khẩu mới
            else if (oldpassword == newpassword)
            {
                ModelState.AddModelError("Error", "Mật khẩu mới trùng với mật khẩu cũ");
                return View();
            }
            // Xác nhận mật khẩu không trùng khớp
            else if(newpassword != confirmpassword)
            {
                ModelState.AddModelError("Error", "Xác nhận mật khẩu không trùng khớp");
                return View();
            }

            UserAccountService.ChangePass(userName, newpassword, TypeOfAccount.Employee);

            // Đổi mật khẩu thành công trả về trang Index
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
    }
}
