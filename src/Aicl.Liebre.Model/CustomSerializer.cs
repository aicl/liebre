using System;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson;

namespace Aicl.Liebre.Model
{
	public class CustomSerializer:IBsonSerializer
	{

		#region Implementation of IBsonSerializer

		public object Deserialize(BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
		{
			return Deserialize(bsonReader, nominalType, null, options);
		}

		public object Deserialize(BsonReader bsonReader, Type nominalType, Type actualType,
			IBsonSerializationOptions options)
		{
			Type type; 
			var bsonType = bsonReader.CurrentBsonType; 
			switch (bsonType){
			case BsonType.Null:
				bsonReader.ReadNull();
				return null;
			case BsonType.Document:
				type = typeof(BsonDocument);
				break;
			case BsonType.Array:
				type = typeof(BsonArray);
				break;
			case BsonType.DateTime:
				type = typeof(BsonDateTime);
				break;
			case BsonType.Boolean:
				type = typeof(BsonBoolean);
				break;
			case BsonType.Int32:
				type = typeof(BsonInt32);
				break;
			case BsonType.Int64:
				type = typeof(BsonInt64);
				break;
			case BsonType.Double:
				type = typeof(BsonDouble);
				break;
			case BsonType.String:
				type = typeof(BsonString);
				break;
			default:
				type = typeof(object);
				break;
			}
			var document = BsonSerializer.Deserialize (bsonReader, type, options);

			return Uri.EscapeUriString( document.ToJson (type));
		}

		public IBsonSerializationOptions GetDefaultSerializationOptions()
		{
			return new DocumentSerializationOptions();
		}

		public void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
		{
			if (value == null) {
				bsonWriter.WriteNull ();
				return;
			}
			object document;
			Type type;

			var json = Uri.UnescapeDataString(value.ToString()) ; //viene con el JSON.stringify.encode...

			if (json.StarstAndEnds ("{", "}")) {
				document = BsonDocument.Parse (json);
				type = typeof(BsonDocument);
			} else if (json.StarstAndEnds ("[", "]")) {
				document = BsonSerializer.Deserialize<BsonArray> (json);
				type = typeof(BsonArray);
			} else if (json.IsBsonISODate()) {
				document = BsonSerializer.Deserialize<BsonDateTime> (json);
				type = typeof(BsonDateTime);
			} else if (json.IsISOString ()) {
				document = BsonSerializer.Deserialize<BsonDateTime> ("ISODate(\"" + json +  "\")");
				type = typeof(BsonDateTime);
			}  else if (json.IsMsFormat()) {
				document = BsonSerializer.Deserialize<BsonDateTime> ("new "+ json.Replace("/",""));
				type = typeof(BsonDateTime);
			}  else if (json.IsBool ()) {
				document = BsonSerializer.Deserialize<BsonBoolean> (json);
				type = typeof(BsonBoolean);
			} else if (json.IsInt ()) {
				document = BsonSerializer.Deserialize<BsonInt32> (json);
				type = typeof(BsonInt32);
			}  else if (json.IsBsonNumberLong ()) {
				document = BsonSerializer.Deserialize<BsonInt64> (json);
				type = typeof(BsonInt64);
			} else if (json.IsLong ()) {
				document = BsonSerializer.Deserialize<BsonInt64> ("NumberLong(\""+json+"\")");
				type = typeof(BsonInt64);
			} else if (json.IsDouble ()) {
				document = BsonSerializer.Deserialize<BsonDouble> (json);
				type = typeof(BsonDouble);
			}  else  {
				document = BsonSerializer.Deserialize<BsonString> (json.StarstAndEnds( "\"", "\"")
					?json
					:"\""+json+"\"" );
				type = typeof(BsonString);
			}/* else {
				Console.WriteLine("Obect ....{0}", json);
				document = value.ToJson (value.GetType ());
				type = typeof(object); // value.GetType();
			}*/

			BsonSerializer.Serialize(bsonWriter, type, document,options);
		}

		#endregion


	}
}

/*
 * http://stackoverflow.com/questions/19664394/mongodb-c-sharp-exception-cannot-deserialize-string-from-bsontype-int32
 * http://stackoverflow.com/questions/10222472/is-there-mongodb-c-sharp-driver-support-system-dynamic-dynamicobject-in-net-4
 * http://blog.abodit.com/2011/09/dynamic-persistence-with-mongodb-look-no-classes-polymorphism-in-c/
 * http://www.charlascylon.com/post/66358243519/tutorial-mongodb-csharp-consultas-i
 * http://stackoverflow.com/questions/179940/convert-utc-gmt-time-to-local-time
 * http://stackoverflow.com/questions/6525538/convert-utc-date-time-to-local-date-time-using-javascript

DateTime convertedDate = DateTime.SpecifyKind(  DateTime.Now,   DateTimeKind.Local);
csharp> convertedDate
23/10/2014 9:19:12 p. m.
csharp> convertedDate.ToLocalTime();
23/10/2014 9:19:12 p. m.
csharp> convertedDate.To
ToBinary Today ToFileTime ToFileTimeUtc ToLocalTime ToLongDateString ToLongTimeString ToOADate ToShortDateString ToShortTimeString ToString ToUniversalTime 
csharp> convertedDate.ToUniversalTime();
24/10/2014 2:19:12 a. m.
csharp> convertedDate.To
ToBinary Today ToFileTime ToFileTimeUtc ToLocalTime ToLongDateString ToLongTimeString ToOADate ToShortDateString ToShortTimeString ToString ToUniversalTime 
csharp> convertedDate.To

var myConventions = new ConventionProfile();
myConventions.SetSerializationOptionsConvention(
    new TypeRepresentationSerializationOptionsConvention(typeof (Guid), BsonType.String));

BsonClassMap.RegisterConventions(myConventions, t => t == typeof (MyClass));

var ss = "{\"ciudad\":\"Cúcuta\",\"metodos\":\"Todos ls posibles\",\"usuarios\":25,\"comp\":{\"1\":\"1\",\"2\":\"2\"}}"
var bd =BsonDocument.Parse(ss)
//{ ciudad=Cúcuta, metodos=Todos ls posibles, usuarios=25, comp={ "1" : "1", "2" : "2" } }
MongoDB.Bson.BsonExtensionMethods.ToJson<MongoDB.Bson.BsonDocument>(bd)  // al ss !

var abd =BsonDocument.Parse("{\"a\":[1,2,3]}")

var abd =BsonDocument.Parse("{\"a\":\"abc\"}")

var ba = BsonSerializer.Deserialize<BsonArray> ("[{\"1\":1,\"2\":2,\"3\":3, \"o\":{\"a\":\"ab\"}}]")
csharp> ba
{ { 1=1, 2=2, 3=3, o={ "a" : "ab" } } }

map.MapProperty(member => member.Data)
   .SetElementName("Data")
   .SetSerializer(new DynamicSerializer());
or on the property

[BsonSerializer(typeof(DynamicSerializer))]
public dynamic Data { get; set; }
And just include the following class

public class DynamicSerializer : IBsonSerializer
{
  #region Implementation of IBsonSerializer

  public object Deserialize(BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
  {
    return Deserialize(bsonReader, nominalType, null, options);
  }

  public object Deserialize(BsonReader bsonReader, Type nominalType, Type actualType,
    IBsonSerializationOptions options)
  {
  // talvez hacerle el encode previamente ?
    if (bsonReader.GetCurrentBsonType() != BsonType.Document) throw new Exception("Not document");
    var bsonDocument = BsonSerializer.Deserialize(bsonReader, typeof(BsonDocument), options) as BsonDocument;
    //return JsonConvert.DeserializeObject<dynamic>(bsonDocument.ToJson());  // MIO 
    return bsonDocument.ToJson();
    //MongoDB.Bson.BsonExtensionMethods.ToJson<MongoDB.Bson.BsonArray>(ba)
  }

  public IBsonSerializationOptions GetDefaultSerializationOptions()
  {
    return new DocumentSerializationOptions();
  }

  public void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
  {
    //var json = (value == null) ? "{}": JsonConvert.SerializeObject(value); // MIO 
    // talvez hacerle el decode previamente ?
    var json = value.ToString(); viene con el JSON.stringify
    BsonDocument document = BsonDocument.Parse(json);
    BsonSerializer.Serialize(bsonWriter, typeof(BsonDocument), document,options); 
  }

  #endregion
}


 */ 