using Dapper;
using DogeCoiner.Data.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace DogeCoiner.Data.Local
{
    public interface IKLineRepo
    {
        void Save(KLine[] items);
    }

    public class KLineRepo : IKLineRepo
    {
        private string _connStr;

        public KLineRepo(IOptions<DogeCoinerDataSettings> opts)
        {
            _connStr = opts.Value.ConnectionString;
        }

        public void Save(KLine[] items)
        {
            using var dbConn = new SqlConnection(_connStr);

            var dt = new KLineDataTableBuilder(items).Build();

            var res = dbConn.Query(
                "dbo.UpsertKLineItems", 
                new { KLineItems = dt.AsTableValuedParameter() }, 
                commandType: CommandType.StoredProcedure);
        }
    }
}