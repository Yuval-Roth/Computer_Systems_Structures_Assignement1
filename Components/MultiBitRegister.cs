using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class represents an n bit register that can maintain an n bit number
    class MultiBitRegister : Gate
    {
        public WireSet Input { get; private set; }
        public WireSet Output { get; private set; }
        //A bit setting the register operation to read or write
        public Wire Load { get; private set; }

        //Word size - number of bits in the register
        public int Size { get; private set; }

        SingleBitRegister[] SBRs;


        public MultiBitRegister(int iSize)
        {
            Size = iSize;
            Input = new WireSet(Size);
            Output = new WireSet(Size);
            Load = new Wire();
            SBRs = new SingleBitRegister[Size];
            for (int i = 0; i < SBRs.Length; i++)
            {
                SBRs[i] = new SingleBitRegister();
                SBRs[i].ConnectLoad(Load);
                SBRs[i].ConnectInput(Input[i]);
                Output[i].ConnectInput(SBRs[i].Output);
            }
        }

        public void ConnectInput(WireSet wsInput)
        {
            Input.ConnectInput(wsInput);
        }

        public override string ToString()
        {
            return "MBR: Input->" + Input + ", Load->" + Load + " Output->" + Output;
        }
        public override bool TestGate()
        {
            Load.Value = 1;
            Input.SetValue(1);
            if (NAndGate.Corrupt == false) Console.WriteLine("Input->"+Input);
            Clock.ClockDown();
            if (NAndGate.Corrupt == false) Console.WriteLine(this);
            if (Output.Get2sComplement() != 0) return false;
            Clock.ClockUp();

            if (NAndGate.Corrupt == false) Console.WriteLine(this);
            if (Output.Get2sComplement() != 1) return false;

            if (NAndGate.Corrupt == false) Console.WriteLine("\n\n\nInput->"+Input);
            Input.SetValue(0);
            Clock.ClockDown();
            if (NAndGate.Corrupt == false) Console.WriteLine(this);
            if (Output.Get2sComplement() != 1) return false;
            Clock.ClockUp();
            if (NAndGate.Corrupt == false) Console.WriteLine(this);
            if (Output.Get2sComplement() != 0) return false;

            return true;
        }
    }
}
