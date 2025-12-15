using DogeCoiner.Data.Dtos;
using System.Data;

namespace DogeCoiner.Data.DAL
{
    public class UserDataTableBuilder : IDataTableBuilder
    {
        public User[] _items;

        public UserDataTableBuilder(User[] items)
        {
            _items = items;
        }

        public DataTable Build()
        {
            var dt = BuildTable();
            BuildData(dt);

            return dt;
        }

        public DataTable BuildTable()
        {
            var dt = new DataTable();
            var cols = dt.Columns;
            cols.Add(nameof(User.UserId));
            cols.Add(nameof(User.Username));
            cols.Add(nameof(User.IsRegistered));

            return dt;
        }

        public void BuildData(DataTable dt)
        {
            foreach (var item in _items)
            {
                {
                    dt.Rows.Add(
                        item.UserId,
                        item.Username,
                        item.IsRegistered);
                }
            }
        }
    }
}
