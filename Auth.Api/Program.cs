using Auth.Api.Endpoints;
using Auth.Api.Extensions;
using Auth.Application.Services.Handlers.QueryHandlers;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddAuthorizationServices(builder.Configuration);

builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetVersion).Assembly));
builder.Services.AddInfrastructureServices();
builder.Services.AddExternalServices();

var app = builder.Build();

app.UseForwardedHeaders();
app.UseCors("AllowAnyPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapServiceEndpoints();
app.MapVisitorsEndpoints();
app.MapUsersEndpoints();

app.UseHttpsRedirection();

app.Run();

public partial class Program { } // For NUnit WebApplication integration tests
