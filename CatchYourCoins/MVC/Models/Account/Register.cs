using System.ComponentModel.DataAnnotations;

namespace MVC.Models.Account;

public class Register
{
    [Required]
    public required string Name { get; init; }
    
    [Required]
    [EmailAddress]
    public required string Email { get; init; }
    
    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; init; }
    
    [Required]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public required string ConfirmPassword { get; init; }
}