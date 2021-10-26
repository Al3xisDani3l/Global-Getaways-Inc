namespace GG.Api.Responses
{
    public class ApiResponse
    {
        public ApiResponse(object data)
        {
            Data = data;
        }

        public object Data { get; set; }
    }
}
