using System.ComponentModel.DataAnnotations;

namespace InstructorsApp.Models
{
	public class Instructor
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string MiddleName { get; set; }

		[Required]
		public string LastName { get; set; }
	}
}
