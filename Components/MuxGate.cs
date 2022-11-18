using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A mux has 2 inputs. There is a single output and a control bit, selecting which of the 2 inpust should be directed to the output.
    class MuxGate : TwoInputGate
    {
        public Wire ControlInput { get; private set; }
        NotGate not;
        AndGate Y_And;
        AndGate X_And;
        OrGate or;

        public MuxGate()
        {
            ControlInput = new Wire();
            not = new NotGate();
            Y_And = new AndGate();
            X_And = new AndGate(); 
            or = new OrGate();

            Y_And.ConnectInput1(Input1);
            not.ConnectInput(ControlInput);
            Y_And.ConnectInput2(not.Output);

            X_And.ConnectInput1(Input2);
            X_And.ConnectInput2(ControlInput);

            or.ConnectInput1(X_And.Output);
            or.ConnectInput2(Y_And.Output);

            Output = or.Output;

        }

        public void ConnectControl(Wire wControl)
        {
            ControlInput.ConnectInput(wControl);
        }


        public override string ToString()
        {
            return "Mux " + Input1.Value + "," + Input2.Value + ",C" + ControlInput.Value + " -> " + Output.Value;
        }



        public override bool TestGate()
        {
            ControlInput.Value = 0;

            for (int i = 0; i <= 1; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    Input1.Value = i;
                    Input2.Value = j;
                    if (Output.Value != i)
                        return false;
                    else if(NAndGate.Corrupt != true) 
                            Console.WriteLine(this);
                }
            }

            ControlInput.Value = 1;

            for (int i = 0; i <= 1; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    Input1.Value = i;
                    Input2.Value = j;
                    if (Output.Value != j)
                        return false;
                    else if (NAndGate.Corrupt != true) 
                            Console.WriteLine(this);
                }
            }

            return true;
        }
    }
}
