using DogeCoiner.Data.Local;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var dbConn =
    builder.Configuration.GetConnectionString("DogeCoinerDb")
        ?? throw new InvalidOperationException("Connection string 'DogeCoinerDb' not found.");

builder.Services.AddDbContext<CoinDataDbContext>(options =>
    options.UseSqlServer(dbConn));

builder.Services.AddCors(opts =>
{
    opts.AddDefaultPolicy(pol => pol.AllowAnyOrigin());
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
