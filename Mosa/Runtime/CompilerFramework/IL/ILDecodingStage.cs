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

using Mosa.Runtime.CompilerFramework.IL;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Represents the IL decoding compilation stage.
    /// </summary>
    /// <remarks>
    /// The IL decoding stage takes a stream of bytes and decodes the
    /// instructions represented into an MSIL based intermediate 
    /// representation. The instructions are grouped into basic blocks
    /// for easier local optimizations in later compiler stages.
    /// </remarks>
    public sealed partial class ILDecodingStage : IMethodCompilerStage, IInstructionsProvider, IInstructionDecoder
    {
        private readonly System.DataConverter LittleEndianBitConverter = System.DataConverter.LittleEndian;

        #region Static data

        private static readonly Dictionary<OpCode, Type> s_opcodeMap = new Dictionary<OpCode,Type>{
            /* 0x000 */ { OpCode.Nop,               typeof(NopInstruction) },
            /* 0x001 */ { OpCode.Break,             typeof(BreakInstruction) },
            /* 0x002 */ { OpCode.Ldarg_0,           typeof(LdargInstruction) },
            /* 0x003 */ { OpCode.Ldarg_1,           typeof(LdargInstruction) },
            /* 0x004 */ { OpCode.Ldarg_2,           typeof(LdargInstruction) },
            /* 0x005 */ { OpCode.Ldarg_3,           typeof(LdargInstruction) },
            /* 0x006 */ { OpCode.Ldloc_0,           typeof(LdlocInstruction) },
            /* 0x007 */ { OpCode.Ldloc_1,           typeof(LdlocInstruction) },
            /* 0x008 */ { OpCode.Ldloc_2,           typeof(LdlocInstruction) },
            /* 0x009 */ { OpCode.Ldloc_3,           typeof(LdlocInstruction) },
            /* 0x00A */ { OpCode.Stloc_0,           typeof(StlocInstruction) },
            /* 0x00B */ { OpCode.Stloc_1,           typeof(StlocInstruction) },
            /* 0x00C */ { OpCode.Stloc_2,           typeof(StlocInstruction) },
            /* 0x00D */ { OpCode.Stloc_3,           typeof(StlocInstruction) },
            /* 0x00E */ { OpCode.Ldarg_s,           typeof(LdargInstruction) },
            /* 0x00F */ { OpCode.Ldarga_s,          typeof(LdargaInstruction) },
            /* 0x010 */ { OpCode.Starg_s,           typeof(StargInstruction) },
            /* 0x011 */ { OpCode.Ldloc_s,           typeof(LdlocInstruction) },
            /* 0x012 */ { OpCode.Ldloca_s,          typeof(LdlocaInstruction) },
            /* 0x013 */ { OpCode.Stloc_s,           typeof(StlocInstruction) },
            /* 0x014 */ { OpCode.Ldnull,            typeof(LdcInstruction) },
            /* 0x015 */ { OpCode.Ldc_i4_m1,         typeof(LdcInstruction) },
            /* 0x016 */ { OpCode.Ldc_i4_0,          typeof(LdcInstruction) },
            /* 0x017 */ { OpCode.Ldc_i4_1,          typeof(LdcInstruction) },
            /* 0x018 */ { OpCode.Ldc_i4_2,          typeof(LdcInstruction) },
            /* 0x019 */ { OpCode.Ldc_i4_3,          typeof(LdcInstruction) },
            /* 0x01A */ { OpCode.Ldc_i4_4,          typeof(LdcInstruction) },
            /* 0x01B */ { OpCode.Ldc_i4_5,          typeof(LdcInstruction) },
            /* 0x01C */ { OpCode.Ldc_i4_6,          typeof(LdcInstruction) },
            /* 0x01D */ { OpCode.Ldc_i4_7,          typeof(LdcInstruction) },
            /* 0x01E */ { OpCode.Ldc_i4_8,          typeof(LdcInstruction) },
            /* 0x01F */ { OpCode.Ldc_i4_s,          typeof(LdcInstruction) },
            /* 0x020 */ { OpCode.Ldc_i4,            typeof(LdcInstruction) },
            /* 0x021 */ { OpCode.Ldc_i8,            typeof(LdcInstruction) },
            /* 0x022 */ { OpCode.Ldc_r4,            typeof(LdcInstruction) },
            /* 0x023 */ { OpCode.Ldc_r8,            typeof(LdcInstruction) },
            /* 0x24 is undefined */
            /* 0x025 */ { OpCode.Dup,               typeof(DupInstruction) },
            /* 0x026 */ { OpCode.Pop,               typeof(PopInstruction) },
            /* 0x027 */ { OpCode.Jmp,               typeof(JumpInstruction) },
            /* 0x028 */ { OpCode.Call,              typeof(CallInstruction) },
            /* 0x029 */ { OpCode.Calli,             typeof(CalliInstruction) },
            /* 0x02A */ { OpCode.Ret,               typeof(ReturnInstruction) },
            /* 0x02B */ { OpCode.Br_s,              typeof(BranchInstruction) },
            /* 0x02C */ { OpCode.Brfalse_s, 		typeof(UnaryBranchInstruction) },
            /* 0x02D */ { OpCode.Brtrue_s, 			typeof(UnaryBranchInstruction) },
            /* 0x02E */ { OpCode.Beq_s, 			typeof(BinaryBranchInstruction) },
            /* 0x02F */ { OpCode.Bge_s, 			typeof(BinaryBranchInstruction) },
            /* 0x030 */ { OpCode.Bgt_s, 			typeof(BinaryBranchInstruction) },
            /* 0x031 */ { OpCode.Ble_s, 			typeof(BinaryBranchInstruction) },
            /* 0x032 */ { OpCode.Blt_s, 			typeof(BinaryBranchInstruction) },
            /* 0x033 */ { OpCode.Bne_un_s, 			typeof(BinaryBranchInstruction) },
            /* 0x034 */ { OpCode.Bge_un_s, 			typeof(BinaryBranchInstruction) },
            /* 0x035 */ { OpCode.Bgt_un_s, 			typeof(BinaryBranchInstruction) },
            /* 0x036 */ { OpCode.Ble_un_s, 			typeof(BinaryBranchInstruction) },
            /* 0x037 */ { OpCode.Blt_un_s, 			typeof(BinaryBranchInstruction) },
            /* 0x038 */ { OpCode.Br,                typeof(BranchInstruction) },
            /* 0x039 */ { OpCode.Brfalse, 			typeof(UnaryBranchInstruction) },
            /* 0x03A */ { OpCode.Brtrue, 			typeof(UnaryBranchInstruction) },
            /* 0x03B */ { OpCode.Beq, 				typeof(BinaryBranchInstruction) },
            /* 0x03C */ { OpCode.Bge, 				typeof(BinaryBranchInstruction) },
            /* 0x03D */ { OpCode.Bgt, 				typeof(BinaryBranchInstruction) },
            /* 0x03E */ { OpCode.Ble, 				typeof(BinaryBranchInstruction) },
            /* 0x03F */ { OpCode.Blt, 				typeof(BinaryBranchInstruction) },
            /* 0x040 */ { OpCode.Bne_un, 			typeof(BinaryBranchInstruction) },
            /* 0x041 */ { OpCode.Bge_un, 			typeof(BinaryBranchInstruction) },
            /* 0x042 */ { OpCode.Bgt_un, 			typeof(BinaryBranchInstruction) },
            /* 0x043 */ { OpCode.Ble_un, 			typeof(BinaryBranchInstruction) },
            /* 0x044 */ { OpCode.Blt_un, 			typeof(BinaryBranchInstruction) },
            /* 0x045 */ { OpCode.Switch, 			typeof(SwitchInstruction) },
            /* 0x046 */ { OpCode.Ldind_i1,          typeof(LdobjInstruction) },
            /* 0x047 */ { OpCode.Ldind_u1,          typeof(LdobjInstruction) },
            /* 0x048 */ { OpCode.Ldind_i2,          typeof(LdobjInstruction) },
            /* 0x049 */ { OpCode.Ldind_u2,          typeof(LdobjInstruction) },
            /* 0x04A */ { OpCode.Ldind_i4,          typeof(LdobjInstruction) },
            /* 0x04B */ { OpCode.Ldind_u4,          typeof(LdobjInstruction) },
            /* 0x04C */ { OpCode.Ldind_i8,          typeof(LdobjInstruction) },
            /* 0x04D */ { OpCode.Ldind_i,           typeof(LdobjInstruction) },
            /* 0x04E */ { OpCode.Ldind_r4,          typeof(LdobjInstruction) },
            /* 0x04F */ { OpCode.Ldind_r8,          typeof(LdobjInstruction) },
            /* 0x050 */ { OpCode.Ldind_ref,         typeof(LdobjInstruction) },
            /* 0x051 */ { OpCode.Stind_ref,         typeof(StobjInstruction) },
            /* 0x052 */ { OpCode.Stind_i1,          typeof(StobjInstruction) },
            /* 0x053 */ { OpCode.Stind_i2,          typeof(StobjInstruction) },
            /* 0x054 */ { OpCode.Stind_i4,          typeof(StobjInstruction) },
            /* 0x055 */ { OpCode.Stind_i8,          typeof(StobjInstruction) },
            /* 0x056 */ { OpCode.Stind_r4,          typeof(StobjInstruction) },
            /* 0x057 */ { OpCode.Stind_r8,          typeof(StobjInstruction) },
            /* 0x058 */ { OpCode.Add,               typeof(AddInstruction) },
            /* 0x059 */ { OpCode.Sub, 				typeof(SubInstruction) },
            /* 0x05A */ { OpCode.Mul, 				typeof(MulInstruction) },
            /* 0x05B */ { OpCode.Div,               typeof(DivInstruction) },
            /* 0x05C */ { OpCode.Div_un,            typeof(BinaryLogicInstruction) },
            /* 0x05D */ { OpCode.Rem,               typeof(RemInstruction) },
            /* 0x05E */ { OpCode.Rem_un,            typeof(BinaryLogicInstruction) },
            /* 0x05F */ { OpCode.And,               typeof(BinaryLogicInstruction) },
            /* 0x060 */ { OpCode.Or,                typeof(BinaryLogicInstruction) },
            /* 0x061 */ { OpCode.Xor,               typeof(BinaryLogicInstruction) },
            /* 0x062 */ { OpCode.Shl,               typeof(ShiftInstruction) },
            /* 0x063 */ { OpCode.Shr,               typeof(ShiftInstruction) },
            /* 0x064 */ { OpCode.Shr_un,            typeof(ShiftInstruction) },
            /* 0x065 */ { OpCode.Neg, 				typeof(NegInstruction) },
            /* 0x066 */ { OpCode.Not,               typeof(NotInstruction) },
            /* 0x067 */ { OpCode.Conv_i1, 			typeof(ConversionInstruction) },
            /* 0x068 */ { OpCode.Conv_i2, 			typeof(ConversionInstruction) },
            /* 0x069 */ { OpCode.Conv_i4, 			typeof(ConversionInstruction) },
            /* 0x06A */ { OpCode.Conv_i8, 			typeof(ConversionInstruction) },
            /* 0x06B */ { OpCode.Conv_r4, 			typeof(ConversionInstruction) },
            /* 0x06C */ { OpCode.Conv_r8, 			typeof(ConversionInstruction) },
            /* 0x06D */ { OpCode.Conv_u4, 			typeof(ConversionInstruction) },
            /* 0x06E */ { OpCode.Conv_u8, 			typeof(ConversionInstruction) },
            /* 0x06F */ { OpCode.Callvirt,          typeof(CallvirtInstruction) },
            /* 0x070 */ { OpCode.Cpobj,             typeof(CpobjInstruction) },
            /* 0x071 */ { OpCode.Ldobj,             typeof(LdobjInstruction) },
            /* 0x072 */ { OpCode.Ldstr,             typeof(LdstrInstruction) },
            /* 0x073 */ { OpCode.Newobj,            typeof(NewobjInstruction) },
            /* 0x074 */ { OpCode.Castclass,         typeof(CastclassInstruction) },
            /* 0x075 */ { OpCode.Isinst,            typeof(IsInstInstruction) },
            /* 0x076 */ { OpCode.Conv_r_un, 		typeof(ConversionInstruction) },
            /* Opcodes 0x077-0x078 undefined */
            /* 0x079 */ { OpCode.Unbox,             typeof(UnboxInstruction) },
            /* 0x07A */ { OpCode.Throw,             typeof(ThrowInstruction) },
            /* 0x07B */ { OpCode.Ldfld,             typeof(LdfldInstruction) },
            /* 0x07C */ { OpCode.Ldflda,            typeof(LdfldaInstruction) },
            /* 0x07D */ { OpCode.Stfld,             typeof(StfldInstruction) },
            /* 0x07E */ { OpCode.Ldsfld,            typeof(LdsfldInstruction) },
            /* 0x07F */ { OpCode.Ldsflda,           typeof(LdsfldaInstruction) },
            /* 0x080 */ { OpCode.Stsfld,            typeof(StsfldInstruction) },
            /* 0x081 */ { OpCode.Stobj,             typeof(StobjInstruction) },
            /* 0x082 */ { OpCode.Conv_ovf_i1_un, 	typeof(ConversionInstruction) },
            /* 0x083 */ { OpCode.Conv_ovf_i2_un, 	typeof(ConversionInstruction) },
            /* 0x084 */ { OpCode.Conv_ovf_i4_un, 	typeof(ConversionInstruction) },
            /* 0x085 */ { OpCode.Conv_ovf_i8_un, 	typeof(ConversionInstruction) },
            /* 0x086 */ { OpCode.Conv_ovf_u1_un, 	typeof(ConversionInstruction) },
            /* 0x087 */ { OpCode.Conv_ovf_u2_un, 	typeof(ConversionInstruction) },
            /* 0x088 */ { OpCode.Conv_ovf_u4_un, 	typeof(ConversionInstruction) },
            /* 0x089 */ { OpCode.Conv_ovf_u8_un, 	typeof(ConversionInstruction) },
            /* 0x08A */ { OpCode.Conv_ovf_i_un, 	typeof(ConversionInstruction) },
            /* 0x08B */ { OpCode.Conv_ovf_u_un, 	typeof(ConversionInstruction) },
            /* 0x08C */ { OpCode.Box,               typeof(BoxInstruction) },
            /* 0x08D */ { OpCode.Newarr,            typeof(NewarrInstruction) },
            /* 0x08E */ { OpCode.Ldlen,             typeof(LdlenInstruction) },
            /* 0x08F */ { OpCode.Ldelema,           typeof(LdelemaInstruction) },
            /* 0x090 */ { OpCode.Ldelem_i1,         typeof(LdelemInstruction) },
            /* 0x091 */ { OpCode.Ldelem_u1,         typeof(LdelemInstruction) },
            /* 0x092 */ { OpCode.Ldelem_i2,         typeof(LdelemInstruction) },
            /* 0x093 */ { OpCode.Ldelem_u2,         typeof(LdelemInstruction) },
            /* 0x094 */ { OpCode.Ldelem_i4,         typeof(LdelemInstruction) },
            /* 0x095 */ { OpCode.Ldelem_u4,         typeof(LdelemInstruction) },
            /* 0x096 */ { OpCode.Ldelem_i8,         typeof(LdelemInstruction) },
            /* 0x097 */ { OpCode.Ldelem_i,          typeof(LdelemInstruction) },
            /* 0x098 */ { OpCode.Ldelem_r4,         typeof(LdelemInstruction) },
            /* 0x099 */ { OpCode.Ldelem_r8,         typeof(LdelemInstruction) },
            /* 0x09A */ { OpCode.Ldelem_ref,        typeof(LdelemInstruction) },
            /* 0x09B */ { OpCode.Stelem_i,          typeof(StelemInstruction) },
            /* 0x09C */ { OpCode.Stelem_i1,         typeof(StelemInstruction) },
            /* 0x09D */ { OpCode.Stelem_i2,         typeof(StelemInstruction) },
            /* 0x09E */ { OpCode.Stelem_i4,         typeof(StelemInstruction) },
            /* 0x09F */ { OpCode.Stelem_i8,         typeof(StelemInstruction) },
            /* 0x0A0 */ { OpCode.Stelem_r4,         typeof(StelemInstruction) },
            /* 0x0A1 */ { OpCode.Stelem_r8,         typeof(StelemInstruction) },
            /* 0x0A2 */ { OpCode.Stelem_ref,        typeof(StelemInstruction) },
            /* 0x0A3 */ { OpCode.Ldelem,            typeof(LdelemInstruction) },
            /* 0x0A4 */ { OpCode.Stelem,            typeof(StelemInstruction) },
            /* 0x0A5 */ { OpCode.Unbox_any,         typeof(UnboxAnyInstruction) },
            /* Opcodes 0x0A6-0x0B2 are undefined */
            /* 0x0B3 */ { OpCode.Conv_ovf_i1, 		typeof(ConversionInstruction) },
            /* 0x0B4 */ { OpCode.Conv_ovf_u1, 		typeof(ConversionInstruction) },
            /* 0x0B5 */ { OpCode.Conv_ovf_i2, 		typeof(ConversionInstruction) },
            /* 0x0B6 */ { OpCode.Conv_ovf_u2, 		typeof(ConversionInstruction) },
            /* 0x0B7 */ { OpCode.Conv_ovf_i4, 		typeof(ConversionInstruction) },
            /* 0x0B8 */ { OpCode.Conv_ovf_u4, 		typeof(ConversionInstruction) },
            /* 0x0B9 */ { OpCode.Conv_ovf_i8, 		typeof(ConversionInstruction) },
            /* 0x0BA */ { OpCode.Conv_ovf_u8, 		typeof(ConversionInstruction) },
            /* Opcodes 0x0BB-0x0C1 are undefined */
            /* 0x0C2 */ { OpCode.Refanyval,         typeof(RefanyvalInstruction) },
            /* 0x0C3 */ { OpCode.Ckfinite, 			typeof(UnaryArithmeticInstruction) },
            /* Opcodes 0x0C4-0x0C5 are undefined */
            /* 0x0C6 */ { OpCode.Mkrefany,          typeof(MkrefanyInstruction) },
            /* Opcodes 0x0C7-0x0CF are reserved */
            /* 0x0D0 */ { OpCode.Ldtoken,           typeof(LdtokenInstruction) },
            /* 0x0D1 */ { OpCode.Conv_u2, 			typeof(ConversionInstruction) },
            /* 0x0D2 */ { OpCode.Conv_u1, 			typeof(ConversionInstruction) },
            /* 0x0D3 */ { OpCode.Conv_i, 			typeof(ConversionInstruction) },
            /* 0x0D4 */ { OpCode.Conv_ovf_i, 		typeof(ConversionInstruction) },
            /* 0x0D5 */ { OpCode.Conv_ovf_u, 		typeof(ConversionInstruction) },
            /* 0x0D6 */ { OpCode.Add_ovf,           typeof(ArithmeticOverflowInstruction) },
            /* 0x0D7 */ { OpCode.Add_ovf_un,        typeof(ArithmeticOverflowInstruction) },
            /* 0x0D8 */ { OpCode.Mul_ovf,           typeof(ArithmeticOverflowInstruction) },
            /* 0x0D9 */ { OpCode.Mul_ovf_un,        typeof(ArithmeticOverflowInstruction) },
            /* 0x0DA */ { OpCode.Sub_ovf,           typeof(ArithmeticOverflowInstruction) },
            /* 0x0DB */ { OpCode.Sub_ovf_un,        typeof(ArithmeticOverflowInstruction) },
            /* 0x0DC */ { OpCode.Endfinally,        typeof(EndfinallyInstruction) },
            /* 0x0DD */ { OpCode.Leave,             typeof(LeaveInstruction) },
            /* 0x0DE */ { OpCode.Leave_s,           typeof(LeaveInstruction) },
            /* 0x0DF */ { OpCode.Stind_i,           typeof(StobjInstruction) },
		    /* 0x0E0 */ { OpCode.Conv_u,            typeof(ConversionInstruction) },
            /* Opcodes 0xE1-0xFF are reserved */
            /* 0x100 */ { OpCode.Arglist,           typeof(ArglistInstruction) },
            /* 0x101 */ { OpCode.Ceq,               typeof(BinaryComparisonInstruction) },
            /* 0x102 */ { OpCode.Cgt,               typeof(BinaryComparisonInstruction) },
            /* 0x103 */ { OpCode.Cgt_un,            typeof(BinaryComparisonInstruction) },
            /* 0x104 */ { OpCode.Clt,               typeof(BinaryComparisonInstruction) },
            /* 0x105 */ { OpCode.Clt_un,            typeof(BinaryComparisonInstruction) },
            /* 0x106 */ { OpCode.Ldftn,             typeof(LdftnInstruction) },
            /* 0x107 */ { OpCode.Ldvirtftn,         typeof(LdvirtftnInstruction) },
            /* Opcode 0x108 is undefined. */
            /* 0x109 */ { OpCode.Ldarg,             typeof(LdargInstruction) },
            /* 0x10A */ { OpCode.Ldarga,            typeof(LdargaInstruction) },
            /* 0x10B */ { OpCode.Starg,             typeof(StargInstruction) },
            /* 0x10C */ { OpCode.Ldloc,             typeof(LdlocInstruction) },
            /* 0x10D */ { OpCode.Ldloca,            typeof(LdlocaInstruction) },
            /* 0x10E */ { OpCode.Stloc,             typeof(StlocInstruction) },
            /* 0x10F */ { OpCode.Localalloc,        typeof(LocalallocInstruction) },
            /* Opcode 0x110 is undefined */
            /* 0x111 */ { OpCode.Endfilter,         typeof(EndfilterInstruction) },
            /* 0x112 */ { OpCode.PreUnaligned,      typeof(UnalignedPrefixInstruction) },
            /* 0x113 */ { OpCode.PreVolatile,       typeof(PrefixInstruction) },
            /* 0x114 */ { OpCode.PreTail,           typeof(PrefixInstruction) },
            /* 0x115 */ { OpCode.InitObj,           typeof(InitObjInstruction) },
            /* 0x116 */ { OpCode.PreConstrained,    typeof(ConstrainedPrefixInstruction) },
            /* 0x117 */ { OpCode.Cpblk,             typeof(CpblkInstruction) },
            /* 0x118 */ { OpCode.Initblk,           typeof(InitblkInstruction) },
            /* 0x119 */ { OpCode.PreNo,             typeof(NoPrefixInstruction) },
            /* 0x11A */ { OpCode.Rethrow,           typeof(RethrowInstruction) },
            /* Opcode 0x11B is undefined */
            /* 0x11C */ { OpCode.Sizeof,            typeof(SizeofInstruction) },
            /* 0x11D */ { OpCode.Refanytype,        typeof(RefanytypeInstruction) },
            /* 0x11E */ { OpCode.PreReadOnly,       typeof(PrefixInstruction) }
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
        private List<Instruction> _instructions = new List<Instruction>();

        #endregion // Data members

        #region Construction

        /// <summary>
        /// The instruction factory used to emit instructions.
        /// </summary>
        public ILDecodingStage()
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
            MethodHeader header = new MethodHeader();

            // Check preconditions
            if (null == compiler)
                throw new ArgumentNullException(@"compiler");

            //Debug.WriteLine(@"Decoding " + compiler.Type.ToString() + "." + compiler.Method.ToString());

            using (Stream code = compiler.GetInstructionStream())
            using (BinaryReader reader = new BinaryReader(code))
            {
                _compiler = compiler;
                _method = compiler.Method;
                _codeReader = reader;

                ReadMethodHeader(reader, ref header);
                //Debug.WriteLine("Decoding " + compiler.Method.ToString());

                if (0 != header.localsSignature)
                {
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
        /// Reads the method header from the instruction stream.
        /// </summary>
        /// <param name="reader">The reader used to decode the instruction stream.</param>
        /// <param name="header">The method header structure to populate.</param>
        private void ReadMethodHeader(BinaryReader reader, ref MethodHeader header)
        {
            header.flags = (MethodFlags)reader.ReadByte();
            switch (header.flags & MethodFlags.HeaderMask)
            {
                case MethodFlags.TinyFormat:
                    header.codeSize = ((uint)(header.flags & MethodFlags.TinyCodeSizeMask) >> 2);
                    header.flags &= MethodFlags.HeaderMask;
                    break;

                case MethodFlags.FatFormat:
                    header.flags = (MethodFlags)(reader.ReadByte() << 8 | (byte)header.flags);
                    if (MethodFlags.ValidHeader != (header.flags & MethodFlags.HeaderSizeMask))
                        throw new InvalidDataException(@"Invalid method header.");
                    header.maxStack = reader.ReadUInt16();
                    header.codeSize = reader.ReadUInt32();
                    header.localsSignature = (TokenTypes)reader.ReadUInt32();
                    break;

                default:
                    throw new InvalidDataException(@"Invalid method header.");
            }

            // Are there sections following the code?
            if (MethodFlags.MoreSections == (header.flags & MethodFlags.MoreSections))
            {
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
                EhClause clause = new EhClause();

                do
                {
                    flags = reader.ReadByte();
                    isFat = (0x40 == (flags & 0x40));
                    if (true == isFat)
                    {
                        byte[] buffer = new byte[4];
                        reader.Read(buffer, 0, 3);
                        length = LittleEndianBitConverter.GetInt32(buffer, 0);
                        blocks = (length - 4) / 24;
                    }
                    else
                    {
                        length = reader.ReadByte();
                        blocks = (length - 4) / 12;

                        /* Read & skip the padding. */
                        reader.ReadInt16();
                    }

                    Debug.Assert(0x01 == (flags & 0x3F), @"Unsupported method datta section.");
                    // Read the clause
                    for (int i = 0; i < blocks; i++)
                    {
                        clause.Read(reader, isFat);
                        // FIXME: Create proper basic blocks for each item in the clause
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
        /// <param name="header">The method header.</param>
        private void Decode(IMethodCompiler compiler, ref MethodHeader header)
        {
            // Instruction object decoded for an IL instruction
            ILInstruction instruction = null;
            // The opcode
            OpCode op;
            // ILInstruction offset
            int instOffset;
            // Start of the code stream
            long codeStart = _codeReader.BaseStream.Position;
            // End of the code stream
            long codeEnd = _codeReader.BaseStream.Position + header.codeSize;
            // AssemblyCompiler target architecture (which fortunately has a vote in IR instruction representation)
            IArchitecture architecture = compiler.Architecture;
            // Branch instruction to patch offsets
            IBranchInstruction branch = null;
            // Prefix instruction
            PrefixInstruction prefix = null;
            // Operand stack for IL instructions
            Stack<Operand> ilStack = new Stack<Operand>();
            // Operand array
            Operand[] ops;
            int opCount;
            // Loop index
            int i;

            while (codeEnd != _codeReader.BaseStream.Position)
            {
                // Determine the instruction offset
                instOffset = (int)(_codeReader.BaseStream.Position - codeStart);

                // Read the next opcode from the stream
                op = (OpCode)_codeReader.ReadByte();
                if (OpCode.Extop == op)
                    op = (OpCode)(0x100 | _codeReader.ReadByte());

                // Create and initialize the corresponding instruction
                instruction = CreateInstruction(op);
                instruction.Decode(this);
                instruction.Offset = instOffset;
                instruction.Prefix = prefix;

                // Assign the operands of the instruction from the IL stack
                //ops = instruction.Operands;
                opCount = instruction.Operands.Length;
                while (0 != opCount--)
                {
                    if (null == instruction.Operands[opCount])
                    {
                        instruction.SetOperand(opCount, ilStack.Pop());
                    }
                }

                // Validate the instruction
                instruction.Validate(_compiler);

                // Push the result operands on the IL stack
                ops = instruction.Results;
                if (null != ops && true == instruction.PushResult && 0 != ops.Length)
                {
                    foreach (Operand operand in ops)
                        ilStack.Push(operand);
                }

                // Do we need to patch branch targets?
                branch = instruction as IBranchInstruction;
                if (null != branch)
                {
                    int pc = (int)(_codeReader.BaseStream.Position - codeStart);
                    int[] targets = branch.BranchTargets;
                    for (i = 0; i < targets.Length; i++)
                    {
                        targets[i] += pc;
                    }
                }

                // Add the instruction to the current block
                prefix = instruction as PrefixInstruction;
                if (null == prefix)
                    _instructions.Add(instruction);
            }

            Debug.Assert(0 == ilStack.Count, @"IL stack not empty.");

            // Not necessary anymore, we've removed the TemporaryOperand usage
            // RemoveTemporaries();
            
            // Breaks the basic block analysis
            //RemoveDeadInstructions();
        }

        /// <summary>
        /// Creates an appropriate instruction for the opcode.
        /// </summary>
        /// <param name="op">The IL opcode to create an instruction for.</param>
        /// <returns>An instance  of ILInstruction, that represents the opcode.</returns>
        private ILInstruction CreateInstruction(OpCode op)
        {
            ILInstruction result = null;
            Type instructionType = null;
            if (false == s_opcodeMap.TryGetValue(op, out instructionType))
                throw new InvalidProgramException();

            result = _compiler.Architecture.CreateInstruction(instructionType, op) as ILInstruction;
            if (null == result)
                throw new InvalidOperationException(@"Architecture created invalid IL instruction type.");
            Debug.Assert(instructionType.IsAssignableFrom(result.GetType()), @"Wrong instruction type returned by the architecture!");
            return result;
        }

        /// <summary>
        /// Removes dead instructions from the instruction list.
        /// </summary>
        private void RemoveDeadInstructions()
        {
            // Iterate all blocks
            for (int index = 0; index < _instructions.Count; index++)
            {
                if (true == _instructions[index].Ignore)
                {
                    _instructions.RemoveAt(index--);
                }
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
            get { return _instructions; }
        }

        /// <summary>
        /// Gibt einen Enumerator zurück, der die Auflistung durchläuft.
        /// </summary>
        /// <returns>
        /// Ein <see cref="T:System.Collections.Generic.IEnumerator`1"/>, der zum Durchlaufen der Auflistung verwendet werden kann.
        /// </returns>
        public IEnumerator<Instruction> GetEnumerator()
        {
            return _instructions.GetEnumerator();
        }

        /// <summary>
        /// Gibt einen Enumerator zurück, der eine Auflistung durchläuft.
        /// </summary>
        /// <returns>
        /// Ein <see cref="T:System.Collections.IEnumerator"/>-Objekt, das zum Durchlaufen der Auflistung verwendet werden kann.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _instructions.GetEnumerator();
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
        /// Decodes <paramref name="value"/> from the instruction stream.
        /// </summary>
        /// <param name="value">Receives the decoded value from the instruction stream.</param>
        void IInstructionDecoder.Decode(out byte value)
        {
            value = _codeReader.ReadByte();
        }

        /// <summary>
        /// Decodes <paramref name="value"/> from the instruction stream.
        /// </summary>
        /// <param name="value">Receives the decoded value from the instruction stream.</param>
        void IInstructionDecoder.Decode(out sbyte value)
        {
            value = _codeReader.ReadSByte();
        }

        /// <summary>
        /// Decodes <paramref name="value"/> from the instruction stream.
        /// </summary>
        /// <param name="value">Receives the decoded value from the instruction stream.</param>
        void IInstructionDecoder.Decode(out short value)
        {
            value = _codeReader.ReadInt16();
        }

        /// <summary>
        /// Decodes <paramref name="value"/> from the instruction stream.
        /// </summary>
        /// <param name="value">Receives the decoded value from the instruction stream.</param>
        void IInstructionDecoder.Decode(out ushort value)
        {
            value = _codeReader.ReadUInt16();
        }

        /// <summary>
        /// Decodes <paramref name="value"/> from the instruction stream.
        /// </summary>
        /// <param name="value">Receives the decoded value from the instruction stream.</param>
        void IInstructionDecoder.Decode(out int value)
        {
            value = _codeReader.ReadInt32();
        }

        /// <summary>
        /// Decodes <paramref name="value"/> from the instruction stream.
        /// </summary>
        /// <param name="value">Receives the decoded value from the instruction stream.</param>
        void IInstructionDecoder.Decode(out uint value)
        {
            value = _codeReader.ReadUInt32();
        }

        /// <summary>
        /// Decodes <paramref name="value"/> from the instruction stream.
        /// </summary>
        /// <param name="value">Receives the decoded value from the instruction stream.</param>
        void IInstructionDecoder.Decode(out long value)
        {
            value = _codeReader.ReadInt64();
        }

        /// <summary>
        /// Decodes <paramref name="value"/> from the instruction stream.
        /// </summary>
        /// <param name="value">Receives the decoded value from the instruction stream.</param>
        void IInstructionDecoder.Decode(out float value)
        {
            value = _codeReader.ReadSingle();
        }

        /// <summary>
        /// Decodes <paramref name="value"/> from the instruction stream.
        /// </summary>
        /// <param name="value">Receives the decoded value from the instruction stream.</param>
        void IInstructionDecoder.Decode(out double value)
        {
            value = _codeReader.ReadDouble();
        }

        /// <summary>
        /// Decodes <paramref name="value"/> from the instruction stream.
        /// </summary>
        /// <param name="value">Receives the decoded value from the instruction stream.</param>
        void IInstructionDecoder.Decode(out TokenTypes value)
        {
             value = (TokenTypes)_codeReader.ReadInt32();
        }

        #endregion
    }
}
