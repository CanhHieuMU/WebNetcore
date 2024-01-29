using SV20T1080063.DomainModels;

namespace SV20T1080063.WebMVC.Models
{
    public class PaginationSearchShipper : PaginationSearchBaseResult
    {
        public IList<Shipper>? Data { get; set; }
    }
}
