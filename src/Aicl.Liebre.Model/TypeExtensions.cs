using System;
using ServiceStack;

namespace Aicl.Liebre.Model
{
	public static class TypeExtensions
	{
		public static string GetCollectionName(this Type model) {
			return model.Name.ToLower ();
		}

		public static string GetUserDataUrn(this Type type, string sessionId, string key){
			return "urn:{0}:UserData:{1}:{2}".Fmt( sessionId, type.Name,key);
		}

		public static string GetUserDataUrn<T>(this T type, string sessionId, string key){
			return "urn:{0}:UserData:{1}:{2}".Fmt(sessionId, typeof(T).Name,key);
		}
	}
}

