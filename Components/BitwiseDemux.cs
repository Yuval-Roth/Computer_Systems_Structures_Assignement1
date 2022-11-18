using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A bitwise gate takes as input WireSets containing n wires, and computes a bitwise function - z_i=f(x_i)
    class BitwiseDemux : Gate
    {
        public int Size { get; private set; }
        public WireSet Output1 { get; private set; }
        public WireSet Output2 { get; private set; }
        public WireSet Input { get; private set; }
        public Wire Control { get; private set; }

        BitwiseAndGate and1;
        BitwiseAndGate and2;
        BitwiseNotGate not;

        public BitwiseDemux(int iSize)
        {
            Size = iSize;
            Control = new Wire();
            Input = new WireSet(Size);

            and1 = new BitwiseAndGate(iSize);
            and2 = new BitwiseAndGate(iSize);
            not = new BitwiseNotGate(iSize);

            for (int i = 0; i < iSize; i++)
            {
                and2.Input1[i].ConnectInput(Control);
                not.Input[i].ConnectInput(Control);
            }
            and1.ConnectInput1(not.Output);

            and1.ConnectInput2(Input);
            and2.ConnectInput2(Input);

            Output1 = and1.Output;
            Output2 = and2.Output;
        }

        public void ConnectControl(Wire wControl)
        {
            Control.ConnectInput(wControl);
        }
        public void ConnectInput(WireSet wsInput)
        {
            Input.ConnectInput(wsInput);
        }

        public override string ToString()
        {
            return "Demux " + Input + ",C" + Control.Value + " -> Outputs: " + Output1 + "," + Output2;
        }

        public override bool TestGate()
        {
            Random rand = new Random();

            for (int i = 0; i < Size; i++)
            {
                Input[i].Value = rand.Next(0, 2);
            }

            Control.Value = 0;

            for (int i = 0; i < Size; i++)
            {
                if (Output1[i].Value != Input[i].Value)
                    return false;
            }
            if (NAndGate.Corrupt != true)
                Console.WriteLine(this);

            Control.Value = 1;

            for (int i = 0; i < Size; i++)
            {
                if (Output2[i].Value != Input[i].Value)
                    return false;
            }
            if (NAndGate.Corrupt != true)
                Console.WriteLine(this);

            return true;
        }
    }
}
