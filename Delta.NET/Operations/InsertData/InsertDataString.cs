using System.Text.Json.Serialization;

namespace Delta.NET
{
    public class InsertDataString : InsertData
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }


        public override string ToString()
        {
            if (string.IsNullOrEmpty(Value))
                return string.Empty;
            return $"Insert {Value} ";
        }

        public override InsertData Clone()
        {
            return (InsertDataString)this.MemberwiseClone();
        }
    }
}
