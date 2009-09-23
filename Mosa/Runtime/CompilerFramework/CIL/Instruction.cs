/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.CIL
{

	/// <summary>
	/// 
	/// </summary>
	public static class Instruction
	{
		#region Static data

		private static readonly Dictionary<OpCode, ICILInstruction> opCodeMap = new Dictionary<OpCode, ICILInstruction>{
            /* 0x000 */ { OpCode.Nop,               new NopInstruction(OpCode.Nop) },
            /* 0x001 */ { OpCode.Break,             new BreakInstruction(OpCode.Break) },
            /* 0x002 */ { OpCode.Ldarg_0,           new LdargInstruction(OpCode.Ldarg_0) },
			/* 0x003 */ { OpCode.Ldarg_1,           new LdargInstruction(OpCode.Ldarg_1) },
            /* 0x004 */ { OpCode.Ldarg_2,           new LdargInstruction(OpCode.Ldarg_2) },
            /* 0x005 */ { OpCode.Ldarg_3,           new LdargInstruction(OpCode.Ldarg_3) },
            /* 0x006 */ { OpCode.Ldloc_0,           new LdargInstruction(OpCode.Ldloc_0) },
            /* 0x007 */ { OpCode.Ldloc_1,           new LdargInstruction(OpCode.Ldloc_1) },
            /* 0x008 */ { OpCode.Ldloc_2,           new LdargInstruction(OpCode.Ldloc_2) },
            /* 0x009 */ { OpCode.Ldloc_3,           new LdargInstruction(OpCode.Ldloc_3) },
			/* 0x00A */ { OpCode.Stloc_0,           new StlocInstruction(OpCode.Stloc_0) },
            /* 0x00B */ { OpCode.Stloc_1,           new StlocInstruction(OpCode.Stloc_1) },
            /* 0x00C */ { OpCode.Stloc_2,           new StlocInstruction(OpCode.Stloc_2) },
            /* 0x00D */ { OpCode.Stloc_3,           new StlocInstruction(OpCode.Stloc_3) },
            /* 0x00E */ { OpCode.Ldarg_s,           new LdargInstruction(OpCode.Ldarg_s) },
            /* 0x00F */ { OpCode.Ldarga_s,          new LdargaInstruction(OpCode.Ldarga_s) },
            /* 0x010 */ { OpCode.Starg_s,           new StargInstruction(OpCode.Starg_s) },
            /* 0x011 */ { OpCode.Ldloc_s,           new LdlocInstruction(OpCode.Ldloc_s) },
            /* 0x012 */ { OpCode.Ldloca_s,          new LdlocaInstruction(OpCode.Ldloca_s) },
            /* 0x013 */ { OpCode.Stloc_s,           new StlocInstruction(OpCode.Stloc_s) },
            /* 0x014 */ { OpCode.Ldnull,            new LdcInstruction(OpCode.Ldnull) },
            /* 0x015 */ { OpCode.Ldc_i4_m1,         new LdcInstruction(OpCode.Ldc_i4_m1) },
            /* 0x016 */ { OpCode.Ldc_i4_0,          new LdcInstruction(OpCode.Ldc_i4_0) },
            /* 0x017 */ { OpCode.Ldc_i4_1,          new LdcInstruction(OpCode.Ldc_i4_1) },
            /* 0x018 */ { OpCode.Ldc_i4_2,          new LdcInstruction(OpCode.Ldc_i4_2) },
            /* 0x019 */ { OpCode.Ldc_i4_3,          new LdcInstruction(OpCode.Ldc_i4_3) },
            /* 0x01A */ { OpCode.Ldc_i4_4,          new LdcInstruction(OpCode.Ldc_i4_4) },
            /* 0x01B */ { OpCode.Ldc_i4_5,          new LdcInstruction(OpCode.Ldc_i4_5) },
            /* 0x01C */ { OpCode.Ldc_i4_6,          new LdcInstruction(OpCode.Ldc_i4_6) },
            /* 0x01D */ { OpCode.Ldc_i4_7,          new LdcInstruction(OpCode.Ldc_i4_7) },
            /* 0x01E */ { OpCode.Ldc_i4_8,          new LdcInstruction(OpCode.Ldc_i4_8) },
            /* 0x01F */ { OpCode.Ldc_i4_s,          new LdcInstruction(OpCode.Ldc_i4_s) },
            /* 0x020 */ { OpCode.Ldc_i4,            new LdcInstruction(OpCode.Ldc_i4) },
            /* 0x021 */ { OpCode.Ldc_i8,            new LdcInstruction(OpCode.Ldc_i8) },
            /* 0x022 */ { OpCode.Ldc_r4,            new LdcInstruction(OpCode.Ldc_r4) },
            /* 0x023 */ { OpCode.Ldc_r8,            new LdcInstruction(OpCode.Ldc_r8) },
            /* 0x024 is undefined */
            /* 0x025 */ { OpCode.Dup,               new DupInstruction(OpCode.Dup) },
            /* 0x026 */ { OpCode.Pop,               new PopInstruction(OpCode.Pop) }, 
            /* 0x027 */ { OpCode.Jmp,               new JumpInstruction(OpCode.Jmp) },
            /* 0x028 */ { OpCode.Call,              new CallInstruction(OpCode.Call) },
            /* 0x029 */ { OpCode.Calli,             new CalliInstruction(OpCode.Calli) },
            /* 0x02A */ { OpCode.Ret,               new ReturnInstruction(OpCode.Ret) },
            /* 0x02B */ { OpCode.Br_s,              new BranchInstruction(OpCode.Br_s) },
            /* 0x02C */ { OpCode.Brfalse_s, 		new UnaryBranchInstruction(OpCode.Brfalse_s) },
            /* 0x02D */ { OpCode.Brtrue_s, 			new UnaryBranchInstruction(OpCode.Brtrue_s) },
            /* 0x02E */ { OpCode.Beq_s, 			new BinaryBranchInstruction(OpCode.Beq_s) },
            /* 0x02F */ { OpCode.Bge_s, 			new BinaryBranchInstruction(OpCode.Bge_s) },
            /* 0x030 */ { OpCode.Bgt_s, 			new BinaryBranchInstruction(OpCode.Bgt_s) },
            /* 0x031 */ { OpCode.Ble_s, 			new BinaryBranchInstruction(OpCode.Ble_s) },
            /* 0x032 */ { OpCode.Blt_s, 			new BinaryBranchInstruction(OpCode.Blt_s) },
            /* 0x033 */ { OpCode.Bne_un_s, 			new BinaryBranchInstruction(OpCode.Bne_un_s) },
            /* 0x034 */ { OpCode.Bge_un_s, 			new BinaryBranchInstruction(OpCode.Bge_un_s) },
            /* 0x035 */ { OpCode.Bgt_un_s, 			new BinaryBranchInstruction(OpCode.Bgt_un_s) },
            /* 0x036 */ { OpCode.Ble_un_s, 			new BinaryBranchInstruction(OpCode.Ble_un_s) },
            /* 0x037 */ { OpCode.Blt_un_s, 			new BinaryBranchInstruction(OpCode.Blt_un_s) },
            /* 0x038 */ { OpCode.Br,                new BranchInstruction(OpCode.Br) },
            /* 0x039 */ { OpCode.Brfalse, 			new UnaryBranchInstruction(OpCode.Brfalse) },
            /* 0x03A */ { OpCode.Brtrue, 			new UnaryBranchInstruction(OpCode.Brtrue) },
            /* 0x03B */ { OpCode.Beq, 				new BinaryBranchInstruction(OpCode.Beq) },
            /* 0x03C */ { OpCode.Bge, 				new BinaryBranchInstruction(OpCode.Bge) },
            /* 0x03D */ { OpCode.Bgt, 				new BinaryBranchInstruction(OpCode.Bgt) },
            /* 0x03E */ { OpCode.Ble, 				new BinaryBranchInstruction(OpCode.Ble) },
            /* 0x03F */ { OpCode.Blt, 				new BinaryBranchInstruction(OpCode.Blt) },
            /* 0x040 */ { OpCode.Bne_un, 			new BinaryBranchInstruction(OpCode.Bne_un) },
            /* 0x041 */ { OpCode.Bge_un, 			new BinaryBranchInstruction(OpCode.Bge_un) },
            /* 0x042 */ { OpCode.Bgt_un, 			new BinaryBranchInstruction(OpCode.Bgt_un) },
            /* 0x043 */ { OpCode.Ble_un, 			new BinaryBranchInstruction(OpCode.Ble_un) },
            /* 0x044 */ { OpCode.Blt_un, 			new BinaryBranchInstruction(OpCode.Blt_un) },
            /* 0x045 */ { OpCode.Switch, 			new SwitchInstruction(OpCode.Switch) },
            /* 0x046 */ { OpCode.Ldind_i1,          new LdobjInstruction(OpCode.Ldind_i1) },
            /* 0x047 */ { OpCode.Ldind_u1,          new LdobjInstruction(OpCode.Ldind_i1) },
            /* 0x048 */ { OpCode.Ldind_i2,          new LdobjInstruction(OpCode.Ldind_i2) },
            /* 0x049 */ { OpCode.Ldind_u2,          new LdobjInstruction(OpCode.Ldind_u2) },
            /* 0x04A */ { OpCode.Ldind_i4,          new LdobjInstruction(OpCode.Ldind_i4) },
            /* 0x04B */ { OpCode.Ldind_u4,          new LdobjInstruction(OpCode.Ldind_u4) },
            /* 0x04C */ { OpCode.Ldind_i8,          new LdobjInstruction(OpCode.Ldind_i8) },
            /* 0x04D */ { OpCode.Ldind_i,           new LdobjInstruction(OpCode.Ldind_i) },
            /* 0x04E */ { OpCode.Ldind_r4,          new LdobjInstruction(OpCode.Ldind_r4) },
            /* 0x04F */ { OpCode.Ldind_r8,          new LdobjInstruction(OpCode.Ldind_r8) },
            /* 0x050 */ { OpCode.Ldind_ref,         new LdobjInstruction(OpCode.Ldind_ref) },
            /* 0x051 */ { OpCode.Stind_ref,         new StobjInstruction(OpCode.Stind_ref) },
            /* 0x052 */ { OpCode.Stind_i1,          new StobjInstruction(OpCode.Stind_i1) },
            /* 0x053 */ { OpCode.Stind_i2,          new StobjInstruction(OpCode.Stind_i2) },
            /* 0x054 */ { OpCode.Stind_i4,          new StobjInstruction(OpCode.Stind_i4) },
            /* 0x055 */ { OpCode.Stind_i8,          new StobjInstruction(OpCode.Stind_i8) },
            /* 0x056 */ { OpCode.Stind_r4,          new StobjInstruction(OpCode.Stind_r4) },
            /* 0x057 */ { OpCode.Stind_r8,          new StobjInstruction(OpCode.Stind_r8) },
            /* 0x058 */ { OpCode.Add,               new AddInstruction(OpCode.Add) },
            /* 0x059 */ { OpCode.Sub, 				new SubInstruction(OpCode.Sub) },
            /* 0x05A */ { OpCode.Mul, 				new MulInstruction(OpCode.Mul) },
            /* 0x05B */ { OpCode.Div,               new DivInstruction(OpCode.Div) },
            /* 0x05C */ { OpCode.Div_un,            new BinaryLogicInstruction(OpCode.Div_un) },
            /* 0x05D */ { OpCode.Rem,               new RemInstruction(OpCode.Rem) },
            /* 0x05E */ { OpCode.Rem_un,            new BinaryLogicInstruction(OpCode.Rem_un) },
            /* 0x05F */ { OpCode.And,               new BinaryLogicInstruction(OpCode.And) },
            /* 0x060 */ { OpCode.Or,                new BinaryLogicInstruction(OpCode.Or) },
            /* 0x061 */ { OpCode.Xor,               new BinaryLogicInstruction(OpCode.Xor) },
            /* 0x062 */ { OpCode.Shl,               new ShiftInstruction(OpCode.Shl) },
            /* 0x063 */ { OpCode.Shr,               new ShiftInstruction(OpCode.Shr) },
            /* 0x064 */ { OpCode.Shr_un,            new ShiftInstruction(OpCode.Shr_un) },
            /* 0x065 */ { OpCode.Neg, 				new NegInstruction(OpCode.Neg) },
            /* 0x066 */ { OpCode.Not,               new NotInstruction(OpCode.Not) },
            /* 0x067 */ { OpCode.Conv_i1, 			new ConversionInstruction(OpCode.Conv_i1) },
            /* 0x068 */ { OpCode.Conv_i2, 			new ConversionInstruction(OpCode.Conv_i2) },
            /* 0x069 */ { OpCode.Conv_i4, 			new ConversionInstruction(OpCode.Conv_i4) },
            /* 0x06A */ { OpCode.Conv_i8, 			new ConversionInstruction(OpCode.Conv_i8) },
            /* 0x06B */ { OpCode.Conv_r4, 			new ConversionInstruction(OpCode.Conv_r4) },
            /* 0x06C */ { OpCode.Conv_r8, 			new ConversionInstruction(OpCode.Conv_r8) },
            /* 0x06D */ { OpCode.Conv_u4, 			new ConversionInstruction(OpCode.Conv_u4) },
            /* 0x06E */ { OpCode.Conv_u8, 			new ConversionInstruction(OpCode.Conv_u8) },
            /* 0x06F */ { OpCode.Callvirt,          new CallvirtInstruction(OpCode.Callvirt) },
            /* 0x070 */ { OpCode.Cpobj,             new CpobjInstruction(OpCode.Cpobj) },
            /* 0x071 */ { OpCode.Ldobj,             new LdobjInstruction(OpCode.Ldobj) },
            /* 0x072 */ { OpCode.Ldstr,             new LdstrInstruction(OpCode.Ldstr) },
            /* 0x073 */ { OpCode.Newobj,            new NewobjInstruction(OpCode.Newobj) },
            /* 0x074 */ { OpCode.Castclass,         new CastclassInstruction(OpCode.Castclass) },
            /* 0x075 */ { OpCode.Isinst,            new IsInstInstruction(OpCode.Isinst) },
            /* 0x076 */ { OpCode.Conv_r_un, 		new ConversionInstruction(OpCode.Conv_r_un) },
            /* Opcodes 0x077-0x078 undefined */
            /* 0x079 */ { OpCode.Unbox,             new UnboxInstruction(OpCode.Unbox) },
            /* 0x07A */ { OpCode.Throw,             new ThrowInstruction(OpCode.Throw) },
            /* 0x07B */ { OpCode.Ldfld,             new LdfldInstruction(OpCode.Ldfld) },
            /* 0x07C */ { OpCode.Ldflda,            new LdfldaInstruction(OpCode.Ldflda) },
            /* 0x07D */ { OpCode.Stfld,             new StfldInstruction(OpCode.Stfld) },
            /* 0x07E */ { OpCode.Ldsfld,            new LdsfldInstruction(OpCode.Ldsfld) },
            /* 0x07F */ { OpCode.Ldsflda,           new LdsfldaInstruction(OpCode.Ldsflda) },
            /* 0x080 */ { OpCode.Stsfld,            new StsfldInstruction(OpCode.Stsfld) },
            /* 0x081 */ { OpCode.Stobj,             new StobjInstruction(OpCode.Stobj) },
            /* 0x082 */ { OpCode.Conv_ovf_i1_un, 	new ConversionInstruction(OpCode.Conv_ovf_i1_un) },
            /* 0x083 */ { OpCode.Conv_ovf_i2_un, 	new ConversionInstruction(OpCode.Conv_ovf_i2_un) },
            /* 0x084 */ { OpCode.Conv_ovf_i4_un, 	new ConversionInstruction(OpCode.Conv_ovf_i4_un) },
            /* 0x085 */ { OpCode.Conv_ovf_i8_un, 	new ConversionInstruction(OpCode.Conv_ovf_i8_un) },
            /* 0x086 */ { OpCode.Conv_ovf_u1_un, 	new ConversionInstruction(OpCode.Conv_ovf_u1_un) },
            /* 0x087 */ { OpCode.Conv_ovf_u2_un, 	new ConversionInstruction(OpCode.Conv_ovf_u2_un) },
            /* 0x088 */ { OpCode.Conv_ovf_u4_un, 	new ConversionInstruction(OpCode.Conv_ovf_u4_un) },
            /* 0x089 */ { OpCode.Conv_ovf_u8_un, 	new ConversionInstruction(OpCode.Conv_ovf_u8_un) },
            /* 0x08A */ { OpCode.Conv_ovf_i_un, 	new ConversionInstruction(OpCode.Conv_ovf_i_un) },
            /* 0x08B */ { OpCode.Conv_ovf_u_un, 	new ConversionInstruction(OpCode.Conv_ovf_u_un) },
            /* 0x08C */ { OpCode.Box,               new BoxInstruction(OpCode.Box) },
            /* 0x08D */ { OpCode.Newarr,            new NewarrInstruction(OpCode.Newarr) },
            /* 0x08E */ { OpCode.Ldlen,             new LdlenInstruction(OpCode.Ldlen) },
            /* 0x08F */ { OpCode.Ldelema,           new LdelemaInstruction(OpCode.Ldelema) },
            /* 0x090 */ { OpCode.Ldelem_i1,         new LdelemInstruction(OpCode.Ldelem_i1) },
            /* 0x091 */ { OpCode.Ldelem_u1,         new LdelemInstruction(OpCode.Ldelem_u1) },
            /* 0x092 */ { OpCode.Ldelem_i2,         new LdelemInstruction(OpCode.Ldelem_i2) },
            /* 0x093 */ { OpCode.Ldelem_u2,         new LdelemInstruction(OpCode.Ldelem_u2) },
            /* 0x094 */ { OpCode.Ldelem_i4,         new LdelemInstruction(OpCode.Ldelem_i4) },
            /* 0x095 */ { OpCode.Ldelem_u4,         new LdelemInstruction(OpCode.Ldelem_u4) },
            /* 0x096 */ { OpCode.Ldelem_i8,         new LdelemInstruction(OpCode.Ldelem_i8) },
            /* 0x097 */ { OpCode.Ldelem_i,          new LdelemInstruction(OpCode.Ldelem_i) },
            /* 0x098 */ { OpCode.Ldelem_r4,         new LdelemInstruction(OpCode.Ldelem_r4) },
            /* 0x099 */ { OpCode.Ldelem_r8,         new LdelemInstruction(OpCode.Ldelem_r8) },
            /* 0x09A */ { OpCode.Ldelem_ref,        new LdelemInstruction(OpCode.Ldelem_ref) },
            /* 0x09B */ { OpCode.Stelem_i,          new StelemInstruction(OpCode.Stelem_i) },
            /* 0x09C */ { OpCode.Stelem_i1,         new StelemInstruction(OpCode.Stelem_i1) },
            /* 0x09D */ { OpCode.Stelem_i2,         new StelemInstruction(OpCode.Stelem_i2) },
            /* 0x09E */ { OpCode.Stelem_i4,         new StelemInstruction(OpCode.Stelem_i4) },
            /* 0x09F */ { OpCode.Stelem_i8,         new StelemInstruction(OpCode.Stelem_i8) },
            /* 0x0A0 */ { OpCode.Stelem_r4,         new StelemInstruction(OpCode.Stelem_r4) },
            /* 0x0A1 */ { OpCode.Stelem_r8,         new StelemInstruction(OpCode.Stelem_r8) },
            /* 0x0A2 */ { OpCode.Stelem_ref,        new StelemInstruction(OpCode.Stelem_ref) },
            /* 0x0A3 */ { OpCode.Ldelem,            new LdelemInstruction(OpCode.Ldelem) },
            /* 0x0A4 */ { OpCode.Stelem,            new StelemInstruction(OpCode.Stelem) },
            /* 0x0A5 */ { OpCode.Unbox_any,         new UnboxAnyInstruction(OpCode.Unbox_any) },
            /* Opcodes 0x0A6-0x0B2 are undefined */
            /* 0x0B3 */ { OpCode.Conv_ovf_i1, 		new ConversionInstruction(OpCode.Conv_ovf_i1) },
            /* 0x0B4 */ { OpCode.Conv_ovf_u1, 		new ConversionInstruction(OpCode.Conv_ovf_u1) },
            /* 0x0B5 */ { OpCode.Conv_ovf_i2, 		new ConversionInstruction(OpCode.Conv_ovf_i2) },
            /* 0x0B6 */ { OpCode.Conv_ovf_u2, 		new ConversionInstruction(OpCode.Conv_ovf_u2) },
            /* 0x0B7 */ { OpCode.Conv_ovf_i4, 		new ConversionInstruction(OpCode.Conv_ovf_i4) },
            /* 0x0B8 */ { OpCode.Conv_ovf_u4, 		new ConversionInstruction(OpCode.Conv_ovf_u4) },
            /* 0x0B9 */ { OpCode.Conv_ovf_i8, 		new ConversionInstruction(OpCode.Conv_ovf_i8) },
            /* 0x0BA */ { OpCode.Conv_ovf_u8, 		new ConversionInstruction(OpCode.Conv_ovf_u8) },
            /* Opcodes 0x0BB-0x0C1 are undefined */
            /* 0x0C2 */ { OpCode.Refanyval,         new RefanyvalInstruction(OpCode.Refanyval) },
            /* 0x0C3 */ { OpCode.Ckfinite, 			new UnaryArithmeticInstruction(OpCode.Ckfinite) },
            /* Opcodes 0x0C4-0x0C5 are undefined */
            /* 0x0C6 */ { OpCode.Mkrefany,          new MkrefanyInstruction(OpCode.Mkrefany) },
            /* Opcodes 0x0C7-0x0CF are reserved */
            /* 0x0D0 */ { OpCode.Ldtoken,           new LdtokenInstruction(OpCode.Ldtoken) },
            /* 0x0D1 */ { OpCode.Conv_u2, 			new ConversionInstruction(OpCode.Conv_u2) },
            /* 0x0D2 */ { OpCode.Conv_u1, 			new ConversionInstruction(OpCode.Conv_u1) },
            /* 0x0D3 */ { OpCode.Conv_i, 			new ConversionInstruction(OpCode.Conv_i) },
            /* 0x0D4 */ { OpCode.Conv_ovf_i, 		new ConversionInstruction(OpCode.Conv_ovf_i) },
            /* 0x0D5 */ { OpCode.Conv_ovf_u, 		new ConversionInstruction(OpCode.Conv_ovf_u) },
            /* 0x0D6 */ { OpCode.Add_ovf,           new ArithmeticOverflowInstruction(OpCode.Add_ovf) },
            /* 0x0D7 */ { OpCode.Add_ovf_un,        new ArithmeticOverflowInstruction(OpCode.Add_ovf_un) },
            /* 0x0D8 */ { OpCode.Mul_ovf,           new ArithmeticOverflowInstruction(OpCode.Mul_ovf) },
            /* 0x0D9 */ { OpCode.Mul_ovf_un,        new ArithmeticOverflowInstruction(OpCode.Mul_ovf_un) },
            /* 0x0DA */ { OpCode.Sub_ovf,           new ArithmeticOverflowInstruction(OpCode.Sub_ovf) },
            /* 0x0DB */ { OpCode.Sub_ovf_un,        new ArithmeticOverflowInstruction(OpCode.Sub_ovf_un) },
            /* 0x0DC */ { OpCode.Endfinally,        new EndfinallyInstruction(OpCode.Endfinally) },
            /* 0x0DD */ { OpCode.Leave,             new LeaveInstruction(OpCode.Leave) },
            /* 0x0DE */ { OpCode.Leave_s,           new LeaveInstruction(OpCode.Leave_s) },
            /* 0x0DF */ { OpCode.Stind_i,           new StobjInstruction(OpCode.Stind_i) },
		    /* 0x0E0 */ { OpCode.Conv_u,            new ConversionInstruction(OpCode.Conv_u) },
            /* Opcodes 0xE1-0xFF are reserved */
            /* 0x100 */ { OpCode.Arglist,           new ArglistInstruction(OpCode.Arglist) },
            /* 0x101 */ { OpCode.Ceq,               new BinaryComparisonInstruction(OpCode.Ceq) },
            /* 0x102 */ { OpCode.Cgt,               new BinaryComparisonInstruction(OpCode.Cgt) },
            /* 0x103 */ { OpCode.Cgt_un,            new BinaryComparisonInstruction(OpCode.Cgt_un) },
            /* 0x104 */ { OpCode.Clt,               new BinaryComparisonInstruction(OpCode.Clt) },
            /* 0x105 */ { OpCode.Clt_un,            new BinaryComparisonInstruction(OpCode.Clt_un) },
            /* 0x106 */ { OpCode.Ldftn,             new LdftnInstruction(OpCode.Ldftn) },
            /* 0x107 */ { OpCode.Ldvirtftn,         new LdvirtftnInstruction(OpCode.Ldvirtftn) },
            /* Opcode 0x108 is undefined. */
            /* 0x109 */ { OpCode.Ldarg,             new LdargInstruction(OpCode.Ldarg) },
            /* 0x10A */ { OpCode.Ldarga,            new LdargaInstruction(OpCode.Ldarga) },
            /* 0x10B */ { OpCode.Starg,             new StargInstruction(OpCode.Starg) },
            /* 0x10C */ { OpCode.Ldloc,             new LdlocInstruction(OpCode.Ldloc) },
            /* 0x10D */ { OpCode.Ldloca,            new LdlocaInstruction(OpCode.Ldloca) },
            /* 0x10E */ { OpCode.Stloc,             new StlocInstruction(OpCode.Stloc) },
            /* 0x10F */ { OpCode.Localalloc,        new LocalallocInstruction(OpCode.Localalloc) },
            /* Opcode 0x110 is undefined */
            /* 0x111 */ { OpCode.Endfilter,         new EndfilterInstruction(OpCode.Endfilter) },
            /* 0x112 */ { OpCode.PreUnaligned,      new UnalignedPrefixInstruction(OpCode.PreUnaligned) },
            /* 0x113 */ { OpCode.PreVolatile,       new PrefixInstruction(OpCode.PreVolatile) },
            /* 0x114 */ { OpCode.PreTail,           new PrefixInstruction(OpCode.PreTail) },
            /* 0x115 */ { OpCode.InitObj,           new InitObjInstruction(OpCode.InitObj) },
            /* 0x116 */ { OpCode.PreConstrained,    new ConstrainedPrefixInstruction(OpCode.PreConstrained) },
            /* 0x117 */ { OpCode.Cpblk,             new CpblkInstruction(OpCode.Cpblk) },
            /* 0x118 */ { OpCode.Initblk,           new InitblkInstruction(OpCode.Initblk) },
            /* 0x119 */ { OpCode.PreNo,             new NoPrefixInstruction(OpCode.PreNo) },
            /* 0x11A */ { OpCode.Rethrow,           new RethrowInstruction(OpCode.Rethrow) },
            /* Opcode 0x11B is undefined */
            /* 0x11C */ { OpCode.Sizeof,            new SizeofInstruction(OpCode.Sizeof) },
            /* 0x11D */ { OpCode.Refanytype,        new RefanytypeInstruction(OpCode.Refanytype) },
            /* 0x11E */ { OpCode.PreReadOnly,       new PrefixInstruction(OpCode.PreReadOnly) }
		};

		#endregion // Static data

		/// <summary>
		/// Gets the instruction.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public static ICILInstruction Get(OpCode opcode)
		{
			return opCodeMap[opcode];
		}
	}
}
