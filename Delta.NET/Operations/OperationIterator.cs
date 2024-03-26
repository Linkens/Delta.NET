using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaNET
{
    public class OpIterator
    {
        public Operation? Current { get; protected set; }
        public int Index { get; protected set; }
        public int Offset { get; protected set; }
        protected List<Operation> Ops;
        public OpIterator(List<Operation> Operations)
        {
            Ops = Operations;
            Current = Operations.FirstOrDefault();
            Offset = 0;
            Index = 0;
        }
        public int PeekLength()
        {
            if (Current == null)
                return int.MaxValue;
            else return Current.GetLength() - Offset;
        }
        public List<Operation> Rest()
        {
            var OldIndex = Index;
            Index = Ops.Count + 1;
            return Ops.Skip(OldIndex).ToList();
        }
        public Operation Next(int Length)
        {
            if (Current == null)
                return new RetainOperation { Value = int.MaxValue };
            var Old = Current;
            var OldOffset = Offset;
            var OpLength = Current.GetLength();
            if (Length >= OpLength - Offset)
            {
                Length = OpLength - Offset;
                if (HasNext())
                    Current = Ops[Index + 1];
                else Current = null;
                Index++;
                Offset = 0;
            }
            else
            {
                Offset += Length;
            }
            if (Old is DeleteOperation)
            {
                var NewDelete = (DeleteOperation)Old.Clone();
                NewDelete.Value = Length;
                return NewDelete;
            }
            else if (Old is RetainOperation)
            {
                var NewRetain = (RetainOperation)Old.Clone();
                NewRetain.Value = Length;
                return NewRetain;
            }
            else
            {
                var NewOP = (InsertOperation)Old.Clone();
                if (NewOP.Insert is InsertDataString datastring)
                {
                    var NewData = (InsertDataString)datastring.Clone();
                    NewData.Value = NewData.Value.Substring(OldOffset, Length);
                    NewOP.Insert = NewData;
                    return NewOP;
                }
                else
                {
                    return NewOP;
                }
            }
        }
        public bool HasNext()
        {
            return Index < Ops.Count - 1;
        }
        public Operation Next()
        {
            return Next(int.MaxValue);

        }
    }
}
