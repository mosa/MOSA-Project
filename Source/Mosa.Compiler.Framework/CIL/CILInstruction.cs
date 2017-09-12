// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// CIL Instruction
	/// </summary>
	public static class CILInstruction
	{
		public const uint MaxOpCodeValue = 0x120;

		#region Static Data

		public static BaseCILInstruction Add = new AddInstruction(OpCode.Add);
		public static BaseCILInstruction Add_ovf = new ArithmeticOverflowInstruction(OpCode.Add_ovf);
		public static BaseCILInstruction Add_ovf_un = new ArithmeticOverflowInstruction(OpCode.Add_ovf_un);
		public static BaseCILInstruction And = new BinaryLogicInstruction(OpCode.And);
		public static BaseCILInstruction Arglist = new ArglistInstruction(OpCode.Arglist);
		public static BaseCILInstruction Beq = new BinaryBranchInstruction(OpCode.Beq);
		public static BaseCILInstruction Beq_s = new BinaryBranchInstruction(OpCode.Beq_s);
		public static BaseCILInstruction Bge = new BinaryBranchInstruction(OpCode.Bge);
		public static BaseCILInstruction Bge_s = new BinaryBranchInstruction(OpCode.Bge_s);
		public static BaseCILInstruction Bge_un = new BinaryBranchInstruction(OpCode.Bge_un);
		public static BaseCILInstruction Bge_un_s = new BinaryBranchInstruction(OpCode.Bge_un_s);
		public static BaseCILInstruction Bgt = new BinaryBranchInstruction(OpCode.Bgt);
		public static BaseCILInstruction Bgt_s = new BinaryBranchInstruction(OpCode.Bgt_s);
		public static BaseCILInstruction Bgt_un = new BinaryBranchInstruction(OpCode.Bgt_un);
		public static BaseCILInstruction Bgt_un_s = new BinaryBranchInstruction(OpCode.Bgt_un_s);
		public static BaseCILInstruction Ble = new BinaryBranchInstruction(OpCode.Ble);
		public static BaseCILInstruction Ble_s = new BinaryBranchInstruction(OpCode.Ble_s);
		public static BaseCILInstruction Ble_un = new BinaryBranchInstruction(OpCode.Ble_un);
		public static BaseCILInstruction Ble_un_s = new BinaryBranchInstruction(OpCode.Ble_un_s);
		public static BaseCILInstruction Blt = new BinaryBranchInstruction(OpCode.Blt);
		public static BaseCILInstruction Blt_s = new BinaryBranchInstruction(OpCode.Blt_s);
		public static BaseCILInstruction Blt_un = new BinaryBranchInstruction(OpCode.Blt_un);
		public static BaseCILInstruction Blt_un_s = new BinaryBranchInstruction(OpCode.Blt_un_s);
		public static BaseCILInstruction Bne_un = new BinaryBranchInstruction(OpCode.Bne_un);
		public static BaseCILInstruction Bne_un_s = new BinaryBranchInstruction(OpCode.Bne_un_s);
		public static BaseCILInstruction Box = new BoxInstruction(OpCode.Box);
		public static BaseCILInstruction Br = new BranchInstruction(OpCode.Br);
		public static BaseCILInstruction Br_s = new BranchInstruction(OpCode.Br_s);
		public static BaseCILInstruction Break = new BreakInstruction(OpCode.Break);
		public static BaseCILInstruction Brfalse = new UnaryBranchInstruction(OpCode.Brfalse);
		public static BaseCILInstruction Brfalse_s = new UnaryBranchInstruction(OpCode.Brfalse_s);
		public static BaseCILInstruction Brtrue = new UnaryBranchInstruction(OpCode.Brtrue);
		public static BaseCILInstruction Brtrue_s = new UnaryBranchInstruction(OpCode.Brtrue_s);
		public static BaseCILInstruction Call = new CallInstruction(OpCode.Call);
		public static BaseCILInstruction Calli = new CalliInstruction(OpCode.Calli);
		public static BaseCILInstruction Callvirt = new CallvirtInstruction(OpCode.Callvirt);
		public static BaseCILInstruction Castclass = new CastclassInstruction(OpCode.Castclass);
		public static BaseCILInstruction Ceq = new BinaryComparisonInstruction(OpCode.Ceq);
		public static BaseCILInstruction Cgt = new BinaryComparisonInstruction(OpCode.Cgt);
		public static BaseCILInstruction Cgt_un = new BinaryComparisonInstruction(OpCode.Cgt_un);
		public static BaseCILInstruction Ckfinite = new UnaryArithmeticInstruction(OpCode.Ckfinite);
		public static BaseCILInstruction Clt = new BinaryComparisonInstruction(OpCode.Clt);
		public static BaseCILInstruction Clt_un = new BinaryComparisonInstruction(OpCode.Clt_un);
		public static BaseCILInstruction Conv_i = new ConversionInstruction(OpCode.Conv_i);
		public static BaseCILInstruction Conv_i1 = new ConversionInstruction(OpCode.Conv_i1);
		public static BaseCILInstruction Conv_i2 = new ConversionInstruction(OpCode.Conv_i2);
		public static BaseCILInstruction Conv_i4 = new ConversionInstruction(OpCode.Conv_i4);
		public static BaseCILInstruction Conv_i8 = new ConversionInstruction(OpCode.Conv_i8);
		public static BaseCILInstruction Conv_ovf_i = new ConversionInstruction(OpCode.Conv_ovf_i);
		public static BaseCILInstruction Conv_ovf_i_un = new ConversionInstruction(OpCode.Conv_ovf_i_un);
		public static BaseCILInstruction Conv_ovf_i1 = new ConversionInstruction(OpCode.Conv_ovf_i1);
		public static BaseCILInstruction Conv_ovf_i1_un = new ConversionInstruction(OpCode.Conv_ovf_i1_un);
		public static BaseCILInstruction Conv_ovf_i2 = new ConversionInstruction(OpCode.Conv_ovf_i2);
		public static BaseCILInstruction Conv_ovf_i2_un = new ConversionInstruction(OpCode.Conv_ovf_i2_un);
		public static BaseCILInstruction Conv_ovf_i4 = new ConversionInstruction(OpCode.Conv_ovf_i4);
		public static BaseCILInstruction Conv_ovf_i4_un = new ConversionInstruction(OpCode.Conv_ovf_i4_un);
		public static BaseCILInstruction Conv_ovf_i8 = new ConversionInstruction(OpCode.Conv_ovf_i8);
		public static BaseCILInstruction Conv_ovf_i8_un = new ConversionInstruction(OpCode.Conv_ovf_i8_un);
		public static BaseCILInstruction Conv_ovf_u = new ConversionInstruction(OpCode.Conv_ovf_u);
		public static BaseCILInstruction Conv_ovf_u_un = new ConversionInstruction(OpCode.Conv_ovf_u_un);
		public static BaseCILInstruction Conv_ovf_u1 = new ConversionInstruction(OpCode.Conv_ovf_u1);
		public static BaseCILInstruction Conv_ovf_u1_un = new ConversionInstruction(OpCode.Conv_ovf_u1_un);
		public static BaseCILInstruction Conv_ovf_u2 = new ConversionInstruction(OpCode.Conv_ovf_u2);
		public static BaseCILInstruction Conv_ovf_u2_un = new ConversionInstruction(OpCode.Conv_ovf_u2_un);
		public static BaseCILInstruction Conv_ovf_u4 = new ConversionInstruction(OpCode.Conv_ovf_u4);
		public static BaseCILInstruction Conv_ovf_u4_un = new ConversionInstruction(OpCode.Conv_ovf_u4_un);
		public static BaseCILInstruction Conv_ovf_u8 = new ConversionInstruction(OpCode.Conv_ovf_u8);
		public static BaseCILInstruction Conv_ovf_u8_un = new ConversionInstruction(OpCode.Conv_ovf_u8_un);
		public static BaseCILInstruction Conv_r_un = new ConversionInstruction(OpCode.Conv_r_un);
		public static BaseCILInstruction Conv_r4 = new ConversionInstruction(OpCode.Conv_r4);
		public static BaseCILInstruction Conv_r8 = new ConversionInstruction(OpCode.Conv_r8);
		public static BaseCILInstruction Conv_u = new ConversionInstruction(OpCode.Conv_u);
		public static BaseCILInstruction Conv_u1 = new ConversionInstruction(OpCode.Conv_u1);
		public static BaseCILInstruction Conv_u2 = new ConversionInstruction(OpCode.Conv_u2);
		public static BaseCILInstruction Conv_u4 = new ConversionInstruction(OpCode.Conv_u4);
		public static BaseCILInstruction Conv_u8 = new ConversionInstruction(OpCode.Conv_u8);
		public static BaseCILInstruction Cpblk = new CpblkInstruction(OpCode.Cpblk);
		public static BaseCILInstruction Cpobj = new CpobjInstruction(OpCode.Cpobj);
		public static BaseCILInstruction Div = new DivInstruction(OpCode.Div);
		public static BaseCILInstruction Div_un = new BinaryLogicInstruction(OpCode.Div_un);
		public static BaseCILInstruction Dup = new DupInstruction(OpCode.Dup);
		public static BaseCILInstruction Endfilter = new EndFilterInstruction(OpCode.Endfilter);
		public static BaseCILInstruction Endfinally = new EndFinallyInstruction(OpCode.Endfinally);
		public static BaseCILInstruction Initblk = new InitblkInstruction(OpCode.Initblk);
		public static BaseCILInstruction InitObj = new InitObjInstruction(OpCode.InitObj);
		public static BaseCILInstruction Isinst = new IsInstInstruction(OpCode.Isinst);
		public static BaseCILInstruction Jmp = new JumpInstruction(OpCode.Jmp);
		public static BaseCILInstruction Ldarg = new LdargInstruction(OpCode.Ldarg);
		public static BaseCILInstruction Ldarg_0 = new LdargInstruction(OpCode.Ldarg_0);
		public static BaseCILInstruction Ldarg_1 = new LdargInstruction(OpCode.Ldarg_1);
		public static BaseCILInstruction Ldarg_2 = new LdargInstruction(OpCode.Ldarg_2);
		public static BaseCILInstruction Ldarg_3 = new LdargInstruction(OpCode.Ldarg_3);
		public static BaseCILInstruction Ldarg_s = new LdargInstruction(OpCode.Ldarg_s);
		public static BaseCILInstruction Ldarga = new LdargaInstruction(OpCode.Ldarga);
		public static BaseCILInstruction Ldarga_s = new LdargaInstruction(OpCode.Ldarga_s);
		public static BaseCILInstruction Ldc_i4 = new LdcInstruction(OpCode.Ldc_i4);
		public static BaseCILInstruction Ldc_i4_0 = new LdcInstruction(OpCode.Ldc_i4_0);
		public static BaseCILInstruction Ldc_i4_1 = new LdcInstruction(OpCode.Ldc_i4_1);
		public static BaseCILInstruction Ldc_i4_2 = new LdcInstruction(OpCode.Ldc_i4_2);
		public static BaseCILInstruction Ldc_i4_3 = new LdcInstruction(OpCode.Ldc_i4_3);
		public static BaseCILInstruction Ldc_i4_4 = new LdcInstruction(OpCode.Ldc_i4_4);
		public static BaseCILInstruction Ldc_i4_5 = new LdcInstruction(OpCode.Ldc_i4_5);
		public static BaseCILInstruction Ldc_i4_6 = new LdcInstruction(OpCode.Ldc_i4_6);
		public static BaseCILInstruction Ldc_i4_7 = new LdcInstruction(OpCode.Ldc_i4_7);
		public static BaseCILInstruction Ldc_i4_8 = new LdcInstruction(OpCode.Ldc_i4_8);
		public static BaseCILInstruction Ldc_i4_m1 = new LdcInstruction(OpCode.Ldc_i4_m1);
		public static BaseCILInstruction Ldc_i4_s = new LdcInstruction(OpCode.Ldc_i4_s);
		public static BaseCILInstruction Ldc_i8 = new LdcInstruction(OpCode.Ldc_i8);
		public static BaseCILInstruction Ldc_r4 = new LdcInstruction(OpCode.Ldc_r4);
		public static BaseCILInstruction Ldc_r8 = new LdcInstruction(OpCode.Ldc_r8);
		public static BaseCILInstruction Ldelem = new LdelemInstruction(OpCode.Ldelem);
		public static BaseCILInstruction Ldelem_i = new LdelemInstruction(OpCode.Ldelem_i);
		public static BaseCILInstruction Ldelem_i1 = new LdelemInstruction(OpCode.Ldelem_i1);
		public static BaseCILInstruction Ldelem_i2 = new LdelemInstruction(OpCode.Ldelem_i2);
		public static BaseCILInstruction Ldelem_i4 = new LdelemInstruction(OpCode.Ldelem_i4);
		public static BaseCILInstruction Ldelem_i8 = new LdelemInstruction(OpCode.Ldelem_i8);
		public static BaseCILInstruction Ldelem_r4 = new LdelemInstruction(OpCode.Ldelem_r4);
		public static BaseCILInstruction Ldelem_r8 = new LdelemInstruction(OpCode.Ldelem_r8);
		public static BaseCILInstruction Ldelem_ref = new LdelemInstruction(OpCode.Ldelem_ref);
		public static BaseCILInstruction Ldelem_u1 = new LdelemInstruction(OpCode.Ldelem_u1);
		public static BaseCILInstruction Ldelem_u2 = new LdelemInstruction(OpCode.Ldelem_u2);
		public static BaseCILInstruction Ldelem_u4 = new LdelemInstruction(OpCode.Ldelem_u4);
		public static BaseCILInstruction Ldelema = new LdelemaInstruction(OpCode.Ldelema);
		public static BaseCILInstruction Ldfld = new LdfldInstruction(OpCode.Ldfld);
		public static BaseCILInstruction Ldflda = new LdfldaInstruction(OpCode.Ldflda);
		public static BaseCILInstruction Ldftn = new LdftnInstruction(OpCode.Ldftn);
		public static BaseCILInstruction Ldind_i = new LdobjInstruction(OpCode.Ldind_i);
		public static BaseCILInstruction Ldind_i1 = new LdobjInstruction(OpCode.Ldind_i1);
		public static BaseCILInstruction Ldind_i2 = new LdobjInstruction(OpCode.Ldind_i2);
		public static BaseCILInstruction Ldind_i4 = new LdobjInstruction(OpCode.Ldind_i4);
		public static BaseCILInstruction Ldind_i8 = new LdobjInstruction(OpCode.Ldind_i8);
		public static BaseCILInstruction Ldind_r4 = new LdobjInstruction(OpCode.Ldind_r4);
		public static BaseCILInstruction Ldind_r8 = new LdobjInstruction(OpCode.Ldind_r8);
		public static BaseCILInstruction Ldind_ref = new LdobjInstruction(OpCode.Ldind_ref);
		public static BaseCILInstruction Ldind_u1 = new LdobjInstruction(OpCode.Ldind_u1);
		public static BaseCILInstruction Ldind_u2 = new LdobjInstruction(OpCode.Ldind_u2);
		public static BaseCILInstruction Ldind_u4 = new LdobjInstruction(OpCode.Ldind_u4);
		public static BaseCILInstruction Ldlen = new LdlenInstruction(OpCode.Ldlen);
		public static BaseCILInstruction Ldloc = new LdlocInstruction(OpCode.Ldloc);
		public static BaseCILInstruction Ldloc_0 = new LdlocInstruction(OpCode.Ldloc_0);
		public static BaseCILInstruction Ldloc_1 = new LdlocInstruction(OpCode.Ldloc_1);
		public static BaseCILInstruction Ldloc_2 = new LdlocInstruction(OpCode.Ldloc_2);
		public static BaseCILInstruction Ldloc_3 = new LdlocInstruction(OpCode.Ldloc_3);
		public static BaseCILInstruction Ldloc_s = new LdlocInstruction(OpCode.Ldloc_s);
		public static BaseCILInstruction Ldloca = new LdlocaInstruction(OpCode.Ldloca);
		public static BaseCILInstruction Ldloca_s = new LdlocaInstruction(OpCode.Ldloca_s);
		public static BaseCILInstruction Ldnull = new LdcInstruction(OpCode.Ldnull);
		public static BaseCILInstruction Ldobj = new LdobjInstruction(OpCode.Ldobj);
		public static BaseCILInstruction Ldsfld = new LdsfldInstruction(OpCode.Ldsfld);
		public static BaseCILInstruction Ldsflda = new LdsfldaInstruction(OpCode.Ldsflda);
		public static BaseCILInstruction Ldstr = new LdstrInstruction(OpCode.Ldstr);
		public static BaseCILInstruction Ldtoken = new LdtokenInstruction(OpCode.Ldtoken);
		public static BaseCILInstruction Ldvirtftn = new LdvirtftnInstruction(OpCode.Ldvirtftn);
		public static BaseCILInstruction Leave = new LeaveInstruction(OpCode.Leave);
		public static BaseCILInstruction Leave_s = new LeaveInstruction(OpCode.Leave_s);
		public static BaseCILInstruction Localalloc = new LocalallocInstruction(OpCode.Localalloc);
		public static BaseCILInstruction Mkrefany = new MkrefanyInstruction(OpCode.Mkrefany);
		public static BaseCILInstruction Mul = new MulInstruction(OpCode.Mul);
		public static BaseCILInstruction Mul_ovf = new ArithmeticOverflowInstruction(OpCode.Mul_ovf);
		public static BaseCILInstruction Mul_ovf_un = new ArithmeticOverflowInstruction(OpCode.Mul_ovf_un);
		public static BaseCILInstruction Neg = new NegInstruction(OpCode.Neg);
		public static BaseCILInstruction Newarr = new NewarrInstruction(OpCode.Newarr);
		public static BaseCILInstruction Newobj = new NewobjInstruction(OpCode.Newobj);
		public static BaseCILInstruction Nop = new NopInstruction(OpCode.Nop);
		public static BaseCILInstruction Not = new NotInstruction(OpCode.Not);
		public static BaseCILInstruction Or = new BinaryLogicInstruction(OpCode.Or);
		public static BaseCILInstruction Pop = new PopInstruction(OpCode.Pop);
		public static BaseCILInstruction PreConstrained = new ConstrainedPrefixInstruction(OpCode.PreConstrained);
		public static BaseCILInstruction PreNo = new NoPrefixInstruction(OpCode.PreNo);
		public static BaseCILInstruction PreReadOnly = new ReadOnlyPrefixInstruction(OpCode.PreReadOnly);
		public static BaseCILInstruction PreTail = new TailPrefixInstruction(OpCode.PreTail);
		public static BaseCILInstruction PreUnaligned = new UnalignedPrefixInstruction(OpCode.PreUnaligned);
		public static BaseCILInstruction PreVolatile = new VolatilePrefixInstruction(OpCode.PreVolatile);
		public static BaseCILInstruction Refanytype = new RefanytypeInstruction(OpCode.Refanytype);
		public static BaseCILInstruction Refanyval = new RefanyvalInstruction(OpCode.Refanyval);
		public static BaseCILInstruction Rem = new RemInstruction(OpCode.Rem);
		public static BaseCILInstruction Rem_un = new BinaryLogicInstruction(OpCode.Rem_un);
		public static BaseCILInstruction Ret = new ReturnInstruction(OpCode.Ret);
		public static BaseCILInstruction Rethrow = new RethrowInstruction(OpCode.Rethrow);
		public static BaseCILInstruction Shl = new ShiftInstruction(OpCode.Shl);
		public static BaseCILInstruction Shr = new ShiftInstruction(OpCode.Shr);
		public static BaseCILInstruction Shr_un = new ShiftInstruction(OpCode.Shr_un);
		public static BaseCILInstruction Sizeof = new SizeofInstruction(OpCode.Sizeof);
		public static BaseCILInstruction Starg = new StargInstruction(OpCode.Starg);
		public static BaseCILInstruction Starg_s = new StargInstruction(OpCode.Starg_s);
		public static BaseCILInstruction Stelem = new StelemInstruction(OpCode.Stelem);
		public static BaseCILInstruction Stelem_i = new StelemInstruction(OpCode.Stelem_i);
		public static BaseCILInstruction Stelem_i1 = new StelemInstruction(OpCode.Stelem_i1);
		public static BaseCILInstruction Stelem_i2 = new StelemInstruction(OpCode.Stelem_i2);
		public static BaseCILInstruction Stelem_i4 = new StelemInstruction(OpCode.Stelem_i4);
		public static BaseCILInstruction Stelem_i8 = new StelemInstruction(OpCode.Stelem_i8);
		public static BaseCILInstruction Stelem_r4 = new StelemInstruction(OpCode.Stelem_r4);
		public static BaseCILInstruction Stelem_r8 = new StelemInstruction(OpCode.Stelem_r8);
		public static BaseCILInstruction Stelem_ref = new StelemInstruction(OpCode.Stelem_ref);
		public static BaseCILInstruction Stfld = new StfldInstruction(OpCode.Stfld);
		public static BaseCILInstruction Stind_i = new StobjInstruction(OpCode.Stind_i);
		public static BaseCILInstruction Stind_i1 = new StobjInstruction(OpCode.Stind_i1);
		public static BaseCILInstruction Stind_i2 = new StobjInstruction(OpCode.Stind_i2);
		public static BaseCILInstruction Stind_i4 = new StobjInstruction(OpCode.Stind_i4);
		public static BaseCILInstruction Stind_i8 = new StobjInstruction(OpCode.Stind_i8);
		public static BaseCILInstruction Stind_r4 = new StobjInstruction(OpCode.Stind_r4);
		public static BaseCILInstruction Stind_r8 = new StobjInstruction(OpCode.Stind_r8);
		public static BaseCILInstruction Stind_ref = new StobjInstruction(OpCode.Stind_ref);
		public static BaseCILInstruction Stloc = new StlocInstruction(OpCode.Stloc);
		public static BaseCILInstruction Stloc_0 = new StlocInstruction(OpCode.Stloc_0);
		public static BaseCILInstruction Stloc_1 = new StlocInstruction(OpCode.Stloc_1);
		public static BaseCILInstruction Stloc_2 = new StlocInstruction(OpCode.Stloc_2);
		public static BaseCILInstruction Stloc_3 = new StlocInstruction(OpCode.Stloc_3);
		public static BaseCILInstruction Stloc_s = new StlocInstruction(OpCode.Stloc_s);
		public static BaseCILInstruction Stobj = new StobjInstruction(OpCode.Stobj);
		public static BaseCILInstruction Stsfld = new StsfldInstruction(OpCode.Stsfld);
		public static BaseCILInstruction Sub = new SubInstruction(OpCode.Sub);
		public static BaseCILInstruction Sub_ovf = new ArithmeticOverflowInstruction(OpCode.Sub_ovf);
		public static BaseCILInstruction Sub_ovf_un = new ArithmeticOverflowInstruction(OpCode.Sub_ovf_un);
		public static BaseCILInstruction Switch = new SwitchInstruction(OpCode.Switch);
		public static BaseCILInstruction Throw = new ThrowInstruction(OpCode.Throw);
		public static BaseCILInstruction Unbox = new UnboxInstruction(OpCode.Unbox);
		public static BaseCILInstruction Unbox_any = new UnboxAnyInstruction(OpCode.Unbox_any);
		public static BaseCILInstruction Xor = new BinaryLogicInstruction(OpCode.Xor);

		/// <summary>
		/// Gets the instructions.
		/// </summary>
		public static BaseCILInstruction[] Instructions { get; } = Initialize();

		#endregion Static Data

		/// <summary>
		/// Gets the instruction.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public static BaseCILInstruction Get(OpCode opcode)
		{
			return CILInstruction.Instructions[(int)opcode];
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		/// <returns></returns>
		public static BaseCILInstruction[] Initialize()
		{
			BaseCILInstruction[] opcodeMap = new BaseCILInstruction[MaxOpCodeValue];

			/* 0x000 */
			opcodeMap[(int)OpCode.Nop] = Nop;
			/* 0x001 */
			opcodeMap[(int)OpCode.Break] = Break;
			/* 0x002 */
			opcodeMap[(int)OpCode.Ldarg_0] = Ldarg_0;
			/* 0x003 */
			opcodeMap[(int)OpCode.Ldarg_1] = Ldarg_1;
			/* 0x004 */
			opcodeMap[(int)OpCode.Ldarg_2] = Ldarg_2;
			/* 0x005 */
			opcodeMap[(int)OpCode.Ldarg_3] = Ldarg_3;
			/* 0x006 */
			opcodeMap[(int)OpCode.Ldloc_0] = Ldloc_0;
			/* 0x007 */
			opcodeMap[(int)OpCode.Ldloc_1] = Ldloc_1;
			/* 0x008 */
			opcodeMap[(int)OpCode.Ldloc_2] = Ldloc_2;
			/* 0x009 */
			opcodeMap[(int)OpCode.Ldloc_3] = Ldloc_3;
			/* 0x00A */
			opcodeMap[(int)OpCode.Stloc_0] = Stloc_0;
			/* 0x00B */
			opcodeMap[(int)OpCode.Stloc_1] = Stloc_1;
			/* 0x00C */
			opcodeMap[(int)OpCode.Stloc_2] = Stloc_2;
			/* 0x00D */
			opcodeMap[(int)OpCode.Stloc_3] = Stloc_3;
			/* 0x00E */
			opcodeMap[(int)OpCode.Ldarg_s] = Ldarg_s;
			/* 0x00F */
			opcodeMap[(int)OpCode.Ldarga_s] = Ldarga_s;
			/* 0x010 */
			opcodeMap[(int)OpCode.Starg_s] = Starg_s;
			/* 0x011 */
			opcodeMap[(int)OpCode.Ldloc_s] = Ldloc_s;
			/* 0x012 */
			opcodeMap[(int)OpCode.Ldloca_s] = Ldloca_s;
			/* 0x013 */
			opcodeMap[(int)OpCode.Stloc_s] = Stloc_s;
			/* 0x014 */
			opcodeMap[(int)OpCode.Ldnull] = Ldnull;
			/* 0x015 */
			opcodeMap[(int)OpCode.Ldc_i4_m1] = Ldc_i4_m1;
			/* 0x016 */
			opcodeMap[(int)OpCode.Ldc_i4_0] = Ldc_i4_0;
			/* 0x017 */
			opcodeMap[(int)OpCode.Ldc_i4_1] = Ldc_i4_1;
			/* 0x018 */
			opcodeMap[(int)OpCode.Ldc_i4_2] = Ldc_i4_2;
			/* 0x019 */
			opcodeMap[(int)OpCode.Ldc_i4_3] = Ldc_i4_3;
			/* 0x01A */
			opcodeMap[(int)OpCode.Ldc_i4_4] = Ldc_i4_4;
			/* 0x01B */
			opcodeMap[(int)OpCode.Ldc_i4_5] = Ldc_i4_5;
			/* 0x01C */
			opcodeMap[(int)OpCode.Ldc_i4_6] = Ldc_i4_6;
			/* 0x01D */
			opcodeMap[(int)OpCode.Ldc_i4_7] = Ldc_i4_7;
			/* 0x01E */
			opcodeMap[(int)OpCode.Ldc_i4_8] = Ldc_i4_8;
			/* 0x01F */
			opcodeMap[(int)OpCode.Ldc_i4_s] = Ldc_i4_s;
			/* 0x020 */
			opcodeMap[(int)OpCode.Ldc_i4] = Ldc_i4;
			/* 0x021 */
			opcodeMap[(int)OpCode.Ldc_i8] = Ldc_i8;
			/* 0x022 */
			opcodeMap[(int)OpCode.Ldc_r4] = Ldc_r4;
			/* 0x023 */
			opcodeMap[(int)OpCode.Ldc_r8] = Ldc_r8;
			/* 0x024 is undefined */
			/* 0x025 */
			opcodeMap[(int)OpCode.Dup] = Dup;
			/* 0x026 */
			opcodeMap[(int)OpCode.Pop] = Pop;
			/* 0x027 */
			opcodeMap[(int)OpCode.Jmp] = Jmp;
			/* 0x028 */
			opcodeMap[(int)OpCode.Call] = Call;
			/* 0x029 */
			opcodeMap[(int)OpCode.Calli] = Calli;
			/* 0x02A */
			opcodeMap[(int)OpCode.Ret] = Ret;
			/* 0x02B */
			opcodeMap[(int)OpCode.Br_s] = Br_s;
			/* 0x02C */
			opcodeMap[(int)OpCode.Brfalse_s] = Brfalse_s;
			/* 0x02D */
			opcodeMap[(int)OpCode.Brtrue_s] = Brtrue_s;
			/* 0x02E */
			opcodeMap[(int)OpCode.Beq_s] = Beq_s;
			/* 0x02F */
			opcodeMap[(int)OpCode.Bge_s] = Bge_s;
			/* 0x030 */
			opcodeMap[(int)OpCode.Bgt_s] = Bgt_s;
			/* 0x031 */
			opcodeMap[(int)OpCode.Ble_s] = Ble_s;
			/* 0x032 */
			opcodeMap[(int)OpCode.Blt_s] = Blt_s;
			/* 0x033 */
			opcodeMap[(int)OpCode.Bne_un_s] = Bne_un_s;
			/* 0x034 */
			opcodeMap[(int)OpCode.Bge_un_s] = Bge_un_s;
			/* 0x035 */
			opcodeMap[(int)OpCode.Bgt_un_s] = Bgt_un_s;
			/* 0x036 */
			opcodeMap[(int)OpCode.Ble_un_s] = Ble_un_s;
			/* 0x037 */
			opcodeMap[(int)OpCode.Blt_un_s] = Blt_un_s;
			/* 0x038 */
			opcodeMap[(int)OpCode.Br] = Br;
			/* 0x039 */
			opcodeMap[(int)OpCode.Brfalse] = Brfalse;
			/* 0x03A */
			opcodeMap[(int)OpCode.Brtrue] = Brtrue;
			/* 0x03B */
			opcodeMap[(int)OpCode.Beq] = Beq;
			/* 0x03C */
			opcodeMap[(int)OpCode.Bge] = Bge;
			/* 0x03D */
			opcodeMap[(int)OpCode.Bgt] = Bgt;
			/* 0x03E */
			opcodeMap[(int)OpCode.Ble] = Ble;
			/* 0x03F */
			opcodeMap[(int)OpCode.Blt] = Blt;
			/* 0x040 */
			opcodeMap[(int)OpCode.Bne_un] = Bne_un;
			/* 0x041 */
			opcodeMap[(int)OpCode.Bge_un] = Bge_un;
			/* 0x042 */
			opcodeMap[(int)OpCode.Bgt_un] = Bgt_un;
			/* 0x043 */
			opcodeMap[(int)OpCode.Ble_un] = Ble_un;
			/* 0x044 */
			opcodeMap[(int)OpCode.Blt_un] = Blt_un;
			/* 0x045 */
			opcodeMap[(int)OpCode.Switch] = Switch;
			/* 0x046 */
			opcodeMap[(int)OpCode.Ldind_i1] = Ldind_i1;
			/* 0x047 */
			opcodeMap[(int)OpCode.Ldind_u1] = Ldind_u1;
			/* 0x048 */
			opcodeMap[(int)OpCode.Ldind_i2] = Ldind_i2;
			/* 0x049 */
			opcodeMap[(int)OpCode.Ldind_u2] = Ldind_u2;
			/* 0x04A */
			opcodeMap[(int)OpCode.Ldind_i4] = Ldind_i4;
			/* 0x04B */
			opcodeMap[(int)OpCode.Ldind_u4] = Ldind_u4;
			/* 0x04C */
			opcodeMap[(int)OpCode.Ldind_i8] = Ldind_i8;
			/* 0x04D */
			opcodeMap[(int)OpCode.Ldind_i] = Ldind_i;
			/* 0x04E */
			opcodeMap[(int)OpCode.Ldind_r4] = Ldind_r4;
			/* 0x04F */
			opcodeMap[(int)OpCode.Ldind_r8] = Ldind_r8;
			/* 0x050 */
			opcodeMap[(int)OpCode.Ldind_ref] = Ldind_ref;
			/* 0x051 */
			opcodeMap[(int)OpCode.Stind_ref] = Stind_ref;
			/* 0x052 */
			opcodeMap[(int)OpCode.Stind_i1] = Stind_i1;
			/* 0x053 */
			opcodeMap[(int)OpCode.Stind_i2] = Stind_i2;
			/* 0x054 */
			opcodeMap[(int)OpCode.Stind_i4] = Stind_i4;
			/* 0x055 */
			opcodeMap[(int)OpCode.Stind_i8] = Stind_i8;
			/* 0x056 */
			opcodeMap[(int)OpCode.Stind_r4] = Stind_r4;
			/* 0x057 */
			opcodeMap[(int)OpCode.Stind_r8] = Stind_r8;
			/* 0x058 */
			opcodeMap[(int)OpCode.Add] = Add;
			/* 0x059 */
			opcodeMap[(int)OpCode.Sub] = Sub;
			/* 0x05A */
			opcodeMap[(int)OpCode.Mul] = Mul;
			/* 0x05B */
			opcodeMap[(int)OpCode.Div] = Div;
			/* 0x05C */
			opcodeMap[(int)OpCode.Div_un] = Div_un;
			/* 0x05D */
			opcodeMap[(int)OpCode.Rem] = Rem;
			/* 0x05E */
			opcodeMap[(int)OpCode.Rem_un] = Rem_un;
			/* 0x05F */
			opcodeMap[(int)OpCode.And] = And;
			/* 0x060 */
			opcodeMap[(int)OpCode.Or] = Or;
			/* 0x061 */
			opcodeMap[(int)OpCode.Xor] = Xor;
			/* 0x062 */
			opcodeMap[(int)OpCode.Shl] = Shl;
			/* 0x063 */
			opcodeMap[(int)OpCode.Shr] = Shr;
			/* 0x064 */
			opcodeMap[(int)OpCode.Shr_un] = Shr_un;
			/* 0x065 */
			opcodeMap[(int)OpCode.Neg] = Neg;
			/* 0x066 */
			opcodeMap[(int)OpCode.Not] = Not;
			/* 0x067 */
			opcodeMap[(int)OpCode.Conv_i1] = Conv_i1;
			/* 0x068 */
			opcodeMap[(int)OpCode.Conv_i2] = Conv_i2;
			/* 0x069 */
			opcodeMap[(int)OpCode.Conv_i4] = Conv_i4;
			/* 0x06A */
			opcodeMap[(int)OpCode.Conv_i8] = Conv_i8;
			/* 0x06B */
			opcodeMap[(int)OpCode.Conv_r4] = Conv_r4;
			/* 0x06C */
			opcodeMap[(int)OpCode.Conv_r8] = Conv_r8;
			/* 0x06D */
			opcodeMap[(int)OpCode.Conv_u4] = Conv_u4;
			/* 0x06E */
			opcodeMap[(int)OpCode.Conv_u8] = Conv_u8;
			/* 0x06F */
			opcodeMap[(int)OpCode.Callvirt] = Callvirt;
			/* 0x070 */
			opcodeMap[(int)OpCode.Cpobj] = Cpobj;
			/* 0x071 */
			opcodeMap[(int)OpCode.Ldobj] = Ldobj;
			/* 0x072 */
			opcodeMap[(int)OpCode.Ldstr] = Ldstr;
			/* 0x073 */
			opcodeMap[(int)OpCode.Newobj] = Newobj;
			/* 0x074 */
			opcodeMap[(int)OpCode.Castclass] = Castclass;
			/* 0x075 */
			opcodeMap[(int)OpCode.Isinst] = Isinst;
			/* 0x076 */
			opcodeMap[(int)OpCode.Conv_r_un] = Conv_r_un;
			/* Opcodes 0x077-0x078 undefined */
			/* 0x079 */
			opcodeMap[(int)OpCode.Unbox] = Unbox;
			/* 0x07A */
			opcodeMap[(int)OpCode.Throw] = Throw;
			/* 0x07B */
			opcodeMap[(int)OpCode.Ldfld] = Ldfld;
			/* 0x07C */
			opcodeMap[(int)OpCode.Ldflda] = Ldflda;
			/* 0x07D */
			opcodeMap[(int)OpCode.Stfld] = Stfld;
			/* 0x07E */
			opcodeMap[(int)OpCode.Ldsfld] = Ldsfld;
			/* 0x07F */
			opcodeMap[(int)OpCode.Ldsflda] = Ldsflda;
			/* 0x080 */
			opcodeMap[(int)OpCode.Stsfld] = Stsfld;
			/* 0x081 */
			opcodeMap[(int)OpCode.Stobj] = Stobj;
			/* 0x082 */
			opcodeMap[(int)OpCode.Conv_ovf_i1_un] = Conv_ovf_i1_un;
			/* 0x083 */
			opcodeMap[(int)OpCode.Conv_ovf_i2_un] = Conv_ovf_i2_un;
			/* 0x084 */
			opcodeMap[(int)OpCode.Conv_ovf_i4_un] = Conv_ovf_i4_un;
			/* 0x085 */
			opcodeMap[(int)OpCode.Conv_ovf_i8_un] = Conv_ovf_i8_un;
			/* 0x086 */
			opcodeMap[(int)OpCode.Conv_ovf_u1_un] = Conv_ovf_u1_un;
			/* 0x087 */
			opcodeMap[(int)OpCode.Conv_ovf_u2_un] = Conv_ovf_u2_un;
			/* 0x088 */
			opcodeMap[(int)OpCode.Conv_ovf_u4_un] = Conv_ovf_u4_un;
			/* 0x089 */
			opcodeMap[(int)OpCode.Conv_ovf_u8_un] = Conv_ovf_u8_un;
			/* 0x08A */
			opcodeMap[(int)OpCode.Conv_ovf_i_un] = Conv_ovf_i_un;
			/* 0x08B */
			opcodeMap[(int)OpCode.Conv_ovf_u_un] = Conv_ovf_u_un;
			/* 0x08C */
			opcodeMap[(int)OpCode.Box] = Box;
			/* 0x08D */
			opcodeMap[(int)OpCode.Newarr] = Newarr;
			/* 0x08E */
			opcodeMap[(int)OpCode.Ldlen] = Ldlen;
			/* 0x08F */
			opcodeMap[(int)OpCode.Ldelema] = Ldelema;
			/* 0x090 */
			opcodeMap[(int)OpCode.Ldelem_i1] = Ldelem_i1;
			/* 0x091 */
			opcodeMap[(int)OpCode.Ldelem_u1] = Ldelem_u1;
			/* 0x092 */
			opcodeMap[(int)OpCode.Ldelem_i2] = Ldelem_i2;
			/* 0x093 */
			opcodeMap[(int)OpCode.Ldelem_u2] = Ldelem_u2;
			/* 0x094 */
			opcodeMap[(int)OpCode.Ldelem_i4] = Ldelem_i4;
			/* 0x095 */
			opcodeMap[(int)OpCode.Ldelem_u4] = Ldelem_u4;
			/* 0x096 */
			opcodeMap[(int)OpCode.Ldelem_i8] = Ldelem_i8;
			/* 0x097 */
			opcodeMap[(int)OpCode.Ldelem_i] = Ldelem_i;
			/* 0x098 */
			opcodeMap[(int)OpCode.Ldelem_r4] = Ldelem_r4;
			/* 0x099 */
			opcodeMap[(int)OpCode.Ldelem_r8] = Ldelem_r8;
			/* 0x09A */
			opcodeMap[(int)OpCode.Ldelem_ref] = Ldelem_ref;
			/* 0x09B */
			opcodeMap[(int)OpCode.Stelem_i] = Stelem_i;
			/* 0x09C */
			opcodeMap[(int)OpCode.Stelem_i1] = Stelem_i1;
			/* 0x09D */
			opcodeMap[(int)OpCode.Stelem_i2] = Stelem_i2;
			/* 0x09E */
			opcodeMap[(int)OpCode.Stelem_i4] = Stelem_i4;
			/* 0x09F */
			opcodeMap[(int)OpCode.Stelem_i8] = Stelem_i8;
			/* 0x0A0 */
			opcodeMap[(int)OpCode.Stelem_r4] = Stelem_r4;
			/* 0x0A1 */
			opcodeMap[(int)OpCode.Stelem_r8] = Stelem_r8;
			/* 0x0A2 */
			opcodeMap[(int)OpCode.Stelem_ref] = Stelem_ref;
			/* 0x0A3 */
			opcodeMap[(int)OpCode.Ldelem] = Ldelem;
			/* 0x0A4 */
			opcodeMap[(int)OpCode.Stelem] = Stelem;
			/* 0x0A5 */
			opcodeMap[(int)OpCode.Unbox_any] = Unbox_any;
			/* Opcodes 0x0A6-0x0B2 are undefined */
			/* 0x0B3 */
			opcodeMap[(int)OpCode.Conv_ovf_i1] = Conv_ovf_i1;
			/* 0x0B4 */
			opcodeMap[(int)OpCode.Conv_ovf_u1] = Conv_ovf_u1;
			/* 0x0B5 */
			opcodeMap[(int)OpCode.Conv_ovf_i2] = Conv_ovf_i2;
			/* 0x0B6 */
			opcodeMap[(int)OpCode.Conv_ovf_u2] = Conv_ovf_u2;
			/* 0x0B7 */
			opcodeMap[(int)OpCode.Conv_ovf_i4] = Conv_ovf_i4;
			/* 0x0B8 */
			opcodeMap[(int)OpCode.Conv_ovf_u4] = Conv_ovf_u4;
			/* 0x0B9 */
			opcodeMap[(int)OpCode.Conv_ovf_i8] = Conv_ovf_i8;
			/* 0x0BA */
			opcodeMap[(int)OpCode.Conv_ovf_u8] = Conv_ovf_u8;
			/* Opcodes 0x0BB-0x0C1 are undefined */
			/* 0x0C2 */
			opcodeMap[(int)OpCode.Refanyval] = Refanyval;
			/* 0x0C3 */
			opcodeMap[(int)OpCode.Ckfinite] = Ckfinite;
			/* Opcodes 0x0C4-0x0C5 are undefined */
			/* 0x0C6 */
			opcodeMap[(int)OpCode.Mkrefany] = Mkrefany;
			/* Opcodes 0x0C7-0x0CF are reserved */
			/* 0x0D0 */
			opcodeMap[(int)OpCode.Ldtoken] = Ldtoken;
			/* 0x0D1 */
			opcodeMap[(int)OpCode.Conv_u2] = Conv_u2;
			/* 0x0D2 */
			opcodeMap[(int)OpCode.Conv_u1] = Conv_u1;
			/* 0x0D3 */
			opcodeMap[(int)OpCode.Conv_i] = Conv_i;
			/* 0x0D4 */
			opcodeMap[(int)OpCode.Conv_ovf_i] = Conv_ovf_i;
			/* 0x0D5 */
			opcodeMap[(int)OpCode.Conv_ovf_u] = Conv_ovf_u;
			/* 0x0D6 */
			opcodeMap[(int)OpCode.Add_ovf] = Add_ovf;
			/* 0x0D7 */
			opcodeMap[(int)OpCode.Add_ovf_un] = Add_ovf_un;
			/* 0x0D8 */
			opcodeMap[(int)OpCode.Mul_ovf] = Mul_ovf;
			/* 0x0D9 */
			opcodeMap[(int)OpCode.Mul_ovf_un] = Mul_ovf_un;
			/* 0x0DA */
			opcodeMap[(int)OpCode.Sub_ovf] = Sub_ovf;
			/* 0x0DB */
			opcodeMap[(int)OpCode.Sub_ovf_un] = Sub_ovf_un;
			/* 0x0DC */
			opcodeMap[(int)OpCode.Endfinally] = Endfinally;
			/* 0x0DD */
			opcodeMap[(int)OpCode.Leave] = Leave;
			/* 0x0DE */
			opcodeMap[(int)OpCode.Leave_s] = Leave_s;
			/* 0x0DF */
			opcodeMap[(int)OpCode.Stind_i] = Stind_i;
			/* 0x0E0 */
			opcodeMap[(int)OpCode.Conv_u] = Conv_u;
			/* Opcodes 0xE1-0xFF are reserved */
			/* 0x100 */
			opcodeMap[(int)OpCode.Arglist] = Arglist;
			/* 0x101 */
			opcodeMap[(int)OpCode.Ceq] = Ceq;
			/* 0x102 */
			opcodeMap[(int)OpCode.Cgt] = Cgt;
			/* 0x103 */
			opcodeMap[(int)OpCode.Cgt_un] = Cgt_un;
			/* 0x104 */
			opcodeMap[(int)OpCode.Clt] = Clt;
			/* 0x105 */
			opcodeMap[(int)OpCode.Clt_un] = Clt_un;
			/* 0x106 */
			opcodeMap[(int)OpCode.Ldftn] = Ldftn;
			/* 0x107 */
			opcodeMap[(int)OpCode.Ldvirtftn] = Ldvirtftn;
			/* Opcode 0x108 is undefined. */
			/* 0x109 */
			opcodeMap[(int)OpCode.Ldarg] = Ldarg;
			/* 0x10A */
			opcodeMap[(int)OpCode.Ldarga] = Ldarga;
			/* 0x10B */
			opcodeMap[(int)OpCode.Starg] = Starg;
			/* 0x10C */
			opcodeMap[(int)OpCode.Ldloc] = Ldloc;
			/* 0x10D */
			opcodeMap[(int)OpCode.Ldloca] = Ldloca;
			/* 0x10E */
			opcodeMap[(int)OpCode.Stloc] = Stloc;
			/* 0x10F */
			opcodeMap[(int)OpCode.Localalloc] = Localalloc;
			/* Opcode 0x110 is undefined */
			/* 0x111 */
			opcodeMap[(int)OpCode.Endfilter] = Endfilter;
			/* 0x112 */
			opcodeMap[(int)OpCode.PreUnaligned] = PreUnaligned;
			/* 0x113 */
			opcodeMap[(int)OpCode.PreVolatile] = PreVolatile;
			/* 0x114 */
			opcodeMap[(int)OpCode.PreTail] = PreTail;
			/* 0x115 */
			opcodeMap[(int)OpCode.InitObj] = InitObj;
			/* 0x116 */
			opcodeMap[(int)OpCode.PreConstrained] = PreConstrained;
			/* 0x117 */
			opcodeMap[(int)OpCode.Cpblk] = Cpblk;
			/* 0x118 */
			opcodeMap[(int)OpCode.Initblk] = Initblk;
			/* 0x119 */
			opcodeMap[(int)OpCode.PreNo] = PreNo;
			/* 0x11A */
			opcodeMap[(int)OpCode.Rethrow] = Rethrow;
			/* Opcode 0x11B is undefined */
			/* 0x11C */
			opcodeMap[(int)OpCode.Sizeof] = Sizeof;
			/* 0x11D */
			opcodeMap[(int)OpCode.Refanytype] = Refanytype;
			/* 0x11E */
			opcodeMap[(int)OpCode.PreReadOnly] = PreReadOnly;

			return opcodeMap;
		}
	}
}
