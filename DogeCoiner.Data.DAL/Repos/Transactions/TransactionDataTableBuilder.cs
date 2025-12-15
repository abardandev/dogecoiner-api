using DogeCoiner.Data.Dtos;
using System.Data;

namespace DogeCoiner.Data.DAL
{
    public class TransactionDataTableBuilder : IDataTableBuilder
    {
        public Transaction[] _items;

        public TransactionDataTableBuilder(Transaction[] items)
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

            // add RowId NOT NULL IDENTITY(1,1)
            // transactions don't have unique keys to match new TransactionIds to
            // so we use an autoincrement column to track uniqueness
            cols.Add(new DataColumn 
            {
                ColumnName = "RowId",
                AutoIncrement = true,
                AutoIncrementSeed = 1,
                AutoIncrementStep = 1,
                AllowDBNull = false,
                Unique = true
            });

            cols.Add(nameof(Transaction.TransactionId));
            cols.Add(nameof(Transaction.PortfolioId));
            cols.Add(nameof(Transaction.Symbol));
            cols.Add(nameof(Transaction.TransactionType));
            cols.Add(nameof(Transaction.Quantity));
            cols.Add(nameof(Transaction.Price));
            cols.Add(nameof(Transaction.TimestampUtc));

            return dt;
        }

        public void BuildData(DataTable dt)
        {
            foreach (var item in _items)
            {
                dt.Rows.Add(
                    DBNull.Value,
                    item.TransactionId,
                    item.PortfolioId,
                    item.Symbol,
                    item.TransactionType,
                    item.Quantity,
                    item.Price,
                    item.TimestampUtc);
            }
        }
    }
}
