using Delta.NET.JsonTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Delta.NET
{
    [JsonConverter(typeof(OperationConverter))]
    public abstract class Operation
    {
        public abstract int GetLength();

        public abstract Operation Clone();
    }
}
