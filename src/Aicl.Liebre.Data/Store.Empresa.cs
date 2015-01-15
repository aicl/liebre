using ServiceStack;
using Aicl.Liebre.Model;
using MongoDB.Driver.Builders;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Aicl.Liebre.Data
{
	public partial class Store
	{
		public List<Empresa> GetEmpresas(ReadEmpresa request){

			var e = Get<Empresa> (q => q.Nombre);
			var ids = e.ConvertAll (x => x.IdPlan).Distinct ().Where (x => !x.IsNullOrEmpty ());;
			var planes = GetCollection<Plan> ().Find (Query<Plan>.In (x => x.Id, ids));
			e.ForEach (x => {
				x.Plan = x.IdPlan.IsNullOrEmpty() ? new Plan () : planes.FirstOrDefault (y => y.Id == x.IdPlan)??new Plan();
			});

			return e;
		}


		public Result<Empresa> CreateEmpresa(CreateEmpresa request)
		{
			var ne = request.Data;
			var plan = Single<Plan> (ne.IdPlan);
			var validator = new EmpresaValidator (this);
			validator.ValidateCreate(ne);
			ne.Llave = Store.CreateRandomPassword (48);
			var r = Post<Empresa> (ne);
			r.Data.Plan = plan;
			return r;
		}

		public Result<Empresa> UpdateEmpresa(UpdateEmpresa request)
		{
			var ne = request.Data;
			var validator = new EmpresaValidator (this);
			validator.ValidateUpdate (ne);
			var r = Put<Empresa> (ne, fieldsToIgnore: e => new { e.Llave, e.IdPlan});
			r.Data.Plan = Single<Plan> (r.Data.IdPlan);
			return r;
		}

		public Result<Empresa> DeleteEmpresa(DeleteEmpresa request)
		{

			var ne = new Empresa{ Id = request.Id };
			var validator = new EmpresaValidator (this);
			validator.ValidateDelete (ne);
			return Delete<Empresa> (ne);

		}


		public Empresa ReadRegistroEmpresa(ReadRegistroEmpresa request){

			var validator = new EmpresaValidator (this);
			validator.ValidateReadRegistro (new Empresa{ Nit = request.Nit, Llave = request.Llave });
			var empresa = Single<Empresa> (Query.And (Query<Empresa>.EQ (q => q.Nit, request.Nit), Query<Empresa>.EQ (q => q.Llave, request.Llave)));
			empresa.Plan = Single<Plan> (empresa.IdPlan);
			return empresa;
		}



		public Result<Empresa> CreateRegistroEmpresa(CreateRegistroEmpresa request)
		{
			var ne = request.Data;

			var empresa = Single<Empresa> (Query<Empresa>.EQ (q => q.Nit, ne.Nit));
			empresa.PopulateWith (ne);

			var validator = new EmpresaValidator (this);
			validator.ValidateCreateRegistro (empresa);


			Plan plan = default(Plan);
			if (!empresa.IdPlan.IsNullOrEmpty ()) {
				plan = GetById<Plan> (empresa.IdPlan);
				if(plan!=default(Plan) && (!plan.Aprobado || !plan.Demo)){
					plan = default(Plan);			
				}
			}
			if (plan == default(Plan))
				plan = GetDemo ();

			empresa.IdPlan = plan.Id;

			empresa.Llave = Store.CreateRandomPassword (48);
			empresa.FechaLLave = DateTime.UtcNow;
			var r = Save(empresa);
			r.Data.Plan = plan;
			return r;
		}

		public Result<Empresa> UpdateRegistroEmpresa(UpdateRegistroEmpresa request)
		{
			var ne = request.Data;
			var validator = new EmpresaValidator (this);
			validator.ValidateUpdateRegistro (ne);

			var r = Put<Empresa> (ne, fieldsToIgnore: e => new {e.Llave, e.IdPlan});
			r.Data.Plan = Single<Plan> (r.Data.IdPlan);
			return r;
		}


		public Result<Empresa> RecuperarLlaveEmpresa(RecuperarLlaveEmpresa request){

			var validator = new EmpresaValidator (this);
			validator.ValidateReadRegistro (new Empresa{ Nit = request.Nit, Llave = request.Llave });

			var empresa = Single<Empresa> (Query.And (Query<Empresa>.EQ (q => q.Nit, request.Nit), Query<Empresa>.EQ (q => q.Llave, request.Llave)));
			validator.ValidateExiste (empresa);
			if (request.Regenerar) {
				empresa.Llave = CreateRandomPassword (48);
				var r = Put<Empresa> (empresa, e=>e.Llave);
				r.Data.Plan = Single<Plan> (empresa.IdPlan);
				return r;
			}
			return new Result<Empresa>{ Data = empresa };
		}


		public Result<Empresa> ConfirmarRegistroEmpresa (ConfirmarRegistroEmpresa request)
		{
			var validator = new EmpresaValidator (this);
			validator.ValidateReadRegistro (new Empresa{ Nit = request.Nit, Llave = request.Llave });

			var empresa = Single<Empresa> (Query.And (Query<Empresa>.EQ (q => q.Nit, request.Nit), Query<Empresa>.EQ (q => q.Llave, request.Llave)));
			validator.ValidateConfirmar(empresa);

			var fr = new EmpresaFechaRegistro{FechaRegistro=DateTime.UtcNow, Id= empresa.Id};

			var r1 = Put(fr);
			empresa.FechaRegistro= r1.Data.FechaRegistro;

			return new Result<Empresa>{
				Data= empresa,
				WriteResult= r1.WriteResult,
			};

		}

		Plan GetDemo(){
			return Single<Plan> (Query.And (Query<Plan>.EQ (q => q.Demo, true), Query<Plan>.EQ (q => q.Aprobado, true)));
		}

	}
}


