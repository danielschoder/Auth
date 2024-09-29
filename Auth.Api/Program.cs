using Auth.Api.Endpoints;
using Auth.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();

var app = builder.Build();

app.MapVersionEndpoints();
app.Run();

public partial class Program { } // For NUnit WebApplication integration tests
