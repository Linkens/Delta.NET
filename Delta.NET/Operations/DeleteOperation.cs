using System.Text.Json.Serialization;

namespace DeltaNET
{
    public class DeleteOperation : Operation
    {
        [JsonPropertyName("delete")]
        public int Value { get; set; }
        public override string ToString()
        {
            return $"Delete {Value}";
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
