// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;
namespace Mosa.TinyCPUSimulator
{
	public class RegisterFloatingPoint : SimRegister
	{
		public virtual FloatingValue Value { get; set; }

		public RegisterFloatingPoint(string name, int index, RegisterType registerType, bool physical)
			: base(name, index, registerType, 128, physical)
		{
		}

		public RegisterFloatingPoint(string name, int index, RegisterType registerType)
			: base(name, index, registerType, 128, true)
		{
		}

		public override string ToString()
		{
			return base.ToString() + " = 0x" + Value.High.ToString("D") + Value.Low.ToString("D");
		}
	}

	[StructLayout(LayoutKind.Explicit, Size = 16)]
	public struct FloatingValue
	{
		// As Float
		//[FieldOffset(0)]
		//public float LowLow;
		//[FieldOffset(4)]
		//public float LowHigh;
		//[FieldOffset(8)]
		//public float HighLow;
		//[FieldOffset(12)]
		//public float HighHigh;

		// To/From Float
		public float LowF
		{
			get { return (float)Low; }
			set { Low = (double)value; }
		}

		public float HighF
		{
			get { return (float)Low; }
			set { Low = (double)value; }
		}

		// As Double
		[FieldOffset(0)]
		public double Low;
		[FieldOffset(8)]
		public double High;

		// As ULong
		[FieldOffset(0)]
		public ulong ULow;
		[FieldOffset(8)]
		public ulong UHigh;
	}
}