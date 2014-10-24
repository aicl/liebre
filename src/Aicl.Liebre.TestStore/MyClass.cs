using System;
using Aicl.Liebre.Data;
using Aicl.Liebre.Model;
using ServiceStack;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Aicl.Liebre.TestStore
{
	public class Data:IDocument
	{
		public Data()
		{
		}
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		[BsonSerializer(typeof(CustomSerializer))]
		public string String { get; set; }
		[BsonSerializer(typeof(CustomSerializer))]
		public Object DateTime { get; set; }
		[BsonSerializer(typeof(CustomSerializer))]
		public string Bool { get; set; }
		[BsonSerializer(typeof(CustomSerializer))]
		public string Int { get; set; }
		[BsonSerializer(typeof(CustomSerializer))]
		public string Long { get; set; }
		[BsonSerializer(typeof(CustomSerializer))]
		public string Double { get; set; }
		[BsonSerializer(typeof(CustomSerializer))]
		public string Object { get; set; }
		[BsonSerializer(typeof(CustomSerializer))]
		public string ArrayString { get; set; }
		[BsonSerializer(typeof(CustomSerializer))]
		public string ArrayInt { get; set; }
		[BsonSerializer(typeof(CustomSerializer))]
		public string ArrayObject { get; set; }
	}

	public class MyClass
	{
		static string SerializeAndEncode(object value){
			return value==null? null:
				Uri.EscapeUriString(MongoDB.Bson.BsonExtensionMethods.ToJson( value, value.GetType()));

			/*
			return value==null? null:
				value is DateTime ?
				Uri.EscapeUriString (MongoDB.Bson.BsonExtensionMethods.ToJson (value, value.GetType())) :
				Uri.EscapeUriString (ServiceStack.Text.JsonSerializer.SerializeToString (value));
*/
		}

		public static void Main ()
		{
			Console.WriteLine ("hello...");

			var store = new Store ("mongodb://liebre:liebre@ds039000.mongolab.com:39000/liebre");

			var data = new Data () {

			};

			data.ArrayInt= SerializeAndEncode (new []{1,2,3});
			data.ArrayObject = SerializeAndEncode (new [] {
				new {Uno = 1, Letra = "A", Fecha = new DateTime ()},
				new {Uno = 1, Letra = "B", Fecha = new DateTime ()}
			});

			data.ArrayString= SerializeAndEncode (new []{"A","B","C"});
			data.Bool= SerializeAndEncode (true);
			data.DateTime = SerializeAndEncode( new DateTime ());
			data.Double = SerializeAndEncode (2.25);
			data.Int = SerializeAndEncode (125);
			data.Long =  SerializeAndEncode (1250000000000000000);
			data.Object = SerializeAndEncode ( new {Letra= "A", Fecha= new DateTime(), Numero= 25, Valor =15.25});
			data.String = SerializeAndEncode ("hola mundo' !");

			store.Post(data);

			var r = store.Single<Data>(f=>f.Id==data.Id);




			Console.WriteLine("ArrayInt OK :{0})", r.ArrayInt==data.ArrayInt); 
			Console.WriteLine("ArrayObject OK :{0})", r.ArrayObject==data.ArrayObject); 
			Console.WriteLine("ArrayString OK :{0})", r.ArrayString==data.ArrayString); 
			Console.WriteLine("bool OK :{0})", r.Bool==data.Bool); 
			Console.WriteLine("DateTime OK :{0})", r.DateTime==data.DateTime); 
			Console.WriteLine("Double OK :{0})", r.Double==data.Double); 
			Console.WriteLine("Int OK :{0})", r.Int==data.Int); 
			Console.WriteLine("Long OK :{0})", r.Long==data.Long); 
			Console.WriteLine("Object OK :{0})", r.Object==data.Object); 
			Console.WriteLine("string OK :{0})", r.String==data.String); 

			Console.WriteLine ("SS datetime {0}", data.DateTime);
			Console.WriteLine ("From Mongo datetime {0}", r.DateTime);

			/*
			 * 
			var plantilla = new Plantilla{ Titulo = "Super Plantilla" };
			store.Post (plantilla);

			Console.WriteLine ("Listo plantilla {0}", plantilla.Id);
			for (var z = 0; z < 7; z++) {
				var guia = new Guia{ IdPlantilla = plantilla.Id, Enunciado = "Guia {0} - Plantilla {1}".Fmt (z, plantilla.Id) };
				store.Post (guia);
			}

			var guias = store.GetByQuery<Guia> (q=>q.IdPlantilla==plantilla.Id).ConvertAll(q=>q.Id);

			Console.WriteLine ("Listo guias; {0}", guias.Count);

			for (var i = 0; i < 5; i++) {
				var capitulo = new Capitulo {
					IdPlantilla = plantilla.Id, 
					Titulo = "Capitulo {0}".Fmt (i),
					Estandar= "Algun Estandar Capitulo {0}".Fmt (i),
				};

				store.Post (capitulo);
				Console.WriteLine ("Listo capitulo: {0}-{1}", capitulo.Id, capitulo.Titulo);

				for (var y = 0; y < 6; y++) {
					var pregunta = new Pregunta{ 
						IdCapitulo = capitulo.Id, 
						Enunciado = "Pregunta {0} Capitulo {1}".Fmt (y, i), 
						Metodo="Revisar {0}-{1}".Fmt(y,i),
						NoAplicaDisponible= (y%2)==0,
						IdGuias= (y%2)!=0? new List<string>(){}:((y%3)!=0? guias: new List<string>(){guias[y]}), 
					};

					store.Post (pregunta);
					Console.WriteLine ("Lista pregunta: {0}-{1}", pregunta.Id, pregunta.Enunciado);

				}
			}

			var empresa = new Empresa{ Nombre = "Mi primera Empresa", Nit = "8797" };
			store.Post (empresa);
			Console.WriteLine ("Lista empresa: {0}-{1}", empresa.Id, empresa.Nombre);

			var diagnostico = new Diagnostico {
				IdPlantilla = plantilla.Id,
				IdEmpresa = empresa.Id,

			};
			store.Post (diagnostico);
			Console.WriteLine ("Lista diagnostico: {0}-{1}-{2}", diagnostico.Id, diagnostico.IdEmpresa, diagnostico.IdPlantilla);

			//var inst = store.GetInstallDiagnosticoResponse (new InstallDiagnostico{ IdDiagnostico = diagnostico.Id });

			//Console.WriteLine (inst);


*/

			/*

			for(var i=0;i<10;i++)
			{
				var plantilla = new Plantilla{
					Name="Plantila {0}".Fmt(i),
					Titulo="El titulo {0}".Fmt(i)
				};
				store.Post (plantilla);
				Console.WriteLine (plantilla.Id);
			}


			var all = store.Get<Plantilla>();

			foreach (var d in all) {
				d.Name = "{0}. My Name is {1}".Fmt (d.Name, "actualizado");
				store.Put (d);
			}

			foreach (var d in all) {
				Console.WriteLine ("Id: {0} - Name : {1} Titulo {2} ", d.Id, d.Name, d.Titulo);
			}


			var p = new Plantilla{ Name = "Me gusta mi plantilla", Titulo = "Nada" };
			store.Post (p);

			var q = store.Single<Plantilla> (e => e.Name == p.Name);

			q.Titulo = "Muy Actualizado";
			store.Put (q);

			var r = store.Single<Plantilla> (e => e.Name == p.Name);

			Console.WriteLine ("Id: {0} - Name : {1} Titulo {2} ", r.Id, r.Name, r.Titulo);

			r = store.Single<Plantilla> (e => e.Name == "p.Name");
			Console.WriteLine ("Id: {0} - Name : {1} Titulo {2} ", r.Id, r.Name, r.Titulo);


			var x = store.GetByQuery<Plantilla> (e => e.Name == "p.Name");
			Console.WriteLine ("0=={0}", x.Count);


			all = store.Get<Plantilla>();
			foreach (var d in all) {
				store.Delete (d);
			}

			var c = store.Get<Plantilla> ().Count;

			Console.WriteLine ("0=={0}", c);
			*/

			Console.WriteLine ("this is The End");

		}
	}
}