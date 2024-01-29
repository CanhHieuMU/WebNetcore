using SV20T1080063.DomainModels;

namespace SV20T1080063.WebMVC.Models
{
    public class PaginationSearchOrder : PaginationSearchBaseResult
    {
        public IList<Order>? Data { get; set; }
    }
}
