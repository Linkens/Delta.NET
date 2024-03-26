using System.Text;
using System.Text.Json.Serialization;

namespace Delta.NET
{
    public class InsertOperation : Operation
    {
        [JsonPropertyName("insert")]
        public InsertData? Insert { get; set; }
        [JsonPropertyName("attributes"), JsonExtensionData]
        public Dictionary<string, AttributeValue> Attributes { get; set; }
        public override string ToString()
        {
            var b = new StringBuilder();
            if (Insert != null)
                b.Append(Insert.ToString());
            if (Attributes.Any())
            {
                if (Insert != null)
                    b.Append(" | ");
                foreach (var item in Attributes)
                {
                    b.Append($"{item.Key} = {item.Value} ");
                }
            }
            return b.ToString();
        }
        public override int GetLength()
        {
            if (Insert is InsertDataString s)
                return s.Value.Length;
            else
                return 1;
        }

        public override Operation Clone()
        {
            var Elem = new InsertOperation();
            if (Insert != null) Elem.Insert = Insert.Clone();
            if (Attributes != null)
            {
                Elem.Attributes = new Dictionary<string, AttributeValue>();
                foreach (var item in Attributes)
                {
                    Elem.Attributes.Add(item.Key, item.Value.Clone());
                }
            }
            return Elem;
        }
    }
}
