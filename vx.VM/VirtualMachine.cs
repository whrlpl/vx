using System;
using System.Reflection;
using vx.Shared;

namespace vx.VM
{
    public class VirtualMachine
    {
        Stack stack;
        AbstractData[] source;
        int currentSrcPos;
        bool running;

        public VirtualMachine()
        {
            stack = new Stack(256, this);
        }

        public string GetCSMethods()
        {
            string opt = "";
            foreach (var method in this.GetType().GetMethods())
            {
                opt += method.Name + "\n";
            }
            return opt;
        }


        public void Run()
        {
            currentSrcPos = 0;
            source = new AbstractData[]
            {
                //Instruction.psh, "Login@Auth@vx.VM",
                Instruction.psh, "Login@Auth@vx.VM",
                Instruction.csc,
                Instruction.opt,

                Instruction.psh, "Enter a number:", // ["Enter a number:"]
                Instruction.opt, // []
                Instruction.inp, // [-1]

                Instruction.psh, 18,  // [-1, 14]
                Instruction.brz, // [-1]
                
                Instruction.psh, 22, // [-1, 18]
                Instruction.brp, // [-1]

                Instruction.psh, "Your number was negative.",
                Instruction.opt,
                Instruction.hlt,

                Instruction.psh, "Your number was zero.",
                Instruction.opt,
                Instruction.hlt,

                Instruction.psh, "Your number was positive.",
                Instruction.opt,
                Instruction.hlt
            };
            running = true;
            while (running)
            {
                ExecuteInstruction(verbose: false);
                //Console.ReadLine();
            }
        }

        public void ExecuteInstruction(bool verbose = false)
        {
            var instruction = source[currentSrcPos++];
            if (instruction.valueType != typeof(Instruction))
                ThrowException("Stack is corrupted! Data was not an instruction.");

            if (verbose)
            {
                Console.WriteLine("srcpos: " + currentSrcPos);
                Console.WriteLine("Executing instruction " + instruction.GetValue().ToString());
            }

            switch ((Instruction)instruction.GetValue())
            {
                case Instruction.psh:
                    {
                        stack.Push(source[currentSrcPos++]);
                        break;
                    }
                case Instruction.pop:
                    {
                        stack.Pop();
                        break;
                    }
                case Instruction.add:
                    { 
                        var a = stack.Pop();
                        var b = stack.Pop();
                        stack.Push(a.GetValue() + b.GetValue());
                        break;
                    }   
                case Instruction.sub:
                    {
                        var a = stack.Pop();
                        var b = stack.Pop();
                        stack.Push(a.GetValue() - b.GetValue());
                        break;
                    }
                case Instruction.brz:
                    {
                        var loc = stack.Pop();
                        var val = stack.Pop();
                        if (val.GetValue() == 0)
                            currentSrcPos = loc.GetValue();
                        stack.Push(val);
                        break;
                    }
                case Instruction.brp:
                    {
                        var loc = stack.Pop();
                        var val = stack.Pop();
                        if (val.GetValue() >= 0)
                            currentSrcPos = loc.GetValue();
                        stack.Push(val);
                        break;
                    }
                case Instruction.jmp:
                    {
                        currentSrcPos = stack.Pop().GetValue();
                        break;
                    }
                case Instruction.csc:
                    {
                        // function signature is ALWAYS:
                        // Function@Class@Namespace

                        var funcSignature = stack.Pop().GetValue().ToString().Split('@');
                        var funcNamespace = funcSignature[2];
                        var funcClass = funcSignature[1];
                        var funcMethod = funcSignature[0];

                        Type type = Assembly.GetCallingAssembly().GetType(funcNamespace + "." + funcClass);
                        if (type == null) ThrowException("Could not find C# class or namespace " + funcNamespace + "." + funcClass);

                        MethodInfo method = type.GetMethod(funcMethod);
                        if (method == null) ThrowException("C# class " + funcNamespace + "." + funcClass + " does not contain the method " + funcMethod);

                        var opt = method.Invoke(null, new object[] { });
                        if (opt != null) stack.Push(new AbstractData(opt));

                        break;
                    }
                case Instruction.inp:
                    {
                        stack.Push(int.Parse(Console.ReadLine()));
                        break;
                    }
                case Instruction.opt:
                    {
                        Console.WriteLine(stack.Pop().GetValue().ToString());
                        break;
                    }
                case Instruction.nop:
                    {
                        break;
                    }
                case Instruction.hlt:
                    {
                        running = false;
                        break;
                    }
                default:
                    {
                        ThrowException("Stack is corrupted! Unknown instruction " + instruction.ToString() + ".");
                        break;
                    }
            }
            if (verbose)
            {
                Console.WriteLine("srcpos: " + currentSrcPos);
            }

        }

        public void ThrowException(string msg)
        {
            throw new Exception(msg);
        }
    }
}
