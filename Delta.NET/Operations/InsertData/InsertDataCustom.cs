using System.Text;
using System.Text.Json.Serialization;

namespace Delta.NET
{
    public class InsertDataCustom : InsertData
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("attributes"), JsonExtensionData]
        public Dictionary<string, AttributeValue> Attributes { get; set; }


        public override string ToString()
        {
            var b = new StringBuilder();
            b.Append($"Insert {Type} : ");
            if (Attributes.Any())
            {
                foreach (var item in Attributes)
                {
                    b.Append($"{item.Key} = {item.Value} ");
                }
            }
            return b.ToString();
        }

        public override InsertData Clone()
        {
            var Elem = new InsertDataCustom();
            Elem.Type = this.Type;
            Elem.Attributes = new Dictionary<string, AttributeValue>();
            if (this.Attributes != null)
            {
                foreach (var item in this.Attributes)
                {
                    Elem.Attributes.Add(item.Key, item.Value.Clone());
                }
            }
            return Elem;
        }
    }
}
