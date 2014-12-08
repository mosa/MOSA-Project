/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.TinyCPUSimulator
{
    public class SimInstruction
    {
        public BaseOpcode Opcode { get; private set; }

        public SimOperand Operand1 { get; private set; }

        public SimOperand Operand2 { get; private set; }

        public SimOperand Operand3 { get; private set; }

        public SimOperand Operand4 { get; private set; }

        public int OperandCount { get; private set; }

        public byte OpcodeSize { get; private set; }

        public byte Size { get; private set; }

        private SimInstruction(BaseOpcode opcode, byte size, int operandCount, byte opcodeSize)
        {
            Opcode = opcode;
            Size = size;
            OperandCount = operandCount;
            OpcodeSize = opcodeSize;
        }

        public SimInstruction(BaseOpcode opcode, byte size, byte opcodeSize)
            : this(opcode, size, 0, opcodeSize)
        { }

        public SimInstruction(BaseOpcode opcode, byte size, SimOperand operand1, byte opcodeSize)
            : this(opcode, size, 1, opcodeSize)
        {
            this.Operand1 = operand1;
        }

        public SimInstruction(BaseOpcode opcode, byte size, SimOperand operand1, SimOperand operand2, byte opcodeSize)
            : this(opcode, size, 2, opcodeSize)
        {
            this.Operand1 = operand1;
            this.Operand2 = operand2;
        }

        public SimInstruction(BaseOpcode opcode, byte size, SimOperand operand1, SimOperand operand2, SimOperand operand3, byte opcodeSize)
            : this(opcode, size, 3, opcodeSize)
        {
            this.Operand1 = operand1;
            this.Operand2 = operand2;
            this.Operand3 = operand3;
        }

        public SimInstruction(BaseOpcode opcode, byte size, SimOperand operand1, SimOperand operand2, SimOperand operand3, SimOperand operand4, byte opcodeSize)
            : this(opcode, size, 4, opcodeSize)
        {
            this.Operand1 = operand1;
            this.Operand2 = operand2;
            this.Operand3 = operand3;
            this.Operand4 = operand4;
        }

        public override string ToString()
        {
            string s = Opcode.ToString();

            if (Size != 0)
                s = s + "/" + Size.ToString();

            if (Operand1 != null)
                s = s + " " + Operand1.ToString();
            if (Operand2 != null)
                s = s + ", " + Operand2.ToString();
            if (Operand3 != null)
                s = s + ", " + Operand3.ToString();
            if (Operand4 != null)
                s = s + ", " + Operand4.ToString();

            return s;
        }
    }
}