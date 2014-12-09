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


namespace Aicl.Liebre.TestStore
{
	public class PreguntasActualizar
	{
		const string url="mongodb://liebre:liebre@ds039000.mongolab.com:39000/liebre";

		MongoUrl Url { get; set; }
		MongoDatabase Db { get; set; }

		public PreguntasActualizar ()
		{
			var conventions = new ConventionPack { new IgnoreExtraElementsConvention(true) };
			ConventionRegistry.Register ("IgnoreExtraElements", conventions, _ => true);

			Url = new MongoUrl(url);
			Db = new MongoClient(Url).GetServer().GetDatabase(Url.DatabaseName);
			var cl = Db.GetCollection<Pregunta> ("pregunta");
			var preguntas = cl.FindAll ().ToList();
			var tabla = TablaNormas.GetData ();

			BulkWriteOperation bw = cl.InitializeOrderedBulkOperation ();
			tabla.ForEach (r =>{
				var item = preguntas.FirstOrDefault(p=>p.Numeral== r.Numeral);
				if(item!= default(Pregunta)){
				bw.Find (Query<Pregunta>.Where (q => q.Numeral == r.Numeral))
					.Upsert ().UpdateOne (Update<Pregunta>
						.Set (f => f.Decreto, r.Decreto).Set (f => f.OHSAS, r.OHSAS).Set(f=>f.RUC,r.RUC) );
				}
				else{
					Console.WriteLine("NO encontrado :'{0}'", r.Numeral);
				}
			});

			bw.Execute ();

		}

	}
}

