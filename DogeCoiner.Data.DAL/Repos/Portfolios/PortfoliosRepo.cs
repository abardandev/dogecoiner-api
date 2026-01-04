using Dapper;
using DogeCoiner.Data.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace DogeCoiner.Data.DAL.Repos.Portfolios
{
    public interface IPortfoliosRepo
    {
        void Save(Portfolio[] items);
    }

    public class PortfoliosRepo : IPortfoliosRepo
    {
        private string _connStr;

        public PortfoliosRepo(IOptions<DogeCoinerSettings> opts)
        {
            _connStr = opts.Value.ConnectionString;
        }

        public void Save(Portfolio[] items)
        {
            using var dbConn = new SqlConnection(_connStr);

            var dt = new PortfolioDataTableBuilder(items).Build();

            var res = dbConn.Query(
                "dbo.UpsertPortfolios", 
                new { Portfolios = dt.AsTableValuedParameter() }, 
                commandType: CommandType.StoredProcedure);
        }
    }
}