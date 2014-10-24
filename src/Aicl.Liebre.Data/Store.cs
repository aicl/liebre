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
using ServiceStack.Text;

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


		public List<T> GetByQuery<T>( Expression<Func<T, bool>> predicate, 
			Func<T, object> orderBy=null, string orderType="") 
			where T:class, IDocument
		{
			var cl = GetCollection<T> (); 
			var docs= cl.Find(Query<T>.Where(predicate));  
			return orderBy == null ? docs.ToList () :
				orderType.ToUpper ().StartsWith ("DES", StringComparison.InvariantCulture) ?
				docs.OrderByDescending (orderBy).ToList () :
				docs.OrderBy (orderBy).ToList ();
		}

		public void Execute<T>(Action<IQueryable<T>> action)
		{
			var cl = GetCollection<T> ().AsQueryable(); 
			action (cl); 
		}

		public IList<T> Execute<T>(Func<IQueryable<T>,IList<T>> func)
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


		public  InstalacionResponse GetInstalacionResponse(ReadInstalacion request)
		{
			var response = new InstalacionResponse ();
			response.Descarga = Single<Descarga> (q => q.Token == request.Token);

			if (response.Descarga.Id.IsNullOrEmpty ()) {
				throw new Exception("No existe informacion para el token:'{0}'".Fmt(request.Token));
			}

			response.Diagnostico = GetById<Diagnostico> (response.Descarga.IdDiagnostico);
			response.Plantilla = GetById<Plantilla> (response.Diagnostico.IdPlantilla);
			response.Empresa = GetById<Empresa> ( response.Diagnostico.IdEmpresa);

			response.Capitulos = GetByQuery<Capitulo> (q => q.IdPlantilla == response.Diagnostico.IdPlantilla, q=>q.Numeral);

			var capIds = response.Capitulos.ConvertAll (e => e.Id);

			var p = GetCollection<Pregunta>().Find( Query<Pregunta>.In ((q) => q.IdCapitulo, capIds ))
				.OrderBy(q=>q.Numeral).ToList();
			var r = GetByQuery<Respuesta> (q => q.IdDiagnostico == response.Diagnostico.Id, q=>q.IdPregunta);

			var g = GetByQuery<Guia> (q => q.IdPlantilla == response.Plantilla.Id, q=>q.Id);
			var rg = GetByQuery<RespuestaGuia> (q => q.IdDiagnostico == response.Diagnostico.Id, q=>q.IdGuia);

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

		public List<Diagnostico> ReadDiagnostico(ReadDiagnostico request){
			var f = 
				!request.Id.IsNullOrEmpty () ?
				(Expression<Func<Diagnostico, bool>>)(q => q.Id == request.Id) :
				(!request.IdEmpresa.IsNullOrEmpty () ?
					(Expression<Func<Diagnostico, bool>>)(q => q.IdEmpresa == request.IdEmpresa) :
					(Expression<Func<Diagnostico, bool>>)(q=>q.IdPlantilla==request.IdPlantilla)
				);

			var r= GetByQuery<Diagnostico> (f, q => q.Id, "desc");

			if (!request.IdEmpresa.IsNullOrEmpty ()) {
				var ids = r.ConvertAll (e => e.Id);

				Execute<Descarga> (q => {
					var d=q.Where( c=> c.IdDiagnostico.In(ids)).OrderByDescending(c=>c.Id).ToList();
					r.ForEach( e=>{
						e.Descargas= d.FindAll(i=>i.IdDiagnostico== e.Id);
					});
				});
			}
			return r;
		}

		public Result<Descarga> PostDescarga(CreateDescarga request){
			request.Data.Fecha = DateTime.UtcNow;
			request.Data.Token = Store.CreateRandomPassword ();
			return Post<Descarga> (request.Data);
		}

		public Result<Diagnostico> PostDiagnostico(CreateDiagnostico request){
			request.Data.Creado = DateTime.UtcNow;
			return Post (request.Data);
		}

		public Result<Diagnostico> PutDiagnostico(UpdateDiagnostico request){
			var r = Put (request.Data);
			Execute<Descarga> (q => {
				r.Data.Descargas= q.Where(c=>c.IdDiagnostico==request.Data.Id).OrderByDescending(e=>e.Id).ToList();
			});
			return r;
		}


		public BulkResult<Descarga> UpdateCuestionario(UpdateCuestionario request){


			var descarga = Single<Descarga> (q => q.Token == request.Data.Descarga.Token &&
				q.IdDiagnostico== request.Data.Descarga.IdDiagnostico
			);

			if (descarga.Id.IsNullOrEmpty ()) {
				throw new Exception("Token token:'{0}' No encontrado".Fmt(request.Data.Descarga.Token));
			}

			if (descarga.Estado == "green") {
				throw new Exception("Cuestionario asociado al Token token:'{0}' ya esta actualizado"
					.Fmt(request.Data.Descarga.Token));
			}

			if (descarga.Estado == "red") {
				descarga.Estado = "yellow";
				Put (descarga);
			}

			var bwr = new BulkWrite ();

			if (request.Data.Respuestas.Count > 0) {

				var cl = GetCollection<Respuesta> ();
				BulkWriteOperation bw = cl.InitializeOrderedBulkOperation ();
				request.Data.Respuestas.ForEach (r =>
					bw.Find (Query<Respuesta>.Where (q => q.IdDiagnostico == descarga.IdDiagnostico && q.IdPregunta == r.IdPregunta))
					.Upsert ().UpdateOne (Update<Respuesta>
						.Set (f => f.Valor, r.Valor).Set (f => f.NoAplicaChecked, r.NoAplicaChecked)));
				var wc = bw.Execute ();
				bwr.PopulateWith (wc);
				bwr.UpsertsCount = wc.Upserts.Count;
			}

			if (request.Data.RespuestasGuias.Count > 0) {

				var gIds = request.Data.RespuestasGuias.ConvertAll (e => e.IdGuia);

				var guias = GetCollection<Guia>().Find( Query<Guia>.In ((q) => q.Id, gIds ));

				var cl2 = GetCollection<RespuestaGuia> ();
				BulkWriteOperation bw2 = cl2.InitializeOrderedBulkOperation ();
				request.Data.RespuestasGuias.ForEach (r => {
					var guia= guias.FirstOrDefault(g=>g.Id== r.IdGuia);
					if (guia==default(Guia)){
						guia=new Guia();
					}
					bw2.Find (Query<RespuestaGuia>
						.Where (q => q.IdDiagnostico == descarga.IdDiagnostico && q.IdGuia == r.IdGuia))
						.Upsert ().UpdateOne (Update<RespuestaGuia>
							.Set (f => f.Valor, r.Valor)
							.Set (f => f.NoAplicaChecked, r.NoAplicaChecked)
							.Set( f => f.Tipo, (guia==default(Guia)? guia.Tipo:"string")));
				});

				var wc2 =bw2.Execute ();
				bwr.DeleteCount += wc2.DeletedCount;
				bwr.InsertedCount += wc2.InsertedCount;
				bwr.MatchedCount += wc2.MatchedCount;
				bwr.UpsertsCount += wc2.Upserts.Count;
			}
			if ((request.Data.Respuestas.Count + request.Data.RespuestasGuias.Count) > 0 &&
			    ((bwr.UpsertsCount + bwr.MatchedCount) ==
					(request.Data.Respuestas.Count + request.Data.RespuestasGuias.Count))) {
				descarga.Estado = "green";
				Put (descarga);
			}
		
			return CreateResult (descarga, bwr);
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
		static BulkResult<T> CreateResult<T>(T data, BulkWrite wcr) where T:IDocument
		{
			return new BulkResult<T> {
				Data = data,
				BulkWrite= wcr
			};
		}

		static string CreateRandomPassword(int passwordLength=32) 
		{ 
			const string allowedChars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789!@$?"; 
			Byte[] randomBytes = new Byte[passwordLength]; 
			char[] chars = new char[passwordLength]; 
			int allowedCharCount = allowedChars.Length; 

			for(int i = 0;i<passwordLength;i++) 
			{ 
				Random randomObj = new Random(); 
				randomObj.NextBytes(randomBytes); 
				chars[i] = allowedChars[(int)randomBytes[i] % allowedCharCount]; 
			} 

			return new string(chars); 
		}

	}
}

