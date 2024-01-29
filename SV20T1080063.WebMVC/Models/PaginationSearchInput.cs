namespace SV20T1080063.WebMVC.Models
{
    public class PaginationSearchInput
    {
        /// <summary>
        /// Thông tin đầu vào dùng để tìm kiếm, phân trang
        /// </summary>
        public int Page { get; set; } =1;
        public int PageSize { get; set; } = 20;
        public string SearchValue { get; set; } = "";
        public int CustomerID { get; set; } = 0;
        public int EmployeeID { get; set; } = 0;
    }
}
