using System;
using Aicl.Liebre.Model;
using Aicl.Liebre.Data;
using MyCouch;
using ServiceStack;
using ServiceStack.Text;
using System.IO;


namespace Aicl.Liebre.TestModelCouchDB
{
	public  class Test
	{
		public static void Main(){

			var client = new JsonServiceClient ("http://127.0.0.1:5984/");

			try {
				using (Stream responseStream = client.Get<Stream> ("/liebre")) {
					Console.WriteLine("response {0}", responseStream);
					var dto = responseStream.ReadFully ()
						.FromUtf8Bytes ();

					Console.WriteLine (dto);
				}
			}
			catch (WebServiceException e){
				return new {error="not_found",reason="missing"};
			}

		}
	}
}
			/* Ojo todo este esta bien !

			Console.WriteLine ("Hello my Friend");

			var store = new Store ("http://localhost:5984/liebre");

			var plantilla = store.Get<Plantilla>("a3b7ef91ab3f03159ab8e2b12e00b0ee");

			var capitulo = store.Post (new Capitulo{ Titulo = "Capitulo 2", Estandar = "Trabajar Cap2", IdPlantilla = plantilla.Id });

			for (var i = 0; i <= 10; i++) {

				store.Post (new Pregunta {
					IdCapitulo = capitulo.Id, 
					Enunciado = string.Format ("Capitluo {0} Pregunta {1}", capitulo.Id, i),
					Metodo = "Preguntar Bastante en II"
				});

			}


			var qi = store.QueryCapitulosPorPlantilla(plantilla.Id,
				r => Console.WriteLine ("capitulo {0}", r.Value.Titulo));

			Console.WriteLine ("qi.OffSet:{0}, qi.RowCount:{1}, qi.TotalRows:{2}",qi.OffSet, qi.RowCount, qi.TotalRows);
		}
		*/


		
	
