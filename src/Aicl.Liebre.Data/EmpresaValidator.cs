using System;
using ServiceStack.FluentValidation;
using Aicl.Liebre.Model;
using ServiceStack;
using MongoDB.Driver.Builders;

namespace Aicl.Liebre.Data
{
	public class EmpresaValidator: AbstractValidator<Empresa>
	{

		public Func<Empresa,bool> NoExistsValidator{ get; set; }

		public EmpresaValidator (IStore store)
		{
			Action common = () => {
				RuleFor (x => x.Nombre).NotEmpty ().WithMessage ("Indique el nombre de la empresa");
				//RuleFor (x => x.Direccion).NotEmpty ().WithMessage ("Indique la dirección de la empresa");
				RuleFor (x => x.Email).EmailAddress ().WithMessage ("Indique un correo electrónico válido");
				RuleFor (x => x.Telefono).NotEmpty ().WithMessage ("Indique un número de teléfono");
			};

			RuleSet ("create", () => {
				common();
				RuleFor (x => x.Nit)
					.Must ( (x,y) => {
				var ee = store.Single<Empresa> (q => q.Nit == y);
				return ee.Id.IsNullOrEmpty ();
					}).WithMessage ("Nit ya Existe");
				});

			RuleSet ("update", () => {
				common();
				RuleFor (x => x.Id)
					.Must ((x,y) => {
					var ee = store.Single<Empresa> (q => q.Id == y);
					if (ee.Nit == x.Nit)
						return true;
					var se = store.Single<Empresa> (q => q.Nit == x.Nit);
					return se.Id.IsNullOrEmpty ();
				}).WithMessage ("Nit ya Existe");
			});

			RuleSet ("delete", () =>
				RuleFor (x => x.Id).Must ( y => 
					!store.Exists<Diagnostico> (Query<Diagnostico>.EQ (q => q.IdEmpresa,  y)))
				.WithMessage ("La Empresa tiene Diagnostico Asociados")
			);

		}
	}
}

