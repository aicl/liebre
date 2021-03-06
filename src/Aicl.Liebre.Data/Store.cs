﻿using System;
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
using System.IO;
using System.Text;

namespace Aicl.Liebre.Data
{
	public partial class Store:IStore
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
			return cl.FindOne(Query<T>.EQ(e=>e.Id,request.Id));
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
			return cl.FindOne(Query<T>.Where(predicate))?? new T();
		}

		public T Single<T>(string id) where T:class, IDocument, new()
		{
			return !id.IsNullOrEmpty () ? Single<T> (Query<T>.EQ (q => q.Id, id)) : new T ();
		}

		public T Single<T>( IMongoQuery query) where T: class, new()
		{
			var cl= GetCollection<T> ();
			return cl.FindOne (query) ?? new T ();
		}

		public Result<T> Post<T>(T request) where T:class, IDocument
		{
			var cl = GetCollection<T> ();
			request.Id = string.Empty;
			return Store.CreateResult(request, cl.Insert (request));
		}

		public Result<T> Put<T>(T  request, Expression<Func<T, object>> fieldsToUpdate=null, 
			Expression<Func<T, object>> fieldsToIgnore=null ) where T:class, IDocument
		{
			var cl= GetCollection<T> ();
			return Store.CreateResult(request, cl.UpdateOnly (request, fieldsToUpdate,fieldsToIgnore));
			//return Store.CreateResult (request,cl.Update (Query<T>.EQ (e => e.Id, request.Id), Update<T>.Replace (request))); 
		}

		Result<T> Save<T>(T request ) where T:class, IDocument
		{
			var cl = GetCollection<T> ();
			return Store.CreateResult(request, cl.Save (request));
		}


		public Result<T> Delete<T>(IHasStringId request) where T:class, IDocument, new() 
		{
			var cl= GetCollection<T> ();
			return Store.CreateResult (new T{ Id = request.Id }, cl.Remove (Query<T>.EQ (e => e.Id, request.Id)));
		}


		public Result<T> Delete<T>(string id) where T:class, IDocument, new()
		{
			var cl= GetCollection<T> ();
			return Store.CreateResult (new T{ Id = id }, cl.Remove (Query<T>.EQ (e => e.Id, id)));
		}

		public Result<T> Delete<T>(Expression<Func<T, bool>> predicate) where T:class, IDocument, new()
		{
			var cl= GetCollection<T> (); 
			return Store.CreateResult (new T (), cl.Remove (Query<T>.Where (predicate)));
		}


		public  ReadCuestionarioResponse DownloadCuestionario(ReadCuestionario request)
		{
			var response = new ReadCuestionarioResponse ();
			response.Descarga = Single<Descarga> (q => q.Token == request.Token);

			if (response.Descarga.Id.IsNullOrEmpty ()) {
				throw new Exception("No existe informacion para el token:'{0}'".Fmt(request.Token));
			}

			if (response.Descarga.Estado != "grey") {
				throw new Exception("No disponible Cuestionario con el token:'{0}'. Estado:'{1}'"
					.Fmt(request.Token, response.Descarga.Estado));
			}

			response.Descarga.Estado = "red";
			Put (response.Descarga);

			response.Diagnostico = GetById<Diagnostico> (response.Descarga.IdDiagnostico);
			response.Plantilla = GetById<Plantilla> (response.Diagnostico.IdPlantilla);
			response.Empresa = GetById<Empresa> ( response.Diagnostico.IdEmpresa);

			response.Capitulos = GetByQuery<Capitulo> (q => q.IdPlantilla == response.Diagnostico.IdPlantilla)
				.OrderBy( q=> NormalizeNumeral(q.Numeral)).ToList();

			var capIds = response.Capitulos.ConvertAll (e => e.Id);

			var p = GetCollection<PreguntaDescarga>().Find( Query<Pregunta>.In ((q) => q.IdCapitulo, capIds ))
				.OrderBy(q=>NormalizeNumeral( q.Numeral)).ToList();
			var r = GetByQuery<Respuesta> (q => q.IdDiagnostico == response.Diagnostico.Id, q=>q.IdPregunta);

			var g = GetByQuery<Guia> (q => q.IdPlantilla == response.Plantilla.Id, q=>q.Id);
			var rg = GetByQuery<RespuestaGuia> (q => q.IdDiagnostico == response.Diagnostico.Id, q=>q.IdGuia);

			p.ForEach (q => response.Preguntas.Add (new ViewPregunta {
				Pregunta = q,
				Respuesta = r.FirstOrDefault (rq => rq.IdPregunta == q.Id) ??
					new Respuesta{ 
					IdPregunta = q.Id, 
					IdDiagnostico = response.Diagnostico.Id, 
					Respuestas=new List<bool>(q.Preguntas.Count),
					Valor = (q.Preguntas.Count > 0 ? (short)0 : default(short?))
				}
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

			var d = GetByQuery<Descarga> (f => f.IdDiagnostico == request.Data.IdDiagnostico && f.Estado != "green");

			if (d.Count > 0) {
				throw new Exception ("Exiten descargas activas para el diagnostico seleccionado"
					.Fmt (request.Data.IdDiagnostico));
			}

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
				request.Data.Respuestas.ForEach (r =>{
					if(r.Respuestas.Count>0){
						r.Valor= (short?)( r.Respuestas.Exists(v=>v==false)?0:1);
					}
					bw.Find (Query<Respuesta>.Where (q => q.IdDiagnostico == descarga.IdDiagnostico && q.IdPregunta == r.IdPregunta))
					.Upsert ().UpdateOne (Update<Respuesta>
							.Set (f => f.Valor, r.Valor).Set (f => f.NoAplicaChecked, r.NoAplicaChecked).Set(f=>f.Respuestas,r.Respuestas) );});
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
					bw2.Find (Query<RespuestaGuia>
						.Where (q => q.IdDiagnostico == descarga.IdDiagnostico && q.IdGuia == r.IdGuia))
						.Upsert ().UpdateOne (Update<RespuestaGuia>
							.Set (f => f.Valor, r.Valor)
							.Set (f => f.NoAplicaChecked, r.NoAplicaChecked)
							.Set( f => f.Tipo, guia.Tipo));
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
				GetCollection<Diagnostico> ().FindAndModify (new FindAndModifyArgs{ 
					Query=Query<Diagnostico>.EQ(x=>x.Id,descarga.IdDiagnostico),
					Update= Update<Diagnostico>.Inc(x=>x.Revision,1).Set(x=>x.FechaRevision, DateTime.UtcNow)
				});
			}
		
			return CreateResult (descarga, bwr);
		}


		public ListResult<CIIU> ReadCIIU(ReadCIIU request){
			return ReadFromFile<CIIU> ("CIIU.json");
		}

		public ListResult<Ciudad> ReadCiudad(ReadCiudad request){
			return ReadFromFile<Ciudad> ("ciudades.json");
		}

		public ListResult<Departamento> ReadDepartamento(ReadDepartamento request){
			return ReadFromFile<Departamento> ("departamentos.json");
		}

		public ListResult<Riesgo> ReadRiesgo(ReadRiesgo request){
			return ReadFromFile<Riesgo> ("riesgos.json");
		}

		public ListResult<Rango> ReadRango(ReadRango request){
			return ReadFromFile<Rango> ("rangos.json");
		}

		public ListResult<Externo> ReadExterno(ReadExterno request){
			return ReadFromFile<Externo> ("externos.json");
		}

		public ListResult<Requisito> ReadRequisito(ReadRequisito request){
			return ReadFromFile<Requisito> ("requisitos.json");
		}

		public ListResult<ActividadAltoRiesgo> ReadActividaAltoRiesgo(ReadActividadAltoRiesgo request){
			return ReadFromFile<ActividadAltoRiesgo> ("actividadesaltoriesgo.json");
		}

		public ListResult<Norma> ReadNorma(ReadNorma request){
			return ReadFromFile<Norma> ("normas.json");
		}

		public List<Pregunta> ReadPregunta(ReadPregunta request){
			var cl = GetCollection<Pregunta> (); 
			var docs= cl.Find(Query<Pregunta>.Where(q=>q.IdCapitulo==request.IdCapitulo));  
			return docs.ToList ().OrderBy (q =>  NormalizeNumeral(q.Numeral)).ToList ();
		}
		public List<Capitulo> ReadCapitulo(ReadCapitulo request){
			var cl = GetCollection<Capitulo> (); 
			var docs= cl.Find(Query<Capitulo>.Where(q=>q.IdPlantilla==request.IdPlantilla));  
			return docs.ToList ().OrderBy (q =>  NormalizeNumeral(q.Numeral)).ToList ();
		}


		public  DiagnosticoInfoResponse ReadDiagnosticoInfo(DiagnosticoInfo request)
		{
			var response = new DiagnosticoInfoResponse ();
			response.Norma = request.Norma;
			response.Normas = ReadNorma (new ReadNorma ()).Data;

			response.Diagnostico = GetById<Diagnostico> (request.Id) ?? new Diagnostico();
			response.Plantilla = GetById<Plantilla> (response.Diagnostico.IdPlantilla) ?? new Plantilla();
			response.Empresa = GetById<EmpresaLogo> (response.Diagnostico.IdEmpresa) ?? new EmpresaLogo ();

			response.Capitulos = GetByQuery<CapituloInfo> (typeof(Capitulo), q => q.IdPlantilla == response.Diagnostico.IdPlantilla)
				.OrderBy( q=> NormalizeNumeral(q.Numeral)).ToList();

			var capIds = response.Capitulos.ConvertAll (e => e.Id);

			var p = GetCollection<Pregunta>().Find( Query<Pregunta>.In ((q) => q.IdCapitulo, capIds ))
				.OrderBy(q=>NormalizeNumeral( q.Numeral)).ToList();
			var r = GetByQuery<Respuesta> (q => q.IdDiagnostico == response.Diagnostico.Id, q=>q.IdPregunta);

			p.ForEach (q =>{
				var vp=  new ViewPreguntaInfo {
					Pregunta = q,
					Respuesta = r.FirstOrDefault (rq => rq.IdPregunta == q.Id) ??
						new Respuesta{ 
						IdPregunta = q.Id, 
						IdDiagnostico = response.Diagnostico.Id, 
						Respuestas=new List<bool>(q.Preguntas.Count),
						Valor = (q.Preguntas.Count > 0 ? (short)0 : default(short?))
					},
				};

				response.Preguntas.Add(vp);
				var cap = response.Capitulos.First(c=>c.Id==q.IdCapitulo);
				++cap.TotalQ; 
				if(vp.Respuesta.Valor==1) ++cap.TotalR;

				response.Normas.ForEach(n=>{
					var sc = n.Alias=="Decreto"?vp.Pregunta.Decreto: n.Alias=="OHSAS"? vp.Pregunta.OHSAS:vp.Pregunta.RUC;
					var item = n.Items.FirstOrDefault(i=>i.Codigo== sc) ;
					if (item==default(NormaItem) ){
						item= new NormaItem{Codigo=vp.Pregunta.Numeral};
						n.Items.Add(item);
					}

					var grupo= n.Grupos.FirstOrDefault(gr=>gr.Codigos.Contains(sc));
					if(grupo==default(NormaGrupo)){
						grupo = new NormaGrupo();
						grupo.Codigos.Add(vp.Pregunta.Numeral);
						n.Grupos.Add(grupo);
					}

					++grupo.TotalQ;
					++item.TotalQ;
					++n.TotalQ;
					if(vp.Respuesta.Valor==1) {
						++n.TotalR;
						++item.TotalR;
						++grupo.TotalR;
					}
				});

			});

			response.Preguntas = response.Preguntas.OrderByDescending (q => q.Respuesta.Valor).ToList();
			response.Normas.ForEach (n => {
				n.Items= n.Items.OrderByDescending(x=>x.TotalR>0?x.TotalQ:x.TotalQ*-1).OrderByDescending (x => (double) x.TotalR / (double) x.TotalQ).ToList ();
				n.Grupos= n.Grupos.OrderByDescending(x=>x.TotalR>0?x.TotalQ:x.TotalQ*-1).OrderByDescending (x => (double) x.TotalR / (double) x.TotalQ).ToList ();
			});
	
			return response;
		}

		public Result<Plantilla> ClonarPlantilla (Plantilla data)
		{
			var cl = GetCollection<Plantilla> ();

			if (cl.FindOne (Query<Plantilla>.EQ (e => e.Id, data.Id)) == default(Plantilla)) {
				throw new Exception ("No Exite plantilla con Id:'{0}'".Fmt (data.Id));
			}

			var clGuias = GetCollection<Guia> ();
			var guias = clGuias.Find(Query<Guia>.EQ(e=>e.IdPlantilla,data.Id)).ToList();

			var clCapitulos = GetCollection<Capitulo> ();
			var capitulos= clCapitulos.Find(Query<Capitulo>.EQ(e=>e.IdPlantilla, data.Id)).ToList();
			var capIds = capitulos.ConvertAll (e => e.Id);

			var clPreguntas = GetCollection<Pregunta> ();
			var preguntas = clPreguntas.Find(Query<Pregunta>.In(e=>e.IdCapitulo, capIds)).ToList();

			data.Id = string.Empty;
			var wcr = cl.Insert (data);

			var gIds = new List<string> ();
			guias.ForEach (g => {
				gIds.Add(g.Id);
				g.IdPlantilla= data.Id;
				g.Id= string.Empty;
			});

			clGuias.InsertBatch (guias);

			capitulos.ForEach (c => {
				c.Id=string.Empty;
				c.IdPlantilla= data.Id;
			});

			clCapitulos.InsertBatch (capitulos);

			preguntas.ForEach (p => {

				var ic = capIds.FindIndex(i=>i==p.IdCapitulo);
				p.IdCapitulo= capitulos[ic].Id;

				var ng = new List<string>();
				p.IdGuias.ForEach(x=> {
					var ig = gIds.FindIndex(i=>i== x);
					ng.Add( guias[ig].Id);
				});
				p.IdGuias=ng;

				p.Id=string.Empty;

			});

			clPreguntas.InsertBatch (preguntas);
			return Store.CreateResult (data, wcr);
		}

		static string NormalizeNumeral(string numeral){
			var a = numeral.Split ('.');
			var t = new StringBuilder ();
			Array.ForEach (a, i => t.Append (i.PadLeft (4, '0')));
			return t.ToString();

		}

		static ListResult<T> ReadFromFile<T>(string fileName){
			var file = PathUtils.CombinePaths("~","json",fileName).MapHostAbsolutePath ();
			using(var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read )){
				var c= JsonSerializer.DeserializeFromStream<List<T>> (fileStream);
				return new ListResult<T> { Data = c };
			}
		}

		MongoCollection<T> GetCollection<T>(){
			return Db.GetCollection<T> (typeof(T).GetCollectionName());
		}

		MongoCollection<T> GetCollection<T>(string collection){
			return Db.GetCollection<T> (collection);
		}

		//MongoCollection<T> GetCollection<T>(Type rootType){
		//	return Db.GetCollection<T> (rootType.GetCollectionName());
		//}

		List<T> GetByQuery<T>(Type rootType, Expression<Func<T, bool>> predicate,Func<T, object> orderBy=null, string orderType="") where T:class, IDocument
		{
			return GetByQuery<T> (rootType.GetCollectionName (), predicate, orderBy, orderType);
		}

		List<T> GetByQuery<T>(string collection, Expression<Func<T, bool>> predicate,Func<T, object> orderBy=null, string orderType="") where T:class, IDocument
		{
			var cl = GetCollection<T> (collection);
			var docs= cl.Find(Query<T>.Where(predicate));
			return orderBy == null ? docs.ToList () :
				orderType.ToUpper ().StartsWith ("DES", StringComparison.InvariantCulture) ?
				docs.OrderByDescending (orderBy).ToList () :
				docs.OrderBy (orderBy).ToList ();
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

		public static string CreateRandomPassword(int passwordLength=32) 
		{ 
			const string allowedChars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789!@$?"; 
			Byte[] randomBytes = new Byte[passwordLength]; 
			char[] chars = new char[passwordLength]; 
			int allowedCharCount = allowedChars.Length; 

			for(int i = 0;i<passwordLength;i++) 
			{ 
				var randomObj = new Random(); 
				randomObj.NextBytes(randomBytes); 
				chars[i] = allowedChars[(int)randomBytes[i] % allowedCharCount]; 
			} 

			return new string(chars); 
		}

		public bool Exists<T> (string id) where T: class, IDocument
		{
			var cl = GetCollection<T> ();
			return cl.FindOne (Query<T>.EQ (e => e.Id, id)) != null;
		}

		public bool Exists<T> (IMongoQuery query) where T: class
		{
			var cl = GetCollection<T> ();
			return cl.FindOne (query) != null;
		}


	}
}

