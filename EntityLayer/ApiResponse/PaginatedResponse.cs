namespace EntityLayer.ApiResponse
{
    public class PaginatedResponse<T> 
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public T PaginatedData { get; set; }

        public PaginatedResponse(T paginatedData, int currentPage, int pageSize, int totalCount)
        {
            PaginatedData = paginatedData;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}