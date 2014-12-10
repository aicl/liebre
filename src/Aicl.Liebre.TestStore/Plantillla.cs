using System;
using Aicl.Liebre.Data;
using Aicl.Liebre.Model;
using ServiceStack;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Linq;
using MongoDB.Bson.Serialization.Conventions;
using System.Collections.Generic;

namespace Aicl.Liebre.TestStore
{
	public class Plantillla
	{
		const string url="mongodb://liebre:liebre@ds039000.mongolab.com:39000/liebre";

		MongoUrl Url { get; set; }
		MongoDatabase Db { get; set; }

		public Plantillla ()
		{
			var conventions = new ConventionPack { new IgnoreExtraElementsConvention(true) };
			ConventionRegistry.Register ("IgnoreExtraElements", conventions, _ => true);

			Url = new MongoUrl(url);
			Db = new MongoClient(Url).GetServer().GetDatabase(Url.DatabaseName);
		}

		public Plantilla Clonar(string idPlantilla, string nombreClone=null){
			var cl = GetCollection<Plantilla> ();
			var pl = cl.FindOne (Query<Plantilla>.EQ (e => e.Id, idPlantilla));
			if (pl == default(Plantilla)) {
				throw new Exception ("No Exite plantilla con Id:'{0}'".Fmt (idPlantilla));
			}

			var clGuias = GetCollection<Guia> ();
			var guias = clGuias.Find(Query<Guia>.EQ(e=>e.IdPlantilla,idPlantilla)).ToList();

			var clCapitulos = GetCollection<Capitulo> ();
			var capitulos= clCapitulos.Find(Query<Capitulo>.EQ(e=>e.IdPlantilla, idPlantilla)).ToList();
			var capIds = capitulos.ConvertAll (e => e.Id);

			var clPreguntas = GetCollection<Pregunta> ();
			var preguntas = clPreguntas.Find(Query<Pregunta>.In(e=>e.IdCapitulo, capIds)).ToList();

			pl.Id = string.Empty;
			pl.Titulo = nombreClone ?? "Copia de: " + pl.Titulo;
			cl.Insert (pl);

			var gIds = new List<string> ();
			guias.ForEach (g => {
				gIds.Add(g.Id);
				g.IdPlantilla= pl.Id;
				g.Id= string.Empty;
			});

			clGuias.InsertBatch (guias);

			capitulos.ForEach (c => {
				c.Id=string.Empty;
				c.IdPlantilla= pl.Id;
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

			return pl;

		}

		MongoCollection<T> GetCollection<T>(){
			return Db.GetCollection<T> (typeof(T).GetCollectionName());
		}

		MongoCollection<T> GetCollection<T>(string collection){
			return Db.GetCollection<T> (collection);
		}

		MongoCollection<T> GetCollection<T>(Type rootType){
			return Db.GetCollection<T> (rootType.GetCollectionName());
		}
	}
}

