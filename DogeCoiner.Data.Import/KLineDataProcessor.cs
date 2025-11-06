using DogeCoiner.Data.Bitunix.Dtos;
using System.Net;

namespace DogeCoiner.Data.Import
{
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
}
