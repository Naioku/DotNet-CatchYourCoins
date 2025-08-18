using Application.Dashboard.Commands;
using Application.Dashboard.DTOs;
using Application.Dashboard.DTOs.CreateDTOs.Expenses;
using Application.Dashboard.DTOs.OutputDTOs.Expenses;
using Application.Dashboard.Queries;
using Domain;
using Domain.Dashboard.Entities.Expenses;
using Domain.Dashboard.Specifications.Expenses;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MVC.Extensions;

namespace MVC.Areas.Dashboard.Controllers;

[Area("Dashboard")]
public class ControllerExpenseCategory(
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
            return RedirectToAction("Index", "ControllerExpenseCategory");
        }

        foreach (KeyValuePair<string, string> error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Value);
        }

        return View(dtos);
    }

    public async Task<IActionResult> Index()
    {
        Result<IReadOnlyList<OutputDTOExpenseCategory>> dto = await mediator.Send(
            new QueryCRUDGet<ExpenseCategory, OutputDTOExpenseCategory>
            {
                Specification = SpecificationExpenseCategory.GetBuilder().Build(),
            }
        );

        if (!dto.IsSuccess)
        {
            foreach (KeyValuePair<string, string> error in dto.Errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }

            return View();
        }

        return View(new DTORange<OutputDTOExpenseCategory>
        {
            Items = dto.Value.ToList(),
        });
    }
}