using System;
using ServiceStack;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;

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
			return GetPropertyInfoCore (propertyRefExpr).Name;
		}

		public static PropertyInfo GetPropertyInfo<TObject> (this TObject type, 
			Expression<Func<TObject, object>> propertyRefExpr)
		{
			return GetPropertyInfoCore (propertyRefExpr.Body);
		}

		public static List<PropertyInfo> GetPropertyInfoList<TObject>(this TObject type,Expression<Func<TObject, object>> propertyRefExpr)
		{
			var lst = new List<PropertyInfo>();
			if (propertyRefExpr.Body.NodeType == ExpressionType.MemberAccess) {
				lst.Add (GetPropertyInfoCore (propertyRefExpr.Body));
			} else if (propertyRefExpr.Body.NodeType == ExpressionType.New) {
				var nex = propertyRefExpr.Body as NewExpression;
				foreach (var a in nex.Arguments) {
					lst.Add (GetPropertyInfoCore (a));
				}
			}
			return lst;
		}

		public static List<string> GetPropertyNameList<TObject> (this TObject type,
			Expression<Func<TObject, object>> propertyRefExpr)
		{
			return GetPropertyInfoList (type, propertyRefExpr).ConvertAll (x => x.Name);
		}


		public static string GetName<TObject> (Expression<Func<TObject, object>> propertyRefExpr)
		{
			return GetPropertyInfoCore (propertyRefExpr).Name;
		}

		public static PropertyInfo GetInfo<TObject> (Expression<Func<TObject, object>> propertyRefExpr)
		{
			return GetPropertyInfoCore (propertyRefExpr.Body);
		}

		public static List<string> GetNames<TObject> (Expression<Func<TObject, object>> propertyRefExpr)
		{
			return GetPropertyInfoList<TObject>(default(TObject), propertyRefExpr).ConvertAll (x => x.Name);
		}

		public static List<PropertyInfo> GetInfos<TObject> (Expression<Func<TObject, object>> propertyRefExpr)
		{
			return GetPropertyInfoList<TObject> (default(TObject), propertyRefExpr);
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


	}

}

