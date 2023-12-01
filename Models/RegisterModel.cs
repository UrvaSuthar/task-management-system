using System.ComponentModel.DataAnnotations;

namespace task_management_system;

public class RegisterModel
{
	
[Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
