using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements a HalfAdder, taking as input 2 bits - 2 numbers and computing the result in the output, and the carry out.

    class HalfAdder : TwoInputGate
    {
        public Wire CarryOutput { get; private set; }

        XorGate xor;
        AndGate and;


        public HalfAdder()
        {
            xor = new XorGate();
            xor.ConnectInput1(Input1);
            xor.ConnectInput2(Input2);
            Output.ConnectInput(xor.Output);

            and = new AndGate();
            and.ConnectInput1(Input1);
            and.ConnectInput2(Input2);

            CarryOutput = and.Output;
        }


        public override string ToString()
        {
            return "HA " + Input1.Value + "," + Input2.Value + " -> " + Output.Value + " (C" + CarryOutput + ")";
        }

        public override bool TestGate()
        {
            for (int i = 0; i <= 1; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    Input1.Value = i;
                    Input2.Value = j;
                    if ((i == 1 & j == 1 & (Output.Value != 0 | CarryOutput.Value != 1)) |
                        (i == 1 & j == 0 & (Output.Value != 1 | CarryOutput.Value != 0)) |
                        (i == 0 & j == 1 & (Output.Value != 1 | CarryOutput.Value != 0)) |
                        (i == 0 & j == 0 & (Output.Value != 0 | CarryOutput.Value != 0)))
                        return false;
                    else if (NAndGate.Corrupt != true)
                        Console.WriteLine(this);
                }
            }
            return true;
        }
    }
}
