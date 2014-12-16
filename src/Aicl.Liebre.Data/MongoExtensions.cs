using System;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Collections.Generic;
using Aicl.Liebre.Model;
using ServiceStack;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Aicl.Liebre.Data
{
	public static class MongoExtensions
	{
		public static List<T>  ToList<T>(this MongoCursor<T> cursor)
		{
			var result = new List<T> ();
			foreach (var doc in cursor) {
				result.Add (doc);
			}
			return result;	 
		}
			

		public static WriteConcernResult UpdateOnly<T> (this MongoCollection collection, T document)where T:class, IDocument
		{
			var type = typeof(T);
			var prop = type.GetProperties ();
			var ub = new UpdateBuilder ();
			foreach (var pi in prop){

				var bs = pi.FirstAttribute<BsonRepresentationAttribute>();
				if ((bs != null && bs.Representation == BsonType.ObjectId )
					|| pi.FirstAttribute<BsonIgnoreAttribute>() !=null)
					continue;
				var nv =pi.GetValue (document);
				var value = nv!=null? BsonValue.Create (nv):BsonNull.Value;
				ub.Set (pi.Name,value );
			};
			return collection.Update (Query<T>.EQ (e => e.Id, document.Id), ub);
		}

	}
}

