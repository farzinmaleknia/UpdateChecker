using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<SecurityQuestion>().HasData(
        new SecurityQuestion
        {
          Id = 1,
          Question = "My Favorite Animal"
        },
        new SecurityQuestion
        {
          Id = 2,
          Question = "My Favorite Country"
        },
        new SecurityQuestion
        {
          Id = 3,
          Question = "My Favorite Color"
        },
        new SecurityQuestion
        {
          Id = 4,
          Question = "My Favorite Car"
        },
        new SecurityQuestion
        {
          Id = 5,
          Question = "My favorite person"
        }
      );

      modelBuilder.Entity<SecurityQuestion>(entity =>
      {
        entity.Property(p => p.Question).HasMaxLength(50).IsUnicode(true);
        entity.Property(p => p.Answer).HasMaxLength(50).IsUnicode(true);
      });
    }
    public DbSet<SecurityQuestion> SecurityQuestions { get; set; }
  }
}

