using Dapper;
using DogeCoiner.Data.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace DogeCoiner.Data.DAL.Repos.Users
{
    public interface IUsersRepo
    {
        void Save(User[] items);
    }

    public class UsersRepo : IUsersRepo
    {
        private string _connStr;

        public UsersRepo(IOptions<DogeCoinerDataSettings> opts)
        {
            _connStr = opts.Value.ConnectionString;
        }

        public void Save(User[] items)
        {
            using var dbConn = new SqlConnection(_connStr);

            var dt = new UserDataTableBuilder(items).Build();

            var res = dbConn.Query(
                "dbo.UpsertUsers", 
                new { Users = dt.AsTableValuedParameter() }, 
                commandType: CommandType.StoredProcedure);
        }
    }
}