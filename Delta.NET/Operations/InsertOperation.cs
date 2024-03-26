using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeltaNET
{
    public class InsertOperation : AttributeOperation
    {
        [JsonPropertyName("insert")]
        public InsertData? Insert { get; set; }

        public override string ToString()
        {
            var b = new StringBuilder();
            if (Insert != null)
                b.Append(Insert.ToString());
            if (Attributes != null)
                b.Append(" | ");
            b.Append(base.ToString());

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
