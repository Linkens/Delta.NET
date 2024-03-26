using System.Text.Json.Serialization;

namespace DeltaNET
{
    public class RetainOperation : AttributeOperation
    {
        [JsonPropertyName("retain")]
        public int Value { get; set; }
        public override string ToString()
        {
            return $"retain {Value}";
        }
        public override int GetLength()
        {
            return Value;
        }
        public override Operation Clone()
        {
            var Retain = (RetainOperation)MemberwiseClone();
            if (Attributes != null)
            {
                Retain.Attributes = new Dictionary<string, AttributeValue>();
                foreach (var item in Attributes)
                {
                    Retain.Attributes.Add(item.Key, item.Value.Clone());
                }
            }
            return Retain;
        }
    }
}
