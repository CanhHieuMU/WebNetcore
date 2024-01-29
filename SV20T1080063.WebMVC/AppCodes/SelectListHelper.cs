using SV20T1080063.BusinessLayers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SV20T1080063.WebMVC
{
    public class SelectListHelper
    {
        public static List<SelectListItem> Province()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn tỉnh/thành --"
            });

            foreach (var item in CommonDataService.ListOfProvinces())
                list.Add(new SelectListItem()
                {
                    Value = item.ProvinceName,
                    Text = item.ProvinceName
                });

            return list;
        }

        public static List<SelectListItem> Customers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn khách hàng --",
            });

            int rowCountCustomer = 0;

            foreach (var item in CommonDataService.ListOfCustomers(out rowCountCustomer, 1, 0, ""))
                list.Add(new SelectListItem()
                {
                    Value = item.CustomerName,
                    Text = item.CustomerName,
                });

            return list;
        }

        public static List<SelectListItem> Employees()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn nhân viên --",
            });

            int rowCountEmployee = 0;

            foreach (var item in CommonDataService.ListOfEmployees(out rowCountEmployee, 1, 0, ""))
                list.Add(new SelectListItem()
                {
                    Value = item.FullName,
                    Text = item.FullName,
                });

            return list;
        }

        public static List<SelectListItem> Categories()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn loại hàng --",
            });

            int rowCountEmployee = 0;

            foreach (var item in CommonDataService.ListOfCategories(out rowCountEmployee, 1, 0, ""))
                list.Add(new SelectListItem()
                {
                    Value = item.CategoryID.ToString(),
                    Text = item.CategoryName,
                });

            return list;
        }
        public static List<SelectListItem> Suppliers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn nhà cung cấp --",
            });

            int rowCountSupplier = 0;

            foreach (var item in CommonDataService.ListOfSuppliers(out rowCountSupplier, 1, 0, ""))
                list.Add(new SelectListItem()
                {
                    Value = item.SupplierID.ToString(),
                    Text = item.SupplierName,
                });

            return list;
        }
    }
}
