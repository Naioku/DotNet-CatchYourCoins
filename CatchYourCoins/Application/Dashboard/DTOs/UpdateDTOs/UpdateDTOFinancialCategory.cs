namespace Application.Dashboard.DTOs.UpdateDTOs;

public abstract class UpdateDTOFinancialCategory : IInputDTODashboardEntity
{
    public required int Id { get; init; }

    public Optional<string> Name { get; private set; } = new();
    public Optional<decimal?> Limit { get; private set; } = new();

    public string SetName
    {
        init => Name = new Optional<string>(value);
    }

    public decimal? SetLimit
    {
        init => Limit = new Optional<decimal?>(value);
    }
}