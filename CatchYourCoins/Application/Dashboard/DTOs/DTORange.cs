namespace Application.Dashboard.DTOs;

public class DTORange<TDTO>
{
    public required List<TDTO> Items { get; init; }
}