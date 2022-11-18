using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class Counter : Gate
    {
        private int m_iValue;
        public WireSet Input { get; private set; }
        public WireSet Output { get; private set; }
        public Wire Set { get; private set; }
        public int Size { get; private set; }

        //The counter contains a register, and supports two possible operations:
        //1. Set = 0: Incrementing the current register value by 1 (++)
        //2. Set = 1: Setting the register to a new value

        public Counter(int iSize)
        {
            Size = iSize;
            Input = new WireSet(Size);
            Output = new WireSet(Size);
            Set = new Wire();

            //your code here
        }

        public void ConnectInput(WireSet ws)
        {
            Input.ConnectInput(ws);
        }
        
        public void ConnectReset(Wire w)
        {
            Set.ConnectInput(w);
        }

        public override string ToString()
        {
            return Output.ToString();
        }

        

        public override bool TestGate()
        {
            throw new NotImplementedException();
        }
    }
}
