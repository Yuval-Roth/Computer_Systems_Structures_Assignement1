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

        MultiBitRegister MBR;
        BitwiseMux mux;
        MultiBitAdder adder;
        WireSet hw_one;


        public Counter(int iSize)
        {
            Size = iSize;
            Input = new WireSet(Size);
            Output = new WireSet(Size);
            Set = new Wire();

            hw_one = new WireSet(Size);
            hw_one.SetValue(1);

            MBR = new MultiBitRegister(iSize);
            mux = new BitwiseMux(iSize);
            adder = new MultiBitAdder(iSize);

            adder.ConnectInput1(hw_one);
            adder.ConnectInput2(MBR.Output);

            mux.ConnectInput1(adder.Output);
            mux.ConnectInput2(Input);
            mux.ConnectControl(Set);

            MBR.ConnectInput(mux.Output);
            MBR.Load.Value = 1;

            Output.ConnectInput(MBR.Output);

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
            return "Input->"+Input+", Set->"+Set+", Output->"+Output;
        }

        

        public override bool TestGate()
        {
            Random rand = new Random();

            for (int j = 0; j < 10; j++)
            {
                Input.SetValue(rand.Next(0, (int)Math.Pow(2,Size-1)));
                Set.Value = rand.Next(0, 2);
                int oldValue = MBR.Output.Get2sComplement();
                Clock.ClockDown();
                Clock.ClockUp();
                if (Set.Value == 1 && Input.Get2sComplement() != Output.Get2sComplement()) return false;
                if (Set.Value == 0 && oldValue != Output.Get2sComplement()-1) return false;
                if (NAndGate.Corrupt == false) Console.WriteLine(this);
            }
            return true;
        }
    }
}
