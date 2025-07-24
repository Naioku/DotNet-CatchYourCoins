using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Tests.Factories;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Services;
using Moq;
using Xunit;

namespace Application.Tests;

public abstract class CQRSHandlerTestBase<THandler> : IAsyncLifetime
    where THandler : class
{
    private readonly Dictionary<Type, object> _mocks = new();

    protected THandler Handler { get; private set; }

    protected Mock<T> GetMock<T>() where T : class => _mocks[typeof(T)] as Mock<T>;
    protected void RegisterMock<T>() where T : class => _mocks[typeof(T)] = new Mock<T>();

    protected void RegisterMock<T, TMock>(TMock mock)
        where T : class
        where TMock : Mock<T>
        => _mocks[typeof(T)] = mock;

    public virtual Task InitializeAsync()
    {
        MockServiceCurrentUser();
        Handler = CreateHandler();
        return Task.CompletedTask;
    }

    protected abstract THandler CreateHandler();

    public virtual Task DisposeAsync()
    {
        _mocks.Clear();
        Handler = null;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Initialize mock with passed user. Defaults to <see cref="TestFactoryUsers.DefaultUser1Authenticated"/>.
    /// </summary>
    /// <param name="loggedInUser">User, which will be returned from the mock.</param>
    protected void MockServiceCurrentUser(CurrentUser loggedInUser = null)
    {
        Mock<IServiceCurrentUser> mock = new();
        mock
            .Setup(m => m.User)
            .Returns(loggedInUser ?? TestFactoryUsers.DefaultUser1Authenticated);

        RegisterMock<IServiceCurrentUser, Mock<IServiceCurrentUser>>(mock);
    }
}