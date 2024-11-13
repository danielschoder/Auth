using Auth.Api.Endpoints;
using Auth.Api.Extensions;
using Auth.Application.Services.Handlers.QueryHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetVersion).Assembly));
builder.Services.AddInfrastructureServices();

var app = builder.Build();

app.MapVersionEndpoints();

app.UseHttpsRedirection();

app.Run();

public partial class Program { } // For NUnit WebApplication integration tests
