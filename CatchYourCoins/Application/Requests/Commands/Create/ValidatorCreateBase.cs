using FluentValidation;

namespace Application.Requests.Commands.Create;

public abstract class ValidatorCreateBase<T> : AbstractValidator<T> where T : CommandCreateBase;