using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Delta.NET.JsonTools
{
    public class OperationConverter : JsonConverter<Operation>
    {
        public override Operation? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            reader.Read();
            var Name = reader.GetString();
            reader.Read();
            switch (Name)
            {
                case "retain":
                    var Op = new RetainOperation() { Value = reader.GetInt32() };
                    reader.Read();
                    return Op;
                case "delete":
                    var OpDelete = new DeleteOperation() { Value = reader.GetInt32() };
                    reader.Read();
                    return OpDelete;
                case "attributes":
                case "insert":
                default:
                    var Insert = new InsertOperation();
                    while (reader.TokenType != JsonTokenType.EndObject)
                    {
                        Dictionary<string, string> Attributes = null;
                        if (reader.TokenType == JsonTokenType.StartObject && Name == "attributes")
                        {
                            Insert.Attributes = GetAttributes(ref reader);
                        }
                        else
                        {
                            Insert.Insert = GetInsertData(ref reader);
                        }
                        reader.Read();
                        if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            Name = reader.GetString();
                            reader.Read();
                        }
                    }
                    return Insert;
            }

        }
        protected Dictionary<string, AttributeValue> GetAttributes(ref Utf8JsonReader reader)
        {
            var Attributes = new Dictionary<string, AttributeValue>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;
                var Key = reader.GetString();
                reader.Read();
                Attributes.Add(Key, GetOneAttribute(ref reader, Attributes));
            }
            return Attributes;
        }
        protected AttributeValue GetOneAttribute(ref Utf8JsonReader reader, Dictionary<string, AttributeValue> Attributes)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Number:
                    return new AttributeValueInt { Value = reader.GetInt32() };
                case JsonTokenType.True:
                case JsonTokenType.False:
                    return new AttributeValueBool { Value = reader.GetBoolean() };
                case JsonTokenType.String:
                default:
                    return new AttributeValueString { Value = reader.GetString() };
            }
        }
        protected InsertData GetInsertData(ref Utf8JsonReader reader)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new InsertDataString { Value = reader.GetString() };
            }
            else if (reader.TokenType == JsonTokenType.StartObject)
            {
                reader.Read();
                var Type = reader.GetString();
                reader.Read();
                var Custom = new InsertDataCustom { Attributes = GetAttributes(ref reader), Type = Type };
                reader.Read(); //End Token
                //reader.Read(); //End Token
                return Custom;
            }
            else if (reader.TokenType == JsonTokenType.EndObject)
            {
                return null;
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, Operation value, JsonSerializerOptions options)
        {
            if (value is RetainOperation retain)
            {
                writer.WriteStartObject();
                writer.WriteNumber("retain", retain.Value);
                writer.WriteEndObject();
            }
            else if (value is DeleteOperation delete)
            {
                writer.WriteStartObject();
                writer.WriteNumber("delete", delete.Value);
                writer.WriteEndObject();
            }
            else if (value is InsertOperation insert)
            {
                writer.WriteStartObject();
                if (insert.Attributes != null)
                {
                    writer.WritePropertyName("attributes");
                    writer.WriteStartObject();
                    foreach (var item in insert.Attributes)
                    {
                        if (item.Value is AttributeValueBool b)
                            writer.WriteBoolean(item.Key, b.Value);
                        else if (item.Value is AttributeValueInt i)
                            writer.WriteNumber(item.Key, i.Value);
                        else if (item.Value is AttributeValueString s1)
                            writer.WriteString(item.Key, s1.Value);
                    }
                    writer.WriteEndObject();
                }
                if (insert.Insert is InsertDataString s)
                {
                    writer.WriteString("insert", s.Value);
                }
                else if (insert.Insert is InsertDataCustom c)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("insert");
                    writer.WriteStartObject();
                    writer.WritePropertyName(c.Type);
                    foreach (var item in c.Attributes)
                    {
                        if (item.Value is AttributeValueBool b)
                            writer.WriteBoolean(item.Key, b.Value);
                        else if (item.Value is AttributeValueInt i)
                            writer.WriteNumber(item.Key, i.Value);
                        else if (item.Value is AttributeValueString s2)
                            writer.WriteString(item.Key, s2.Value);
                    }
                    writer.WriteEndObject();
                    writer.WriteEndObject();
                }
                writer.WriteEndObject();

            }

        }

    }
}
