using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements an adder, receving as input two n bit numbers, and outputing the sum of the two numbers
    class MultiBitAdder : Gate
    {
        //Word size - number of bits in each input
        public int Size { get; private set; }

        public WireSet Input1 { get; private set; }
        public WireSet Input2 { get; private set; }
        public WireSet Output { get; private set; }
        //An overflow bit for the summation computation
        public Wire Overflow { get; private set; }

        FullAdder[] adders;

        public MultiBitAdder(int iSize)
        {
            Size = iSize;
            Input1 = new WireSet(Size);
            Input2 = new WireSet(Size);
            Output = new WireSet(Size);

            adders = new FullAdder[Size];
            for (int i = 0; i < Size; i++)
            {
                adders[i] = new FullAdder();
                adders[i].ConnectInput1(Input1[i]);
                adders[i].ConnectInput2(Input2[i]);
                if (i > 0)
                {
                    adders[i].CarryInput.ConnectInput(adders[i - 1].CarryOutput);
                }
                Output[i].ConnectInput(adders[i].Output);
            }
            Overflow = adders[Size - 1].CarryOutput;
        }

        public override string ToString()
        {
            return Input1 + "(" + Input1.Get2sComplement() + ")" + " + " + Input2 + "(" + Input2.Get2sComplement() + ")" + " = " + Output + "(" + Output.Get2sComplement() + ")";
        }

        public void ConnectInput1(WireSet wInput)
        {
            Input1.ConnectInput(wInput);
        }
        public void ConnectInput2(WireSet wInput)
        {
            Input2.ConnectInput(wInput);
        }


        public override bool TestGate()
        {
            Random rand = new Random();
            for (int i = 0; i < Size; i++)
            {
                Input1[i].Value = rand.Next(0, 2);
                Input2[i].Value = rand.Next(0, 2);
            }
            if (NAndGate.Corrupt != true)
            {
                Console.WriteLine("Input1 -> " + Input1);
                Console.WriteLine("Input2 -> " + Input2);
            }

            WireSet wires = new WireSet(Size+1);
            wires.SetValue(Input1.GetValue() + Input2.GetValue());
            for (int i = 0; i < Size; i++)
            {
                if (Output[i].Value != wires[i].Value)
                    return false;
            }
            if (Overflow.Value != wires[Size].Value) return false;
            if (NAndGate.Corrupt != true)
            {
                Console.WriteLine("Output-> " + Output);
                Console.WriteLine("Overflow bit-> " + Overflow);
            }
                
            return true;
        }
    }
}
