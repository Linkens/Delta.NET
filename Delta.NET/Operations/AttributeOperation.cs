using System.Text;
using System.Text.Json.Serialization;

namespace DeltaNET
{
    public abstract class AttributeOperation : Operation
    {
        [JsonPropertyName("attributes"), JsonExtensionData]
        public Dictionary<string, AttributeValue> Attributes { get; set; }
        public override string ToString()
        {
            var b = new StringBuilder();
            if (Attributes.Any())
            {
                foreach (var item in Attributes)
                {
                    b.Append($"{item.Key} = {item.Value} ");
                }
            }
            return b.ToString();
        }
        public bool SameAttributes(AttributeOperation Other)
        {
            if (Attributes == null && Other.Attributes == null) return true;
            if (Attributes == null || Other.Attributes == null) return false;
            if (Attributes.Count != Other.Attributes.Count) return false;
            var IsSame = true;
            for (int i = 0; i < Attributes.Count; i++)
            {
                var MineAtt = Attributes.ElementAt(i);
                var OtherAtt = Other.Attributes.ElementAt(i);
                IsSame = IsSame && (MineAtt.Key == OtherAtt.Key && MineAtt.Value == OtherAtt.Value);
            }
            return IsSame;
        }
        public void AddString(string Key, string Value)
        {
            if (Attributes == null) Attributes = new();
            Attributes.Add(Key, new AttributeValueString { Value = Value });
        }
        public void AddInt(string Key, int Value)
        {
            if (Attributes == null) Attributes = new();
            Attributes.Add(Key, new AttributeValueInt { Value = Value });
        }
        public void AddBool(string Key, bool Value)
        {
            if (Attributes == null) Attributes = new();
            Attributes.Add(Key, new AttributeValueBool { Value = Value });
        }
    }
}
