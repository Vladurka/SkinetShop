namespace Core.Specifications
{
    public class ProductSpecParams
    {
        private const int MAX_PAGE_SIZE = 24;
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 24;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value;
        }

        private List<string> _brands = [];
        public List<string> Brands
        {
            get => _brands;
            set => _brands = value.SelectMany(x => x.Split(",",
                StringSplitOptions.RemoveEmptyEntries)).ToList();
        }

        private List<string> _types = [];
        public List<string> Types
        {
            get => _types;
            set => _types = value.SelectMany(x => x.Split(",",
                StringSplitOptions.RemoveEmptyEntries)).ToList();
        }

        public string? Sort { get; set; }

        private string? _search;

        public string Search
        {
            get => _search ?? "";
            set => _search = value.ToLower();
        }
    }
}
