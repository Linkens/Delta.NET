using System.Text.Json.Serialization;

namespace DeltaNET
{
    public class RetainOperation : Operation
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
            return (Operation)MemberwiseClone();
        }
    }
}
