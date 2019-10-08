using InstructorsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstructorsApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InstructorController : Controller
	{
		private InstructorsAppDbContext dbContext;

		public InstructorController(InstructorsAppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		// GET: api/instructor
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Instructor>>> GetInstructors()
		{
			return await this.dbContext.Instructors.ToListAsync();
		}

		// GET: api/instructor/1
		[HttpGet("{id:int}")]
		public async Task<ActionResult<Instructor>> GetInstructor(int id)
		{
			if (id <= 0)
			{
				return BadRequest();
			}
			
			var instructor = await this.dbContext.Instructors.FindAsync(id);

			if (instructor == null)
			{
				return NotFound(); // 404
			}

			return instructor;
		}

		// POST: api/instructor
		[HttpPost]
		public async Task<ActionResult<Instructor>> AddInstructor([FromBody] Instructor instructor)
		{
			this.dbContext.Instructors.Add(instructor);

			await this.dbContext.SaveChangesAsync();
			
			return CreatedAtAction(nameof(GetInstructor), new { id = instructor.Id }, instructor); // 201
		}

		// PUT: api/instructor/1
		[HttpPut("{id:int}")]
		public async Task<ActionResult> UpdateInstructor(int id, [FromBody] Instructor instructor)
		{
			if (id <= 0 || id != instructor.Id)
			{
				return BadRequest();
			}

			// According to the HTTP specification, a PUT request requires the client to send the entire updated entity, not just the changes.

			this.dbContext.Entry(instructor).State = EntityState.Modified;

			try
			{
				await this.dbContext.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException exc)
			{
				// If record does not exists next exception is thrown
				// DbUpdateConcurrencyException: Attempted to update or delete an entity that does not exist in the store.

				return NotFound(exc.Message); // 404
			}

			// https://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html
			// If an existing resource is modified, either the 200 (OK) or 204 (No Content) response codes SHOULD be sent to indicate successful completion of the request.

			return NoContent(); // 204
		}

		// DELETE: api/instructor/1
		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeleteInstructor(int id)
		{
			if (id <= 0)
			{
				return BadRequest();
			}

			// Пытаемся найти запись с указанным идентификатором. Используем метод Find, который может вернуть запись из памяти,
			// не обращаясь к БД, если она уже загружена в контекст.

			var instructor = await this.dbContext.Instructors.FindAsync(id);

			if (instructor == null)
			{
				return NotFound(); // 404
			}

			// Удаляем запись.

			this.dbContext.Instructors.Remove(instructor);

			await this.dbContext.SaveChangesAsync();

			// https://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html
			// A successful response SHOULD be 200 (OK) if the response includes an entity describing the status, 202 (Accepted) if the action has not yet been enacted, or 204 (No Content) if the action has been enacted but the response does not include an entity. 

			return NoContent(); // 204
		}
	}
}
