using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vx.Shared;

namespace vx.VM
{
    public class Stack
    {
        public AbstractData[] stack;
        private int currentStackPos;
        private VirtualMachine vm;

        public Stack(int size, VirtualMachine vm)
        {
            this.vm = vm;
            stack = new AbstractData[size];
            for (int i = 0; i < stack.Length; ++i)
            {
                stack[i] = null;
            }
        }

        public AbstractData Pop()
        {
            if (currentStackPos <= 0)
                vm.ThrowException("Attempted to pop outside of stack bounds!");
            return stack[--currentStackPos];
        }

        public void Push(AbstractData data)
        {
            if (currentStackPos > stack.Length)
                vm.ThrowException("Attempted to push past stack bounds!");
            stack[currentStackPos++] = data;
        }

        public void SetPos(int pos)
        {
            currentStackPos = pos;
        }
    }
}
