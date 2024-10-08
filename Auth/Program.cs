using Auth.DAL;
using Auth.ApiEndpoints;
using Auth.Data;
using Microsoft.EntityFrameworkCore;
using Auth.BLL;
using Auth.Helpers;

var builder = WebApplication.CreateBuilder(args);

#region Database

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

builder.Services.AddScoped<IUserAccessor, UserAccessor>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<TokenProvider, TokenProvider>();

builder.Services.AddCors();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapApiEndpoints();

app.Run();

// This line is needed for NUnit WebApplication integration tests:
public partial class Program { }
