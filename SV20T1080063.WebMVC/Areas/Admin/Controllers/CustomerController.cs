using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1080063.BusinessLayers;
using SV20T1080063.DomainModels;
using SV20T1080063.WebMVC.Models;

namespace SV20T1080063.WebMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private const string CUSTOMER_SEARCH = "Customer_Search";
        private const int PAGE_SIZE = 10;
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public IActionResult Index(int page = 1, string searchValue = "")
        //{
        //    int rowCount = 0;
        //    var data = CommonDataService.ListOfCustomers(out rowCount, page, PAGE_SIZE, searchValue ?? "");
        //    var model = new PaginationSearchCustomer()
        //    {
        //        Page = page,
        //        PageSize = PAGE_SIZE,
        //        SearchValue = searchValue ?? "",
        //        RowCount = rowCount,
        //        Data = data
        //    };

        //    string? errorMessage = Convert.ToString(TempData["ErrorMessage"]);
        //    ViewBag.ErrorMessage = errorMessage;

        //    return View(model);
        //}

        public IActionResult Index()
        {
            var input = ApplicationContext.GetSessionData<PaginationSearchInput>(CUSTOMER_SEARCH);
            if (input == null)
            {
                input = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }
            return View(input);
        }

        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfCustomers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new PaginationSearchCustomer()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            ApplicationContext.SetSessionData(CUSTOMER_SEARCH, input); //Lưu lại điều kiện tìm kiếm

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            var model = new Customer()
            {
                CustomerID = 0
            };
            ViewBag.Title = "Bổ sung khách hàng";
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Edit(int id = 0)
        {
            var model = CommonDataService.GetCustomer(id);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Cập nhật khách hàng";
            return View("Create", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IActionResult Save(Customer data)
        {
            ViewBag.Title = data.CustomerID == 0 ? "Bổ sung khách hàng" : "Cập nhật khách hàng";
            if (string.IsNullOrWhiteSpace(data.CustomerName))
                ModelState.AddModelError(nameof(data.CustomerName), "Tên khách hàng không được rỗng");
            if (string.IsNullOrWhiteSpace(data.ContactName))
                ModelState.AddModelError(nameof(data.ContactName), "Tên giao dịch không được rỗng");
            if (string.IsNullOrWhiteSpace(data.Phone))
                ModelState.AddModelError(nameof(data.Phone), "Số điện thoại không được rỗng");
            if (string.IsNullOrWhiteSpace(data.Address))
                ModelState.AddModelError(nameof(data.Address), "Địa chỉ không hợp lệ");
            if (string.IsNullOrWhiteSpace(data.Province))
                ModelState.AddModelError(nameof(data.Province), "Vui lòng chọn tỉnh/thành");
            if (string.IsNullOrWhiteSpace(data.Email))
                ModelState.AddModelError(nameof(data.Email), "Email không hợp lệ");

            if (!ModelState.IsValid)
            {
                return View("Create", data);
            }

            if (data.CustomerID == 0)
            {
                int customerID = CommonDataService.AddCustomer(data);
                if (customerID > 0)
                    return RedirectToAction("Index");

                ViewBag.ErrorMessage = "Không bổ sung được dữ liệu";
                return View("Create", data);
            }
            else
            {
                bool success = CommonDataService.UpdateCustomer(data);
                if (success)
                    return RedirectToAction("Index");

                ViewBag.ErrorMessage = "Không cập nhật được dữ liệu";
                return View("Create", data);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public IActionResult ChangePass(int id)
        {
            var userAccount = CommonDataService.GetCustomer(id);
            return View(userAccount);
        }
        [HttpPost]
        public IActionResult ChangePass(int id, string oldpassword, string newpassword, string confirmpassword)
        {
            var userAccount = CommonDataService.GetCustomer(id);

            // so sanh pass cu
            if (oldpassword != userAccount.Password)
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
            else if (newpassword != confirmpassword)
            {
                ModelState.AddModelError("Error", "Xác nhận mật khẩu không trùng khớp");
                return View();
            }

            UserAccountService.ChangePass(userAccount.Email, newpassword, TypeOfAccount.Customer);

            // Đổi mật khẩu thành công trả về trang Index
            return RedirectToAction("Index", "Customer", new { area = "Admin" });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool success = CommonDataService.DeleteCustomer(id);
                if (!success)
                    ViewBag.ErrorMessage = "Không thể xóa khách hàng này";

                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetCustomer(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
    }
}
