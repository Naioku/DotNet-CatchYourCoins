using System.ComponentModel.DataAnnotations;

namespace MVC.Models.Account;

public class Login
{
    [Required]
    [EmailAddress]
    public required string Email { get; init; }
    
    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; init; }
}