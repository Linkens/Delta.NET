using System.Text.Json.Serialization;

namespace DeltaNET
{
    public class AttributeValueString : AttributeValue
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }

        public override AttributeValue Clone()
        {
            return (AttributeValue)this.MemberwiseClone();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
