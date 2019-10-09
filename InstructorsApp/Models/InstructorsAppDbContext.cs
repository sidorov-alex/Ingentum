using Microsoft.EntityFrameworkCore;

namespace InstructorsApp.Models
{
	public class InstructorsAppDbContext : DbContext
	{
		public DbSet<Instructor> Instructors { get; set; }

		public InstructorsAppDbContext(DbContextOptions<InstructorsAppDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Instructor>().HasData(
			   new Instructor { Id = 1, FirstName = "Michelangelo", MiddleName = "", LastName = "Buonarroti" },
			   new Instructor { Id = 2, FirstName = "Leonardo", MiddleName = "", LastName = "da Vinci" },
			   new Instructor { Id = 3, FirstName = "Lorenzo", MiddleName = "", LastName = "Ghiberti" }
			);
		}
	}
}
