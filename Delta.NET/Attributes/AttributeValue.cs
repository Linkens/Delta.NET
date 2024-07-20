using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaNET
{
    public abstract class AttributeValue
    {
        public abstract AttributeValue Clone();

        public static bool operator ==(AttributeValue a, AttributeValue b)
        {
            if (a.GetType() != a.GetType()) return false;
            if (a is AttributeValueBool ab)
            {
                var bb = (AttributeValueBool)b;
                return ab.Value == bb.Value;
            }
            if (a is AttributeValueInt ai)
            {
                var bi = (AttributeValueInt)b;
                return ai.Value == bi.Value;
            }
            if (a is AttributeValueString asv)
            {
                var bsv = (AttributeValueString)b;
                return asv.Value == bsv.Value;
            }
            return false;
        }
        public static bool operator !=(AttributeValue a, AttributeValue b)
        {
            if (a.GetType() != a.GetType()) return true;
            if (a is AttributeValueBool ab)
            {
                var bb = (AttributeValueBool)b;
                return ab.Value != bb.Value;
            }
            if (a is AttributeValueInt ai)
            {
                var bi = (AttributeValueInt)b;
                return ai.Value != bi.Value;
            }
            if (a is AttributeValueString asv)
            {
                var bsv = (AttributeValueString)b;
                return asv.Value != bsv.Value;
            }
            return true;
        }
    }
}
