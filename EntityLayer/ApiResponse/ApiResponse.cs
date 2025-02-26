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

}