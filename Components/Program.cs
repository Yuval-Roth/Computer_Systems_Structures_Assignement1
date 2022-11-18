using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class Program
    {
        static void Main(string[] args)
        {
            Tests_1_4();

        }
        static void Tests_1_4()
        {
            Gate[] gates = {
                new SingleBitRegister(),
                new MultiBitRegister(5),
            };

            foreach (Gate gate in gates)
            {
                GateTest(gate);
            }
        }
        static void GateTest(Gate gate)
        {
            Console.WriteLine("testing " + gate.GetType().Name +"\n");
            if (!gate.TestGate())
                Console.WriteLine("bugbug");

            NAndGate.Corrupt = true;

            if (gate.TestGate())
            {
                if (gate is BitwiseNotGate) Console.WriteLine("\nnAnd corruption test skipped");
                else Console.WriteLine("bugbug");
            }
            else Console.WriteLine("\nnAnd corruption test successfull!");

            NAndGate.Corrupt = false;
            Console.WriteLine("================================================");
        }
        static void TestsUpTo_1_3() 
        {
            Gate[] gates = {
                new OrGate(),
                new XorGate(),
                new MuxGate(),
                new Demux(),
                new MultiBitAndGate(5),
                new MultiBitOrGate(5),
                new BitwiseAndGate(5),
                new BitwiseNotGate(5),
                new BitwiseOrGate(5),
                new BitwiseMux(5),
                new BitwiseDemux(5),
                new BitwiseMultiwayMux(5,3),
                new BitwiseMultiwayDemux(5,3),
            };

            foreach (Gate gate in gates)
            {
                GateTest(gate);
            }


            WireSet wires = new WireSet(8);
            int[] nums = { 5, 10, 27 };
            foreach (int num in nums)
            {
                wires.SetValue(num);
                Console.WriteLine("wires.SetValue(" + num + ") -> " + wires);
                Console.WriteLine("wires.GetValue() -> " + wires.GetValue());
                Console.WriteLine("================================================");
            }

            Gate[] gates2 = {
                new HalfAdder(),
                new FullAdder(),
                new MultiBitAdder(5),
            };

            foreach (Gate gate in gates2)
            {
                GateTest(gate);
            }

            WireSet wires2;
            int[] nums2 = { 1, -1, 5, -5, 10, -10, 100, -100 };
            foreach (int num in nums2)
            {
                wires2 = new WireSet(8);
                wires2.Set2sComplement(num);
                Console.WriteLine("wires2.Set2sComplement(" + num + ") -> " + wires2);
                Console.WriteLine("wires2.Get2sComplement() -> " + wires2.Get2sComplement());
                Console.WriteLine("================================================");
            }

            GateTest(new ALU(6));
        }
    }
}
