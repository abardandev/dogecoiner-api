using DogeCoiner.WebApi.Extensions;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add DogeCoiner services
builder.Services.AddDogeCoiner(builder.Configuration);
builder.Services.AddDogeCoinerSecurity(builder.Configuration);

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

if (app.Environment.IsDevelopment())
{
    app.UseHangfireDashboard("/hangfire");
}

app.MapGet("/", context => 
{ 
    context.Response.Redirect("/status", true);
    return Task.CompletedTask;
});

app.Run();
