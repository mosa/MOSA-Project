/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.TinyCPUSimulator.x86
{
	/// <summary>
	/// X86 protected mode only emulator
	/// </summary>
	public class CPUx86 : SimCPU
	{
		public Register32Bit EIP { get; private set; }

		public FlagsRegister FLAGS { get; private set; }

		public GeneralPurposeRegister EAX { get; private set; }

		public GeneralPurposeRegister EBX { get; private set; }

		public GeneralPurposeRegister ECX { get; private set; }

		public GeneralPurposeRegister EDX { get; private set; }

		public GeneralPurposeRegister ESI { get; private set; }

		public GeneralPurposeRegister EDI { get; private set; }

		public GeneralPurposeRegister EBP { get; private set; }

		public GeneralPurposeRegister ESP { get; private set; }

		public CR0 CR0 { get; private set; }

		public ControlRegister CR2 { get; private set; }

		public ControlRegister CR3 { get; private set; }

		public ControlRegister CR4 { get; private set; }

		public Legacy16BitRegister AX { get; private set; }

		public Legacy16BitRegister BX { get; private set; }

		public Legacy16BitRegister CX { get; private set; }

		public Legacy16BitRegister DX { get; private set; }

		public Legacy16BitRegister SI { get; private set; }

		public Legacy16BitRegister DI { get; private set; }

		public Legacy16BitRegister BP { get; private set; }

		public Legacy8BitLowRegister AL { get; private set; }

		public Legacy8BitLowRegister BL { get; private set; }

		public Legacy8BitLowRegister CL { get; private set; }

		public Legacy8BitLowRegister DL { get; private set; }

		public Legacy8BitHighRegister AH { get; private set; }

		public Legacy8BitHighRegister BH { get; private set; }

		public Legacy8BitHighRegister CH { get; private set; }

		public Legacy8BitHighRegister DH { get; private set; }

		public RegisterFloatingPoint XMM0 { get; private set; }

		public RegisterFloatingPoint XMM1 { get; private set; }

		public RegisterFloatingPoint XMM2 { get; private set; }

		public RegisterFloatingPoint XMM3 { get; private set; }

		public RegisterFloatingPoint XMM4 { get; private set; }

		public RegisterFloatingPoint XMM5 { get; private set; }

		public RegisterFloatingPoint XMM6 { get; private set; }

		public RegisterFloatingPoint XMM7 { get; private set; }

		public RegisterFloatingPoint ST0 { get; private set; }

		public SegmentRegister CS { get; private set; }

		public SegmentRegister DS { get; private set; }

		public SegmentRegister ES { get; private set; }

		public SegmentRegister FS { get; private set; }

		public SegmentRegister GS { get; private set; }

		public SegmentRegister SS { get; private set; }

		public CPUx86()
		{
			EIP = new Register32Bit("EIP", 0, RegisterType.InstructionPointer, false);
			FLAGS = new FlagsRegister();

			EAX = new GeneralPurposeRegister("EAX", 0);
			EBX = new GeneralPurposeRegister("EBX", 1);
			ECX = new GeneralPurposeRegister("ECX", 2);
			EDX = new GeneralPurposeRegister("EDX", 3);
			ESP = new GeneralPurposeRegister("ESP", 4);
			EBP = new GeneralPurposeRegister("EBP", 5);
			ESI = new GeneralPurposeRegister("ESI", 6);
			EDI = new GeneralPurposeRegister("EDI", 7);

			CR0 = new CR0();
			CR2 = new ControlRegister("CR2", 2);
			CR3 = new ControlRegister("CR3", 3);
			CR4 = new ControlRegister("CR3", 4);

			AX = new Legacy16BitRegister("AX", EAX);
			BX = new Legacy16BitRegister("BX", EBX);
			CX = new Legacy16BitRegister("CX", ECX);
			DX = new Legacy16BitRegister("DX", EDX);
			SI = new Legacy16BitRegister("SI", ESI);
			DI = new Legacy16BitRegister("DI", EDI);
			BP = new Legacy16BitRegister("BP", EBP);

			AL = new Legacy8BitLowRegister("AL", EAX);
			BL = new Legacy8BitLowRegister("BL", EBX);
			CL = new Legacy8BitLowRegister("CL", ECX);
			DL = new Legacy8BitLowRegister("DL", EDX);

			AH = new Legacy8BitHighRegister("AH", EAX);
			BH = new Legacy8BitHighRegister("BH", EBX);
			CH = new Legacy8BitHighRegister("CH", ECX);
			DH = new Legacy8BitHighRegister("DH", EDX);

			XMM0 = new RegisterFloatingPoint("XMM0", 0, RegisterType.FloatingPoint);
			XMM1 = new RegisterFloatingPoint("XMM1", 1, RegisterType.FloatingPoint);
			XMM2 = new RegisterFloatingPoint("XMM2", 2, RegisterType.FloatingPoint);
			XMM3 = new RegisterFloatingPoint("XMM3", 3, RegisterType.FloatingPoint);
			XMM4 = new RegisterFloatingPoint("XMM4", 4, RegisterType.FloatingPoint);
			XMM5 = new RegisterFloatingPoint("XMM5", 5, RegisterType.FloatingPoint);
			XMM6 = new RegisterFloatingPoint("XMM6", 6, RegisterType.FloatingPoint);
			XMM7 = new RegisterFloatingPoint("XMM7", 7, RegisterType.FloatingPoint);

			ST0 = new RegisterFloatingPoint("ST0", -1, RegisterType.FloatingPoint);

			CS = new SegmentRegister("CS", 0);
			DS = new SegmentRegister("DS", 1);
			ES = new SegmentRegister("ES", 2);
			FS = new SegmentRegister("FS", 3);
			GS = new SegmentRegister("GS", 4);
			SS = new SegmentRegister("SS", 5);

			Reset();
		}

		public override void Reset()
		{
			EIP.Value = 0;
			FLAGS.Value = 0;
			EAX.Value = 0;
			EBX.Value = 0;
			ECX.Value = 0;
			EDX.Value = 0;
			ESI.Value = 0;
			EBP.Value = 0;
			ESP.Value = 0;
			ESP.Value = 0;

			CR0.Value = 0;
			CR2.Value = 0;
			CR3.Value = 0;
			CR4.Value = 0;

			base.Reset();
		}

		public override ulong CurrentInstructionPointer { get { return EIP.Value; } set { EIP.Value = (uint)value; } }

		public override ulong StackPointer { get { return ESP.Value; } set { ESP.Value = (uint)value; } }

		public override ulong FramePointer { get { return EBP.Value; } set { EBP.Value = (uint)value; } }

		protected override ulong TranslateToPhysical(ulong address)
		{
			if ((CR0.Value & 0x80000000) == 0)
				return address;

			uint pd = DirectRead32(CR3.Value + ((address >> 22) * 4));
			uint pt = DirectRead32((pd & 0xFFFFF000) + ((address >> 12 & 0x03FF) * 4));

			return (address & 0xFFF) | (pt & 0xFFFFF000);
		}

		protected override void ExecuteOpcode(SimInstruction instruction)
		{
			try
			{
				base.ExecuteOpcode(instruction);
			}
			catch (InvalidMemoryAccess e)
			{
				throw;
			}
			catch (DivideByZero e)
			{
				throw;
			}
		}

		private static string ToHex(uint value)
		{
			return "0x" + (string.Format("{0:X}", value).PadLeft(8, '0'));
		}

		public override string CompactDump()
		{
			//s.AppendLine("EIP        EAX        EBX        ECX        EDX        ESI        EDI        ESP        EBP        FLAGS");
			return ToHex(EIP.Value) + " " + ToHex(EAX.Value) + " " + ToHex(EBX.Value) + " " + ToHex(ECX.Value) + " " + ToHex(EDX.Value) + " " + ToHex(ESI.Value) + " " + ToHex(EDI.Value) + " " + ToHex(ESP.Value) + " " + ToHex(EBP.Value) + " "
				+ (FLAGS.Zero ? "Z" : "-")
				+ (FLAGS.Carry ? "C" : "-")
				+ (FLAGS.Direction ? "D" : "-")
				+ (FLAGS.Overflow ? "O" : "-")
				+ (FLAGS.Parity ? "P" : "-")
				+ (FLAGS.Sign ? "S" : "-");
			//"[" + Tick.ToString("D5") + "] "
		}

		public override SimState GetState()
		{
			SimState simState = base.GetState();

			simState.StoreValue("IP.Formatted", ToHex(EIP.Value));
			//simState.StoreValue("EIP.Last", ToHex((uint)LastCurrentInstructionPointer));

			simState.StoreValue("Register.1.EIP", ToHex(EIP.Value));
			simState.StoreValue("Register.2.EAX", ToHex(EAX.Value));
			simState.StoreValue("Register.3.EBX", ToHex(EBX.Value));
			simState.StoreValue("Register.4.ECX", ToHex(ECX.Value));
			simState.StoreValue("Register.5.EDX", ToHex(EDX.Value));
			simState.StoreValue("Register.6.ESP", ToHex(ESP.Value));
			simState.StoreValue("Register.7.EBP", ToHex(EBP.Value));
			simState.StoreValue("Register.8.ESI", ToHex(ESI.Value));
			simState.StoreValue("Register.9.EDI", ToHex(EDI.Value));

			simState.StoreValue("CR0", ToHex(CR0.Value));
			simState.StoreValue("CR2", ToHex(CR2.Value));
			simState.StoreValue("CR3", ToHex(CR3.Value));
			simState.StoreValue("CR4", ToHex(CR4.Value));

			simState.StoreValue("XXM0", XMM0.Value.ToString());
			simState.StoreValue("XXM1", XMM1.Value.ToString());
			simState.StoreValue("XXM2", XMM2.Value.ToString());
			simState.StoreValue("XXM3", XMM3.Value.ToString());
			simState.StoreValue("XXM4", XMM4.Value.ToString());
			simState.StoreValue("XXM5", XMM5.Value.ToString());
			simState.StoreValue("XXM6", XMM6.Value.ToString());
			simState.StoreValue("XXM7", XMM7.Value.ToString());

			simState.StoreValue("FLAGS", FLAGS.Value.ToString());
			simState.StoreValue("FLAGS.Zero", FLAGS.Zero.ToString());
			simState.StoreValue("FLAGS.Parity", FLAGS.Parity.ToString());
			simState.StoreValue("FLAGS.Carry", FLAGS.Carry.ToString());
			simState.StoreValue("FLAGS.Direction", FLAGS.Direction.ToString());
			simState.StoreValue("FLAGS.Sign", FLAGS.Sign.ToString());
			simState.StoreValue("FLAGS.Adjust", FLAGS.Adjust.ToString());
			simState.StoreValue("FLAGS.Overflow", FLAGS.Overflow.ToString());

			uint ebp = EBP.Value;
			uint index = 0;

			while (ebp > ESP.Value && index < 32)
			{
				simState.StoreValue("StackFrame.Index." + index.ToString(), ToHex(Read32(ebp)));
				ebp = ebp - 4;
				index++;
			}

			simState.StoreValue("StackFrame.Index.Count", index.ToString());

			uint esp = ESP.Value + 4;
			index = 0;

			while (index < 16)
			{
				simState.StoreValue("Stack.Index." + index.ToString(), ToHex(Read32(esp)));
				esp = esp + 4;
				index++;
			}

			simState.StoreValue("Stack.Index.Count", index.ToString());

			return simState;
		}
	}
}