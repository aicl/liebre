using System;
using ServiceStack;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Reflection;

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

	public static class PropertyUtil
	{
		public static string GetPropertyName<TObject> (this TObject type,
			Expression<Func<TObject, object>> propertyRefExpr)
		{
			return GetPropertyNameCore (propertyRefExpr.Body);
		}

		public static PropertyInfo GetPropertyInfo<TObject> (this TObject type, 
			Expression<Func<TObject, object>> propertyRefExpr)
		{
			return GetPropertyInfoCore (propertyRefExpr.Body);
		}

		public static string GetName<TObject> (Expression<Func<TObject, object>> propertyRefExpr)
		{
			return GetPropertyNameCore (propertyRefExpr.Body);
		}

		static PropertyInfo GetPropertyInfoCore (Expression propertyRefExpr)
		{
			if (propertyRefExpr == null)
				throw new ArgumentNullException ("propertyRefExpr", "propertyRefExpr is null.");

			MemberExpression memberExpr = propertyRefExpr as MemberExpression;
			if (memberExpr == null) {
				UnaryExpression unaryExpr = propertyRefExpr as UnaryExpression;
				if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
					memberExpr = unaryExpr.Operand as MemberExpression;
			}

			if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
				return memberExpr.Member as PropertyInfo;

			throw new ArgumentException ("No property reference expression was found.",
				"propertyRefExpr");
		}

		static string GetPropertyNameCore (Expression propertyRefExpr)
		{
			return GetPropertyInfoCore (propertyRefExpr).Name;
		}
	}

}

