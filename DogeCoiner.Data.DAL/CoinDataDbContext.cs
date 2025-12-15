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
            BuildUsers(modelBuilder);
            BuildPortfolios(modelBuilder);
            BuildTransactions(modelBuilder);
            BuildKLines(modelBuilder);
        }

        private void BuildUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users", "dbo");

                entity.HasKey(e => e.UserId);

                entity.Property(e => e.Username)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(e => e.IsRegistered)
                    .HasColumnType("bit");

                entity.HasMany(e => e.Portfolios)
                    .WithOne(e => e.User);
            });
        }

        private void BuildPortfolios(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity <Portfolio>(entity =>
            {
                entity.ToTable("Portfolios", "dbo");

                entity.HasKey(e => e.PortfolioId);

                entity.Property(e => e.UserId)
                    .HasColumnType("bigint")
                    .IsRequired();

                entity.Property(e => e.PortfolioName)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.HasMany(e => e.Transactions)
                    .WithOne(e => e.Portfolio);
            });
        }

        private void BuildTransactions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transactions", "dbo");

                entity.HasKey(e => e.TransactionId);

                entity.Property(e => e.PortfolioId)
                    .HasColumnType("bigint")
                    .IsRequired();

                entity.Property(e => e.Symbol)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(e => e.TransactionType)
                    .HasMaxLength(4)
                    .IsRequired();

                entity.Property(e => e.Quantity)
                    .HasColumnType("decimal(24,12)")
                    .IsRequired();

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(24,12)")
                    .IsRequired();

                entity.Property(e => e.TimestampUtc)
                    .HasColumnType("datetime2")
                    .IsRequired();

                entity.HasOne(e => e.Portfolio)
                    .WithMany(e => e.Transactions);
            });
        }

        private void BuildKLines(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KLine>(entity =>
            {
                entity.ToTable("KLines", "dbo");

                entity.HasKey(e => e.KLineId);

                entity.Property(e => e.Symbol)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(e => e.Interval)
                    .HasColumnType("varchar(1)")
                    .IsRequired();

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
