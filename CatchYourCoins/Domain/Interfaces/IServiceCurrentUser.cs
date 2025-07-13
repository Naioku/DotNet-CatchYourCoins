using Domain.Dashboard.Entities;

namespace Domain.Interfaces;

public interface IServiceCurrentUser
{
    CurrentUser User { get; }
}