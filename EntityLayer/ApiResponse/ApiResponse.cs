using EntityLayer.Dtos;

namespace EntityLayer.ApiResponse
{
    public class ApiResponse<T>
    {
        private bool v;
        private object value;

        public PaginationInfo Pagination { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponse(bool success, string message, T data, int currentPage, int pageSize, int totalCount)
        {
            Pagination = new PaginationInfo
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
            Success = success;
            Message = message;
            Data = data;
        }

        public ApiResponse(bool success, string message, T data, List<string> validationErrors)
        {
            Success = success;
            Message = message;
            Data = data;        }

        public ApiResponse(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }

    public class PaginationInfo
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}