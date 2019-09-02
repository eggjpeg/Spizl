using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    class VarState
    {
        public string Name { get; set; }
        public VarType Type { get; set; }
        public object Value { get; set; }

        public VarState(string name, VarType type, object value)
        {
            Name = name;
            Type = type;
            //Suspicious smell
            if ((type == VarType.Lint || type == VarType.Lstr) && value == null)
                Value = new List<object>();
            else
                Value = value;

        }
    }
}
