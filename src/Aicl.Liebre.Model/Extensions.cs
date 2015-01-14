using System;
using ServiceStack;
using System.Text.RegularExpressions;

namespace Aicl.Liebre.Model
{
	public static class Extensions
	{
		public static string GetCollectionName(this Type model) {
			var attr = model.FirstAttribute<CollectionAttribute> () 
				?? new CollectionAttribute (model.BaseType==typeof(object)? model:model.BaseType);
			return attr.CollectionType.Name.ToLower ();
		}

		public static string GetUserDataUrn(this Type type, string sessionId, string key){
			return "urn:{0}:UserData:{1}:{2}".Fmt( sessionId, type.Name,key);
		}

		public static string GetUserDataUrn<T>(this T type, string sessionId, string key){
			return "urn:{0}:UserData:{1}:{2}".Fmt(sessionId, typeof(T).Name,key);
		}

		public static bool StarstAndEnds(this string value, string start, string end){
			return value.StartsWith (start, StringComparison.InvariantCulture) 
				&& value.EndsWith (end,StringComparison.InvariantCulture);
		}

		public static bool IsISOString(this string value){
			return Regex.IsMatch (value, @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}.\d+Z$");
		}

		public static bool IsBsonISODate(this string value){
			return Regex.IsMatch (value, @"^ISODate\(""\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}.*\d*Z""\)$");
		}

		public static bool IsMsFormat(this string value){
			return Regex.IsMatch (value, @"/Date\(([^)]+)\)/");
		}

		public static bool IsInt(this string value){
			int x;
			return int.TryParse (value, out x);
		}

		public static bool IsLong(this string value){
			long x;
			return long.TryParse (value, out x);
		}

		public static bool IsBsonNumberLong(this string value){
			return Regex.IsMatch (value, @"^NumberLong\(""\d{5,}""\)$");
		}

		public static bool IsDouble(this string value){
			double x;
			return double.TryParse (value, out x);
		}

		public static bool IsBool(this string value){
			bool x;
			return bool.TryParse (value, out x);
		}

		public static string ToISOString(this DateTime value){
			return value.ToUniversalTime().ToString("o");
		}

	}
}

