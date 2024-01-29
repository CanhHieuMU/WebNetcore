using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1080063.BusinessLayers;
using SV20T1080063.DataLayers;
using SV20T1080063.DomainModels;
using SV20T1080063.WebMVC.AppCodes;
using SV20T1080063.WebMVC.Models;
using System;
using System.Data;
using System.Reflection;

namespace SV20T1080063.WebMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]
    [Area("Admin")]
    public class EmployeeController : Controller
    {




        private const string EMPLOYEE_SEARCH = "Employee_Search";
        private const int PAGE_SIZE = 10;
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="page"></param>
        ///// <param name="searchValue"></param>
        ///// <returns></returns>
        //public IActionResult Index(int page = 1, string searchValue = "")
        //{
        //    int rowCount = 0;
        //    var data = CommonDataService.ListOfEmployees(out rowCount, page, PAGE_SIZE, searchValue ?? "");
        //    var model = new PaginationSearchEmployee()
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var input = ApplicationContext.GetSessionData<PaginationSearchInput>(EMPLOYEE_SEARCH);
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
        
        public IActionResult Search(PaginationSearchInput input) {
            int rowCount = 0;
            var data = CommonDataService.ListOfEmployees(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new PaginationSearchEmployee()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            ApplicationContext.SetSessionData(EMPLOYEE_SEARCH, input); //Lưu lại điều kiện tìm kiếm

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            var model = new Employee()
            {
                EmployeeID = 0
            };
            ViewBag.Title = "Bổ sung nhân viên";
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Edit(int id = 0)
        {
            var model = CommonDataService.GetEmployee(id);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Chỉnh sửa nhân viên";
            return View("Create", model);
        }
        public IActionResult Save(Employee data, string birthday, IFormFile? uploadPhoto)
        {
            ViewBag.Title = data.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật nhân viên";

            //Xử lý ngày sinh
            DateTime? dBirthDate = Converter.StringToDateTime(birthday);
            if (dBirthDate == null)
                ModelState.AddModelError(nameof(data.BirthDate), "Ngày sinh không hợp lệ");
            else
                data.BirthDate = dBirthDate.Value;

            //Xử lý với ảnh
            //Upload ảnh lên (nếu có), sau khi upload xong thì mới lấy tên file ảnh vừa upload
            //để gán cho trường Photo của Employee
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string filePath = System.IO.Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\employees", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }

            //Kiểm tra đầu vào của model

            if (!ModelState.IsValid)
                return View("Create", data);


            if (data.EmployeeID == 0)
            {
                int emloyeeID = CommonDataService.AddEmployee(data);
                if (emloyeeID > 0)
                    return RedirectToAction("Index");

                ViewBag.ErrorMessage = "Không bổ sung được dữ liệu";
                return View("Create", data);
            }
            else
            {
                bool success = CommonDataService.UpdateEmployee(data);
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
        public IActionResult Delete(int id)
        {
            if (Request.Method == "POST")
            {
                bool success = CommonDataService.DeleteEmployee(id);
                if (!success)
                    TempData["ErrorMessage"] = "Không thể xóa nhân viên này";

                return RedirectToAction("Delete");
            }
            var model = CommonDataService.GetEmployee(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public IActionResult ChangePass(int id)
        {
            var userAccount = CommonDataService.GetEmployee(id);
            return View(userAccount);
        }
        [HttpPost]
        public IActionResult ChangePass(int id, string oldpassword, string newpassword, string confirmpassword)
        {
            var userAccount = CommonDataService.GetEmployee(id);

            // so sanh pass cu
            if(oldpassword != userAccount.Password)
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

            UserAccountService.ChangePass(userAccount.Email, newpassword, TypeOfAccount.Employee);

            // Đổi mật khẩu thành công trả về trang Index
            return RedirectToAction("Index", "Employee", new { area = "Admin" });
        }


    }
}
