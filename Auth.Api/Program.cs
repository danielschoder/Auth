using Auth.Api.Endpoints;
using Auth.Api.Extensions;
using Auth.Application.Services.Handlers.QueryHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetVersion).Assembly));
builder.Services.AddInfrastructureServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyPolicy", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});

var app = builder.Build();

app.MapVersionEndpoints();
app.MapAuthEndpoints();

app.UseCors("AllowAnyPolicy");
app.UseHttpsRedirection();

app.Run();

public partial class Program { } // For NUnit WebApplication integration tests
