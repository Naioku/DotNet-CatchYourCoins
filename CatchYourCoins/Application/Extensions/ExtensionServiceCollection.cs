using Application.Account.Commands;
using Application.Dashboard.Commands;
using Application.Dashboard.DTOs.CreateDTOs.Expenses;
using Application.Dashboard.DTOs.CreateDTOs.Incomes;
using Application.Dashboard.DTOs.OutputDTOs.Expenses;
using Application.Dashboard.DTOs.OutputDTOs.Incomes;
using Application.Dashboard.DTOs.UpdateDTOs.Expenses;
using Application.Dashboard.DTOs.UpdateDTOs.Incomes;
using Application.Dashboard.Queries;
using Application.Mapping;
using Application.MappingProfiles;
using Application.MappingProfiles.Expenses;
using Application.MappingProfiles.Incomes;
using AutoMapper;
using Domain;
using Domain.Dashboard.Entities.Expenses;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.Extensions;

public static class ExtensionServiceCollection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => { config.RegisterServicesFromAssemblyContaining<CommandRegister>(); });

        services.AddCrudHandlers();
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
        });
        services.AddScoped(provider => new MapperConfiguration(config =>
            {
                using (IServiceScope scope = provider.CreateScope())
                {
                    IServiceCurrentUser serviceCurrentUser = scope.ServiceProvider.GetRequiredService<IServiceCurrentUser>();
                    config.AddProfile(new MappingProfileDashboardEntity(serviceCurrentUser));
                    config.AddProfile(new MappingProfileFinancialCategory(serviceCurrentUser));
                    config.AddProfile(new MappingProfileFinancialOperation(serviceCurrentUser));
                    config.AddProfile(new MappingProfileExpensePaymentMethod());
                    config.AddProfile(new MappingProfileExpenseCategory());
                    config.AddProfile(new MappingProfileExpense());
                    config.AddProfile(new MappingProfileIncomeCategory());
                    config.AddProfile(new MappingProfileIncome());
                }
            },
                provider.GetService<ILoggerFactory>()).CreateMapper()
        );
        services.AddScoped<IMapperExtended, MapperExtended>();

        services.AddValidatorsFromAssemblyContaining<ValidatorRegister>();

        return services;
    }
    
    public static IServiceCollection AddCrudHandlers(this IServiceCollection services)
    {
        Tuple<Type, Type, Type, Type>[] mappings =
        [
            new(typeof(ExpenseCategory), typeof(CreateDTOExpenseCategory), typeof(OutputDTOExpenseCategory), typeof(UpdateDTOExpenseCategory)),
            new(typeof(ExpensePaymentMethod), typeof(CreateDTOExpensePaymentMethod), typeof(OutputDTOExpensePaymentMethod), typeof(UpdateDTOExpensePaymentMethod)),
            new(typeof(Expense), typeof(CreateDTOExpense), typeof(OutputDTOExpense), typeof(UpdateDTOExpense)),
            new(typeof(IncomeCategory), typeof(CreateDTOIncomeCategory), typeof(OutputDTOIncomeCategory), typeof(UpdateDTOIncomeCategory)),
            new(typeof(Income), typeof(CreateDTOIncome), typeof(OutputDTOIncome), typeof(UpdateDTOIncome)),
        ];
    
        foreach (Tuple<Type, Type, Type, Type> mapping in mappings)
        {
            Type typeEntity = mapping.Item1;
            Type typeInputDTO = mapping.Item2;
            Type command = typeof(CommandCRUDCreate<>).MakeGenericType(typeInputDTO);
            Type handler = typeof(HandlerCRUDCreate<,>).MakeGenericType(typeEntity, typeInputDTO);
            Type requestHandler = typeof(IRequestHandler<,>).MakeGenericType(command, typeof(Result));
            services.AddTransient(requestHandler, handler);
        }
        
        foreach (Tuple<Type, Type, Type, Type> mapping in mappings)
        {
            Type typeEntity = mapping.Item1;
            Type typeInputDTO = mapping.Item2;
            Type command = typeof(CommandCRUDCreateRange<>).MakeGenericType(typeInputDTO);
            Type handler = typeof(HandlerCRUDCreateRange<,>).MakeGenericType(typeEntity, typeInputDTO);
            Type requestHandler = typeof(IRequestHandler<,>).MakeGenericType(command, typeof(Result));
            services.AddTransient(requestHandler, handler);
        }
        
        foreach (Tuple<Type, Type, Type, Type> mapping in mappings)
        {
            Type typeEntity = mapping.Item1;
            Type typeUpdateDTO = mapping.Item4;
            Type command = typeof(CommandCRUDUpdate<,>).MakeGenericType(typeEntity, typeUpdateDTO);
            Type handler = typeof(HandlerCRUDUpdate<,>).MakeGenericType(typeEntity, typeUpdateDTO);
            Type requestHandler = typeof(IRequestHandler<,>).MakeGenericType(command, typeof(Result));
            services.AddTransient(requestHandler, handler);
        }
        
        foreach (Tuple<Type, Type, Type, Type> mapping in mappings)
        {
            Type typeEntity = mapping.Item1;
            Type command = typeof(CommandCRUDDelete<>).MakeGenericType(typeEntity);
            Type handler = typeof(HandlerCRUDDelete<>).MakeGenericType(typeEntity);
            Type requestHandler = typeof(IRequestHandler<,>).MakeGenericType(command, typeof(Result));
            services.AddTransient(requestHandler, handler);
        }
        
        foreach (Tuple<Type, Type, Type, Type> mapping in mappings)
        {
            Type typeEntity = mapping.Item1;
            Type typeOutputDTO = mapping.Item3;
            Type command = typeof(QueryCRUDGet<,>).MakeGenericType(typeEntity, typeOutputDTO);
            Type handler = typeof(HandlerCRUDGet<,>).MakeGenericType(typeEntity, typeOutputDTO);
            Type result = typeof(Result<>).MakeGenericType(typeof(IReadOnlyList<>).MakeGenericType(typeOutputDTO));
            Type requestHandler = typeof(IRequestHandler<,>).MakeGenericType(command, result);
            services.AddTransient(requestHandler, handler);
        }
    
        return services;
    }

}