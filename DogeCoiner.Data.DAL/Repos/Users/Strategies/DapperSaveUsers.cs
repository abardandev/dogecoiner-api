using Dapper;
using DogeCoiner.Data.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace DogeCoiner.Data.DAL.Repos.Users.Strategies
{
    public class DapperSaveUsers : ISaveUsers
    {
        private readonly string _connStr;

        public DapperSaveUsers(IOptions<DogeCoinerSettings> opts)
        {
            _connStr = opts.Value.ConnectionString;
        }

        public async Task SaveAsync(User[] users)
        {
            using var dbConn = new SqlConnection(_connStr);

            var dt = new UserDataTableBuilder(users).Build();

            var res = await dbConn.QueryAsync(
                "dbo.UpsertUsers",
                new { Users = dt.AsTableValuedParameter() },
                commandType: CommandType.StoredProcedure);
        }
    }
}