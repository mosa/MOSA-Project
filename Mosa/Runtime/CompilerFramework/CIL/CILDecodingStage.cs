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

//using Mosa.Runtime.CompilerFramework.CIL;
//using Mosa.Runtime.CompilerFramework.IL;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// Represents the IL decoding compilation stage.
	/// </summary>
	/// <remarks>
	/// The IL decoding stage takes a stream of bytes and decodes the
	/// instructions represented into an MSIL based intermediate 
	/// representation. The instructions are grouped into basic Blocks
	/// for easier local optimizations in later compiler stages.
	/// </remarks>
	public sealed partial class CILDecodingStage : IMethodCompilerStage, IInstructionsProvider, IInstructionDecoder
	{
		private readonly System.DataConverter LittleEndianBitConverter = System.DataConverter.LittleEndian;

		#region Static data

		private static readonly Dictionary<OpCode, ICILInstruction> opCodeMap = new Dictionary<OpCode, ICILInstruction>{
            /* 0x000 */ { OpCode.Nop,               new NopInstruction() },
            /* 0x001 */ { OpCode.Break,             new BreakInstruction() },
            /* 0x002 */ { OpCode.Ldarg_0,           new LdargInstruction() },
			/* 0x003 */ { OpCode.Ldarg_1,           new LdargInstruction() },
            /* 0x004 */ { OpCode.Ldarg_2,           new LdargInstruction() },
            /* 0x005 */ { OpCode.Ldarg_3,           new LdargInstruction() },
            /* 0x006 */ { OpCode.Ldloc_0,           new LdargInstruction() },
            /* 0x007 */ { OpCode.Ldloc_1,           new LdargInstruction() },
            /* 0x008 */ { OpCode.Ldloc_2,           new LdargInstruction() },
            /* 0x009 */ { OpCode.Ldloc_3,           new LdargInstruction() },
			/* 0x00A */ { OpCode.Stloc_0,           new StlocInstruction() },
            /* 0x00B */ { OpCode.Stloc_1,           new StlocInstruction() },
            /* 0x00C */ { OpCode.Stloc_2,           new StlocInstruction() },
            /* 0x00D */ { OpCode.Stloc_3,           new StlocInstruction() },
            /* 0x00E */ { OpCode.Ldarg_s,           new LdargInstruction() },
            /* 0x00F */ { OpCode.Ldarga_s,          new LdargaInstruction() },
            /* 0x010 */ { OpCode.Starg_s,           new StargInstruction() },
            /* 0x011 */ { OpCode.Ldloc_s,           new LdlocInstruction() },
            /* 0x012 */ { OpCode.Ldloca_s,          new LdlocaInstruction() },
            /* 0x013 */ { OpCode.Stloc_s,           new StlocInstruction() },
            /* 0x014 */ { OpCode.Ldnull,            new LdcInstruction() },
            /* 0x015 */ { OpCode.Ldc_i4_m1,         new LdcInstruction() },
            /* 0x016 */ { OpCode.Ldc_i4_0,          new LdcInstruction() },
            /* 0x017 */ { OpCode.Ldc_i4_1,          new LdcInstruction() },
            /* 0x018 */ { OpCode.Ldc_i4_2,          new LdcInstruction() },
            /* 0x019 */ { OpCode.Ldc_i4_3,          new LdcInstruction() },
            /* 0x01A */ { OpCode.Ldc_i4_4,          new LdcInstruction() },
            /* 0x01B */ { OpCode.Ldc_i4_5,          new LdcInstruction() },
            /* 0x01C */ { OpCode.Ldc_i4_6,          new LdcInstruction() },
            /* 0x01D */ { OpCode.Ldc_i4_7,          new LdcInstruction() },
            /* 0x01E */ { OpCode.Ldc_i4_8,          new LdcInstruction() },
            /* 0x01F */ { OpCode.Ldc_i4_s,          new LdcInstruction() },
            /* 0x020 */ { OpCode.Ldc_i4,            new LdcInstruction() },
            /* 0x021 */ { OpCode.Ldc_i8,            new LdcInstruction() },
            /* 0x022 */ { OpCode.Ldc_r4,            new LdcInstruction() },
            /* 0x023 */ { OpCode.Ldc_r8,            new LdcInstruction() },
            /* 0x024 is undefined */
            /* 0x025 */ { OpCode.Dup,               new DupInstruction() },
            /* 0x026 */ { OpCode.Pop,               new PopInstruction() }, // here
            /* 0x027 */ { OpCode.Jmp,               new JumpInstruction() },
            /* 0x028 */ { OpCode.Call,              new CallInstruction() },
            /* 0x029 */ { OpCode.Calli,             new CalliInstruction() },
            /* 0x02A */ { OpCode.Ret,               new ReturnInstruction() },
            /* 0x02B */ { OpCode.Br_s,              new BranchInstruction() },
            /* 0x02C */ { OpCode.Brfalse_s, 		new UnaryBranchInstruction() },
            /* 0x02D */ { OpCode.Brtrue_s, 			new UnaryBranchInstruction() },
            /* 0x02E */ { OpCode.Beq_s, 			new BinaryBranchInstruction() },
            /* 0x02F */ { OpCode.Bge_s, 			new BinaryBranchInstruction() },
            /* 0x030 */ { OpCode.Bgt_s, 			new BinaryBranchInstruction() },
            /* 0x031 */ { OpCode.Ble_s, 			new BinaryBranchInstruction() },
            /* 0x032 */ { OpCode.Blt_s, 			new BinaryBranchInstruction() },
            /* 0x033 */ { OpCode.Bne_un_s, 			new BinaryBranchInstruction() },
            /* 0x034 */ { OpCode.Bge_un_s, 			new BinaryBranchInstruction() },
            /* 0x035 */ { OpCode.Bgt_un_s, 			new BinaryBranchInstruction() },
            /* 0x036 */ { OpCode.Ble_un_s, 			new BinaryBranchInstruction() },
            /* 0x037 */ { OpCode.Blt_un_s, 			new BinaryBranchInstruction() },
            /* 0x038 */ { OpCode.Br,                new BranchInstruction() },
            /* 0x039 */ { OpCode.Brfalse, 			new UnaryBranchInstruction() },
            /* 0x03A */ { OpCode.Brtrue, 			new UnaryBranchInstruction() },
            /* 0x03B */ { OpCode.Beq, 				new BinaryBranchInstruction() },
            /* 0x03C */ { OpCode.Bge, 				new BinaryBranchInstruction() },
            /* 0x03D */ { OpCode.Bgt, 				new BinaryBranchInstruction() },
            /* 0x03E */ { OpCode.Ble, 				new BinaryBranchInstruction() },
            /* 0x03F */ { OpCode.Blt, 				new BinaryBranchInstruction() },
            /* 0x040 */ { OpCode.Bne_un, 			new BinaryBranchInstruction() },
            /* 0x041 */ { OpCode.Bge_un, 			new BinaryBranchInstruction() },
            /* 0x042 */ { OpCode.Bgt_un, 			new BinaryBranchInstruction() },
            /* 0x043 */ { OpCode.Ble_un, 			new BinaryBranchInstruction() },
            /* 0x044 */ { OpCode.Blt_un, 			new BinaryBranchInstruction() },
            /* 0x045 */ { OpCode.Switch, 			new SwitchInstruction() },
            /* 0x046 */ { OpCode.Ldind_i1,          new LdobjInstruction() },
            /* 0x047 */ { OpCode.Ldind_u1,          new LdobjInstruction() },
            /* 0x048 */ { OpCode.Ldind_i2,          new LdobjInstruction() },
            /* 0x049 */ { OpCode.Ldind_u2,          new LdobjInstruction() },
            /* 0x04A */ { OpCode.Ldind_i4,          new LdobjInstruction() },
            /* 0x04B */ { OpCode.Ldind_u4,          new LdobjInstruction() },
            /* 0x04C */ { OpCode.Ldind_i8,          new LdobjInstruction() },
            /* 0x04D */ { OpCode.Ldind_i,           new LdobjInstruction() },
            /* 0x04E */ { OpCode.Ldind_r4,          new LdobjInstruction() },
            /* 0x04F */ { OpCode.Ldind_r8,          new LdobjInstruction() },
            /* 0x050 */ { OpCode.Ldind_ref,         new LdobjInstruction() },
            /* 0x051 */ { OpCode.Stind_ref,         new StobjInstruction() },
            /* 0x052 */ { OpCode.Stind_i1,          new StobjInstruction() },
            /* 0x053 */ { OpCode.Stind_i2,          new StobjInstruction() },
            /* 0x054 */ { OpCode.Stind_i4,          new StobjInstruction() },
            /* 0x055 */ { OpCode.Stind_i8,          new StobjInstruction() },
            /* 0x056 */ { OpCode.Stind_r4,          new StobjInstruction() },
            /* 0x057 */ { OpCode.Stind_r8,          new StobjInstruction() },
            /* 0x058 */ { OpCode.Add,               new AddInstruction() },
            /* 0x059 */ { OpCode.Sub, 				new SubInstruction() },
            /* 0x05A */ { OpCode.Mul, 				new MulInstruction() },
            /* 0x05B */ { OpCode.Div,               new DivInstruction() },
            /* 0x05C */ { OpCode.Div_un,            new BinaryLogicInstruction() },
            /* 0x05D */ { OpCode.Rem,               new RemInstruction() },
            /* 0x05E */ { OpCode.Rem_un,            new BinaryLogicInstruction() },
            /* 0x05F */ { OpCode.And,               new BinaryLogicInstruction() },
            /* 0x060 */ { OpCode.Or,                new BinaryLogicInstruction() },
            /* 0x061 */ { OpCode.Xor,               new BinaryLogicInstruction() },
            /* 0x062 */ { OpCode.Shl,               new ShiftInstruction() },
            /* 0x063 */ { OpCode.Shr,               new ShiftInstruction() },
            /* 0x064 */ { OpCode.Shr_un,            new ShiftInstruction() },
            /* 0x065 */ { OpCode.Neg, 				new NegInstruction() },
            /* 0x066 */ { OpCode.Not,               new NotInstruction() },
            /* 0x067 */ { OpCode.Conv_i1, 			new ConversionInstruction() },
            /* 0x068 */ { OpCode.Conv_i2, 			new ConversionInstruction() },
            /* 0x069 */ { OpCode.Conv_i4, 			new ConversionInstruction() },
            /* 0x06A */ { OpCode.Conv_i8, 			new ConversionInstruction() },
            /* 0x06B */ { OpCode.Conv_r4, 			new ConversionInstruction() },
            /* 0x06C */ { OpCode.Conv_r8, 			new ConversionInstruction() },
            /* 0x06D */ { OpCode.Conv_u4, 			new ConversionInstruction() },
            /* 0x06E */ { OpCode.Conv_u8, 			new ConversionInstruction() },
            /* 0x06F */ { OpCode.Callvirt,          new CallvirtInstruction() },
            /* 0x070 */ { OpCode.Cpobj,             new CpobjInstruction() },
            /* 0x071 */ { OpCode.Ldobj,             new LdobjInstruction() },
            /* 0x072 */ { OpCode.Ldstr,             new LdstrInstruction() },
            /* 0x073 */ { OpCode.Newobj,            new NewobjInstruction() },
            /* 0x074 */ { OpCode.Castclass,         new CastclassInstruction() },
            /* 0x075 */ { OpCode.Isinst,            new IsInstInstruction() },
            /* 0x076 */ { OpCode.Conv_r_un, 		new ConversionInstruction() },
            /* Opcodes 0x077-0x078 undefined */
            /* 0x079 */ { OpCode.Unbox,             new UnboxInstruction() },
            /* 0x07A */ { OpCode.Throw,             new ThrowInstruction() },
            /* 0x07B */ { OpCode.Ldfld,             new LdfldInstruction() },
            /* 0x07C */ { OpCode.Ldflda,            new LdfldaInstruction() },
            /* 0x07D */ { OpCode.Stfld,             new StfldInstruction() },
            /* 0x07E */ { OpCode.Ldsfld,            new LdsfldInstruction() },
            /* 0x07F */ { OpCode.Ldsflda,           new LdsfldaInstruction() },
            /* 0x080 */ { OpCode.Stsfld,            new StsfldInstruction() },
            /* 0x081 */ { OpCode.Stobj,             new StobjInstruction() },
            /* 0x082 */ { OpCode.Conv_ovf_i1_un, 	new ConversionInstruction() },
            /* 0x083 */ { OpCode.Conv_ovf_i2_un, 	new ConversionInstruction() },
            /* 0x084 */ { OpCode.Conv_ovf_i4_un, 	new ConversionInstruction() },
            /* 0x085 */ { OpCode.Conv_ovf_i8_un, 	new ConversionInstruction() },
            /* 0x086 */ { OpCode.Conv_ovf_u1_un, 	new ConversionInstruction() },
            /* 0x087 */ { OpCode.Conv_ovf_u2_un, 	new ConversionInstruction() },
            /* 0x088 */ { OpCode.Conv_ovf_u4_un, 	new ConversionInstruction() },
            /* 0x089 */ { OpCode.Conv_ovf_u8_un, 	new ConversionInstruction() },
            /* 0x08A */ { OpCode.Conv_ovf_i_un, 	new ConversionInstruction() },
            /* 0x08B */ { OpCode.Conv_ovf_u_un, 	new ConversionInstruction() },
            /* 0x08C */ { OpCode.Box,               new BoxInstruction() },
            /* 0x08D */ { OpCode.Newarr,            new NewarrInstruction() },
            /* 0x08E */ { OpCode.Ldlen,             new LdlenInstruction() },
            /* 0x08F */ { OpCode.Ldelema,           new LdelemaInstruction() },
            /* 0x090 */ { OpCode.Ldelem_i1,         new LdelemInstruction() },
            /* 0x091 */ { OpCode.Ldelem_u1,         new LdelemInstruction() },
            /* 0x092 */ { OpCode.Ldelem_i2,         new LdelemInstruction() },
            /* 0x093 */ { OpCode.Ldelem_u2,         new LdelemInstruction() },
            /* 0x094 */ { OpCode.Ldelem_i4,         new LdelemInstruction() },
            /* 0x095 */ { OpCode.Ldelem_u4,         new LdelemInstruction() },
            /* 0x096 */ { OpCode.Ldelem_i8,         new LdelemInstruction() },
            /* 0x097 */ { OpCode.Ldelem_i,          new LdelemInstruction() },
            /* 0x098 */ { OpCode.Ldelem_r4,         new LdelemInstruction() },
            /* 0x099 */ { OpCode.Ldelem_r8,         new LdelemInstruction() },
            /* 0x09A */ { OpCode.Ldelem_ref,        new LdelemInstruction() },
            /* 0x09B */ { OpCode.Stelem_i,          new StelemInstruction() },
            /* 0x09C */ { OpCode.Stelem_i1,         new StelemInstruction() },
            /* 0x09D */ { OpCode.Stelem_i2,         new StelemInstruction() },
            /* 0x09E */ { OpCode.Stelem_i4,         new StelemInstruction() },
            /* 0x09F */ { OpCode.Stelem_i8,         new StelemInstruction() },
            /* 0x0A0 */ { OpCode.Stelem_r4,         new StelemInstruction() },
            /* 0x0A1 */ { OpCode.Stelem_r8,         new StelemInstruction() },
            /* 0x0A2 */ { OpCode.Stelem_ref,        new StelemInstruction() },
            /* 0x0A3 */ { OpCode.Ldelem,            new LdelemInstruction() },
            /* 0x0A4 */ { OpCode.Stelem,            new StelemInstruction() },
            /* 0x0A5 */ { OpCode.Unbox_any,         new UnboxAnyInstruction() },
            /* Opcodes 0x0A6-0x0B2 are undefined */
            /* 0x0B3 */ { OpCode.Conv_ovf_i1, 		new ConversionInstruction() },
            /* 0x0B4 */ { OpCode.Conv_ovf_u1, 		new ConversionInstruction() },
            /* 0x0B5 */ { OpCode.Conv_ovf_i2, 		new ConversionInstruction() },
            /* 0x0B6 */ { OpCode.Conv_ovf_u2, 		new ConversionInstruction() },
            /* 0x0B7 */ { OpCode.Conv_ovf_i4, 		new ConversionInstruction() },
            /* 0x0B8 */ { OpCode.Conv_ovf_u4, 		new ConversionInstruction() },
            /* 0x0B9 */ { OpCode.Conv_ovf_i8, 		new ConversionInstruction() },
            /* 0x0BA */ { OpCode.Conv_ovf_u8, 		new ConversionInstruction() },
            /* Opcodes 0x0BB-0x0C1 are undefined */
            /* 0x0C2 */ { OpCode.Refanyval,         new RefanyvalInstruction() },
            /* 0x0C3 */ { OpCode.Ckfinite, 			new UnaryArithmeticInstruction() },
            /* Opcodes 0x0C4-0x0C5 are undefined */
            /* 0x0C6 */ { OpCode.Mkrefany,          new MkrefanyInstruction() },
            /* Opcodes 0x0C7-0x0CF are reserved */
            /* 0x0D0 */ { OpCode.Ldtoken,           new LdtokenInstruction() },
            /* 0x0D1 */ { OpCode.Conv_u2, 			new ConversionInstruction() },
            /* 0x0D2 */ { OpCode.Conv_u1, 			new ConversionInstruction() },
            /* 0x0D3 */ { OpCode.Conv_i, 			new ConversionInstruction() },
            /* 0x0D4 */ { OpCode.Conv_ovf_i, 		new ConversionInstruction() },
            /* 0x0D5 */ { OpCode.Conv_ovf_u, 		new ConversionInstruction() },
            /* 0x0D6 */ { OpCode.Add_ovf,           new ArithmeticOverflowInstruction() },
            /* 0x0D7 */ { OpCode.Add_ovf_un,        new ArithmeticOverflowInstruction() },
            /* 0x0D8 */ { OpCode.Mul_ovf,           new ArithmeticOverflowInstruction() },
            /* 0x0D9 */ { OpCode.Mul_ovf_un,        new ArithmeticOverflowInstruction() },
            /* 0x0DA */ { OpCode.Sub_ovf,           new ArithmeticOverflowInstruction() },
            /* 0x0DB */ { OpCode.Sub_ovf_un,        new ArithmeticOverflowInstruction() },
            /* 0x0DC */ { OpCode.Endfinally,        new EndfinallyInstruction() },
            /* 0x0DD */ { OpCode.Leave,             new LeaveInstruction() },
            /* 0x0DE */ { OpCode.Leave_s,           new LeaveInstruction() },
            /* 0x0DF */ { OpCode.Stind_i,           new StobjInstruction() },
		    /* 0x0E0 */ { OpCode.Conv_u,            new ConversionInstruction() },
            /* Opcodes 0xE1-0xFF are reserved */
            /* 0x100 */ { OpCode.Arglist,           new ArglistInstruction() },
            /* 0x101 */ { OpCode.Ceq,               new BinaryComparisonInstruction() },
            /* 0x102 */ { OpCode.Cgt,               new BinaryComparisonInstruction() },
            /* 0x103 */ { OpCode.Cgt_un,            new BinaryComparisonInstruction() },
            /* 0x104 */ { OpCode.Clt,               new BinaryComparisonInstruction() },
            /* 0x105 */ { OpCode.Clt_un,            new BinaryComparisonInstruction() },
            /* 0x106 */ { OpCode.Ldftn,             new LdftnInstruction() },
            /* 0x107 */ { OpCode.Ldvirtftn,         new LdvirtftnInstruction() },
            /* Opcode 0x108 is undefined. */
            /* 0x109 */ { OpCode.Ldarg,             new LdargInstruction() },
            /* 0x10A */ { OpCode.Ldarga,            new LdargaInstruction() },
            /* 0x10B */ { OpCode.Starg,             new StargInstruction() },
            /* 0x10C */ { OpCode.Ldloc,             new LdlocInstruction() },
            /* 0x10D */ { OpCode.Ldloca,            new LdlocaInstruction() },
            /* 0x10E */ { OpCode.Stloc,             new StlocInstruction() },
            /* 0x10F */ { OpCode.Localalloc,        new LocalallocInstruction() },
            /* Opcode 0x110 is undefined */
            /* 0x111 */ { OpCode.Endfilter,         new EndfilterInstruction() },
            /* 0x112 */ { OpCode.PreUnaligned,      new UnalignedPrefixInstruction() },
            /* 0x113 */ { OpCode.PreVolatile,       new PrefixInstruction() },
            /* 0x114 */ { OpCode.PreTail,           new PrefixInstruction() },
            /* 0x115 */ { OpCode.InitObj,           new InitObjInstruction() },
            /* 0x116 */ { OpCode.PreConstrained,    new ConstrainedPrefixInstruction() },
            /* 0x117 */ { OpCode.Cpblk,             new CpblkInstruction() },
            /* 0x118 */ { OpCode.Initblk,           new InitblkInstruction() },
            /* 0x119 */ { OpCode.PreNo,             new NoPrefixInstruction() },
            /* 0x11A */ { OpCode.Rethrow,           new RethrowInstruction() },
            /* Opcode 0x11B is undefined */
            /* 0x11C */ { OpCode.Sizeof,            new SizeofInstruction() },
            /* 0x11D */ { OpCode.Refanytype,        new RefanytypeInstruction() },
            /* 0x11E */ { OpCode.PreReadOnly,       new PrefixInstruction() }
		};

		#endregion // Static data

		#region Data members

		/// <summary>
		/// The reader used to process the code stream.
		/// </summary>
		private BinaryReader _codeReader;

		/// <summary>
		/// The current compiler context.
		/// </summary>
		private IMethodCompiler _compiler;

		/// <summary>
		/// The method implementation of the currently compiled method.
		/// </summary>
		private RuntimeMethod _method;

		/// <summary>
		/// List of instructions decoded by the decoder.
		/// </summary>
		Instructions _instructions = new Instructions(1024 * 1024);

		#endregion // Data members

		#region Construction

		/// <summary>
		/// The instruction factory used to emit instructions.
		/// </summary>
		public CILDecodingStage()
		{
		}

		#endregion // Construction

		#region Properties

		// <summary>
		// Returns the binary reader to access the code stream.
		// </summary>
		// <value></value>
		//public BinaryReader CodeReader
		//{
		//    get { return _codeReader; }
		//}

		#endregion // Properties

		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public string Name
		{
			get { return @"CIL decoder"; }
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public void Run(IMethodCompiler compiler)
		{
			// The size of the code in bytes
			Mosa.Runtime.CompilerFramework.IL.MethodHeader header = new Mosa.Runtime.CompilerFramework.IL.MethodHeader();

			// Check preconditions
			if (null == compiler)
				throw new ArgumentNullException(@"compiler");

			//Debug.WriteLine(@"Decoding " + compiler.Type.ToString() + "." + compiler.Method.ToString());

			using (Stream code = compiler.GetInstructionStream())
			using (BinaryReader reader = new BinaryReader(code)) {
				_compiler = compiler;
				_method = compiler.Method;
				_codeReader = reader;

				ReadMethodHeader(reader, ref header);
				//Debug.WriteLine("Decoding " + compiler.Method.ToString());

				if (0 != header.localsSignature) {
					StandAloneSigRow row;
					IMetadataProvider md = _method.Module.Metadata;
					md.Read(header.localsSignature, out row);
					compiler.SetLocalVariableSignature(LocalVariableSignature.Parse(md, row.SignatureBlobIdx));
				}

				/* Decode the instructions */
				Decode(compiler, ref header);

				// When we leave, the operand stack must only contain the locals...
				//Debug.Assert(_operandStack.Count == _method.Locals.Count);
				_codeReader = null;
				_compiler = null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pipeline"></param>
		public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.Add(this);
		}

		#endregion // IMethodCompilerStage Members

		#region Internals

		/// <summary>
		/// Reads the method _header From the instruction stream.
		/// </summary>
		/// <param name="reader">The reader used to decode the instruction stream.</param>
		/// <param name="header">The method _header structure to populate.</param>
		private void ReadMethodHeader(BinaryReader reader, ref Mosa.Runtime.CompilerFramework.IL.MethodHeader header)
		{
			header.flags = (Mosa.Runtime.CompilerFramework.IL.MethodFlags)reader.ReadByte();
			switch (header.flags & Mosa.Runtime.CompilerFramework.IL.MethodFlags.HeaderMask) {
				case Mosa.Runtime.CompilerFramework.IL.MethodFlags.TinyFormat:
					header.codeSize = ((uint)(header.flags & Mosa.Runtime.CompilerFramework.IL.MethodFlags.TinyCodeSizeMask) >> 2);
					header.flags &= Mosa.Runtime.CompilerFramework.IL.MethodFlags.HeaderMask;
					break;

				case Mosa.Runtime.CompilerFramework.IL.MethodFlags.FatFormat:
					header.flags = (Mosa.Runtime.CompilerFramework.IL.MethodFlags)(reader.ReadByte() << 8 | (byte)header.flags);
					if (Mosa.Runtime.CompilerFramework.IL.MethodFlags.ValidHeader != (header.flags & Mosa.Runtime.CompilerFramework.IL.MethodFlags.HeaderSizeMask))
						throw new InvalidDataException(@"Invalid method _header.");
					header.maxStack = reader.ReadUInt16();
					header.codeSize = reader.ReadUInt32();
					header.localsSignature = (TokenTypes)reader.ReadUInt32();
					break;

				default:
					throw new InvalidDataException(@"Invalid method _header.");
			}

			// Are there sections following the code?
			if (Mosa.Runtime.CompilerFramework.IL.MethodFlags.MoreSections == (header.flags & Mosa.Runtime.CompilerFramework.IL.MethodFlags.MoreSections)) {
				// Yes, seek to them and process those sections
				long codepos = reader.BaseStream.Position;

				// Seek to the end of the code...
				long dataSectPos = codepos + header.codeSize;
				if (0 != (dataSectPos & 3))
					dataSectPos += (4 - (dataSectPos % 4));
				reader.BaseStream.Position = dataSectPos;

				// Read all headers, so the IL decoder knows how to handle these...
				byte flags;
				int length, blocks;
				bool isFat;
				Mosa.Runtime.CompilerFramework.IL.EhClause clause = new Mosa.Runtime.CompilerFramework.IL.EhClause();

				do {
					flags = reader.ReadByte();
					isFat = (0x40 == (flags & 0x40));
					if (true == isFat) {
						byte[] buffer = new byte[4];
						reader.Read(buffer, 0, 3);
						length = LittleEndianBitConverter.GetInt32(buffer, 0);
						blocks = (length - 4) / 24;
					}
					else {
						length = reader.ReadByte();
						blocks = (length - 4) / 12;

						/* Read & skip the padding. */
						reader.ReadInt16();
					}

					Debug.Assert(0x01 == (flags & 0x3F), @"Unsupported method datta section.");
					// Read the clause
					for (int i = 0; i < blocks; i++) {
						clause.Read(reader, isFat);
						// FIXME: Create proper basic Blocks for each item in the clause
					}
				}
				while (0x80 == (flags & 0x80));

				reader.BaseStream.Position = codepos;
			}
		}

		/// <summary>
		/// Decodes the instruction stream of the reader and populates the compiler.
		/// </summary>
		/// <param name="compiler">The compiler to populate.</param>
		/// <param name="header">The method _header.</param>
		private void Decode(IMethodCompiler compiler, ref Mosa.Runtime.CompilerFramework.IL.MethodHeader header)
		{
			// Start of the code stream
			long codeStart = _codeReader.BaseStream.Position;

			// End of the code stream
			long codeEnd = _codeReader.BaseStream.Position + header.codeSize;

			// Prefix instruction
			PrefixInstruction prefix = null;

			// Setup first instruction
			int at = -1;

			while (codeEnd != _codeReader.BaseStream.Position) {
				// Determine the instruction offset
				int instOffset = (int)(_codeReader.BaseStream.Position - codeStart);

				// Read the next opcode From the stream
				OpCode op = (OpCode)_codeReader.ReadByte();
				if (OpCode.Extop == op)
					op = (OpCode)(0x100 | _codeReader.ReadByte());

				ICILInstruction instruction = opCodeMap[op];

				if (instruction is PrefixInstruction) {
					prefix = instruction as PrefixInstruction;
					continue;
				}

				// Create and initialize the corresponding instruction
				at = _instructions.InsertAfter(at);
				_instructions.instructions[at].Instruction = instruction;
				_instructions.instructions[at].Prefix = prefix;
				_instructions.instructions[at].Offset = instOffset;
				instruction.Decode(ref _instructions.instructions[at], op, this);

				// FIXME! 
				//// Do we need to patch branch targets?
				//IBranchInstruction branch = instruction as IBranchInstruction;
				//if (branch != null) {
				//    int pc = (int)(_codeReader.BaseStream.Position - codeStart);
				//    int[] targets = branch.BranchTargets;

				//    for (int i = 0; i < targets.Length; i++)
				//        targets[i] += pc;
				//}

				prefix = null;
			}
		}

		#endregion // Internals

		#region IInstructionsProvider members

		/// <summary>
		/// Gets a list of instructions in intermediate representation.
		/// </summary>
		/// <value></value>
		List<Instruction> IInstructionsProvider.Instructions
		{
			get { return null; }
		}

		/// <summary>
		/// Gets a list of instructions in intermediate representation.
		/// </summary>
		/// <value></value>
		public Instructions Instructions2
		{
			get { return _instructions; }
		}

		#endregion //  IInstructionsProvider members

		#region IInstructionDecoder Members

		/// <summary>
		/// Gets the compiler, that is currently executing.
		/// </summary>
		/// <value></value>
		IMethodCompiler IInstructionDecoder.Compiler
		{
			get { return _compiler; }
		}

		/// <summary>
		/// Gets the RuntimeMethod being compiled.
		/// </summary>
		/// <value></value>
		RuntimeMethod IInstructionDecoder.Method
		{
			get { return _method; }
		}

		/// <summary>
		/// Decodes <paramref name="value"/> From the instruction stream.
		/// </summary>
		/// <param name="value">Receives the decoded value From the instruction stream.</param>
		void IInstructionDecoder.Decode(out byte value)
		{
			value = _codeReader.ReadByte();
		}

		/// <summary>
		/// Decodes <paramref name="value"/> From the instruction stream.
		/// </summary>
		/// <param name="value">Receives the decoded value From the instruction stream.</param>
		void IInstructionDecoder.Decode(out sbyte value)
		{
			value = _codeReader.ReadSByte();
		}

		/// <summary>
		/// Decodes <paramref name="value"/> From the instruction stream.
		/// </summary>
		/// <param name="value">Receives the decoded value From the instruction stream.</param>
		void IInstructionDecoder.Decode(out short value)
		{
			value = _codeReader.ReadInt16();
		}

		/// <summary>
		/// Decodes <paramref name="value"/> From the instruction stream.
		/// </summary>
		/// <param name="value">Receives the decoded value From the instruction stream.</param>
		void IInstructionDecoder.Decode(out ushort value)
		{
			value = _codeReader.ReadUInt16();
		}

		/// <summary>
		/// Decodes <paramref name="value"/> From the instruction stream.
		/// </summary>
		/// <param name="value">Receives the decoded value From the instruction stream.</param>
		void IInstructionDecoder.Decode(out int value)
		{
			value = _codeReader.ReadInt32();
		}

		/// <summary>
		/// Decodes <paramref name="value"/> From the instruction stream.
		/// </summary>
		/// <param name="value">Receives the decoded value From the instruction stream.</param>
		void IInstructionDecoder.Decode(out uint value)
		{
			value = _codeReader.ReadUInt32();
		}

		/// <summary>
		/// Decodes <paramref name="value"/> From the instruction stream.
		/// </summary>
		/// <param name="value">Receives the decoded value From the instruction stream.</param>
		void IInstructionDecoder.Decode(out long value)
		{
			value = _codeReader.ReadInt64();
		}

		/// <summary>
		/// Decodes <paramref name="value"/> From the instruction stream.
		/// </summary>
		/// <param name="value">Receives the decoded value From the instruction stream.</param>
		void IInstructionDecoder.Decode(out float value)
		{
			value = _codeReader.ReadSingle();
		}

		/// <summary>
		/// Decodes <paramref name="value"/> From the instruction stream.
		/// </summary>
		/// <param name="value">Receives the decoded value From the instruction stream.</param>
		void IInstructionDecoder.Decode(out double value)
		{
			value = _codeReader.ReadDouble();
		}

		/// <summary>
		/// Decodes <paramref name="value"/> From the instruction stream.
		/// </summary>
		/// <param name="value">Receives the decoded value From the instruction stream.</param>
		void IInstructionDecoder.Decode(out TokenTypes value)
		{
			value = (TokenTypes)_codeReader.ReadInt32();
		}

		#endregion
	}
}
