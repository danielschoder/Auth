using Auth.Api.Endpoints;
using Auth.Api.Extensions;
using Auth.Application.Services.Handlers.QueryHandlers;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetVersion).Assembly));
builder.Services.AddInfrastructureServices();
builder.Services.AddExternalServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyPolicy", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();

app.UseForwardedHeaders();

app.MapAuthEndpoints();
app.MapVersionEndpoints();
app.MapVisitorsEndpoints();

app.UseCors("AllowAnyPolicy");
app.UseHttpsRedirection();

app.Run();

public partial class Program { } // For NUnit WebApplication integration tests
