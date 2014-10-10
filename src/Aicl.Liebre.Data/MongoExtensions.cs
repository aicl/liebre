using System;
using MongoDB.Driver;
using System.Collections.Generic;

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
	}
}

