using System;
using ServiceStack.FluentValidation;
using Aicl.Liebre.Model;
using ServiceStack;
using MongoDB.Driver.Builders;

namespace Aicl.Liebre.Data
{
	public class EmpresaValidator: AbstractValidator<Empresa>
	{
		Empresa _ee=default(Empresa);

		IStore Store{ get; set; }

		public Func<Empresa,bool> NoExistsValidator{ get; set; }

		public EmpresaValidator (IStore store)
		{
			Store = store;
			Action common = () => {
				RuleFor (x => x.Nombre).NotEmpty ().WithMessage ("Indique el nombre de la empresa");
				//RuleFor (x => x.Direccion).NotEmpty ().WithMessage ("Indique la dirección de la empresa");
				RuleFor (x => x.Email).EmailAddress ().WithMessage ("Indique un correo electrónico válido");
				RuleFor (x => x.Telefono).NotEmpty ().WithMessage ("Indique un número de teléfono");
			};

			RuleSet ("create", () => {
				common();
				RuleFor (x => x.Nit)
					.Must ((x, y) => !store.Exists<Empresa>(Query<Empresa>.EQ (q => q.Nit, y)))
					.WithMessage ("Nit ya Existe");
				});

			RuleSet ("update", () => {
				Console.WriteLine ("udpate validador");
				common();
				RuleFor (x => x.Id)
					.Must ((x, y) => GetById (y).Nit == x.Nit || !store.Exists<Empresa> (Query<Empresa>.EQ (q => q.Nit, x.Nit)))
					.WithMessage ("Nit ya Existe");
				RuleFor(x=>x.Llave)
					.Must((x, y) => GetById (x.Id).Llave == y)
					.WithMessage("Llave incorrecta");
			});

			RuleSet ("delete", () =>
				RuleFor (x => x.Id).Must ( y => 
					!store.Exists<Diagnostico> (Query<Diagnostico>.EQ (q => q.IdEmpresa,  y)))
				.WithMessage ("La Empresa tiene Diagnostico Asociados")
			);

		}


		Empresa GetById(string id){
			Console.WriteLine ("GetById validador {0}", id);
			if (_ee == default(Empresa) || _ee.Id != id)
				_ee = Store.Single<Empresa> (Query<Empresa>.EQ (q => q.Id, id));
			return _ee;
		}
	}
}

