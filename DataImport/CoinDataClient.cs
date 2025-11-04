using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace DataImport
{
    public class DataImportConfig
    {
        public KLineRequest[] Requests {  get; set; }
    }

    public class KLineDataProcessor
    {
        public DataImportConfig Config { get; set; }

        public HttpStatusCode ProcessConfig()
        {
            // for each config
            // check missing data - local ctx
            // get kline data - api
            // save data diff - local ctx

            return HttpStatusCode.OK;
        }

        public void CheckMissingData(KLineRequest request)
        {

        }

        public void SaveKLineData(KLineApiResponse response)
        {

        }
    }

    public interface ICoinDataClient
    {
        Uri BuildRequestUri(KLineRequest req);
        string BuildQueryParams(KLineRequest req);
        Task<KLineApiResponse> GetKLineHistory(KLineRequest req);
    }

    public class CoinDataApiSettings
    {
        public string ApiBaseUri { get; set; }
        public string KLineHistoryUri { get; set; }
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

    public class KLineItem
    {
        public string Symbol { get; set; }

        public string Interval { get; set; }

        public DateTime Timestamp { get; set; }

        public string Open { get; set; }

        public string High { get; set; }

        public string Low { get; set; }

        public string Close { get; set; }

        public string Volume { get; set; }
    }

    public class KLineRequest
    {
        public string Symbol { get; set; }
        public string Interval { get; set; }
        public long? EndTime { get; set; }
        public int? Limit { get; set; }

        public KLineRequest(string symbol, string interval, long? endTime = null, int? limit = null)
        {
            Symbol = symbol;
            Interval = interval;
            EndTime = endTime;
            Limit = limit;
        }
    }

    public class KLineApiResponse : ApiResponse
    {
        public KLineApiItem[] data { get; set; }
    }

    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        
        public string code { get; set; }

        public string msg { get; set; }

        public bool success { get; set; }
    }

    public class KLineApiItem
    {
        public string symbol { get; set; }

        public DateTime ts { get; set; }

        public string open {  get; set; }

        public string high { get; set; }

        public string low { get; set; }

        public string close { get; set; }

        public string volume { get; set; }
    }
}
