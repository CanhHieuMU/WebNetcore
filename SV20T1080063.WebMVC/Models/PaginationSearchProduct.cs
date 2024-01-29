using SV20T1080063.DomainModels;

namespace SV20T1080063.WebMVC.Models
{
    public class PaginationSearchProduct : PaginationSearchBaseResult
    {
        public IList<Product>? Data { get; set; }
    }
}
