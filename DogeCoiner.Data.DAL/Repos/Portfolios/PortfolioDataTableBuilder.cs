using DogeCoiner.Data.Dtos;
using System.Data;

namespace DogeCoiner.Data.DAL
{
    public class PortfolioDataTableBuilder : IDataTableBuilder
    {
        public Portfolio[] _items;

        public PortfolioDataTableBuilder(Portfolio[] items)
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
            cols.Add(nameof(Portfolio.PortfolioId));
            cols.Add(nameof(Portfolio.UserId));
            cols.Add(nameof(Portfolio.PortfolioName));
            
            return dt;
        }

        public void BuildData(DataTable dt)
        {
            foreach (var item in _items)
            {
                dt.Rows.Add(
                    item.PortfolioId,
                    item.UserId,
                    item.PortfolioName);
            }
        }
    }
}
