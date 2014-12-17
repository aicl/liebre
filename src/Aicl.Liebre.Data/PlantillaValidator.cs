using ServiceStack.FluentValidation;
using Aicl.Liebre.Model;
using ServiceStack;
using MongoDB.Driver.Builders;

namespace Aicl.Liebre.Data
{
	public class PlantillaValidator:AbstractValidator<Plantilla>
	{
		Plantilla _ee=default(Plantilla);

		IStore Store{ get; set; }

		public PlantillaValidator (IStore store)
		{
			Store = store;
			RuleSet ("delete", () => {
				RuleFor(x=>x.Id).NotEmpty().WithMessage("Dege indicar el Identificador de la Plantilla");
				RuleFor(x=>x.Id)
					.Must((x, y) => !GetById (y).Id.IsNullOrEmpty ())
					.WithMessage("No existe Plantilla con el Identificador:'{0}'", x=>x.Id);
				RuleFor(x=>x.Id)
					.Must((x, y) => !Store.Exists<Plan> (Query<Plan>.EQ (q => q.IdPlantilla, y)))
					.WithMessage("La Plantilla tiene Planes Asociados");

			});
		}

		Plantilla GetById(string id){

			if (_ee == default(Plantilla) || _ee.Id != id)
				_ee = Store.Single<Plantilla> (id);
			return _ee;
		}

	}
}

