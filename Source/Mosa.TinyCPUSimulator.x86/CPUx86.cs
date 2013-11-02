/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;

namespace Mosa.TinyCPUSimulator.x86
{
	/// <summary>
	/// X86 protected mode only emulator
	/// </summary>
	public class CPUx86 : SimCPU
	{
		public Register32Bit EIP { get; private set; }

		public FlagsRegister EFLAGS { get; private set; }

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

		public uint GDTR { get; set; }

		public uint IDTR { get; set; }

		public CPUx86()
		{
			EIP = new Register32Bit("EIP", 0, RegisterType.InstructionPointer, false);
			EFLAGS = new FlagsRegister();

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
			EFLAGS.Value = 0;
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

			GDTR = 0;
			IDTR = 0;

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

			if ((pt & 0x1) == 0)
			{
				// page not present
				throw new PageFaultException(address);
			}

			return (address & 0xFFF) | (pt & 0xFFFFF000);
		}

		protected override void ExecuteOpcode(SimInstruction instruction)
		{
			uint eip = EIP.Value;
			uint flag = EFLAGS.Value;

			try
			{
				instruction.Opcode.Execute(this, instruction);
			}
			catch (PageFaultException e)
			{
				LastException = e; // note page fault

				// initiate page fault
				CR2.Value = (uint)e.Address;

				// Start Interrupt
				StartInterrupt(14, -1);
			}
		}

		private void StartInterrupt(byte vector, int errorCode)
		{
			try
			{
				uint idt = DirectRead32(IDTR);

				Write32(ESP.Value - (8 * 0), EFLAGS.Value);
				Write32(ESP.Value - (8 * 1), CS.Value);
				Write32(ESP.Value - (8 * 2), EIP.Value);

				if (errorCode == 8 || (errorCode >= 10 && errorCode <= 14))
				{
					Write32(ESP.Value - (8 * 3), EIP.Value);
				}

				EIP.Value = DirectRead32((ulong)(idt + vector * 4));
			}
			catch (PageFaultException e)
			{
				// This is technically a double fault - no support
			}
		}

		public override string CompactDump()
		{
			//s.AppendLine("EIP        EAX        EBX        ECX        EDX        ESI        EDI        ESP        EBP        FLAGS");
			return EIP.Value + " " + EAX.Value + " " + EBX.Value + " " + ECX.Value + " " + EDX.Value + " " + ESI.Value + " " + EDI.Value + " " + ESP.Value + " " + EBP.Value + " "
				+ (EFLAGS.Zero ? "Z" : "-")
				+ (EFLAGS.Carry ? "C" : "-")
				+ (EFLAGS.Direction ? "D" : "-")
				+ (EFLAGS.Overflow ? "O" : "-")
				+ (EFLAGS.Parity ? "P" : "-")
				+ (EFLAGS.Sign ? "S" : "-");
			//"[" + Tick.ToString("D5") + "] "
		}

		private string[] registerList = new string[] { "EIP", "EAX", "EBX", "ECX", "EDX", "ESP", "EBP", "ESI", "EDI", "CR0", "CR2", "CR3", "CR4" };
		private string[] flagList = new string[] { "Zero", "Parity", "Carry", "Direction", "Sign", "Adjust", "Overflow", "Value" };

		public override SimState GetState()
		{
			SimState simState = base.GetState();

			simState.StoreValue("Register.Size", (uint)32);

			simState.StoreValue("Register.List", registerList);
			simState.StoreValue("Flag.List", flagList);

			simState.StoreValue("Register.EIP", EIP.Value);
			simState.StoreValue("Register.EAX", EAX.Value);
			simState.StoreValue("Register.EBX", EBX.Value);
			simState.StoreValue("Register.ECX", ECX.Value);
			simState.StoreValue("Register.EDX", EDX.Value);
			simState.StoreValue("Register.ESP", ESP.Value);
			simState.StoreValue("Register.EBP", EBP.Value);
			simState.StoreValue("Register.ESI", ESI.Value);
			simState.StoreValue("Register.EDI", EDI.Value);

			simState.StoreValue("Register.CR0", CR0.Value);
			simState.StoreValue("Register.CR2", CR2.Value);
			simState.StoreValue("Register.CR3", CR3.Value);
			simState.StoreValue("Register.CR4", CR4.Value);

			simState.StoreValue("Register.XXM0", XMM0.Value);
			simState.StoreValue("Register.XXM1", XMM1.Value);
			simState.StoreValue("Register.XXM2", XMM2.Value);
			simState.StoreValue("Register.XXM3", XMM3.Value);
			simState.StoreValue("Register.XXM4", XMM4.Value);
			simState.StoreValue("Register.XXM5", XMM5.Value);
			simState.StoreValue("Register.XXM6", XMM6.Value);
			simState.StoreValue("Register.XXM7", XMM7.Value);

			simState.StoreValue("Flag.Value", EFLAGS.Value);
			simState.StoreValue("Flag.Zero", EFLAGS.Zero);
			simState.StoreValue("Flag.Parity", EFLAGS.Parity);
			simState.StoreValue("Flag.Carry", EFLAGS.Carry);
			simState.StoreValue("Flag.Direction", EFLAGS.Direction);
			simState.StoreValue("Flag.Sign", EFLAGS.Sign);
			simState.StoreValue("Flag.Adjust", EFLAGS.Adjust);
			simState.StoreValue("Flag.Overflow", EFLAGS.Overflow);

			var stack = new List<ulong[]>();
			var frame = new List<ulong[]>();

			simState.StoreValue("Stack", stack);
			simState.StoreValue("StackFrame", frame);

			uint esp = ESP.Value;
			uint index = 0;

			while (index < 16)
			{
				stack.Add(new ulong[2] { (ulong)Read32(esp), esp });
				esp = esp + 4;
				index++;
			}

			uint ebp = EBP.Value;
			index = 0;

			while (ebp > ESP.Value && index < 32)
			{
				frame.Add(new ulong[2] { (ulong)Read32(ebp), ebp });
				ebp = ebp - 4;
				index++;
			}

			return simState;
		}

		public override void ExtendState(SimState simState)
		{
			var list = new List<ulong>();

			uint ip = EIP.Value;
			uint ebp = EBP.Value;

			simState.StoreValue("CallStack", list);

			list.Add((ulong)ip);

			try
			{
				for (int i = 0; i < 20; i++)
				{
					if (ebp == 0)
						return;

					ip = DirectRead32(ebp + 4);

					if (ip == 0)
						return;

					list.Add((ulong)ip);

					ebp = DirectRead32(ebp);
				}
			}
			catch (SimCPUException e)
			{
			}
		}
	}
}