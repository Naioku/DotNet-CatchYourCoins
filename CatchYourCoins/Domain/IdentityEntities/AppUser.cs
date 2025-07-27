using Domain.Dashboard.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain.IdentityEntities;

public class AppUser : IdentityUser<Guid>
{
    public ICollection<CategoryExpenses> CategoriesExpenses { get; } = new List<CategoryExpenses>();
    public ICollection<PaymentMethod> PaymentMethods { get; } = new List<PaymentMethod>();
    public ICollection<Expense> Expenses { get; } = new List<Expense>();
    public ICollection<CategoryIncomes> CategoriesIncomes { get; } = new List<CategoryIncomes>();
    public ICollection<Income>? Incomes { get; } = new List<Income>();
}