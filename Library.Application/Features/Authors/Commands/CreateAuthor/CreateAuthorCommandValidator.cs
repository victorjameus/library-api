namespace Library.Application.Features.Authors.Commands.CreateAuthor
{
    public sealed class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
    {
        public CreateAuthorCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("El nombre es requerido")
                .MaximumLength(50)
                .WithMessage("El nombre no puede exceder 50 caracteres")
                .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$")
                .WithMessage("El nombre solo puede contener letras y espacios");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("El apellido es requerido")
                .MaximumLength(50)
                .WithMessage("El apellido no puede exceder 50 caracteres")
                .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$")
                .WithMessage("El apellido solo puede contener letras y espacios");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Now)
                .WithMessage("La fecha de nacimiento debe ser anterior a hoy")
                .When(x => x.DateOfBirth.HasValue);

            RuleFor(x => x.Nationality)
                .MaximumLength(50)
                .WithMessage("La nacionalidad no puede exceder 50 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Nationality));

            RuleFor(x => x.Biography)
                .MaximumLength(2000)
                .WithMessage("La biografía no puede exceder 2000 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Biography));

            RuleFor(x => new { x.FirstName, x.LastName })
                .MustAsync((names, cancellationToken) =>
                {
                    return Task.FromResult
                    (
                        !string.IsNullOrWhiteSpace(names.FirstName) &&
                        !string.IsNullOrWhiteSpace(names.LastName)
                    );
                })
                .WithMessage("Debe proporcionar tanto nombre como apellido");
        }
    }
}
