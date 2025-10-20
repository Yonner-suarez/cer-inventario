using FluentValidation;

namespace microInventario.API.Model.Request.Validators
{
    public class ActualizarStockProductoValidator : AbstractValidator<ActualizarStockProducto>
    {
        public ActualizarStockProductoValidator()
        {
            RuleFor(x => x.IdProducto)
                .GreaterThan(0)
                .WithMessage("Debe ingresar un ID de producto válido.");

            RuleFor(x => x.Cantidad)
                .GreaterThanOrEqualTo(0)
                .WithMessage("La cantidad no puede ser negativa.");
        }
    }
}
