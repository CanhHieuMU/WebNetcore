using SV20T1080063.DomainModels;

namespace SV20T1080063.WebMVC.Models
{
    public class PaginationSearchSupplier : PaginationSearchBaseResult
    {
        public IList<Supplier>? Data { get; set; }
    }
}
