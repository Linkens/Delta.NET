using System.Text.Json.Serialization;

namespace DeltaNET
{
    public class AttributeValueBool : AttributeValue
    {
        [JsonPropertyName("value")]
        public bool Value { get; set; }

        public override AttributeValue Clone()
        {
            return (AttributeValue)this.MemberwiseClone();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
