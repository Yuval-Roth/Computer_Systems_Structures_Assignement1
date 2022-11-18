using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //Multibit gates take as input k bits, and compute a function over all bits - z=f(x_0,x_1,...,x_k)
    class MultiBitAndGate : MultiBitGate
    {
        AndGate[] gates;

        public MultiBitAndGate(int iInputCount)
            : base(iInputCount)
        {
            gates = new AndGate[iInputCount - 1];
            for (int i = 0; i < gates.Length; i++)
            {
                gates[i] = new AndGate();
            }  

            gates[0].ConnectInput1(m_wsInput[0]);
            gates[0].ConnectInput2(m_wsInput[1]);
            int inputCounter = 2;
            for (int i = 1; i < gates.Length; i++)
            {
                gates[i].ConnectInput1(gates[i - 1].Output);
                gates[i].ConnectInput2(m_wsInput[inputCounter++]);
            }
            Output = gates[gates.Length-1].Output;
        }

        public override string ToString()
        {
            string output = "MultiBitAndGate ";

            for (int i = 0; i < m_wsInput.Size; i++)
            {
                output += m_wsInput[i] + ",";
            }
                
            output += " -> " + Output.Value;
            return output;
        }

        public override bool TestGate()
        {
            for (int i = 0; i < gates.Length + 1; i++)
                m_wsInput[i].Value = 1;
            if (Output.Value != 1)
                return false;
            else if (NAndGate.Corrupt != true)
                Console.WriteLine(this);

            m_wsInput[0].Value = 0;
            for (int i = 1; i < gates.Length + 1; i++)
                m_wsInput[i].Value = 1;
            if (Output.Value != 0)
                return false;
            else if (NAndGate.Corrupt != true)
                Console.WriteLine(this);

            return true;
        }
    }
}
