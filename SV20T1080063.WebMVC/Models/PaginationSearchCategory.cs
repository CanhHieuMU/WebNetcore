using SV20T1080063.DomainModels;

namespace SV20T1080063.WebMVC.Models
{
    public class PaginationSearchCategory : PaginationSearchBaseResult
    {

        public IList<Category>? Data { get; set; }
    }
}
