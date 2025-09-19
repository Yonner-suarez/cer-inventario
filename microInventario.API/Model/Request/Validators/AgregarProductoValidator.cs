using FluentValidation;

namespace microInventario.API.Model.Request.Validators
{
        public class AgregarProductoValidator:  AbstractValidator<AgregarProductoRequest>
        {
            public AgregarProductoValidator()
            {
                RuleFor(x => x.marca)
                   .Must(m => m != 0)
                   .WithMessage("Debe ingresar una marca válida.");
            }
        }
}
