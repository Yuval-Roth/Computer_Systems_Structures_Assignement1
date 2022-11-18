using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A two input bitwise gate takes as input two WireSets containing n wires, and computes a bitwise function - z_i=f(x_i,y_i)
    class BitwiseOrGate : BitwiseTwoInputGate
    {
        BitwiseNotGate Not1Gate;
        BitwiseNotGate Not2Gate;
        BitwiseNotGate Not3Gate;
        BitwiseAndGate AndGate;


        public BitwiseOrGate(int iSize)
            : base(iSize)
        {
            Not1Gate = new BitwiseNotGate(iSize);
            Not2Gate = new BitwiseNotGate(iSize);
            Not3Gate = new BitwiseNotGate(iSize);
            AndGate = new BitwiseAndGate(iSize);

            Not1Gate.ConnectInput(Input1);
            Not2Gate.ConnectInput(Input2);

            AndGate.ConnectInput1(Not1Gate.Output);
            AndGate.ConnectInput2(Not2Gate.Output);

            Not3Gate.ConnectInput(AndGate.Output);

            Output = Not3Gate.Output;
        }

        //an implementation of the ToString method is called, e.g. when we use Console.WriteLine(or)
        //this is very helpful during debugging
        public override string ToString()
        {
            return "Or " + Input1 + ", " + Input2 + " -> " + Output;
        }

        public override bool TestGate()
        {
            for (int count = 0; count < 4; count++)
            {
                Random rand = new Random();

                for (int i = 0; i < Size; i++)
                {
                    Input1[i].Value = rand.Next(0, 2);
                    Input2[i].Value = rand.Next(0, 2);
                }
                for (int i = 0; i < Size; i++)
                {
                    if (Input1[i].Value == 1 | Input2[i].Value == 1)
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
