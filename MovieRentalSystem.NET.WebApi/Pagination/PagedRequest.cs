namespace MovieRentalSystem.NET.WebApi.Pagination
{
    public class PagedRequest
    {
        const int MaxPageSize = 50;
        private const int DefaultPageSize = 10;
        private const int DefaultPageNumber = 1;
        private int _pageNumber = DefaultPageNumber;
        private int _pageSize = DefaultPageSize;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? DefaultPageNumber : value;
        }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (value < 1)
                {
                    _pageSize = DefaultPageSize;
                }
                else if (value > MaxPageSize)
                {
                    _pageSize = MaxPageSize;
                }
                else
                {
                    _pageSize = value;
                }
            }
        }
    }
}
