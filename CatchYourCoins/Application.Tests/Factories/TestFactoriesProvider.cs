using System;
using System.Collections.Generic;
using Application.DTOs.InputDTOs.Expenses;
using Application.DTOs.InputDTOs.Incomes;
using Application.DTOs.OutputDTOs.Expenses;
using Application.DTOs.OutputDTOs.Incomes;
using Application.Tests.Factories.DTOs;
using Application.Tests.Factories.DTOs.InputDTO;
using Application.Tests.Factories.DTOs.OutputDTO;
using Application.Tests.Factories.Entity;
using Domain.Dashboard.Entities.Expenses;
using Domain.Dashboard.Entities.Incomes;

namespace Application.Tests.Factories;

public static class TestFactoriesProvider
{
    private static readonly Dictionary<Type, object> Factories = new();
    
    public static T GetFactory<T>() => (T)Factories[typeof(T)];
    
    static TestFactoriesProvider()
    {
        RegisterEntities();
        RegisterInputDTOs();
        RegisterOutputDTOs();
        Factories.Add(typeof(TestFactoryUsers), new TestFactoryUsers());
    }

    private static void RegisterEntities()
    {
        Factories.Add(typeof(TestFactoryEntityBase<Expense>), new TestFactoryExpense());
        Factories.Add(typeof(TestFactoryEntityBase<ExpenseCategory>), new TestFactoryExpenseCategory());
        Factories.Add(typeof(TestFactoryEntityBase<ExpensePaymentMethod>), new TestFactoryExpensePaymentMethod());
        Factories.Add(typeof(TestFactoryEntityBase<Income>), new TestFactoryIncome());
        Factories.Add(typeof(TestFactoryEntityBase<IncomeCategory>), new TestFactoryIncomeCategory());
    }
    
    private static void RegisterInputDTOs()
    {
        Factories.Add(typeof(TestFactoryDTOBase<Expense, InputDTOExpense>), new TestFactoryInputDTOExpense());
        Factories.Add(typeof(TestFactoryDTOBase<ExpenseCategory, InputDTOExpenseCategory>), new TestFactoryInputDTOExpenseCategory());
        Factories.Add(typeof(TestFactoryDTOBase<ExpensePaymentMethod, InputDTOExpensePaymentMethod>), new TestFactoryInputDTOExpensePaymentMethod());
        Factories.Add(typeof(TestFactoryDTOBase<Income, InputDTOIncome>), new TestFactoryInputDTOIncome());
        Factories.Add(typeof(TestFactoryDTOBase<IncomeCategory, InputDTOIncomeCategory>), new TestFactoryInputDTOIncomeCategory());
    }
    
    private static void RegisterOutputDTOs()
    {
        Factories.Add(typeof(TestFactoryDTOBase<Expense, OutputDTOExpense>), new TestFactoryOutputDTOExpense());
        Factories.Add(typeof(TestFactoryDTOBase<ExpenseCategory, OutputDTOExpenseCategory>), new TestFactoryOutputDTOExpenseCategory());
        Factories.Add(typeof(TestFactoryDTOBase<ExpensePaymentMethod, OutputDTOExpensePaymentMethod>), new TestFactoryOutputDTOExpensePaymentMethod());
        Factories.Add(typeof(TestFactoryDTOBase<Income, OutputDTOIncome>), new TestFactoryOutputDTOIncome());
        Factories.Add(typeof(TestFactoryDTOBase<IncomeCategory, OutputDTOIncomeCategory>), new TestFactoryOutputDTOIncomeCategory());
    }
}