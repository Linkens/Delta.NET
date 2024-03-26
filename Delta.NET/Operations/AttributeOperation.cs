using System.Text;
using System.Text.Json.Serialization;

namespace DeltaNET
{
    public abstract class AttributeOperation : Operation
    {
        [JsonPropertyName("attributes"), JsonExtensionData]
        public Dictionary<string, AttributeValue> Attributes { get; set; }
        public override string ToString()
        {
            var b = new StringBuilder();
            if (Attributes.Any())
            {
                foreach (var item in Attributes)
                {
                    b.Append($"{item.Key} = {item.Value} ");
                }
            }
            return b.ToString();
        }
        public void AddString(string Key, string Value)
        {
            if (Attributes == null) Attributes = new();
            Attributes.Add(Key, new AttributeValueString { Value = Value });
        }
        public void AddInt(string Key, int Value)
        {
            if (Attributes == null) Attributes = new();
            Attributes.Add(Key, new AttributeValueInt { Value = Value });
        }
        public void AddBool(string Key, bool Value)
        {
            if (Attributes == null) Attributes = new();
            Attributes.Add(Key, new AttributeValueBool { Value = Value });
        }
    }
}
