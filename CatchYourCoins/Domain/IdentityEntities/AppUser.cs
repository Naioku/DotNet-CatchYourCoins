using Domain.Dashboard.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain.IdentityEntities;

public class AppUser : IdentityUser<Guid>
{
    public ICollection<Expense> Expenses { get; } = new List<Expense>();
    public ICollection<CategoryExpenses> Categories { get; } = new List<CategoryExpenses>();
    public ICollection<PaymentMethod> PaymentMethods { get; } = new List<PaymentMethod>();
}