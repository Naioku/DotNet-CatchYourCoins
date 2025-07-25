using Domain.Dashboard.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain.IdentityEntities;

public class AppUser : IdentityUser<Guid>
{
    public ICollection<Expense> Expenses { get; } = new List<Expense>();
    public ICollection<Category> Categories { get; } = new List<Category>();
    public ICollection<PaymentMethod> PaymentMethods { get; } = new List<PaymentMethod>();
}