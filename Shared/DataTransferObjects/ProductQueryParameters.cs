
using Shared.DataTransferObjects.Products;

namespace Shared.DataTransferObjects
{
    public class ProductQueryParameters
    {
        private const int DefaultPageSize = 12;
        private const int MaxPageSize = 50;

        private int _pageSize = DefaultPageSize;
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public ProductSortingOptions Sort { get; set; } 
        public string? Search { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize
        {
            get => _pageSize;
            
            set
            {
                if (value > MaxPageSize)
                {
                    _pageSize = MaxPageSize; // Clamp to the max
                }
                else if (value > 0)
                {
                    _pageSize = value; // Use the provided value
                }
                else
                {
                    _pageSize = DefaultPageSize; // Fallback for invalid values
                }
            }
        }
    }
}
