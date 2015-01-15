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

		public EmpresaValidator (IStore store)
		{
			Store = store;

			RuleSet("common", () => {
				RuleFor (x => x.Nombre).NotEmpty ().WithMessage ("Indique el nombre de la empresa");
				RuleFor ( x=>x.Nit).NotEmpty ().WithMessage ("Indique el nit de la empresa");
				//RuleFor (x => x.Direccion).NotEmpty ().WithMessage ("Indique la dirección de la empresa");
				RuleFor (x => x.Email).EmailAddress ().WithMessage ("Indique un correo electrónico válido");
				RuleFor (x => x.Telefono).NotEmpty ().WithMessage ("Indique un número de teléfono");
			});

			RuleSet ("create", () =>
				RuleFor (x => x.Nit).Must ((x, y) => NoNit (x.Nit)).WithMessage ("Nit ya Existe"));

			RuleSet ("update", () => {
				RuleFor (x => x.Id).NotEmpty ().WithMessage ("Debe indicar el Identificador de la empresa");
				RuleFor (x=> x.Id )
					.Must((x, y) => !GetById (y).Id.IsNullOrEmpty ())
					.WithMessage("No existe empresa con el identificado",x=>x.Id);
				RuleFor (x => x.Id)
					.Must ((x, y) => GetById (y).Nit == x.Nit || NoNit (x.Nit))
					.WithMessage ("Nit ya Existe" );
			});

			RuleSet ("existe", () => 
				RuleFor (x => x.Id).NotEmpty ().WithMessage ("No existe empresa con la información suministrada"));

			RuleSet ("delete", () => {
				RuleFor (x => x.Id).NotEmpty ().WithMessage ("Debe indicar el Identificador de la empresa");
				RuleFor (x=> x.Id )
					.Must((x, y) => !GetById (y).Id.IsNullOrEmpty ())
					.WithMessage("No existe empresa con el identificado",x=>x.Id);
				RuleFor (x => x.Id).Must (y => 
					!Store.Exists<Diagnostico> (Query<Diagnostico>.EQ (q => q.IdEmpresa, y)))
					.WithMessage ("La Empresa tiene Diagnostico Asociados");
			});
				
			RuleSet ("updateregistro", () => {
				RuleFor (x => x.IdPlan).Must ((x, y) => y.IsNullOrEmpty() || GetById (x.Id).IdPlan == y).WithMessage ("No puede cambiar su plan");
				RuleFor(x=>x.Llave)
					.Must((x, y) => GetById (x.Id).Llave == y)
					.WithMessage("Llave incorrecta");
			});

			RuleSet ("readregistro", () => {
				RuleFor(x=>x.Nit).NotEmpty().WithMessage("Debe indicar el Nit de la empresa");
				RuleFor(x=>x.Llave).NotEmpty().WithMessage("Debe indicar la Llave de la empresa");
			});

			RuleSet ("confirmar", () => 
				RuleFor (x => x.FechaRegistro).Must ((x, y) =>!y.HasValue).WithMessage ("Empresa ya confirmada"));

			RuleSet ("createregistro", () =>
				RuleFor (x => x.FechaRegistro).Must ((x, y) => !y.HasValue && (System.DateTime.UtcNow - x.FechaLLave).Days > 1)
				.When (x => !x.Id.IsNullOrEmpty ())
				.WithMessage ("La empresa ya esta Registrada"));

		}


		public void ValidateCreate(Empresa instance){
			this.MyValidate (instance, "common", "create");
		}

		public void ValidateUpdate(Empresa instance){
			this.MyValidate (instance, "common", "update");
		}

		public void ValidateDelete(Empresa instance){
			this.MyValidate (instance, "delete");
		}

		public void ValidateCreateRegistro(Empresa instance){
			this.MyValidate (instance, "common", "createregistro");
		}

		public void ValidateUpdateRegistro(Empresa instance){
			this.MyValidate (instance, "common", "update", "updateregistro");
		}
		public void ValidateReadRegistro(Empresa instance){
			this.MyValidate (instance, "readregistro");
		}

		public void ValidateExiste(Empresa instance){
			this.MyValidate (instance, "existe");
		}

		public void ValidateConfirmar(Empresa instance){
			this.MyValidate (instance, "existe", "confirmar");
		}

		Empresa GetById(string id){
			if (_ee == default(Empresa) || _ee.Id != id)
				_ee = Store.Single<Empresa> (id);
			return _ee;
		}

		bool NoNit(string nit){
			var e= Store.Single<Empresa> (Query<Empresa>.EQ (q => q.Nit, nit));
			return e.Id.IsNullOrEmpty ();

		}
	}
}

