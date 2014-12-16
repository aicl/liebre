using Aicl.Liebre.Model;
using ServiceStack.FluentValidation;

namespace Aicl.Liebre.Data
{
	public partial class Store
	{
		public Result<Empresa> CreateEmpresa(CreateEmpresa request)
		{

			var ne = request.Data;
			var plan = Single<Plan> (q => q.Id == ne.IdPlan);
			var validator = new EmpresaValidator (this);
			validator.ValidateAndThrow (ne, "create");
			ne.Llave = Store.CreateRandomPassword (48);
			var r = Post<Empresa> (ne);
			r.Data.Plan = plan;
			return r;
		}

		public Result<Empresa> UpdateEmpresa(UpdateEmpresa request)
		{
			var ne = request.Data;
			var validator = new EmpresaValidator (this);
			validator.ValidateAndThrow (ne, "update");
			return Put<Empresa> (ne);
		}

		public Result<Empresa> DeleteEmpresa(DeleteEmpresa request)
		{

			var ne = new Empresa{ Id = request.Id };
			var validator = new EmpresaValidator (this);
			validator.ValidateAndThrow (ne, "delete");
			return Delete<Empresa> (ne);

		}

		public Result<Empresa> CreateRegistroEmpresa(CreateRegistroEmpresa request)
		{

			var ne = request.Data;
			var plan = Single<Plan> (q => q.Demo == true && q.Aprobado == true);
			ne.IdPlan = plan.Id;
			var validator = new EmpresaValidator (this);
			validator.ValidateAndThrow (ne, "create");
			ne.Llave = Store.CreateRandomPassword (48);
			var r = Post<Empresa> (ne);
			r.Data.Plan = plan;
			return r;
		}

	}
}


