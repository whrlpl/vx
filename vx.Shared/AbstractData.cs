using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vx.Shared
{
    public class AbstractData
    {
        public object value;
        public System.Type valueType;

        public AbstractData(object value)
        {
            this.value = value;
            valueType = value.GetType();
        }

        public dynamic GetValue() => Convert.ChangeType(value, valueType);

        public static implicit operator AbstractData(bool value) => new AbstractData(value);
        public static implicit operator AbstractData(float value) => new AbstractData(value);
        public static implicit operator AbstractData(int value) => new AbstractData(value);
        public static implicit operator AbstractData(string value) => new AbstractData(value);
        public static implicit operator AbstractData(long value) => new AbstractData(value);
        public static implicit operator AbstractData(char value) => new AbstractData(value);
        public static implicit operator AbstractData(Instruction value) => new AbstractData(value);
    }
}
