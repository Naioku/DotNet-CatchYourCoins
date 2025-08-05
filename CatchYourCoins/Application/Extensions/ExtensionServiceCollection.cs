using Application.Account.Commands;
using Application.Dashboard.Commands;
using Application.Dashboard.DTOs.InputDTOs.Expenses;
using Application.Dashboard.DTOs.InputDTOs.Incomes;
using Application.Dashboard.DTOs.OutputDTOs.Expenses;
using Application.Dashboard.DTOs.OutputDTOs.Incomes;
using Application.Dashboard.Queries;
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

        services.AddValidatorsFromAssemblyContaining<ValidatorRegister>();

        return services;
    }
    
    public static IServiceCollection AddCrudHandlers(this IServiceCollection services)
    {
        Tuple<Type, Type, Type>[] mappings =
        [
            new(typeof(ExpenseCategory), typeof(InputDTOExpenseCategory), typeof(OutputDTOExpenseCategory)),
            new(typeof(ExpensePaymentMethod), typeof(InputDTOExpensePaymentMethod), typeof(OutputDTOExpensePaymentMethod)),
            new(typeof(Expense), typeof(InputDTOExpense), typeof(OutputDTOExpense)),
            new(typeof(IncomeCategory), typeof(InputDTOIncomeCategory), typeof(OutputDTOIncomeCategory)),
            new(typeof(Income), typeof(InputDTOIncome), typeof(OutputDTOIncome)),
        ];
    
        foreach (Tuple<Type, Type, Type> mapping in mappings)
        {
            (Type typeEntity, Type typeInputDTO, Type _) = mapping;
            Type command = typeof(CommandCRUDCreate<>).MakeGenericType(typeInputDTO);
            Type handler = typeof(HandlerCRUDCreate<,>).MakeGenericType(typeEntity, typeInputDTO);
            Type requestHandler = typeof(IRequestHandler<,>).MakeGenericType(command, typeof(Result));
            services.AddTransient(requestHandler, handler);
        }
        
        foreach (Tuple<Type, Type, Type> mapping in mappings)
        {
            (Type typeEntity, Type typeInputDTO, Type _) = mapping;
            Type command = typeof(CommandCRUDCreateRange<>).MakeGenericType(typeInputDTO);
            Type handler = typeof(HandlerCRUDCreateRange<,>).MakeGenericType(typeEntity, typeInputDTO);
            Type requestHandler = typeof(IRequestHandler<,>).MakeGenericType(command, typeof(Result));
            services.AddTransient(requestHandler, handler);
        }
        
        foreach (Tuple<Type, Type, Type> mapping in mappings)
        {
            (Type typeEntity, Type _, Type _) = mapping;
            Type command = typeof(CommandCRUDDelete<>).MakeGenericType(typeEntity);
            Type handler = typeof(HandlerCRUDDelete<>).MakeGenericType(typeEntity);
            Type requestHandler = typeof(IRequestHandler<,>).MakeGenericType(command, typeof(Result));
            services.AddTransient(requestHandler, handler);
        }
        
        foreach (Tuple<Type, Type, Type> mapping in mappings)
        {
            (Type typeEntity, Type _, Type typeOutputDTO) = mapping;
            Type command = typeof(QueryCRUDGetById<>).MakeGenericType(typeOutputDTO);
            Type handler = typeof(HandlerCRUDGetById<,>).MakeGenericType(typeEntity, typeOutputDTO);
            Type result = typeof(Result<>).MakeGenericType(typeOutputDTO);
            Type requestHandler = typeof(IRequestHandler<,>).MakeGenericType(command, result);
            services.AddTransient(requestHandler, handler);
        }
        
        foreach (Tuple<Type, Type, Type> mapping in mappings)
        {
            (Type typeEntity, Type _, Type typeOutputDTO) = mapping;
            Type command = typeof(QueryCRUDGetAll<>).MakeGenericType(typeOutputDTO);
            Type handler = typeof(HandlerCRUDGetAll<,>).MakeGenericType(typeEntity, typeOutputDTO);
            Type result = typeof(Result<>).MakeGenericType(typeof(IReadOnlyList<>).MakeGenericType(typeOutputDTO));
            Type requestHandler = typeof(IRequestHandler<,>).MakeGenericType(command, result);
            services.AddTransient(requestHandler, handler);
        }
    
        return services;
    }

}