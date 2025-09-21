using FluentValidation;

namespace microInventario.API.Model.Request.Validators
{
    public class AgregarProductoValidator : AbstractValidator<AgregarProductoRequest>
    {
        public AgregarProductoValidator()
        {
            RuleFor(x => x.IdCategoria)
                .GreaterThan(0).WithMessage("Debe ingresar una categoría válida.");

            RuleFor(x => x.IdMarca)
                .GreaterThan(0).WithMessage("Debe ingresar una marca válida.");

            RuleFor(x => x.Nombre)
                .NotNull().WithMessage("El nombre no puede ser nulo.")
                .Must(nombre => !string.IsNullOrWhiteSpace(nombre))
                .WithMessage("Debe ingresar un nombre de producto.")
                .MaximumLength(150).WithMessage("El nombre no puede superar los 150 caracteres.");

            RuleFor(x => x.Descripcion)
                .NotNull().WithMessage("La descripción no puede ser nula.")
                .Must(desc => !string.IsNullOrWhiteSpace(desc))
                .WithMessage("Debe ingresar una descripción del producto.")
                .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres.");

            RuleFor(x => x.Image)
                .NotNull().WithMessage("Debe ingresar una imagen del producto.")
                .Must(img => img.Length > 0).WithMessage("La imagen no puede estar vacía.")
                .Must(img => img.Length <= 5 * 1024 * 1024) // 5 MB límite
                .WithMessage("La imagen no puede superar los 5 MB.");

            RuleFor(x => x.Precio)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");

            RuleFor(x => x.Cantidad)
                .GreaterThanOrEqualTo(0).WithMessage("La cantidad no puede ser negativa.");
        }
    }
}
