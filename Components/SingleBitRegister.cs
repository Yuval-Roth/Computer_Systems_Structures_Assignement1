using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements a register that can maintain 1 bit.
    class SingleBitRegister : Gate
    {
        public Wire Input { get; private set; }
        public Wire Output { get; private set; }
        //A bit setting the register operation to read or write
        public Wire Load { get; private set; }

        DFlipFlopGate DFF;
        MuxGate mux;

        public SingleBitRegister()
        {
            
            Input = new Wire();
            Load = new Wire();
            DFF = new DFlipFlopGate();
            mux = new MuxGate();

            mux.ConnectControl(Load);
            mux.ConnectInput1(DFF.Output);
            mux.ConnectInput2(Input);
            DFF.ConnectInput(mux.Output);
            Output = DFF.Output;

        }

        public void ConnectInput(Wire wInput)
        {
            Input.ConnectInput(wInput);
        }

      
        public void ConnectLoad(Wire wLoad)
        {
            Load.ConnectInput(wLoad);
        }

        public override string ToString()
        {
            return "SBR: Input->" + Input + ", Load->" + Load + " Output->" + Output;
        }
        public override bool TestGate()
        {
            Input.Value = 0;
            Clock.ClockDown();
            Clock.ClockUp();

            Load.Value = 1;
            Input.Value = 1;
            if (NAndGate.Corrupt == false) Console.WriteLine("Input->1");
            Clock.ClockDown();
            if (NAndGate.Corrupt == false) Console.WriteLine(this);
            if (Output.Value != 0) return false;
            Clock.ClockUp();
            
            if (NAndGate.Corrupt == false) Console.WriteLine(this);
            if (Output.Value != 1) return false;

            if (NAndGate.Corrupt == false) Console.WriteLine("\n\n\nInput->0");
            Input.Value = 0;
            Clock.ClockDown();
            if (NAndGate.Corrupt == false) Console.WriteLine(this);
            if (Output.Value != 1) return false;
            Clock.ClockUp();
            if (NAndGate.Corrupt == false) Console.WriteLine(this);
            if (Output.Value != 0) return false;

            return true;
        }
    }
}
