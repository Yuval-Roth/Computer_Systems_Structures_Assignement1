using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class is used to implement the ALU
    class ALU : Gate
    {
        //The word size = number of bit in the input and output
        public int Size { get; private set; }

        //Input and output n bit numbers
        //inputs
        public WireSet InputX { get; private set; }
        public WireSet InputY { get; private set; }
        public WireSet Control { get; private set; }

        //outputs
        public WireSet Output { get; private set; }
        public Wire Zero { get; private set; }
        public Wire Negative { get; private set; }


        private BitwiseMultiwayMux mux;
        private WireSet hw_One;
        private WireSet hw_Zero;
        private WireSet hw_MinusOne;
        private BitwiseNotGate Not_Input;
        private OrGate Or_Mux_Control;
        private AndGate And_Mux_Control;
        private BitwiseMux Select_Input;
        private BitwiseMux Select_Opposite_Input;
        private BitwiseMux One_or_MinusOne;
        private BitwiseMux And_Mux0;
        private BitwiseMux And_Mux1;
        private BitwiseMux Or_Mux;
        private MultiBitAdder neg_Input;
        private MultiBitAdder MainAdder;
        private BitwiseAndGate BitwiseAnd;
        private BitwiseOrGate BitwiseOr;
        private OrGate Logical_or;
        private AndGate Logical_and;
        private MultiBitOrGate x_bool_value;
        private MultiBitOrGate y_bool_value;
        private MultiBitOrGate zero_MB_or;
        private NotGate zero_not;

        public ALU(int iSize)
        {
            Size = iSize;
            InputX = new WireSet(Size);
            InputY = new WireSet(Size);
            Control = new WireSet(5);
            Zero = new Wire();

            mux = new BitwiseMultiwayMux(Size, 5);
            mux.ConnectControl(Control);
            Output = mux.Output;

            // Negative
            Negative = Output[Size - 1];

            // Zero
            zero_MB_or = new MultiBitOrGate(Size);
            zero_not = new NotGate();
            zero_MB_or.ConnectInput(Output);
            zero_not.ConnectInput(zero_MB_or.Output);
            Zero.ConnectInput(zero_not.Output);

            Select_Input = new BitwiseMux(Size);
            Select_Input.ConnectInput1(InputX);
            Select_Input.ConnectInput2(InputY);
            Select_Input.ConnectControl(Control[0]);

            Select_Opposite_Input = new BitwiseMux(Size);
            Select_Opposite_Input.ConnectInput1(InputY);
            Select_Opposite_Input.ConnectInput2(InputX);
            Select_Opposite_Input.ConnectControl(Control[0]);

            hw_Zero = new WireSet(Size);
            hw_One = new WireSet(Size);
            hw_MinusOne = new WireSet(Size);
            hw_One.SetValue(1);
            hw_MinusOne.Set2sComplement(-1);

            Not_Input = new BitwiseNotGate(Size);
            Not_Input.ConnectInput(Select_Input.Output);

            neg_Input = new MultiBitAdder(Size);
            neg_Input.ConnectInput1(Not_Input.Output);
            neg_Input.ConnectInput2(hw_One);

            One_or_MinusOne = new BitwiseMux(Size);
            One_or_MinusOne.ConnectInput1(hw_One);
            One_or_MinusOne.ConnectInput2(hw_MinusOne);
            One_or_MinusOne.ConnectControl(Control[1]);

            And_Mux_Control = new AndGate();
            And_Mux_Control.ConnectInput1(Control[2]);
            And_Mux_Control.ConnectInput2(Control[3]);

            Or_Mux_Control = new OrGate();
            Or_Mux_Control.ConnectInput1(Control[0]);
            Or_Mux_Control.ConnectInput2(Control[1]);

            Or_Mux = new BitwiseMux(Size);
            Or_Mux.ConnectInput1(InputX);
            Or_Mux.ConnectInput2(neg_Input.Output);
            Or_Mux.ConnectControl(Or_Mux_Control.Output);

            And_Mux0 = new BitwiseMux(Size);
            And_Mux0.ConnectInput1(One_or_MinusOne.Output);
            And_Mux0.ConnectInput2(Or_Mux.Output);
            And_Mux0.ConnectControl(And_Mux_Control.Output);

            And_Mux1 = new BitwiseMux(Size);
            And_Mux1.ConnectInput1(Select_Input.Output);
            And_Mux1.ConnectInput2(Select_Opposite_Input.Output);
            And_Mux1.ConnectControl(And_Mux_Control.Output);

            MainAdder = new MultiBitAdder(Size);
            MainAdder.ConnectInput1(And_Mux0.Output);
            MainAdder.ConnectInput2(And_Mux1.Output);

            BitwiseAnd = new BitwiseAndGate(Size);
            BitwiseAnd.ConnectInput1(InputX);
            BitwiseAnd.ConnectInput2(InputY);

            x_bool_value = new MultiBitOrGate(Size);
            y_bool_value = new MultiBitOrGate(Size);
            x_bool_value.ConnectInput(InputX);
            y_bool_value.ConnectInput(InputY);
            
            Logical_and = new AndGate();    
            Logical_and.ConnectInput1(x_bool_value.Output);
            Logical_and.ConnectInput2(y_bool_value.Output);
            WireSet Logical_and_output = new WireSet(Size);
            for (int i = 0; i < Size; i++)
            {
                Logical_and_output[i].ConnectInput(Logical_and.Output);
            }
            
            BitwiseOr = new BitwiseOrGate(Size);
            BitwiseOr.ConnectInput1(InputX);
            BitwiseOr.ConnectInput2(InputY);
            
            Logical_or = new OrGate();
            Logical_or.ConnectInput1(x_bool_value.Output);
            Logical_or.ConnectInput2(y_bool_value.Output);
            WireSet Logical_or_output = new WireSet(Size);
            for (int i = 0; i < Size; i++)
            {
                Logical_or_output[i].ConnectInput(Logical_or.Output);
            }

            //mux     
            mux.ConnectInput(0, hw_Zero);
            mux.ConnectInput(1, hw_One);
            mux.ConnectInput(2, InputX);
            mux.ConnectInput(3, InputY);
            mux.ConnectInput(4, Not_Input.Output);
            mux.ConnectInput(5, Not_Input.Output);
            mux.ConnectInput(6, neg_Input.Output);
            mux.ConnectInput(7, neg_Input.Output);
            mux.ConnectInput(8, MainAdder.Output);
            mux.ConnectInput(9, MainAdder.Output);
            mux.ConnectInput(10, MainAdder.Output);
            mux.ConnectInput(11, MainAdder.Output);
            mux.ConnectInput(12, MainAdder.Output);
            mux.ConnectInput(13, MainAdder.Output);
            mux.ConnectInput(14, MainAdder.Output);
            mux.ConnectInput(15, BitwiseAnd.Output);
            mux.ConnectInput(16, Logical_and_output);
            mux.ConnectInput(17, BitwiseOr.Output);
            mux.ConnectInput(18, Logical_or_output);

        }

        public override bool TestGate()
        {
            Random rand = new Random();
            int num = (int)Math.Pow(2, Size - 2);
            InputX.Set2sComplement(rand.Next(-num,num));
            InputY.Set2sComplement(rand.Next(-num,num));

            Control.SetValue(4);
            for (int i = 0; i < Size; i++)
            {
                if ((InputX[i].Value == 0 & Output[i].Value == 0) | (InputX[i].Value == 1 & Output[i].Value == 1))
                {
                    return false;
                }
            }
            Control.SetValue(5);
            for (int i = 0; i < Size; i++)
            {
                if ((InputY[i].Value == 0 & Output[i].Value == 0) | (InputY[i].Value == 1 & Output[i].Value == 1))
                {
                    return false;
                }
            }
            Control.SetValue(6);
            for (int i = 0; i < Size; i++)
            {
                if (InputX.Get2sComplement() != -Output.Get2sComplement())
                {
                    return false;
                }
            }
            Control.SetValue(7);
            for (int i = 0; i < Size; i++)
            {
                if (InputY.Get2sComplement() != -Output.Get2sComplement())
                {
                    return false;
                }
            }
            Control.SetValue(8);
            if (Output.Get2sComplement() != InputX.Get2sComplement() + 1)
            {
                return false;
            }
            Control.SetValue(9);
            if (Output.Get2sComplement() != InputY.Get2sComplement() + 1)
            {
                return false;
            }
            Control.SetValue(10);
            if (Output.Get2sComplement() != InputX.Get2sComplement() - 1)
            {
                return false;
            }
            Control.SetValue(11);
            if (Output.Get2sComplement() != InputY.Get2sComplement() - 1)
            {
                return false;
            }
            Control.SetValue(12);
            if (Output.Get2sComplement() != InputX.Get2sComplement() + InputY.Get2sComplement())
            {
                return false;
            }
            Control.SetValue(13);
            if (Output.Get2sComplement() != InputX.Get2sComplement() - InputY.Get2sComplement())
            {
                return false;
            }
            Control.SetValue(14);
            if (Output.Get2sComplement() != InputY.Get2sComplement() - InputX.Get2sComplement())
            {
                return false;
            }
            Control.SetValue(15);
            for (int i = 0; i < Size; i++)
            {
                if ((InputX[i].Value == 0 & InputY[i].Value == 0 & Output[i].Value != 0) |
                    (InputX[i].Value == 0 & InputY[i].Value == 1 & Output[i].Value != 0) |
                    (InputX[i].Value == 1 & InputY[i].Value == 0 & Output[i].Value != 0) |
                    (InputX[i].Value == 1 & InputY[i].Value == 1 & Output[i].Value != 1))
                {
                    return false;
                }
            }
            Control.SetValue(17);
            for (int i = 0; i < Size; i++)
            {
                if ((InputX[i].Value == 0 & InputY[i].Value == 0 & Output[i].Value != 0) |
                    (InputX[i].Value == 0 & InputY[i].Value == 1 & Output[i].Value != 1) |
                    (InputX[i].Value == 1 & InputY[i].Value == 0 & Output[i].Value != 1) |
                    (InputX[i].Value == 1 & InputY[i].Value == 1 & Output[i].Value != 1))
                {
                    return false;
                }
            }
            Control.SetValue(16);
            
            InputX.SetValue(0);
            InputY.SetValue(0);
            if (Output.Get2sComplement() != 0)
            {
                return false;
            }
            InputX.SetValue(1);
            InputY.SetValue(0);
            if (Output.Get2sComplement() != 0)
            {
                return false;
            }
            InputX.SetValue(0);
            InputY.SetValue(1);
            if (Output.Get2sComplement() != 0)
            {
                return false;
            }
            InputX.SetValue(1);
            InputY.SetValue(1);
            if (Output.Get2sComplement() == 0)
            {
                return false;
            }
            
            Control.SetValue(18);
            
            InputX.SetValue(0);
            InputY.SetValue(0);
            if (Output.Get2sComplement() != 0)
            {
                return false;
            }
            InputX.SetValue(1);
            InputY.SetValue(0);
            if (Output.Get2sComplement() == 0)
            {
                return false;
            }

            InputX.SetValue(0);
            InputY.SetValue(1);
            if (Output.Get2sComplement() == 0)
            {
                return false;
            }
            InputX.SetValue(1);
            InputY.SetValue(1);
            if (Output.Get2sComplement() == 0)
            {
                return false;
            }
            Console.WriteLine("ALU Test Passed");
            return true;
        }
    }
}
