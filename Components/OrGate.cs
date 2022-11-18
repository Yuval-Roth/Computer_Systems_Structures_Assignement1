using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This gate implements the or operation. To implement it, follow the example in the And gate.
    class OrGate : TwoInputGate
    {
        private NotGate NotX;
        private NotGate NotY;
        private AndGate And;
        private NotGate LastNot;

        public OrGate()
        {
            NotX = new NotGate();
            NotY = new NotGate();
            And = new AndGate();
            LastNot = new NotGate();

            NotX.ConnectInput(Input1);
            NotY.ConnectInput(Input2);
            And.ConnectInput1(NotX.Output);
            And.ConnectInput2(NotY.Output);
            LastNot.ConnectInput(And.Output);
            Output = LastNot.Output;
        }


        public override string ToString()
        {
            return "Or " + Input1.Value + "," + Input2.Value + " -> " + Output.Value;
        }

        public override bool TestGate()
        {
            Input1.Value = 0;
            Input2.Value = 0;
            if (Output.Value != 0)
                return false;
            else if (NAndGate.Corrupt != true)
                Console.WriteLine(this);
            Input1.Value = 0;
            Input2.Value = 1;
            if (Output.Value != 1)
                return false;
            else if (NAndGate.Corrupt != true)
                Console.WriteLine(this);
            Input1.Value = 1;
            Input2.Value = 0;
            if (Output.Value != 1)
                return false;
            else if (NAndGate.Corrupt != true)
                Console.WriteLine(this);
            Input1.Value = 1;
            Input2.Value = 1;
            if (Output.Value != 1)
                return false;
            else if (NAndGate.Corrupt != true)
                Console.WriteLine(this);
            return true;
        }
    }

}
