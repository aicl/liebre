using System;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson;

namespace Aicl.Liebre.Model
{
	public class DynamicSerializer:IBsonSerializer
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
			return  document;
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

			var json = value.ToString() ; 
			Console.WriteLine ("serialize");
			Console.WriteLine (json);

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
			}

			BsonSerializer.Serialize(bsonWriter, type, document,options);
		}

		#endregion


	}
}

