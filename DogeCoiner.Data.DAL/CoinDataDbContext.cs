using DogeCoiner.Data.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DogeCoiner.Data.DAL
{
    public class CoinDataDbContext : DbContext
    {
        public DbSet<KLine> KLines { get; set; }

        public CoinDataDbContext(DbContextOptions<CoinDataDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KLine>(entity =>
            {
                entity.ToTable("KLines", "dbo");

                entity.HasKey(e => e.KLineId);

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Interval)
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.TimestampUtc)
                    .HasColumnType("datetime2")
                    .IsRequired();

                entity.Property(e => e.OpenPrice)
                    .HasColumnType("decimal(24,12)")
                    .IsRequired();

                entity.Property(e => e.HighPrice)
                    .HasColumnType("decimal(24,12)")
                    .IsRequired();

                entity.Property(e => e.LowPrice)
                    .HasColumnType("decimal(24,12)")
                    .IsRequired();

                entity.Property(e => e.ClosePrice)
                    .HasColumnType("decimal(24,12)")
                    .IsRequired();

                entity.Property(e => e.Volume)
                    .HasColumnType("decimal(24,8)")
                    .IsRequired();

                entity.HasIndex(e => new { e.Symbol, e.Interval, e.TimestampUtc })
                    .IsUnique();
            });
        }
    }
}
