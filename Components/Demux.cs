using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A demux has 2 outputs. There is a single input and a control bit, selecting whether the input should be directed to the first or second output.
    class Demux : Gate
    {
        public Wire Output1 { get; private set; }
        public Wire Output2 { get; private set; }
        public Wire Input { get; private set; }
        public Wire Control { get; private set; }

        AndGate and1;
        AndGate and2;
        NotGate not;

        public Demux()
        {
            Input = new Wire();
            and1 = new AndGate();
            and2 = new AndGate();
            not = new NotGate();
            Control = new Wire();

            not.ConnectInput(Control);
            and1.ConnectInput1(not.Output);
            and2.ConnectInput1(Control);

            and1.ConnectInput2(Input);
            and2.ConnectInput2(Input);

            Output1 = and1.Output;
            Output2 = and2.Output;

        }

        public void ConnectControl(Wire wControl)
        {
            Control.ConnectInput(wControl);
        }
        public void ConnectInput(Wire wInput)
        {
            Input.ConnectInput(wInput);
        }

        public override string ToString()
        {
            return "Demux " + Input.Value + ",C" + Control.Value + " -> Outputs: "+Output1+","+Output2;
        }

        public override bool TestGate()
        {
            Input.Value = 1;

            Control.Value = 0;
            if (Output1.Value != Input.Value)
                return false;
            else if (NAndGate.Corrupt != true)
                Console.WriteLine(this);

            Control.Value = 1;
            if (Output2.Value != Input.Value)
                return false;
            else if (NAndGate.Corrupt != true)
                Console.WriteLine(this);

            if (Output1.Value == Output2.Value) return false;

            return true;
        }
    }
}
