using DogeCoiner.Data.DAL;
using DogeCoiner.WebApi.Authentication;
using DogeCoiner.WebApi.Configuration;
using DogeCoiner.WebApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var dbConn =
    builder.Configuration.GetConnectionString("DogeCoinerDb")
        ?? throw new InvalidOperationException("Connection string 'DogeCoinerDb' not found.");

builder.Services.AddDbContext<CoinDataDbContext>(options =>
    options.UseSqlServer(dbConn));

// Configure JWE settings
builder.Services.Configure<JweSettings>(
    builder.Configuration.GetSection(JweSettings.SectionName));

// Register JWE decryption service
builder.Services.AddScoped<IJweDecryptionService, JweDecryptionService>();

// Configure authentication with JWE handler
builder.Services.AddAuthentication("JweBearer")
    .AddScheme<AuthenticationSchemeOptions, JweAuthenticationHandler>("JweBearer", null);

builder.Services.AddAuthorization();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
