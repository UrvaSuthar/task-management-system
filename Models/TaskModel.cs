namespace task_management_system;

public class TaskModel
{
	public int Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public bool IsActive { get; set; }
	public DateTime CreatedAt { get; set; }
}
