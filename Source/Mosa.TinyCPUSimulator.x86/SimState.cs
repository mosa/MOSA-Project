/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;

namespace Mosa.TinyCPUSimulator.x86
{
	public class SimState : BaseSimState
	{
		private static string[] registerList = new string[] { "EIP", "EAX", "EBX", "ECX", "EDX", "ESP", "EBP", "ESI", "EDI", "EFLAGS", "CR0", "CR2", "CR3", "CR4", "XMM0", "XMM1", "XMM2", "XMM3", "XMM4", "XMM5", "XMM6", "XMM7", };
		private static string[] flagList = new string[] { "Zero", "Parity", "Carry", "Direction", "Sign", "Adjust", "Overflow" };

		public override int NativeRegisterSize { get { return 32; } }

		public override string[] RegisterList { get { return registerList; } }

		public override string[] FlagList { get { return flagList; } }

		public uint EIP { get; private set; }

		public uint EAX { get; private set; }

		public uint EBX { get; private set; }

		public uint ECX { get; private set; }

		public uint EDX { get; private set; }

		public uint ESP { get; private set; }

		public uint EBP { get; private set; }

		public uint ESI { get; private set; }

		public uint EDI { get; private set; }

		public uint EFLAGS { get; private set; }

		public bool Zero { get; private set; }

		public bool Parity { get; private set; }

		public bool Carry { get; private set; }

		public bool Direction { get; private set; }

		public bool Sign { get; private set; }

		public bool Adjust { get; private set; }

		public bool Overflow { get; private set; }

		public uint CR0 { get; private set; }

		public uint CR2 { get; private set; }

		public uint CR3 { get; private set; }

		public uint CR4 { get; private set; }

		public double XMM0 { get; private set; }

		public double XMM1 { get; private set; }

		public double XMM2 { get; private set; }

		public double XMM3 { get; private set; }

		public double XMM4 { get; private set; }

		public double XMM5 { get; private set; }

		public double XMM6 { get; private set; }

		public double XMM7 { get; private set; }

		public SimState(CPUx86 x86)
			: base(x86)
		{
			EIP = x86.EIP.Value;
			EAX = x86.EAX.Value;
			EBX = x86.EBX.Value;
			ECX = x86.ECX.Value;
			EDX = x86.EDX.Value;
			ESP = x86.ESP.Value;
			EBP = x86.EBP.Value;
			ESI = x86.ESI.Value;
			EDI = x86.EDI.Value;

			CR0 = x86.CR0.Value;
			CR2 = x86.CR2.Value;
			CR3 = x86.CR3.Value;
			CR4 = x86.CR4.Value;

			XMM0 = x86.XMM0.Value.Low;
			XMM1 = x86.XMM1.Value.Low;
			XMM2 = x86.XMM2.Value.Low;
			XMM3 = x86.XMM3.Value.Low;
			XMM4 = x86.XMM4.Value.Low;
			XMM5 = x86.XMM5.Value.Low;
			XMM6 = x86.XMM6.Value.Low;
			XMM7 = x86.XMM7.Value.Low;

			Zero = x86.EFLAGS.Zero;
			Parity = x86.EFLAGS.Parity;
			Carry = x86.EFLAGS.Carry;
			Direction = x86.EFLAGS.Direction;
			Sign = x86.EFLAGS.Sign;
			Adjust = x86.EFLAGS.Adjust;
			Overflow = x86.EFLAGS.Overflow;
		}

		public override void ExtendState(SimCPU simCPU)
		{
			var x86 = simCPU as CPUx86;
			AddStack(x86);
			AddStackFrame(x86);
			AddCallStack(x86);
		}

		private void AddStack(CPUx86 x86)
		{
			var stack = new List<ulong[]>();

			StoreValue("Stack", stack);

			uint esp = x86.ESP.Value;
			uint index = 0;

			while (index < 16)
			{
				stack.Add(new ulong[2] { (ulong)x86.Read32(esp), esp });
				esp = esp + 4;
				index++;
			}
		}

		private void AddStackFrame(CPUx86 x86)
		{
			var frame = new List<ulong[]>();

			StoreValue("StackFrame", frame);

			uint ebp = x86.EBP.Value;
			uint index = 0;

			while (ebp > x86.ESP.Value && index < 32)
			{
				frame.Add(new ulong[2] { (ulong)x86.Read32(ebp), ebp });
				ebp = ebp - 4;
				index++;
			}
		}

		private void AddCallStack(CPUx86 x86)
		{
			var callStack = new List<ulong>();

			uint ip = x86.EIP.Value;
			uint ebp = x86.EBP.Value;

			StoreValue("CallStack", callStack);

			callStack.Add((ulong)ip);

			try
			{
				for (int i = 0; i < 20; i++)
				{
					if (ebp == 0)
						return;

					ip = x86.DirectRead32(ebp + 4);

					if (ip == 0)
						return;

					callStack.Add((ulong)ip);

					ebp = x86.DirectRead32(ebp);
				}
			}
			catch (SimCPUException e)
			{
			}
		}

		public override object GetRegister(string name)
		{
			switch (name)
			{
				case "EIP": return (object)EIP;
				case "EAX": return (object)EAX;
				case "EBX": return (object)EBX;
				case "ECX": return (object)ECX;
				case "EDX": return (object)EDX;
				case "ESP": return (object)ESP;
				case "EBP": return (object)EBP;
				case "ESI": return (object)ESI;
				case "EDI": return (object)EDI;
				case "CR0": return (object)CR0;
				case "CR2": return (object)CR2;
				case "CR3": return (object)CR3;
				case "CR4": return (object)CR4;
				case "EFLAGS": return (object)EFLAGS;

				case "XMM0": return (object)XMM0;
				case "XMM1": return (object)XMM1;
				case "XMM2": return (object)XMM2;
				case "XMM3": return (object)XMM3;
				case "XMM4": return (object)XMM4;
				case "XMM5": return (object)XMM5;
				case "XMM6": return (object)XMM6;
				case "XMM7": return (object)XMM7;

				case "Zero": return (object)Zero;
				case "Parity": return (object)Parity;
				case "Carry": return (object)Carry;
				case "Direction": return (object)Direction;
				case "Sign": return (object)Sign;
				case "Adjust": return (object)Adjust;
				case "Overflow": return (object)Overflow;

				default: return null;
			}
		}
	}
}