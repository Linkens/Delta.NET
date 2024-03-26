﻿using System.Text.Json.Serialization;

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
            var DoOnce = false;
            if (ItOther.Current != null && ItOther.Current is RetainOperation retain)
            {
                //Faster this way to deal with the first retain operation
                var FirstLeft = retain.Value;
                while (ItMine.Current != null && ItMine.Current is InsertOperation insert && FirstLeft >= insert.GetLength())
                {
                    FirstLeft -= insert.GetLength();
                    NewDelta.Operations.Add(ItMine.Current.Clone());
                    ItMine.Next();
                }
                if (retain.Value - FirstLeft > 0)
                    ItOther.Next(retain.Value - FirstLeft);
            }
            else
            {
                //Event if there is no 'next' operation, we need to check the first one
                DoOnce = true;
            }
            while (ItMine.HasNext() || ItOther.HasNext() || DoOnce)
            {
                DoOnce = false;
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
                if (OtherOp is RetainOperation rOther)
                {
                    if (MineOp is RetainOperation rMine)
                    {
                        rMine.Value = SharedLength;
                        NewDelta.Operations.Add(rMine);
                    }
                    else
                        NewDelta.Operations.Add(MineOp);
                }
                if (!ItOther.HasNext())
                {
                    //We can escape here by dealing the end of Mine operations
                    if (ItMine.Offset != 0)
                    {
                        NewDelta.Operations.Add(ItMine.Next());
                    }
                    NewDelta.Operations.AddRange(ItMine.Rest());
                    break;
                }
            }

            return NewDelta;
        }

    }
}
