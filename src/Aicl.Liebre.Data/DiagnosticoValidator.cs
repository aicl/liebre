using ServiceStack.FluentValidation;
using Aicl.Liebre.Model;
using ServiceStack;
using MongoDB.Driver.Builders;

namespace Aicl.Liebre.Data
{
	public class DiagnosticoValidator:AbstractValidator<Diagnostico>
	{
		Diagnostico _ee=default(Diagnostico);

		IStore Store{ get; set; }

		public DiagnosticoValidator (IStore store)
		{
			Store = store;
			RuleSet ("delete", () => {
				RuleFor(x=>x.Id).NotEmpty().WithMessage("Dege indicar el Identificador del Diagnostico");
				RuleFor(x=>x.Id)
					.Must((x, y) => !GetById (y).Id.IsNullOrEmpty ())
					.WithMessage("No existe Diagnostico con el Identificador:'{0}'", x=>x.Id);
				RuleFor(x=>x.Id)
					.Must((x, y) => !Store.Exists<Respuesta> (Query<Respuesta>.EQ (q => q.IdDiagnostico, y)))
					.WithMessage("El Diagnostico tiene Respuestas Asociadas");
				RuleFor(x=>x.Id)
					.Must((x, y) => !Store.Exists<RespuestaGuia> (Query<RespuestaGuia>.EQ (q => q.IdDiagnostico, y)))
					.WithMessage("El Diagnostico tiene Respuestas a las Guias Asociadas");
			});
		}

		Diagnostico GetById(string id){

			if (_ee == default(Diagnostico) || _ee.Id != id)
				_ee = Store.Single<Diagnostico> (id);
			return _ee;
		}

	}
}

