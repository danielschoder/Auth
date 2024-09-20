using Auth.DAL;
using Auth.ApiEndpoints;
using Auth.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Database

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserAccessor, UserAccessor>();

#endregion

builder.Services.AddCors();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapApiEndpoints();

app.Run();

// This line is needed for NUnit WebApplication integration tests:
public partial class Program { }
