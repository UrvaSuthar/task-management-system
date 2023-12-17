// TasksController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task_management_system;
using task_management_system.Models;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
	private readonly AppDbContext _context;

	public TasksController(AppDbContext context)
	{
		_context = context;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<TaskModel>>> GetTasks()
	{
		return await _context.Tasks.ToListAsync();
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<TaskModel>> GetTask(int id)
	{
		var task = await _context.Tasks.FindAsync(id);

		if (task == null)
		{
			return NotFound();
		}

		return task;
	}

	[HttpPost]
	public async Task<ActionResult<TaskModel>> PostTask(TaskModel task)
	{
		try
		{
			_context.Tasks.Add(task);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
		}
		catch (Exception ex)
		{
			// Log the exception details
			Console.WriteLine(ex.Message);
			throw; // rethrow the exception or handle it appropriately
		}
	}

	// Add other CRUD actions...

	[HttpPut("{id}")]
	public async Task<IActionResult> PutTask(int id, TaskModel task)
	{
		var existingTask = await _context.Tasks.FindAsync(id);

		if (existingTask == null)
		{
			return NotFound();
		}

		existingTask.Title = task.Title;
		existingTask.Description = task.Description;
		existingTask.CreatedAt = task.CreatedAt;
		existingTask.IsActive = task.IsActive;

		await _context.SaveChangesAsync();

		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteTask(int id)
	{
		var task = await _context.Tasks.FindAsync(id);

		if (task == null)
		{
			return NotFound();
		}

		_context.Tasks.Remove(task);
		await _context.SaveChangesAsync();

		return NoContent();
	}
}
