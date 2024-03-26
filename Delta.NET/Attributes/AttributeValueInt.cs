using System.Text.Json.Serialization;

namespace DeltaNET
{
    public class AttributeValueInt : AttributeValue
    {
        [JsonPropertyName("value")]
        public int Value { get; set; }

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
