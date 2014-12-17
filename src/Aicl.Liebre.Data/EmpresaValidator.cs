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

		//string contacto = string.Empty;
		//string email = string.Empty;

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
				RuleFor(x=>x.Llave)
					.Must((x, y) => GetById (x.Id).Llave == y)
					.WithMessage("Llave incorrecta");
			});

			RuleSet ("delete", () => {
				RuleFor (x => x.Id).NotEmpty ().WithMessage ("Debe indicar el Identificador de la empresa");
				RuleFor (x=> x.Id )
					.Must((x, y) => !GetById (y).Id.IsNullOrEmpty ())
					.WithMessage("No existe empresa con el identificado",x=>x.Id);
				RuleFor (x => x.Id).Must (y => 
					!Store.Exists<Diagnostico> (Query<Diagnostico>.EQ (q => q.IdEmpresa, y)))
					.WithMessage ("La Empresa tiene Diagnostico Asociados");
			});
				
			RuleSet ("updateregistro", () => 
				RuleFor (x => x.IdPlan).Must ((x, y) => GetById (x.Id).IdPlan == y).WithMessage ("No puede cambiar su plan"));

			RuleSet ("readregistro", () => {
				RuleFor(x=>x.Nit).NotEmpty().WithMessage("Debe indicar el Nit de la empresa");
				RuleFor(x=>x.Llave).NotEmpty().WithMessage("Debe indicar la Llave de la empresa");
			});

		}


		public void ValidateCreate(Empresa instance){
			IValidatorExtensions.MyValidate (this,instance, "common","create");
		}

		public void ValidateUpdate(Empresa instance){
			IValidatorExtensions.MyValidate (this,instance, "common","update");
		}

		public void ValidateDelete(Empresa instance){
			IValidatorExtensions.MyValidate (this,instance, "delete");
		}

		public void ValidateCreateRegistro(Empresa instance){
			IValidatorExtensions.MyValidate (this,instance, "common","create");
		}

		public void ValidateUpdateRegistro(Empresa instance){
			IValidatorExtensions.MyValidate (this,instance, "common","update","updateregistro");
		}
		public void ValidateReadRegistro(Empresa instance){
			IValidatorExtensions.MyValidate (this,instance, "readregistro");
		}

		Empresa GetById(string id){
			if (_ee == default(Empresa) || _ee.Id != id)
				_ee = Store.Single<Empresa> (id);
			return _ee;
		}

		bool NoNit(string nit){
			var e= Store.Single<Empresa> (Query<Empresa>.EQ (q => q.Nit, nit));
			//contacto = e.Contacto.HideContent ();
			//email = e.Email.HideContent ();
			return e.Id.IsNullOrEmpty ();

		}
	}
}

