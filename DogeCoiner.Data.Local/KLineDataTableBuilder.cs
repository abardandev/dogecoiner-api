using DogeCoiner.Data.Dtos;
using System.Data;

namespace DogeCoiner.Data.Local
{
    public class KLineDataTableBuilder : IDataTableBuilder
    {
        public KLine[] _items;

        public KLineDataTableBuilder(KLine[] items)
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
            cols.Add(nameof(KLine.ID));
            cols.Add(nameof(KLine.Symbol));
            cols.Add(nameof(KLine.Interval));
            cols.Add(nameof(KLine.Timestamp));
            cols.Add(nameof(KLine.OpenPrice));
            cols.Add(nameof(KLine.HighPrice));
            cols.Add(nameof(KLine.LowPrice));
            cols.Add(nameof(KLine.ClosePrice));
            cols.Add(nameof(KLine.Volume));

            return dt;
        }

        public void BuildData(DataTable dt)
        {
            foreach (var item in _items)
            {
                {
                    dt.Rows.Add(
                        item.ID,
                        item.Symbol,
                        item.Interval,
                        item.Timestamp,
                        item.OpenPrice,
                        item.HighPrice,
                        item.LowPrice,
                        item.ClosePrice,
                        item.Volume);
                }
            }
        }
    }
}
