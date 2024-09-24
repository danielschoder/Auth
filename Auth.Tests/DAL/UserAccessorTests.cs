using Auth.DAL;
using Auth.Data;
using Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Tests.DAL;

[TestFixture]
public class UserServiceTests
{
    private DbContextOptions<ApplicationDbContext> _dbContextOptions;
    private ApplicationDbContext _applicationDbContext;
    private UserAccessor _userAccessor;

    [SetUp]
    public void SetUp()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _applicationDbContext = new ApplicationDbContext(_dbContextOptions);

        _applicationDbContext.Database.EnsureDeleted();
        _applicationDbContext.Database.EnsureCreated();

        _userAccessor = new UserAccessor(_applicationDbContext);
    }

    [Test]
    [TestCase("existinguser@example.com", true)]
    [TestCase("nonexistinguser@example.com", false)]
    public async Task GetByEmailAsync_Should_Find_User_In_Db(string email, bool shouldExist)
    {
        var existingUser = new User
        {
            Id = new Guid(),
            Email = "existinguser@example.com",
            PasswordHash = "hashedpassword123"
        };

        _applicationDbContext.AUTH_Users.Add(existingUser);
        await _applicationDbContext.SaveChangesAsync();

        var result = await _userAccessor.GetByEmailAsync(email);

        Assert.That(result, shouldExist ? Is.Not.Null : Is.Null);
        if (shouldExist)
        {
            Assert.Multiple(() =>
            {
                Assert.That(result.Email, Is.EqualTo("existinguser@example.com"));
                Assert.That(result.PasswordHash, Is.EqualTo("hashedpassword123"));
            });
        }
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetAnyAsync_Should_Return_True_False(bool userTableHasEntries)
    {
        if (userTableHasEntries)
        {
            var existingUser = new User
            {
                Id = new Guid(),
                Email = "existinguser@example.com",
                PasswordHash = "hashedpassword123"
            };

            _applicationDbContext.AUTH_Users.Add(existingUser);
            await _applicationDbContext.SaveChangesAsync();
        }

        var result = await _userAccessor.AnyAsync();

        Assert.That(result, Is.EqualTo(userTableHasEntries));
    }

    [TearDown]
    public void TearDown()
    {
        _applicationDbContext.Dispose();
    }
}
