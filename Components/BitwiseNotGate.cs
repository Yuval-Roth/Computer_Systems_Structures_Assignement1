using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This bitwise gate takes as input one WireSet containing n wires, and computes a bitwise function - z_i=f(x_i)
    class BitwiseNotGate : Gate
    {
        public WireSet Input { get; private set; }
        public WireSet Output { get; private set; }
        public int Size { get; private set; }

        NotGate[] gates;

        public BitwiseNotGate(int iSize)
        {
            Size = iSize;
            Input = new WireSet(Size);
            Output = new WireSet(Size);

            gates = new NotGate[Size];
            for (int i = 0; i < Size; i++)
            {
                gates[i] = new NotGate();
                gates[i].ConnectInput(Input[i]);
                Output[i].ConnectInput(gates[i].Output);
            }

        }

        public void ConnectInput(WireSet ws)
        {
            Input.ConnectInput(ws);
        }

        //an implementation of the ToString method is called, e.g. when we use Console.WriteLine(not)
        //this is very helpful during debugging
        public override string ToString()
        {
            return "Not " + Input + " -> " + Output;
        }

        public override bool TestGate()
        {
            for (int count = 0; count < 4; count++)
            {
                Random rand = new Random();

                for (int i = 0; i < gates.Length; i++)
                {
                    Input[i].Value = rand.Next(0, 2);
                }
                for (int i = 0; i < gates.Length; i++)
                {
                    if (Input[i].Value == Output[i].Value)
                    {
                        return false;
                    }
                }
                if (NAndGate.Corrupt != true)
                    Console.WriteLine(this);
            }
            return true;
        }
    }
}
