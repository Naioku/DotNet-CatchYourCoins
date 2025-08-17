using Application.Dashboard.Commands;
using Application.Dashboard.DTOs;
using Application.Dashboard.DTOs.CreateDTOs.Expenses;
using Domain;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MVC.Extensions;

namespace MVC.Areas.Dashboard.Controllers;

[Area("Dashboard")]
public class ExpenseCategory(
    IMediator mediator,
    IValidator<DTORange<CreateDTOExpenseCategory>> validatorCreateExpenseCategory)
    : Controller
{
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(DTORange<CreateDTOExpenseCategory> dtos, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await validatorCreateExpenseCategory.ValidateAsync(dtos, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return View(dtos);
        }
        
        CommandCRUDCreateRange<CreateDTOExpenseCategory> command = new()
        {
            Data = dtos.Items,
        };
        
        Result result = await mediator.Send(
            command,
            cancellationToken
        );
        
        if (result.IsSuccess)
        {
            return View();
        }

        foreach (KeyValuePair<string, string> error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Value);
        }

        return View(dtos);
    }
}