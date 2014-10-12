using System;
using System.Collections.Generic;
using ServiceStack;
using Aicl.Liebre.Model;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;
using ServiceStack.Model;
using System.Linq;

namespace Aicl.Liebre.Data
{
	public class Store
	{
		MongoUrl Url { get; set; }
		MongoDatabase Db { get; set; }


		public Store (string url)
		{
			Url = new MongoUrl(url);
			Db = new MongoClient(Url).GetServer().GetDatabase(Url.DatabaseName);

		}

		public List<T> Get<T>(Func<T, object> orderBy=null) where T:class, IDocument
		{
			var cl= GetCollection<T> ();
			var docs = cl.FindAll (); 
			return orderBy == null ? docs.ToList () : docs.OrderBy (orderBy).ToList();
		}

		public List<T> Get<T>(IHasStringId request,Func<T, object> orderBy=null) where T:class, IDocument
		{
			var cl= GetCollection<T> ();
			var docs = request.Id.IsNullOrEmpty()? cl.FindAll (): cl.Find(Query<T>.EQ(e=>e.Id,request.Id)) ;
			return orderBy == null ? docs.ToList () : docs.OrderBy (orderBy).ToList();
		}

		public T GetById<T>(IHasStringId request) where T:class, IDocument, new() 
		{
			var cl= GetCollection<T> ();
			return cl.FindOne(Query<T>.EQ(e=>e.Id,request.Id))??new T() ;
		}

		public T GetById<T>(string id) where T:class, IDocument
		{
			var cl= GetCollection<T> ();
			return cl.FindOne (Query<T>.EQ (e => e.Id, id));
		}


		public List<T> GetByQuery<T>( Expression<Func<T, bool>> predicate, Func<T, object> orderBy=null) 
			where T:class, IDocument
		{
			var cl = GetCollection<T> (); 
			var docs= cl.Find(Query<T>.Where(predicate));  
			return orderBy == null ? docs.ToList () : docs.OrderBy (orderBy).ToList();
		}

		public void Execute<T>(Action<IQueryable<T>> action)
		{
			var cl = GetCollection<T> ().AsQueryable(); 
			action (cl); 
		}

		public T Execute<T>(Func<IQueryable<T>,T> func)
		{
			var cl = GetCollection<T> ().AsQueryable(); 
			return func (cl);
		}


		public T Single<T>( Expression<Func<T, bool>> predicate) where T:class, IDocument, new()
		{
			var cl= GetCollection<T> ();
			var doc= cl.FindOne(Query<T>.Where(predicate));
			return doc?? new T();
		}

		public Result<T> Post<T>(T request) where T:class, IDocument
		{
			var cl = GetCollection<T> ();
			request.Id = string.Empty;
			return Store.CreateResult(request, cl.Insert (request));
		}

		public Result<T> Put<T>(T  request ) where T:class, IDocument
		{
			var cl= GetCollection<T> ();

			return Store.CreateResult (request,
				cl.Update (Query<T>.EQ (e => e.Id, request.Id), Update<T>.Replace (request))); 

		}

		public Result<T> Save<T>(T request ) where T:class, IDocument
		{
			return request.Id.IsNullOrEmpty () ? Post (request) : Put (request);
		}

		public Result<T> Delete<T>(IHasStringId request) where T:class, IDocument, new() 
		{
			var cl= GetCollection<T> ();
			return Store.CreateResult (new T{Id=request.Id}, cl.Remove (Query<T>.EQ (e => e.Id, request.Id)));
		}


		public Result<T> Delete<T>(string id) where T:class, IDocument, new()
		{
			var cl= GetCollection<T> ();
			return Store.CreateResult (new T{Id=id}, cl.Remove (Query<T>.EQ (e => e.Id, id)));
		}

		public Result<T> Delete<T>(Expression<Func<T, bool>> predicate) where T:class, IDocument, new()
		{
			var cl= GetCollection<T> (); 
			return Store.CreateResult (new T (), cl.Remove (Query<T>.Where (predicate)));
		}


		public  InstallDiagnosticoResponse GetInstallDiagnosticoResponse(InstallDiagnostico request)
		{
			var response = new InstallDiagnosticoResponse ();

			response.Diagnostico = GetById<Diagnostico> (request.IdDiagnostico);
			response.Plantilla = GetById<Plantilla> (response.Diagnostico.IdPlantilla);
			response.Empresa = GetById<Empresa> ( response.Diagnostico.IdEmpresa);

			response.Capitulos = GetByQuery<Capitulo> (q => q.IdPlantilla == response.Diagnostico.IdPlantilla);

			var capIds = response.Capitulos.ConvertAll (e => e.Id);

			var p = GetCollection<Pregunta>().Find( Query<Pregunta>.In ((q) => q.IdCapitulo, capIds )).ToList();
			var r = GetByQuery<Respuesta> (q => q.IdDiagnostico == response.Diagnostico.Id);

			var g = GetByQuery<Guia> (q => q.IdPlantilla == response.Plantilla.Id);
			var rg = GetByQuery<RespuestaGuia> (q => q.IdDiagnostico == response.Diagnostico.Id);

			p.ForEach (q => response.Preguntas.Add (new ViewPregunta {
				Pregunta = q,
				Respuesta = r.FirstOrDefault (rq => rq.IdPregunta == q.Id) ??
				new Respuesta{ IdPregunta = q.Id, IdDiagnostico = response.Diagnostico.Id }
			}));


			g.ForEach (q => response.Guias.Add (new ViewGuia { 
				Guia = q, 
				Respuesta = rg.FirstOrDefault (rq => rq.IdGuia == q.Id) ??
				new RespuestaGuia { IdGuia = q.Id, IdDiagnostico = response.Diagnostico.Id }
			}));

			return response;


		}

		MongoCollection<T> GetCollection<T>(){
			return Db.GetCollection<T> (typeof(T).GetCollectionName());
		}

		MongoCollection<T> GetCollection<T>(string collection){
			return Db.GetCollection<T> (collection);
		}

		List<T> GetByQuery<T>(string collection, Expression<Func<T, bool>> predicate) where T:class, IDocument
		{
			var cl = GetCollection<T> (collection);
			var docs= cl.Find(Query<T>.Where(predicate));
			return docs.ToList ();
		}


		static Result<T> CreateResult<T>(T data, WriteConcernResult wcr) where T:IDocument
		{
			var wc = new WriteResult(); 
			wc.PopulateWith(wcr);
			return new Result<T> {
				Data = data,
				WriteResult = wc
			};

		}

	}

}

