using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements a memory unit, containing k registers, each of size n bits.
    class Memory : SequentialGate
    {
        //The address size determines the number of registers
        public int AddressSize { get; private set; }
        //The word size determines the number of bits in each register
        public int WordSize { get; private set; }

        //Data input and output - a number with n bits
        public WireSet Input { get; private set; }
        public WireSet Output { get; private set; }
        //The address of the active register
        public WireSet Address { get; private set; }
        //A bit setting the memory operation to read or write
        public Wire Load { get; private set; }

        BitwiseMultiwayMux mux;
        BitwiseMultiwayDemux demux;
        MultiBitRegister[] MBRs;


        public Memory(int iAddressSize, int iWordSize)
        { 

            AddressSize = iAddressSize;
            WordSize = iWordSize;

            Input = new WireSet(WordSize);
            Output = new WireSet(WordSize);
            Address = new WireSet(AddressSize);
            Load = new Wire();

            mux = new BitwiseMultiwayMux(WordSize, AddressSize);
            demux = new BitwiseMultiwayDemux(1,AddressSize);
            demux.Input[0].ConnectInput(Load);
            mux.ConnectControl(Address);
            demux.ConnectControl(Address);

            MBRs = new MultiBitRegister[(int)Math.Pow(2,AddressSize)];
            for (int i = 0; i < MBRs.Length; i++)
            {
                MBRs[i] = new MultiBitRegister(WordSize);
                MBRs[i].Load.ConnectInput(demux.Outputs[i][0]);
                MBRs[i].ConnectInput(Input);
                mux.ConnectInput(i,MBRs[i].Output);
            }
            Output.ConnectInput(mux.Output);

        }

        public void ConnectInput(WireSet wsInput)
        {
            Input.ConnectInput(wsInput);
        }
        public void ConnectAddress(WireSet wsAddress)
        {
            Address.ConnectInput(wsAddress);
        }

        public override void OnClockUp()
        {
        }

        public override void OnClockDown()
        {
        }

        public override string ToString()
        {
            string output = "Memory -> \n";
            output += "Input->" + Input +", Load->"+Load +", Address->" + Address + "\n Registers -> \n";
            for (int i = 0; i < MBRs.Length; i++)
            {
                output += MBRs[i] + "\n";
            }
            output += "Output ->" + Output;
            return output;
        }

        public override bool TestGate()
        {
            Load.Value = 1;
            Input.SetValue(0);
            Clock.ClockDown();
            Clock.ClockUp();

            Random rand = new Random();


            for(int j = 0; j < 10; j++)
            {
                for (int i = 0; i < WordSize; i++)
                {
                    Input[i].Value = rand.Next(0, 2);
                }
                for (int i = 0; i < AddressSize; i++)
                {
                    Address[i].Value = rand.Next(0, 2);
                }

                int address = Convert.ToInt32(Address.ToString()[1..^1], 2);
                Clock.ClockDown();
                Clock.ClockUp();
                if (Input.Get2sComplement() != MBRs[address].Output.Get2sComplement()) return false;
                if (NAndGate.Corrupt == false) Console.WriteLine(this);
            }
            return true;
        }
    }
}
