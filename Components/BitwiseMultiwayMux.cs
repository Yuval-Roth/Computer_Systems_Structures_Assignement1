using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements a mux with k input, each input with n wires. The output also has n wires.

    class BitwiseMultiwayMux : Gate
    {
        class BitwiseMuxHeap
        {

            BitwiseMux[] gates;
            int size;

            public BitwiseMuxHeap(int iSize, int ControlBits)
            {
                size = iSize;
                gates = new BitwiseMux[(int)Math.Pow(2, ControlBits)-1];
                BuildHeap(0);
            }

            public BitwiseMux[] GetLevel(int level)
            {
                BitwiseMux[] output = new BitwiseMux[(int)Math.Pow(2,level)];

                int start = 0;
                for (int i = 0; i < level; i++)
                {
                    start = start * 2 + 1;
                }
                int end = start * 2;
                for (int i = start, j = 0; i <= end; i++,j++)
                {
                    output[j] = gates[i];
                }
                return output;
            }

            private void BuildHeap(int current)
            {
                if (current == 0) gates[current] = new BitwiseMux(size);

                //Left son
                if (current * 2 + 1 < gates.Length)
                {
                    gates[current * 2 + 1] = new BitwiseMux(size);
                    gates[current].ConnectInput1(gates[current * 2 + 1].Output);
                    BuildHeap(current * 2 + 1);
                }
                //Right son
                if (current * 2 + 2 < gates.Length)
                {
                    gates[current * 2 + 2] = new BitwiseMux(size);
                    gates[current].ConnectInput2(gates[current * 2 + 2].Output);
                    BuildHeap(current * 2 + 2);
                }
            }        
        }



        //Word size - number of bits in each output
        public int Size { get; private set; }

        //The number of control bits needed for k outputs
        public int ControlBits { get; private set; }

        public WireSet Output { get; private set; }
        public WireSet Control { get; private set; }
        public WireSet[] Inputs { get; private set; }

        BitwiseMuxHeap heap;

        public BitwiseMultiwayMux(int iSize, int cControlBits)
        {
            Size = iSize;
            ControlBits = cControlBits; 
            Output = new WireSet(Size);
            Control = new WireSet(cControlBits);
            Inputs = new WireSet[(int)Math.Pow(2, cControlBits)];
            
            for (int i = 0; i < Inputs.Length; i++)
            {
                Inputs[i] = new WireSet(Size);
                
            }
            heap = new BitwiseMuxHeap(iSize, cControlBits);
            
            for (int i = cControlBits -1 ; i >= 0 ; i--)
            {
                BitwiseMux[] level = heap.GetLevel(i);
                foreach (BitwiseMux gate in level)
                {
                    gate.ConnectControl(Control[cControlBits - 1 - i]);
                }
            }

            BitwiseMux[] Lastlevel = heap.GetLevel(cControlBits-1);
            for (int i = 0,j=0; i < Lastlevel.Length; i++,j+=2)
            {
                Lastlevel[i].ConnectInput1(Inputs[j]);
                Lastlevel[i].ConnectInput2(Inputs[j+1]);
            }

            Output.ConnectInput(heap.GetLevel(0)[0].Output);


        }


        public void ConnectInput(int i, WireSet wsInput)
        {
            Inputs[i].ConnectInput(wsInput);
        }
        public void ConnectControl(WireSet wsControl)
        {
            Control.ConnectInput(wsControl);
        }

        public override string ToString()
        {
            string output = "MultiwayMux -> \n";
            for (int i = 0; i < Inputs.Length; i++)
            {
                output += Inputs[i] + "\n";
            }
            output += "C=" + Control + " output -> " + Output;
            return output;
        }

        public override bool TestGate()
        {
            int failed = 0;

            bool Test()
            {
                Random rand = new Random();

                for (int i = 0; i < Inputs.Length; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        Inputs[i][j].Value = rand.Next(0, 2);
                    }
                }
                for (int i = 0; i < ControlBits; i++)
                {
                    Control[i].Value = rand.Next(0, 2);
                }
                int ctrl = Convert.ToInt32(Control.ToString()[1..^1], 2);

                for (int i = 0; i < Size; i++)
                {
                    if (Output[i].Value != Inputs[ctrl][i].Value)
                        return false;
                }
                return true;
            }

            for (int i = 0; i < 20; i++)
            {
                if (!Test()) failed++;
            }

            if (failed <= 2)
            {
                if (NAndGate.Corrupt != true)
                    Console.WriteLine(this);
                return true;
            }
            else return false;

            
        }
    }
}
