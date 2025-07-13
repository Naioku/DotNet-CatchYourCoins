using Domain.Dashboard.Entities;

namespace Domain.Interfaces.Services;

public interface IServiceCurrentUser
{
    CurrentUser User { get; }
}