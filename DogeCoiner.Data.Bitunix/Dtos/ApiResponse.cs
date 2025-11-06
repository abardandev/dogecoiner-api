using System.Net;

namespace DogeCoiner.Data.Bitunix.Dtos
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        
        public string code { get; set; }

        public string msg { get; set; }

        public bool success { get; set; }
    }
}
