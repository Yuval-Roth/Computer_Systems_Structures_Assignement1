using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A two input bitwise gate takes as input two WireSets containing n wires, and computes a bitwise function - z_i=f(x_i,y_i)
    class BitwiseAndGate : BitwiseTwoInputGate
    {
        AndGate[] gates;

        public BitwiseAndGate(int iSize)
            : base(iSize)
        {
            gates = new AndGate[iSize];
            for (int i = 0; i < gates.Length; i++)
            {
                gates[i] = new AndGate();
                gates[i].ConnectInput1(Input1[i]);
                gates[i].ConnectInput2(Input2[i]);
                Output[i].ConnectInput(gates[i].Output);
            }
        }

        //an implementation of the ToString method is called, e.g. when we use Console.WriteLine(and)
        //this is very helpful during debugging
        public override string ToString()
        {
            return "And " + Input1 + ", " + Input2 + " -> " + Output;
        }

        public override bool TestGate()
        {
            for (int count = 0; count < 4; count++)
            {
                Random rand = new Random();

                for (int i = 0; i < gates.Length; i++)
                {
                    Input1[i].Value = rand.Next(0, 2);
                    Input2[i].Value = rand.Next(0, 2);
                }
                for (int i = 0; i < gates.Length; i++)
                {
                    if (Input1[i].Value == 1 & Input2[i].Value == 1)
                    {
                        if (Output[i].Value != 1) return false;
                    }
                    else
                    {
                        if (Output[i].Value != 0) return false;
                    }

                }
                if (NAndGate.Corrupt != true)
                    Console.WriteLine(this);            
            }
            return true;
        }
    }
}
