using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vx.Shared
{
    public enum Instruction : byte
    {
        psh,
        pop,
        add,
        sub,

        brz,
        brp,

        jmp,
        hlt,

        csc,

        inp,
        opt,


        nop = 0xFF
    }
}
