using Moq;
using Moq.Language.Flow;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace DogeCoiner.Data.Bitunix.Tests
{
    public static class MoqExtensions
    {
        public static ISetup<HttpMessageHandler, Task<HttpResponseMessage>> SetupSendAsync(this Mock<HttpMessageHandler> handler, HttpMethod requestMethod, string requestUrl)
        {
            return handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Method == requestMethod &&
                    r.RequestUri != null &&
                    r.RequestUri.ToString() == requestUrl),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        public static IReturnsResult<HttpMessageHandler> ReturnsHttpResponseAsync(this ISetup<HttpMessageHandler, Task<HttpResponseMessage>> moqSetup, object? responseBody, HttpStatusCode responseCode)
        {
            var serializedResponse = JsonSerializer.Serialize(responseBody);
            return ReturnsHttpResponseAsync(moqSetup, serializedResponse, responseCode);
        }

        public static IReturnsResult<HttpMessageHandler> ReturnsHttpResponseAsync(this ISetup<HttpMessageHandler, Task<HttpResponseMessage>> moqSetup, string json, HttpStatusCode responseCode)
        {
            var stringContent = new StringContent(json ?? string.Empty);

            var responseMessage = new HttpResponseMessage
            {
                StatusCode = responseCode,
                Content = stringContent
            };

            return moqSetup.ReturnsAsync(responseMessage);
        }
    }
}
