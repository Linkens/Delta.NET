using System.Text.Json.Serialization;

namespace DeltaNET
{
    public class Delta
    {
        [JsonPropertyName("ops")]

        public List<Operation> Operations { get; set; }

        public Delta Compose(Delta Other)
        {
            var NewDelta = new Delta() { Operations = new() };
            var ItMine = new OpIterator(Operations);
            var ItOther = new OpIterator(Other.Operations);
            if (ItOther.Current != null && ItOther.Current is RetainOperation retain)
            {
                var FirstLeft = retain.Value;
                while (ItMine.Current != null && ItMine.Current is InsertOperation insert && FirstLeft >= insert.GetLength())
                {
                    FirstLeft -= insert.GetLength();
                    NewDelta.Operations.Add(ItMine.Current.Clone());
                    ItMine.Next();
                }
                if (retain.Value - FirstLeft > 0)
                {
                    ItOther.Next(retain.Value - FirstLeft);
                }
                while (ItMine.HasNext() || ItOther.HasNext())
                {
                    if (ItOther.Current is InsertOperation)
                    {
                        NewDelta.Operations.Add(ItOther.Current.Clone());
                        ItOther.Next();
                    }
                    else if (ItMine.Current is DeleteOperation)
                    {
                        NewDelta.Operations.Add(ItMine.Current.Clone());
                        ItMine.Next();
                    }
                    var SharedLength = Math.Min(ItMine.PeekLength(), ItOther.PeekLength());
                    var MineOp = ItMine.Next(SharedLength);
                    var OtherOp = ItOther.Next(SharedLength);
                    Operation NewOp = null;
                    if (OtherOp is RetainOperation rOther)
                    {
                        if (MineOp is RetainOperation rMine)
                        {
                            rMine.Value = SharedLength;
                            NewDelta.Operations.Add(rMine);
                        }
                        else
                        {
                            NewDelta.Operations.Add(MineOp);
                        }
                    }
                    if (!ItOther.HasNext() && ItMine.Offset == 0 && !(OtherOp is DeleteOperation))
                    {
                        NewDelta.Operations.AddRange(ItMine.Rest());
                        break;
                    }
                }
            }
            return NewDelta;
        }

    }
}
