using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A bitwise gate takes as input WireSets containing n wires, and computes a bitwise function - z_i=f(x_i)
    class BitwiseMux : BitwiseTwoInputGate
    {
        public Wire ControlInput { get; private set; }

        BitwiseNotGate not;
        BitwiseAndGate and1;
        BitwiseAndGate and2;
        BitwiseOrGate or;

        public BitwiseMux(int iSize)
            : base(iSize)
        {
            ControlInput = new Wire();


            not = new BitwiseNotGate(iSize);
            and1 = new BitwiseAndGate(iSize);
            and2 = new BitwiseAndGate(iSize);
            or = new BitwiseOrGate(iSize);


            for (int i = 0; i < iSize; i++)
            {
                and2.Input1[i].ConnectInput(ControlInput);
                not.Input[i].ConnectInput(ControlInput);
            }
            and1.ConnectInput1(not.Output);

            and1.ConnectInput2(Input1);
            and2.ConnectInput2(Input2);

            or.ConnectInput1(and1.Output);
            or.ConnectInput2(and2.Output);

            Output = or.Output;
        }

        public void ConnectControl(Wire wControl)
        {
            ControlInput.ConnectInput(wControl);
        }

        public override string ToString()
        {
            return "Mux " + Input1 + "," + Input2 + ",C" + ControlInput.Value + " -> " + Output;
        }

        public override bool TestGate()
        {
            for (int j = 0; j < 4; j++)
            {
                ControlInput.Value = 0;

                Random rand = new Random();

                for (int i = 0; i < Size; i++)
                {
                    Input1[i].Value = rand.Next(0, 2);
                    Input2[i].Value = rand.Next(0, 2);
                }

                for (int i = 0; i < Size; i++)
                {
                    if (Output[i].Value != Input1[i].Value)
                        return false;
                }
                if (NAndGate.Corrupt != true)
                    Console.WriteLine(this);

                ControlInput.Value = 1;

                for (int i = 0; i < Size; i++)
                {
                    if (Output[i].Value != Input2[i].Value)
                        return false;
                }
                if (NAndGate.Corrupt != true)
                    Console.WriteLine(this);
            }

            return true;
        }
    }
}
