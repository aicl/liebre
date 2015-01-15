using System;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Collections.Generic;
using Aicl.Liebre.Model;
using ServiceStack;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Linq.Expressions;
using System.Linq;

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
			

		public static WriteConcernResult UpdateOnly<T> (this MongoCollection collection, T document,
			Expression<Func<T, object>> fieldsToUpdate=null,
			Expression<Func<T, object>> fieldsToIgnore=null)where T:class, IDocument
		{
			var type = typeof(T);
			var prop = fieldsToUpdate==null? type.GetProperties (): PropertyUtil.GetInfos(fieldsToUpdate).ToArray();
			var ign = fieldsToIgnore==null? new string[]{}: PropertyUtil.GetNames(fieldsToIgnore).ToArray();
			var ub = new UpdateBuilder ();
			foreach (var pi in prop){
				var bs = pi.FirstAttribute<BsonRepresentationAttribute>();
				if ((bs != null && bs.Representation == BsonType.ObjectId )
					|| pi.FirstAttribute<BsonIgnoreAttribute>() !=null || pi.FirstAttribute<ReadOnlyAttribute>()!=null
					|| ign.Contains(pi.Name)
				)
					continue;
				var nv =pi.GetValue (document);
				var value = nv!=null? BsonValue.Create (nv):BsonNull.Value;
				ub.Set (pi.Name,value );
			};
			return collection.Update (Query<T>.EQ (e => e.Id, document.Id), ub);
		}
	}

	public static class StringExtensions{

		public static string HideContent(this string email, int step=2){
			email = email ?? string.Empty;
			var chars = new char[email.Length]; 
			for (var i = 0; i < email.Length; i++) {
				chars [i] = i%step!=0  || email[i]=='@' ? email [i]:'*';
			}
			return new string (chars);
		}

	}
}

