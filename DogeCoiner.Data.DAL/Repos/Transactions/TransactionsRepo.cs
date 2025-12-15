using Dapper;
using DogeCoiner.Data.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace DogeCoiner.Data.DAL.Repos.Transactions
{
    public interface ITransactionsRepo
    {
        void Save(Transaction[] items);
    }

    public class TransactionsRepo : ITransactionsRepo
    {
        private string _connStr;

        public TransactionsRepo(IOptions<DogeCoinerDataSettings> opts)
        {
            _connStr = opts.Value.ConnectionString;
        }

        public void Save(Transaction[] items)
        {
            using var dbConn = new SqlConnection(_connStr);

            var dt = new TransactionDataTableBuilder(items).Build();

            var res = dbConn.Query(
                "dbo.UpsertTransactions", 
                new { Transactions = dt.AsTableValuedParameter() }, 
                commandType: CommandType.StoredProcedure);
        }
    }
}