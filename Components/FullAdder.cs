using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements a FullAdder, taking as input 3 bits - 2 numbers and a carry, and computing the result in the output, and the carry out.
    class FullAdder : TwoInputGate
    {
        public Wire CarryInput { get; private set; }
        public Wire CarryOutput { get; private set; }

        HalfAdder ha1;
        HalfAdder ha2;
        OrGate or;


        public FullAdder()
        {
            CarryInput = new Wire();
            ha1 = new HalfAdder();
            ha1.ConnectInput1(Input1);
            ha1.ConnectInput2(Input2);

            ha2 = new HalfAdder();
            ha2.ConnectInput1(ha1.Output);
            ha2.ConnectInput2(CarryInput);

            or = new OrGate();
            or.ConnectInput1(ha1.CarryOutput);
            or.ConnectInput2(ha2.CarryOutput);

            Output = ha2.Output;
            CarryOutput = or.Output;

        }


        public override string ToString()
        {
            return Input1.Value + "+" + Input2.Value + " (C" + CarryInput.Value + ") = " + Output.Value + " (C" + CarryOutput.Value + ")";
        }

        public override bool TestGate()
        {
            for (int i = 0; i <= 1; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    for (int k = 0; k <= 1; k++)
                    {
                        Input1.Value = i;
                        Input2.Value = j;
                        CarryInput.Value = k;

                        if ((i == 1 & j == 1 & k == 0 & (Output.Value != 0 | CarryOutput.Value != 1)) |
                            (i == 1 & j == 0 & k == 0 & (Output.Value != 1 | CarryOutput.Value != 0)) |
                            (i == 0 & j == 1 & k == 0 & (Output.Value != 1 | CarryOutput.Value != 0)) |
                            (i == 0 & j == 0 & k == 0 & (Output.Value != 0 | CarryOutput.Value != 0)) |
                            (i == 1 & j == 1 & k == 1 & (Output.Value != 1 | CarryOutput.Value != 1)) |
                            (i == 1 & j == 0 & k == 1 & (Output.Value != 0 | CarryOutput.Value != 1)) |
                            (i == 0 & j == 1 & k == 1 & (Output.Value != 0 | CarryOutput.Value != 1)) |
                            (i == 0 & j == 0 & k == 1 & (Output.Value != 1 | CarryOutput.Value != 0)))
                            return false;
                        else if (NAndGate.Corrupt != true)
                            Console.WriteLine(this);
                    }
                }
            }
            return true;
        }
    }
}
