namespace Domain.Dashboard.Entities.Expenses;

public sealed class Expense : FinancialOperation<ExpenseCategory>
{
    public int? PaymentMethodId { get; init; }
    public ExpensePaymentMethod? PaymentMethod { get; init; }

    private bool Equals(Expense other) =>
        base.Equals(other) &&
        PaymentMethodId == other.PaymentMethodId &&
        Equals(PaymentMethod, other.PaymentMethod);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Expense)obj);
    }

    public override int GetHashCode() => HashCode.Combine(
        base.GetHashCode(),
        PaymentMethodId,
        PaymentMethod
    );
}