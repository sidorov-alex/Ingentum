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
	}
}
