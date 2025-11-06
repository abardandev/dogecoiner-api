using DogeCoiner.Data.Bitunix.Dtos;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace DogeCoiner.Data.Bitunix
{
    public interface ICoinDataClient
    {
        Uri BuildRequestUri(KLineRequest req);
        string BuildQueryParams(KLineRequest req);
        Task<KLineApiResponse> GetKLineHistory(KLineRequest req);
    }

    public class CoinDataClient : ICoinDataClient
    {
        private CoinDataApiSettings _apiConfig;
        private readonly HttpClient _httpClient;

        public CoinDataClient(IOptions<CoinDataApiSettings> apiConfig, HttpClient httpClient)
        {
            _apiConfig = apiConfig.Value;
            _httpClient = httpClient;
        }

        public async Task<KLineApiResponse> GetKLineHistory(KLineRequest req)
        {
            var uri = BuildRequestUri(req);
            var res = await _httpClient.GetAsync(uri);

            var result = new KLineApiResponse();

            var data = await res.Content.ReadAsStringAsync();
            result = JsonSerializer.Deserialize<KLineApiResponse>(data);
            result.StatusCode = res.StatusCode;

            return result;
        }

        public Uri BuildRequestUri(KLineRequest req)
        {
            var builder = new UriBuilder(_apiConfig.ApiBaseUri);
            builder.Path = _apiConfig.KLineHistoryUri;
            builder.Query = BuildQueryParams(req);
            return builder.Uri;
        }

        public string BuildQueryParams(KLineRequest req)
        {
            var query = new Dictionary<string, string>
            {
                { "symbol", req.Symbol },
                { "interval", req.Interval }
            };

            if (req.EndTime.HasValue)
            {
                query.Add("endTime", req.EndTime.ToString());
            }

            if (req.Limit.HasValue)
            {
                query.Add("limit", req.Limit.ToString());
            }
            return string.Join("&", query.Select(o => $"{o.Key}={o.Value}"));
        }
    }
}
