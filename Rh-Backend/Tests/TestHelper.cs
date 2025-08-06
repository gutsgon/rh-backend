using Microsoft.EntityFrameworkCore;
using Rh_Backend.Data; 

public class TestHelper
{
    public static AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "RhTestDb")
            .Options;

        var context = new AppDbContext(options);

        // Se quiser, pode limpar o banco antes:
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        return context;
    }
}
