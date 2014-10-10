using System;
using Aicl.Liebre.Data;
using Aicl.Liebre.Model;
using ServiceStack;
using System.Collections.Generic;

namespace Aicl.Liebre.TestStore
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("hello...");

			var store = new Store ("mongodb://liebre:liebre@ds039000.mongolab.com:39000/liebre");

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

			var inst = store.GetInstallDiagnosticoResponse (new InstallDiagnostico{ IdDiagnostico = diagnostico.Id });

			Console.WriteLine (inst);




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
