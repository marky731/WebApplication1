using EntityLayer.Dtos;

namespace EntityLayer.ApiResponse
{
    public class ApiResponse<T>
    {
        public bool Result { get; set; } 
        public string Message { get; set; }
        public T? Data { get; set; }    

        public ApiResponse(bool result, string message, T? data)
        {
            Result = result;
            Message = message;
            Data = data;
        }
    }

    public class PaginatedResponse<T> 
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public T Data { get; set; }

        public PaginatedResponse(T data, int currentPage, int pageSize, int totalCount)
        {
            Data = data;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}