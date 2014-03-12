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
	public class SimOperand
	{
		public bool IsRegister { get; private set; }

		public bool IsImmediate { get; private set; }

		public bool IsMemory { get; private set; }

		public SimRegister Register { get; private set; }

		public SimRegister Index { get; private set; }

		public int Displacement { get; private set; }

		public int Scale { get; private set; }

		public int Size { get; set; }

		public ulong Immediate { get; private set; }

		public string Label { get; private set; }

		public bool IsLabel { get { return Label != null; } }

		private SimOperand(int size)
		{
			Size = size;
			IsRegister = false;
			IsMemory = false;
			IsImmediate = false;
		}

		public SimOperand(SimRegister register)
			: this(register.Size)
		{
			Register = register;
			IsRegister = true;
		}

		public static SimOperand CreateImmediate(ulong immediate, int size)
		{
			var op = new SimOperand(size);
			op.Immediate = immediate;
			op.IsImmediate = true;
			return op;
		}

		public static SimOperand CreateImmediate(uint immediate)
		{
			var op = new SimOperand(32);
			op.Immediate = immediate;
			op.IsImmediate = true;
			return op;
		}

		public static SimOperand CreateImmediate(ushort immediate)
		{
			var op = new SimOperand(16);
			op.Immediate = immediate;
			op.IsImmediate = true;
			return op;
		}

		public static SimOperand CreateImmediate(byte immediate)
		{
			var op = new SimOperand(8);
			op.Immediate = immediate;
			op.IsImmediate = true;
			return op;
		}

		public static SimOperand CreateMemoryAddress(int size, SimRegister baseRegister, SimRegister index, int scale, int displacement)
		{
			var op = new SimOperand(size);
			op.IsMemory = true;
			op.Register = baseRegister;
			op.Index = index;
			op.Scale = scale;
			op.Displacement = displacement;
			return op;
		}

		public static SimOperand CreateMemoryAddress(int size, ulong immediate)
		{
			var op = new SimOperand(size);
			op.IsMemory = true;
			op.Immediate = immediate;
			return op;
		}

		//public static SimOperand CreateLabel(string label)
		//{
		//	var op = new SimOperand(32);
		//	op.Label = label;
		//	return op;
		//}

		public static SimOperand CreateLabel(int size, string label)
		{
			var op = new SimOperand(size);
			op.Label = label;
			return op;
		}

		public static SimOperand CreateMemoryAddressLabel(int size, string label)
		{
			var op = new SimOperand(size);
			op.Label = label;
			op.IsMemory = true;
			return op;
		}

		public override string ToString()
		{
			if (IsRegister)
				return Register.Name;

			if (IsImmediate)
				return Immediate.ToString();

			if (IsLabel)
			{
				if (IsMemory)
					return "[" + Label + "]";
				else
					return Label;
			}

			if (IsMemory)
			{
				//  Register + (Index * Scale) + Displacement
				string s = "[";

				if (Register != null)
					s = s + Register.Name;
				else
					s = s + Immediate.ToString();

				if (Index != null)
					s = s + "+" + Index.Name;

				if (Scale != 0)
					s = s + "*" + Scale.ToString();

				if (Displacement != 0)
					s = s + "+" + Displacement.ToString();

				s = s + "]";

				return s;
			}

			return string.Empty;
		}
	}
}