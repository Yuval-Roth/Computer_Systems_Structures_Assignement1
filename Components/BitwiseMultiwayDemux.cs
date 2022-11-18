using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements a demux with k outputs, each output with n wires. The input also has n wires.

    class BitwiseMultiwayDemux : Gate
    {

        class BitwiseDemuxHeap
        {
            BitwiseDemux[] gates;
            int size;

            public BitwiseDemuxHeap(int iSize, int ControlBits)
            {
                size = iSize;
                gates = new BitwiseDemux[(int)Math.Pow(2, ControlBits) - 1];
                BuildHeap(0);
            }

            public BitwiseDemux[] GetLevel(int level)
            {
                BitwiseDemux[] output = new BitwiseDemux[(int)Math.Pow(2, level)];

                int start = 0;
                for (int i = 0; i < level; i++)
                {
                    start = start * 2 + 1;
                }
                int end = start * 2;
                for (int i = start, j = 0; i <= end; i++, j++)
                {
                    output[j] = gates[i];
                }
                return output;
            }

            private void BuildHeap(int current)
            {
                if (current == 0) gates[current] = new BitwiseDemux(size);

                //Left son
                if (current * 2 + 1 < gates.Length)
                {
                    gates[current * 2 + 1] = new BitwiseDemux(size);
                    gates[current * 2 + 1].ConnectInput(gates[current].Output1);
                    BuildHeap(current * 2 + 1);
                }
                //Right son
                if (current * 2 + 2 < gates.Length)
                {
                    gates[current * 2 + 2] = new BitwiseDemux(size);
                    gates[current * 2 + 2].ConnectInput(gates[current].Output2);
                    BuildHeap(current * 2 + 2);
                }
            }
        }
        //Word size - number of bits in each output
        public int Size { get; private set; }

        //The number of control bits needed for k outputs
        public int ControlBits { get; private set; }

        public WireSet Input { get; private set; }
        public WireSet Control { get; private set; }
        public WireSet[] Outputs { get; private set; }

        BitwiseDemuxHeap heap;

        public BitwiseMultiwayDemux(int iSize, int cControlBits)
        {
            Size = iSize;
            ControlBits = cControlBits;
            Input = new WireSet(Size);
            Control = new WireSet(cControlBits);
            Outputs = new WireSet[(int)Math.Pow(2, cControlBits)];
            for (int i = 0; i < Outputs.Length; i++)
            {
                Outputs[i] = new WireSet(Size);
            }
            heap = new BitwiseDemuxHeap(iSize, cControlBits);

            for (int i = 0; i < cControlBits ; i++)
            {
                BitwiseDemux[] level = heap.GetLevel(i);
                foreach (BitwiseDemux gate in level)
                {
                    gate.ConnectControl(Control[cControlBits - 1 - i]);
                }
            }

            BitwiseDemux[] Lastlevel = heap.GetLevel(cControlBits - 1);
            for (int i = 0, j = 0; i < Lastlevel.Length; i++, j += 2)
            {
                Outputs[j].ConnectInput(Lastlevel[i].Output1);
                Outputs[j + 1].ConnectInput(Lastlevel[i].Output2);
            }

            heap.GetLevel(0)[0].ConnectInput(Input);

        }


        public void ConnectInput(WireSet wsInput)
        {
            Input.ConnectInput(wsInput);
        }
        public void ConnectControl(WireSet wsControl)
        {
            Control.ConnectInput(wsControl);
        }

        public override string ToString()
        {
            string output = "MultiwayDemux -> \n";
            output += "C=" + Control + " Input -> " + Input + "\n Outputs -> \n";
            for (int i = 0; i < Outputs.Length; i++)
            {
                output += Outputs[i] + "\n";
            }         
            return output;
        }

        public override bool TestGate()
        {
            int failed = 0;


            bool Test()
            {
                Random rand = new Random();

                for (int i = 0; i < Size; i++)
                {
                    Input[i].Value = rand.Next(0, 2);
                }
                for (int i = 0; i < ControlBits; i++)
                {
                    Control[i].Value = rand.Next(0, 2);
                }

                int ctrl = Convert.ToInt32(Control.ToString()[1..^1], 2);

                for (int i = 0; i < Size; i++)
                {
                    if (Outputs[ctrl][i].Value != Input[i].Value)
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
