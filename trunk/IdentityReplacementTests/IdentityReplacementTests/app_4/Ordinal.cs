using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace app_4
{
    public class OrdinalAttribute : Attribute
    {
        private int ordinal;

        public OrdinalAttribute(int Ordinal)
        {
            ordinal = Ordinal;
        }
    }
}
