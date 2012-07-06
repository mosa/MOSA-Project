/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Compiler.Framework.CIL
{

	/// <summary>
	/// 
	/// </summary>
	public static class CILInstruction
	{
		#region Static Data

		private static BaseCILInstruction[] opcodeMap = Initialize();

		/// <summary>
		/// Gets the instructions.
		/// </summary>
		public static BaseCILInstruction[] Instructions
		{
			get { return opcodeMap; }
		}

		#endregion // Static Data

		/// <summary>
		/// Gets the instruction.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public static BaseCILInstruction Get(OpCode opcode)
		{
			return opcodeMap[(int)opcode];
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		/// <returns></returns>
		public static BaseCILInstruction[] Initialize()
		{
			BaseCILInstruction[] opcodeMap = new BaseCILInstruction[0x120];

			/* 0x000 */
			opcodeMap[(int)OpCode.Nop] = new NopInstruction(OpCode.Nop);
			/* 0x001 */
			opcodeMap[(int)OpCode.Break] = new BreakInstruction(OpCode.Break);
			/* 0x002 */
			opcodeMap[(int)OpCode.Ldarg_0] = new LdargInstruction(OpCode.Ldarg_0);
			/* 0x003 */
			opcodeMap[(int)OpCode.Ldarg_1] = new LdargInstruction(OpCode.Ldarg_1);
			/* 0x004 */
			opcodeMap[(int)OpCode.Ldarg_2] = new LdargInstruction(OpCode.Ldarg_2);
			/* 0x005 */
			opcodeMap[(int)OpCode.Ldarg_3] = new LdargInstruction(OpCode.Ldarg_3);
			/* 0x006 */
			opcodeMap[(int)OpCode.Ldloc_0] = new LdlocInstruction(OpCode.Ldloc_0);
			/* 0x007 */
			opcodeMap[(int)OpCode.Ldloc_1] = new LdlocInstruction(OpCode.Ldloc_1);
			/* 0x008 */
			opcodeMap[(int)OpCode.Ldloc_2] = new LdlocInstruction(OpCode.Ldloc_2);
			/* 0x009 */
			opcodeMap[(int)OpCode.Ldloc_3] = new LdlocInstruction(OpCode.Ldloc_3);
			/* 0x00A */
			opcodeMap[(int)OpCode.Stloc_0] = new StlocInstruction(OpCode.Stloc_0);
			/* 0x00B */
			opcodeMap[(int)OpCode.Stloc_1] = new StlocInstruction(OpCode.Stloc_1);
			/* 0x00C */
			opcodeMap[(int)OpCode.Stloc_2] = new StlocInstruction(OpCode.Stloc_2);
			/* 0x00D */
			opcodeMap[(int)OpCode.Stloc_3] = new StlocInstruction(OpCode.Stloc_3);
			/* 0x00E */
			opcodeMap[(int)OpCode.Ldarg_s] = new LdargInstruction(OpCode.Ldarg_s);
			/* 0x00F */
			opcodeMap[(int)OpCode.Ldarga_s] = new LdargaInstruction(OpCode.Ldarga_s);
			/* 0x010 */
			opcodeMap[(int)OpCode.Starg_s] = new StargInstruction(OpCode.Starg_s);
			/* 0x011 */
			opcodeMap[(int)OpCode.Ldloc_s] = new LdlocInstruction(OpCode.Ldloc_s);
			/* 0x012 */
			opcodeMap[(int)OpCode.Ldloca_s] = new LdlocaInstruction(OpCode.Ldloca_s);
			/* 0x013 */
			opcodeMap[(int)OpCode.Stloc_s] = new StlocInstruction(OpCode.Stloc_s);
			/* 0x014 */
			opcodeMap[(int)OpCode.Ldnull] = new LdcInstruction(OpCode.Ldnull);
			/* 0x015 */
			opcodeMap[(int)OpCode.Ldc_i4_m1] = new LdcInstruction(OpCode.Ldc_i4_m1);
			/* 0x016 */
			opcodeMap[(int)OpCode.Ldc_i4_0] = new LdcInstruction(OpCode.Ldc_i4_0);
			/* 0x017 */
			opcodeMap[(int)OpCode.Ldc_i4_1] = new LdcInstruction(OpCode.Ldc_i4_1);
			/* 0x018 */
			opcodeMap[(int)OpCode.Ldc_i4_2] = new LdcInstruction(OpCode.Ldc_i4_2);
			/* 0x019 */
			opcodeMap[(int)OpCode.Ldc_i4_3] = new LdcInstruction(OpCode.Ldc_i4_3);
			/* 0x01A */
			opcodeMap[(int)OpCode.Ldc_i4_4] = new LdcInstruction(OpCode.Ldc_i4_4);
			/* 0x01B */
			opcodeMap[(int)OpCode.Ldc_i4_5] = new LdcInstruction(OpCode.Ldc_i4_5);
			/* 0x01C */
			opcodeMap[(int)OpCode.Ldc_i4_6] = new LdcInstruction(OpCode.Ldc_i4_6);
			/* 0x01D */
			opcodeMap[(int)OpCode.Ldc_i4_7] = new LdcInstruction(OpCode.Ldc_i4_7);
			/* 0x01E */
			opcodeMap[(int)OpCode.Ldc_i4_8] = new LdcInstruction(OpCode.Ldc_i4_8);
			/* 0x01F */
			opcodeMap[(int)OpCode.Ldc_i4_s] = new LdcInstruction(OpCode.Ldc_i4_s);
			/* 0x020 */
			opcodeMap[(int)OpCode.Ldc_i4] = new LdcInstruction(OpCode.Ldc_i4);
			/* 0x021 */
			opcodeMap[(int)OpCode.Ldc_i8] = new LdcInstruction(OpCode.Ldc_i8);
			/* 0x022 */
			opcodeMap[(int)OpCode.Ldc_r4] = new LdcInstruction(OpCode.Ldc_r4);
			/* 0x023 */
			opcodeMap[(int)OpCode.Ldc_r8] = new LdcInstruction(OpCode.Ldc_r8);
			/* 0x024 is undefined */
			/* 0x025 */
			opcodeMap[(int)OpCode.Dup] = new DupInstruction(OpCode.Dup);
			/* 0x026 */
			opcodeMap[(int)OpCode.Pop] = new PopInstruction(OpCode.Pop);
			/* 0x027 */
			opcodeMap[(int)OpCode.Jmp] = new JumpInstruction(OpCode.Jmp);
			/* 0x028 */
			opcodeMap[(int)OpCode.Call] = new CallInstruction(OpCode.Call);
			/* 0x029 */
			opcodeMap[(int)OpCode.Calli] = new CalliInstruction(OpCode.Calli);
			/* 0x02A */
			opcodeMap[(int)OpCode.Ret] = new ReturnInstruction(OpCode.Ret);
			/* 0x02B */
			opcodeMap[(int)OpCode.Br_s] = new BranchInstruction(OpCode.Br_s);
			/* 0x02C */
			opcodeMap[(int)OpCode.Brfalse_s] = new UnaryBranchInstruction(OpCode.Brfalse_s);
			/* 0x02D */
			opcodeMap[(int)OpCode.Brtrue_s] = new UnaryBranchInstruction(OpCode.Brtrue_s);
			/* 0x02E */
			opcodeMap[(int)OpCode.Beq_s] = new BinaryBranchInstruction(OpCode.Beq_s);
			/* 0x02F */
			opcodeMap[(int)OpCode.Bge_s] = new BinaryBranchInstruction(OpCode.Bge_s);
			/* 0x030 */
			opcodeMap[(int)OpCode.Bgt_s] = new BinaryBranchInstruction(OpCode.Bgt_s);
			/* 0x031 */
			opcodeMap[(int)OpCode.Ble_s] = new BinaryBranchInstruction(OpCode.Ble_s);
			/* 0x032 */
			opcodeMap[(int)OpCode.Blt_s] = new BinaryBranchInstruction(OpCode.Blt_s);
			/* 0x033 */
			opcodeMap[(int)OpCode.Bne_un_s] = new BinaryBranchInstruction(OpCode.Bne_un_s);
			/* 0x034 */
			opcodeMap[(int)OpCode.Bge_un_s] = new BinaryBranchInstruction(OpCode.Bge_un_s);
			/* 0x035 */
			opcodeMap[(int)OpCode.Bgt_un_s] = new BinaryBranchInstruction(OpCode.Bgt_un_s);
			/* 0x036 */
			opcodeMap[(int)OpCode.Ble_un_s] = new BinaryBranchInstruction(OpCode.Ble_un_s);
			/* 0x037 */
			opcodeMap[(int)OpCode.Blt_un_s] = new BinaryBranchInstruction(OpCode.Blt_un_s);
			/* 0x038 */
			opcodeMap[(int)OpCode.Br] = new BranchInstruction(OpCode.Br);
			/* 0x039 */
			opcodeMap[(int)OpCode.Brfalse] = new UnaryBranchInstruction(OpCode.Brfalse);
			/* 0x03A */
			opcodeMap[(int)OpCode.Brtrue] = new UnaryBranchInstruction(OpCode.Brtrue);
			/* 0x03B */
			opcodeMap[(int)OpCode.Beq] = new BinaryBranchInstruction(OpCode.Beq);
			/* 0x03C */
			opcodeMap[(int)OpCode.Bge] = new BinaryBranchInstruction(OpCode.Bge);
			/* 0x03D */
			opcodeMap[(int)OpCode.Bgt] = new BinaryBranchInstruction(OpCode.Bgt);
			/* 0x03E */
			opcodeMap[(int)OpCode.Ble] = new BinaryBranchInstruction(OpCode.Ble);
			/* 0x03F */
			opcodeMap[(int)OpCode.Blt] = new BinaryBranchInstruction(OpCode.Blt);
			/* 0x040 */
			opcodeMap[(int)OpCode.Bne_un] = new BinaryBranchInstruction(OpCode.Bne_un);
			/* 0x041 */
			opcodeMap[(int)OpCode.Bge_un] = new BinaryBranchInstruction(OpCode.Bge_un);
			/* 0x042 */
			opcodeMap[(int)OpCode.Bgt_un] = new BinaryBranchInstruction(OpCode.Bgt_un);
			/* 0x043 */
			opcodeMap[(int)OpCode.Ble_un] = new BinaryBranchInstruction(OpCode.Ble_un);
			/* 0x044 */
			opcodeMap[(int)OpCode.Blt_un] = new BinaryBranchInstruction(OpCode.Blt_un);
			/* 0x045 */
			opcodeMap[(int)OpCode.Switch] = new SwitchInstruction(OpCode.Switch);
			/* 0x046 */
			opcodeMap[(int)OpCode.Ldind_i1] = new LdobjInstruction(OpCode.Ldind_i1);
			/* 0x047 */
			opcodeMap[(int)OpCode.Ldind_u1] = new LdobjInstruction(OpCode.Ldind_u1);
			/* 0x048 */
			opcodeMap[(int)OpCode.Ldind_i2] = new LdobjInstruction(OpCode.Ldind_i2);
			/* 0x049 */
			opcodeMap[(int)OpCode.Ldind_u2] = new LdobjInstruction(OpCode.Ldind_u2);
			/* 0x04A */
			opcodeMap[(int)OpCode.Ldind_i4] = new LdobjInstruction(OpCode.Ldind_i4);
			/* 0x04B */
			opcodeMap[(int)OpCode.Ldind_u4] = new LdobjInstruction(OpCode.Ldind_u4);
			/* 0x04C */
			opcodeMap[(int)OpCode.Ldind_i8] = new LdobjInstruction(OpCode.Ldind_i8);
			/* 0x04D */
			opcodeMap[(int)OpCode.Ldind_i] = new LdobjInstruction(OpCode.Ldind_i);
			/* 0x04E */
			opcodeMap[(int)OpCode.Ldind_r4] = new LdobjInstruction(OpCode.Ldind_r4);
			/* 0x04F */
			opcodeMap[(int)OpCode.Ldind_r8] = new LdobjInstruction(OpCode.Ldind_r8);
			/* 0x050 */
			opcodeMap[(int)OpCode.Ldind_ref] = new LdobjInstruction(OpCode.Ldind_ref);
			/* 0x051 */
			opcodeMap[(int)OpCode.Stind_ref] = new StobjInstruction(OpCode.Stind_ref);
			/* 0x052 */
			opcodeMap[(int)OpCode.Stind_i1] = new StobjInstruction(OpCode.Stind_i1);
			/* 0x053 */
			opcodeMap[(int)OpCode.Stind_i2] = new StobjInstruction(OpCode.Stind_i2);
			/* 0x054 */
			opcodeMap[(int)OpCode.Stind_i4] = new StobjInstruction(OpCode.Stind_i4);
			/* 0x055 */
			opcodeMap[(int)OpCode.Stind_i8] = new StobjInstruction(OpCode.Stind_i8);
			/* 0x056 */
			opcodeMap[(int)OpCode.Stind_r4] = new StobjInstruction(OpCode.Stind_r4);
			/* 0x057 */
			opcodeMap[(int)OpCode.Stind_r8] = new StobjInstruction(OpCode.Stind_r8);
			/* 0x058 */
			opcodeMap[(int)OpCode.Add] = new AddInstruction(OpCode.Add);
			/* 0x059 */
			opcodeMap[(int)OpCode.Sub] = new SubInstruction(OpCode.Sub);
			/* 0x05A */
			opcodeMap[(int)OpCode.Mul] = new MulInstruction(OpCode.Mul);
			/* 0x05B */
			opcodeMap[(int)OpCode.Div] = new DivInstruction(OpCode.Div);
			/* 0x05C */
			opcodeMap[(int)OpCode.Div_un] = new BinaryLogicInstruction(OpCode.Div_un);
			/* 0x05D */
			opcodeMap[(int)OpCode.Rem] = new RemInstruction(OpCode.Rem);
			/* 0x05E */
			opcodeMap[(int)OpCode.Rem_un] = new BinaryLogicInstruction(OpCode.Rem_un);
			/* 0x05F */
			opcodeMap[(int)OpCode.And] = new BinaryLogicInstruction(OpCode.And);
			/* 0x060 */
			opcodeMap[(int)OpCode.Or] = new BinaryLogicInstruction(OpCode.Or);
			/* 0x061 */
			opcodeMap[(int)OpCode.Xor] = new BinaryLogicInstruction(OpCode.Xor);
			/* 0x062 */
			opcodeMap[(int)OpCode.Shl] = new ShiftInstruction(OpCode.Shl);
			/* 0x063 */
			opcodeMap[(int)OpCode.Shr] = new ShiftInstruction(OpCode.Shr);
			/* 0x064 */
			opcodeMap[(int)OpCode.Shr_un] = new ShiftInstruction(OpCode.Shr_un);
			/* 0x065 */
			opcodeMap[(int)OpCode.Neg] = new NegInstruction(OpCode.Neg);
			/* 0x066 */
			opcodeMap[(int)OpCode.Not] = new NotInstruction(OpCode.Not);
			/* 0x067 */
			opcodeMap[(int)OpCode.Conv_i1] = new ConversionInstruction(OpCode.Conv_i1);
			/* 0x068 */
			opcodeMap[(int)OpCode.Conv_i2] = new ConversionInstruction(OpCode.Conv_i2);
			/* 0x069 */
			opcodeMap[(int)OpCode.Conv_i4] = new ConversionInstruction(OpCode.Conv_i4);
			/* 0x06A */
			opcodeMap[(int)OpCode.Conv_i8] = new ConversionInstruction(OpCode.Conv_i8);
			/* 0x06B */
			opcodeMap[(int)OpCode.Conv_r4] = new ConversionInstruction(OpCode.Conv_r4);
			/* 0x06C */
			opcodeMap[(int)OpCode.Conv_r8] = new ConversionInstruction(OpCode.Conv_r8);
			/* 0x06D */
			opcodeMap[(int)OpCode.Conv_u4] = new ConversionInstruction(OpCode.Conv_u4);
			/* 0x06E */
			opcodeMap[(int)OpCode.Conv_u8] = new ConversionInstruction(OpCode.Conv_u8);
			/* 0x06F */
			opcodeMap[(int)OpCode.Callvirt] = new CallvirtInstruction(OpCode.Callvirt);
			/* 0x070 */
			opcodeMap[(int)OpCode.Cpobj] = new CpobjInstruction(OpCode.Cpobj);
			/* 0x071 */
			opcodeMap[(int)OpCode.Ldobj] = new LdobjInstruction(OpCode.Ldobj);
			/* 0x072 */
			opcodeMap[(int)OpCode.Ldstr] = new LdstrInstruction(OpCode.Ldstr);
			/* 0x073 */
			opcodeMap[(int)OpCode.Newobj] = new NewobjInstruction(OpCode.Newobj);
			/* 0x074 */
			opcodeMap[(int)OpCode.Castclass] = new CastclassInstruction(OpCode.Castclass);
			/* 0x075 */
			opcodeMap[(int)OpCode.Isinst] = new IsInstInstruction(OpCode.Isinst);
			/* 0x076 */
			opcodeMap[(int)OpCode.Conv_r_un] = new ConversionInstruction(OpCode.Conv_r_un);
			/* Opcodes 0x077-0x078 undefined */
			/* 0x079 */
			opcodeMap[(int)OpCode.Unbox] = new UnboxInstruction(OpCode.Unbox);
			/* 0x07A */
			opcodeMap[(int)OpCode.Throw] = new ThrowInstruction(OpCode.Throw);
			/* 0x07B */
			opcodeMap[(int)OpCode.Ldfld] = new LdfldInstruction(OpCode.Ldfld);
			/* 0x07C */
			opcodeMap[(int)OpCode.Ldflda] = new LdfldaInstruction(OpCode.Ldflda);
			/* 0x07D */
			opcodeMap[(int)OpCode.Stfld] = new StfldInstruction(OpCode.Stfld);
			/* 0x07E */
			opcodeMap[(int)OpCode.Ldsfld] = new LdsfldInstruction(OpCode.Ldsfld);
			/* 0x07F */
			opcodeMap[(int)OpCode.Ldsflda] = new LdsfldaInstruction(OpCode.Ldsflda);
			/* 0x080 */
			opcodeMap[(int)OpCode.Stsfld] = new StsfldInstruction(OpCode.Stsfld);
			/* 0x081 */
			opcodeMap[(int)OpCode.Stobj] = new StobjInstruction(OpCode.Stobj);
			/* 0x082 */
			opcodeMap[(int)OpCode.Conv_ovf_i1_un] = new ConversionInstruction(OpCode.Conv_ovf_i1_un);
			/* 0x083 */
			opcodeMap[(int)OpCode.Conv_ovf_i2_un] = new ConversionInstruction(OpCode.Conv_ovf_i2_un);
			/* 0x084 */
			opcodeMap[(int)OpCode.Conv_ovf_i4_un] = new ConversionInstruction(OpCode.Conv_ovf_i4_un);
			/* 0x085 */
			opcodeMap[(int)OpCode.Conv_ovf_i8_un] = new ConversionInstruction(OpCode.Conv_ovf_i8_un);
			/* 0x086 */
			opcodeMap[(int)OpCode.Conv_ovf_u1_un] = new ConversionInstruction(OpCode.Conv_ovf_u1_un);
			/* 0x087 */
			opcodeMap[(int)OpCode.Conv_ovf_u2_un] = new ConversionInstruction(OpCode.Conv_ovf_u2_un);
			/* 0x088 */
			opcodeMap[(int)OpCode.Conv_ovf_u4_un] = new ConversionInstruction(OpCode.Conv_ovf_u4_un);
			/* 0x089 */
			opcodeMap[(int)OpCode.Conv_ovf_u8_un] = new ConversionInstruction(OpCode.Conv_ovf_u8_un);
			/* 0x08A */
			opcodeMap[(int)OpCode.Conv_ovf_i_un] = new ConversionInstruction(OpCode.Conv_ovf_i_un);
			/* 0x08B */
			opcodeMap[(int)OpCode.Conv_ovf_u_un] = new ConversionInstruction(OpCode.Conv_ovf_u_un);
			/* 0x08C */
			opcodeMap[(int)OpCode.Box] = new BoxInstruction(OpCode.Box);
			/* 0x08D */
			opcodeMap[(int)OpCode.Newarr] = new NewarrInstruction(OpCode.Newarr);
			/* 0x08E */
			opcodeMap[(int)OpCode.Ldlen] = new LdlenInstruction(OpCode.Ldlen);
			/* 0x08F */
			opcodeMap[(int)OpCode.Ldelema] = new LdelemaInstruction(OpCode.Ldelema);
			/* 0x090 */
			opcodeMap[(int)OpCode.Ldelem_i1] = new LdelemInstruction(OpCode.Ldelem_i1);
			/* 0x091 */
			opcodeMap[(int)OpCode.Ldelem_u1] = new LdelemInstruction(OpCode.Ldelem_u1);
			/* 0x092 */
			opcodeMap[(int)OpCode.Ldelem_i2] = new LdelemInstruction(OpCode.Ldelem_i2);
			/* 0x093 */
			opcodeMap[(int)OpCode.Ldelem_u2] = new LdelemInstruction(OpCode.Ldelem_u2);
			/* 0x094 */
			opcodeMap[(int)OpCode.Ldelem_i4] = new LdelemInstruction(OpCode.Ldelem_i4);
			/* 0x095 */
			opcodeMap[(int)OpCode.Ldelem_u4] = new LdelemInstruction(OpCode.Ldelem_u4);
			/* 0x096 */
			opcodeMap[(int)OpCode.Ldelem_i8] = new LdelemInstruction(OpCode.Ldelem_i8);
			/* 0x097 */
			opcodeMap[(int)OpCode.Ldelem_i] = new LdelemInstruction(OpCode.Ldelem_i);
			/* 0x098 */
			opcodeMap[(int)OpCode.Ldelem_r4] = new LdelemInstruction(OpCode.Ldelem_r4);
			/* 0x099 */
			opcodeMap[(int)OpCode.Ldelem_r8] = new LdelemInstruction(OpCode.Ldelem_r8);
			/* 0x09A */
			opcodeMap[(int)OpCode.Ldelem_ref] = new LdelemInstruction(OpCode.Ldelem_ref);
			/* 0x09B */
			opcodeMap[(int)OpCode.Stelem_i] = new StelemInstruction(OpCode.Stelem_i);
			/* 0x09C */
			opcodeMap[(int)OpCode.Stelem_i1] = new StelemInstruction(OpCode.Stelem_i1);
			/* 0x09D */
			opcodeMap[(int)OpCode.Stelem_i2] = new StelemInstruction(OpCode.Stelem_i2);
			/* 0x09E */
			opcodeMap[(int)OpCode.Stelem_i4] = new StelemInstruction(OpCode.Stelem_i4);
			/* 0x09F */
			opcodeMap[(int)OpCode.Stelem_i8] = new StelemInstruction(OpCode.Stelem_i8);
			/* 0x0A0 */
			opcodeMap[(int)OpCode.Stelem_r4] = new StelemInstruction(OpCode.Stelem_r4);
			/* 0x0A1 */
			opcodeMap[(int)OpCode.Stelem_r8] = new StelemInstruction(OpCode.Stelem_r8);
			/* 0x0A2 */
			opcodeMap[(int)OpCode.Stelem_ref] = new StelemInstruction(OpCode.Stelem_ref);
			/* 0x0A3 */
			opcodeMap[(int)OpCode.Ldelem] = null; // new LdelemInstruction(OpCode.Ldelem);
			/* 0x0A4 */
			opcodeMap[(int)OpCode.Stelem] = new StelemInstruction(OpCode.Stelem);
			/* 0x0A5 */
			opcodeMap[(int)OpCode.Unbox_any] = new UnboxAnyInstruction(OpCode.Unbox_any);
			/* Opcodes 0x0A6-0x0B2 are undefined */
			/* 0x0B3 */
			opcodeMap[(int)OpCode.Conv_ovf_i1] = new ConversionInstruction(OpCode.Conv_ovf_i1);
			/* 0x0B4 */
			opcodeMap[(int)OpCode.Conv_ovf_u1] = new ConversionInstruction(OpCode.Conv_ovf_u1);
			/* 0x0B5 */
			opcodeMap[(int)OpCode.Conv_ovf_i2] = new ConversionInstruction(OpCode.Conv_ovf_i2);
			/* 0x0B6 */
			opcodeMap[(int)OpCode.Conv_ovf_u2] = new ConversionInstruction(OpCode.Conv_ovf_u2);
			/* 0x0B7 */
			opcodeMap[(int)OpCode.Conv_ovf_i4] = new ConversionInstruction(OpCode.Conv_ovf_i4);
			/* 0x0B8 */
			opcodeMap[(int)OpCode.Conv_ovf_u4] = new ConversionInstruction(OpCode.Conv_ovf_u4);
			/* 0x0B9 */
			opcodeMap[(int)OpCode.Conv_ovf_i8] = new ConversionInstruction(OpCode.Conv_ovf_i8);
			/* 0x0BA */
			opcodeMap[(int)OpCode.Conv_ovf_u8] = new ConversionInstruction(OpCode.Conv_ovf_u8);
			/* Opcodes 0x0BB-0x0C1 are undefined */
			/* 0x0C2 */
			opcodeMap[(int)OpCode.Refanyval] = new RefanyvalInstruction(OpCode.Refanyval);
			/* 0x0C3 */
			opcodeMap[(int)OpCode.Ckfinite] = new UnaryArithmeticInstruction(OpCode.Ckfinite);
			/* Opcodes 0x0C4-0x0C5 are undefined */
			/* 0x0C6 */
			opcodeMap[(int)OpCode.Mkrefany] = new MkrefanyInstruction(OpCode.Mkrefany);
			/* Opcodes 0x0C7-0x0CF are reserved */
			/* 0x0D0 */
			opcodeMap[(int)OpCode.Ldtoken] = new LdtokenInstruction(OpCode.Ldtoken);
			/* 0x0D1 */
			opcodeMap[(int)OpCode.Conv_u2] = new ConversionInstruction(OpCode.Conv_u2);
			/* 0x0D2 */
			opcodeMap[(int)OpCode.Conv_u1] = new ConversionInstruction(OpCode.Conv_u1);
			/* 0x0D3 */
			opcodeMap[(int)OpCode.Conv_i] = new ConversionInstruction(OpCode.Conv_i);
			/* 0x0D4 */
			opcodeMap[(int)OpCode.Conv_ovf_i] = new ConversionInstruction(OpCode.Conv_ovf_i);
			/* 0x0D5 */
			opcodeMap[(int)OpCode.Conv_ovf_u] = new ConversionInstruction(OpCode.Conv_ovf_u);
			/* 0x0D6 */
			opcodeMap[(int)OpCode.Add_ovf] = new ArithmeticOverflowInstruction(OpCode.Add_ovf);
			/* 0x0D7 */
			opcodeMap[(int)OpCode.Add_ovf_un] = new ArithmeticOverflowInstruction(OpCode.Add_ovf_un);
			/* 0x0D8 */
			opcodeMap[(int)OpCode.Mul_ovf] = new ArithmeticOverflowInstruction(OpCode.Mul_ovf);
			/* 0x0D9 */
			opcodeMap[(int)OpCode.Mul_ovf_un] = new ArithmeticOverflowInstruction(OpCode.Mul_ovf_un);
			/* 0x0DA */
			opcodeMap[(int)OpCode.Sub_ovf] = new ArithmeticOverflowInstruction(OpCode.Sub_ovf);
			/* 0x0DB */
			opcodeMap[(int)OpCode.Sub_ovf_un] = new ArithmeticOverflowInstruction(OpCode.Sub_ovf_un);
			/* 0x0DC */
			opcodeMap[(int)OpCode.Endfinally] = new EndFinallyInstruction(OpCode.Endfinally);
			/* 0x0DD */
			opcodeMap[(int)OpCode.Leave] = new LeaveInstruction(OpCode.Leave);
			/* 0x0DE */
			opcodeMap[(int)OpCode.Leave_s] = new LeaveInstruction(OpCode.Leave_s);
			/* 0x0DF */
			opcodeMap[(int)OpCode.Stind_i] = new StobjInstruction(OpCode.Stind_i);
			/* 0x0E0 */
			opcodeMap[(int)OpCode.Conv_u] = new ConversionInstruction(OpCode.Conv_u);
			/* Opcodes 0xE1-0xFF are reserved */
			/* 0x100 */
			opcodeMap[(int)OpCode.Arglist] = new ArglistInstruction(OpCode.Arglist);
			/* 0x101 */
			opcodeMap[(int)OpCode.Ceq] = new BinaryComparisonInstruction(OpCode.Ceq);
			/* 0x102 */
			opcodeMap[(int)OpCode.Cgt] = new BinaryComparisonInstruction(OpCode.Cgt);
			/* 0x103 */
			opcodeMap[(int)OpCode.Cgt_un] = new BinaryComparisonInstruction(OpCode.Cgt_un);
			/* 0x104 */
			opcodeMap[(int)OpCode.Clt] = new BinaryComparisonInstruction(OpCode.Clt);
			/* 0x105 */
			opcodeMap[(int)OpCode.Clt_un] = new BinaryComparisonInstruction(OpCode.Clt_un);
			/* 0x106 */
			opcodeMap[(int)OpCode.Ldftn] = new LdftnInstruction(OpCode.Ldftn);
			/* 0x107 */
			opcodeMap[(int)OpCode.Ldvirtftn] = new LdvirtftnInstruction(OpCode.Ldvirtftn);
			/* Opcode 0x108 is undefined. */
			/* 0x109 */
			opcodeMap[(int)OpCode.Ldarg] = new LdargInstruction(OpCode.Ldarg);
			/* 0x10A */
			opcodeMap[(int)OpCode.Ldarga] = new LdargaInstruction(OpCode.Ldarga);
			/* 0x10B */
			opcodeMap[(int)OpCode.Starg] = new StargInstruction(OpCode.Starg);
			/* 0x10C */
			opcodeMap[(int)OpCode.Ldloc] = new LdlocInstruction(OpCode.Ldloc);
			/* 0x10D */
			opcodeMap[(int)OpCode.Ldloca] = new LdlocaInstruction(OpCode.Ldloca);
			/* 0x10E */
			opcodeMap[(int)OpCode.Stloc] = new StlocInstruction(OpCode.Stloc);
			/* 0x10F */
			opcodeMap[(int)OpCode.Localalloc] = new LocalallocInstruction(OpCode.Localalloc);
			/* Opcode 0x110 is undefined */
			/* 0x111 */
			opcodeMap[(int)OpCode.Endfilter] = new EndFilterInstruction(OpCode.Endfilter);
			/* 0x112 */
			opcodeMap[(int)OpCode.PreUnaligned] = new UnalignedPrefixInstruction(OpCode.PreUnaligned);
			/* 0x113 */
			opcodeMap[(int)OpCode.PreVolatile] = new VolatilePrefixInstruction(OpCode.PreVolatile);
			/* 0x114 */
			opcodeMap[(int)OpCode.PreTail] = new TailPrefixInstruction(OpCode.PreTail);
			/* 0x115 */
			opcodeMap[(int)OpCode.InitObj] = new InitObjInstruction(OpCode.InitObj);
			/* 0x116 */
			opcodeMap[(int)OpCode.PreConstrained] = new ConstrainedPrefixInstruction(OpCode.PreConstrained);
			/* 0x117 */
			opcodeMap[(int)OpCode.Cpblk] = new CpblkInstruction(OpCode.Cpblk);
			/* 0x118 */
			opcodeMap[(int)OpCode.Initblk] = new InitblkInstruction(OpCode.Initblk);
			/* 0x119 */
			opcodeMap[(int)OpCode.PreNo] = new NoPrefixInstruction(OpCode.PreNo);
			/* 0x11A */
			opcodeMap[(int)OpCode.Rethrow] = new RethrowInstruction(OpCode.Rethrow);
			/* Opcode 0x11B is undefined */
			/* 0x11C */
			opcodeMap[(int)OpCode.Sizeof] = new SizeofInstruction(OpCode.Sizeof);
			/* 0x11D */
			opcodeMap[(int)OpCode.Refanytype] = new RefanytypeInstruction(OpCode.Refanytype);
			/* 0x11E */
			opcodeMap[(int)OpCode.PreReadOnly] = new ReadOnlyPrefixInstruction(OpCode.PreReadOnly);

			return opcodeMap;
		}
	}
}
