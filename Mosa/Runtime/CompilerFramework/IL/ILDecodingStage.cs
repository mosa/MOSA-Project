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
        private MethodCompilerBase _compiler;

        /// <summary>
        /// Optional signature of stack local variables.
        /// </summary>
        private LocalVariableSignature _localsSig;

        /// <summary>
        /// The method implementation of the currently compiled method.
        /// </summary>
        private RuntimeMethod _method;

        /// <summary>
        /// List of instructions decoded by the decoder.
        /// </summary>
        private List<Instruction> _instructions = new List<Instruction>();

        /// <summary>
        /// Holds a list of operands, which represent method parameters.
        /// </summary>
        private List<Operand> _parameters;

        /// <summary>
        /// Holds a list of operands, which represent local variables.
        /// </summary>
        private List<Operand> _locals;

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

        /// <summary>
        /// Returns the binary reader to access the code stream.
        /// </summary>
        public BinaryReader CodeReader
        {
            get { return _codeReader; }
        }

        /// <summary>
        /// Retrieves the current compiler context.
        /// </summary>
        public MethodCompilerBase Compiler
        {
            get { return _compiler; }
        }

        /// <summary>
        /// Returns the currently compiled method.
        /// </summary>
        public RuntimeMethod Method
        {
            get { return _method; }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Retrieves an Operand, that represents a parameter of the current method.
        /// </summary>
        /// <param name="index">The parameter index to retrieve.</param>
        /// <returns>An operand, that represents the requested parameter.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The <paramref name="index"/> is invalid for the method.</exception>
        public Operand GetParameterOperand(int index)
        {
            // HACK: Returning a new instance here breaks object identity. We should reuse operands,
            // which represent the same memory location. If we need to move a variable in an optimization
            // stage to a different memory location, it should actually be a new one so sharing object
            // only saves runtime space/perf.

            MethodSignature sig = _method.Signature;
            if (true == sig.HasThis || true == sig.HasExplicitThis)
            {
                if (0 == index)
                {
                    return new ParameterOperand(_compiler.Architecture.StackFrameRegister, new RuntimeParameter(_method.Module, @"this", 0, ParameterAttributes.In), new ClassSigType(_compiler.Type.Token));
                }
                else
                {
                    // Decrement the index, as the caller actually wants a real parameter
                    index--;
                }
            }

            // A normal argument, decode it...
            IList<RuntimeParameter> parameters = _method.Parameters;
            Debug.Assert(null != parameters, @"Method doesn't have arguments.");
            Debug.Assert(index < parameters.Count, @"Invalid argument index requested.");
            if (null == parameters || parameters.Count <= index)
                throw new ArgumentOutOfRangeException(@"index", index, @"Invalid parameter index");

            Operand param = null;
            if (_parameters.Count > index)
                param = _parameters[index];

            if (null == param)
            {
                param = new ParameterOperand(_compiler.Architecture.StackFrameRegister, parameters[index], sig.Parameters[index]);
                _parameters[index] = param;
            }

            return param;
        }

        /// <summary>
        /// Retrieves an operand, which represents a local variable in the method being compiled.
        /// </summary>
        /// <param name="index">The index of the local variable.</param>
        /// <returns>An operand object, which represents the local variable.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The <paramref name="index"/> is invalid for the method.</exception>
        public Operand GetLocalOperand(int index)
        {
            // HACK: Returning a new instance here breaks object identity. We should reuse operands,
            // which represent the same memory location. If we need to move a variable in an optimization
            // stage to a different memory location, it should actually be a new one so sharing object
            // only saves runtime space/perf.
            Debug.Assert(null != _localsSig, @"Method doesn't have locals.");
            Debug.Assert(index <= _localsSig.Types.Length, @"Invalid local index requested.");
            if (null == _localsSig || _localsSig.Types.Length <= index)
                throw new ArgumentOutOfRangeException(@"index", index, @"Invalid parameter index");

            Operand local = null;
            if (_locals.Count > index)
                local = _locals[index];

            if (null == local)
            {
                local = new LocalVariableOperand(_compiler.Architecture.StackFrameRegister, String.Format("L_{0}", index), index, _localsSig.Types[index]);
                _locals[index] = local;
            }

            return local;
        }

        #endregion // Methods

        #region IMethodCompilerStage Members

        string IMethodCompilerStage.Name
        {
            get { return @"CIL decoder"; }
        }

        void IMethodCompilerStage.Run(MethodCompilerBase compiler)
        {
            // The size of the code in bytes
            MethodHeader header = new MethodHeader();

            // Check preconditions
            if (null == compiler)
                throw new ArgumentNullException(@"compiler");

            //Debug.Assert(0x06002448 != compiler.Method.Token);
            //Debug.Assert(0x0600000E != compiler.Method.Token);
            Debug.WriteLine(@"Decoding " + compiler.Type.ToString() + "." + compiler.Method.ToString());

            using (Stream code = compiler.GetInstructionStream())
            using (BinaryReader reader = new BinaryReader(code))
            {
                _compiler = compiler;
                _method = compiler.Method;
                _codeReader = reader;
                _parameters = new List<Operand>(new Operand[_method.Parameters.Count]);

                ReadMethodHeader(reader, ref header);
                //Debug.WriteLine("Decoding " + compiler.Method.ToString());

                if (0 != header.localsSignature)
                {
                    StandAloneSigRow row;
                    IMetadataProvider md = _method.Module.Metadata;
                    md.Read(header.localsSignature, out row);
                    _localsSig = LocalVariableSignature.Parse(md, row.SignatureBlobIdx);
                    _locals = new List<Operand>(new Operand[_localsSig.Types.Length]);
                }

                /* Decode the instructions */
                Decode(compiler, ref header);

                /* Remove temporaries stored into locals */
                //RemoveShortTemporaries();

                /* Remove dead instructions */
                //RemoveDeadInstructions();

                // When we leave, the operand stack must only contain the locals...
                //Debug.Assert(_operandStack.Count == _method.Locals.Count);
                _codeReader = null;
                //_method = null;
                _compiler = null;
            }
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
                        length = BitConverter.ToInt32(buffer, 0);
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
        /// <param name="reader">The reader for the instruction stream.</param>
        /// <param name="header">The method header.</param>
        private void Decode(MethodCompilerBase compiler, ref MethodHeader header)
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
                for (i = 0; i < opCount; i++)
                {
                    instruction.SetOperand(i, ilStack.Pop());
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

        /// <summary>
        /// Removes short lived virtual registers from the instruction stream.
        /// </summary>
        /// <remarks>
        /// The purpose of this function is it to make later register allocation easier. We also
        /// change the semantics of certain IL instructions, that usually place their result on the
        /// operand stack. In the case that their result is directly followed by a store instruction,
        /// we remove the store instruction in favor of a direct store by the previous instruction.
        /// </remarks>
        private void RemoveTemporaries()
        {
            Instruction prevInst;
            IStoreInstruction store;

            // Iterate all blocks
            for (int index = _instructions.Count - 1; 0 < index; index--)
            {
                store = _instructions[index] as IStoreInstruction;
                if (null != store && store.Operands[0] is TemporaryOperand)
                {
                    // Check if the previous statement creates the virtual register...
                    prevInst = _instructions[index - 1];
                    if (null != prevInst && true == System.Object.ReferenceEquals(prevInst.Results[0], store.Operands[0]))
                    {
                        prevInst.Results[0] = store.Results[0];
                        store.Ignore = true;
                        index--;
                    }
                }
            }
        }

        #endregion // Internals

        #region IInstructionsProvider members

        List<Instruction> IInstructionsProvider.Instructions
        {
            get { return _instructions; }
        }

        public IEnumerator<Instruction> GetEnumerator()
        {
            return _instructions.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _instructions.GetEnumerator();
        }

        #endregion //  IInstructionsProvider members

        #region IInstructionDecoder members

        //public AssemblyCompiler AssemblyCompiler
        //{
        //    get { return _compiler.AssemblyCompiler; }
        //}

        public IArchitecture Architecture
        {
            get { return _compiler.Architecture; }
        }

        public IMetadataProvider Metadata
        {
            get { return _compiler.Assembly.Metadata; }
        }

        public byte DecodeByte()
        {
            return _codeReader.ReadByte();
        }

        public sbyte DecodeSByte()
        {
            return _codeReader.ReadSByte();
        }

        public short DecodeInt16()
        {
            return _codeReader.ReadInt16();
        }

        public ushort DecodeUInt16()
        {
            return _codeReader.ReadUInt16();
        }

        public int DecodeInt32()
        {
            return _codeReader.ReadInt32();
        }

        public uint DecodeUInt32()
        {
            return _codeReader.ReadUInt32();
        }

        public long DecodeInt64()
        {
            return _codeReader.ReadInt64();
        }

        public float DecodeSingle()
        {
            return _codeReader.ReadSingle();
        }

        public double DecodeDouble()
        {
            return _codeReader.ReadDouble();
        }

        public TokenTypes DecodeToken()
        {
            return (TokenTypes)_codeReader.ReadUInt32();
        }

        public int GetStackSize()
        {
            return _locals.Count;
        }

        #endregion // IInstructionDecoder members
    }

#if OLD
        /// <summary>
        /// Decodes the instruction stream of the reader and populates the compiler.
        /// </summary>
        /// <param name="compiler">The compiler to populate.</param>
        /// <param name="reader">The reader for the instruction stream.</param>
        /// <param name="header">The method header.</param>
        private void Decode(MethodCompiler compiler, ref MethodHeader header)
		{
            // IR instruction decoded for an IL instruction
            Instruction instruction = null;
            // The opcode
			OpCode op;
            // Instruction offset
            int instOffset;
            // Start of the code stream
            long codeStart = _codeReader.BaseStream.Position;
			// End of the code stream
			long codeEnd = _codeReader.BaseStream.Position + header.codeSize;
            // Compiler target architecture (which fortunately has a vote in IR instruction representation)
            IArchitecture architecture = compiler.Architecture;
            // Branch instruction
            IBranchInstruction branch;
            // Retrieve method instructions
            List<Instruction> instructions;
            // Block schedule
            List<DecoderBasicBlock> scheduledBlocks = new List<DecoderBasicBlock>();
            // Next block
            DecoderBasicBlock currentBlock, nextBlock;

            /*
             * This method traces execution of a function, creates instructions for every
             * encountered IL opcode and basic blocks as appropriate. It follows binary branches
             * in the order true and then false. It schedules block decoding to run sequentially
             * and performs automatic SameInstruction insertion if it encounters a reused block.
             * 
             */

            // FIXME: Debug.Assert(0 == _blocks.Count);
            _blocks.Clear();
            _operandStack.Clear();

            // Create an initial block
            scheduledBlocks.Add(FindOrCreateBlock(null, 0));

            // Iterate all scheduled blocks to decode
            while (0 != scheduledBlocks.Count)
            {
                currentBlock = scheduledBlocks[0]; 
                scheduledBlocks.RemoveAt(0);
                nextBlock = (0 != scheduledBlocks.Count ? scheduledBlocks[0] : null);
                instructions = currentBlock.Instructions;

                _blocks.Add(currentBlock);

                // Position the code reader to the block label
                _codeReader.BaseStream.Position = codeStart + currentBlock.Label;

                // Iterate all instructions until the block ends
                while (null != currentBlock)
                {
                    // Determine the instruction offset
                    instOffset = (int)(_codeReader.BaseStream.Position - codeStart);

                    // Check if this is a new block
                    if (null != nextBlock && instOffset == nextBlock.Label)
                    {
                        // Add an unconditional branch to this block
                        // FIXME: Use IArchitecture for this
                        instruction = new BranchInstruction(OpCode.Br_s, new int[] { instOffset });
                        instructions.Add(instruction);

                        // Save the out stacks
                        currentBlock.OutStack = _operandStack.ToArray();
                        Array.Reverse(currentBlock.OutStack);
                        LinkBlocks(currentBlock, nextBlock);

                        // Done, the stack should match...
                        currentBlock = null;
                        continue;
                    }

                    // Read the next opcode from the stream
                    op = (OpCode)_codeReader.ReadByte();
                    if (OpCode.Extop == op)
                        op = (OpCode)(0x100 | _codeReader.ReadByte());

                    // Create and initialize the corresponding instruction
                    instruction = architecture.CreateInstruction(op);
                    instruction.Initialize(this);
                    instruction.Offset = instOffset;

                    // Add the instruction to the current block
                    instructions.Add(instruction);

                    // Does this instruction terminate a block?
                    if (0 != (FlowControl.BlockEndMask & instruction.FlowControl))
                    {
                        // Complete the current block by saving its stack to be able to
                        // properly flow the stack to the following blocks
                        currentBlock.OutStack = _operandStack.ToArray();
                        Array.Reverse(currentBlock.OutStack);

                        branch = instruction as IBranchInstruction;
                        if (null != branch)
                        {
                            // Next instruction pc
                            int pc = (int)(_codeReader.BaseStream.Position - codeStart);

                            // Fixup branch targets...
                            FixBranchTargets(scheduledBlocks, currentBlock, branch.BranchTargets, pc);
                        }

                        // Clear the current block
                        currentBlock = null;
                    }
                }
            }
        }

            // Iterate all IL instructions until we're done
			while (codeEnd != _codeReader.BaseStream.Position)
			{
                // Determine the instruction offset
                instOffset = (int)(_codeReader.BaseStream.Position - codeStart);

                // Check if this is a new block
                if (0 != scheduledBlocks.Count && instOffset == scheduledBlocks[0].Label)
                {
                    // Retrieve the next block
                    DecoderBasicBlock nextBlock = scheduledBlocks[0];
                    scheduledBlocks.RemoveAt(0);

                    // Add an unconditional branch to the next block to the current block
                    BranchInstruction dummyBranch = new BranchInstruction(OpCode.Br_s, new int[] { nextBlock.Label });
                    instructions.Add(dummyBranch);

                    // Save the branch label and link the blocks
                    currentBlock.OutStack = _operandStack.ToArray();
                    Array.Reverse(currentBlock.OutStack);
                    LinkBlocks(currentBlock, nextBlock);

                    // Done, stack should match...
                    currentBlock = nextBlock;
                    instructions = currentBlock.Instructions;

                    // Rewire the stack...
                    RewireStack(currentBlock);
                }

				// Read the opcode from the stream
                op = (OpCode)_codeReader.ReadByte();
				if (OpCode.Extop == op)
                    op = (OpCode)(0x100 | _codeReader.ReadByte());

                // Create and initialize the corresponding instruction
                instruction = architecture.CreateInstruction(op);
                instruction.Initialize(this);
                instruction.Offset = instOffset;

                // Add the instruction to the current block
                instructions.Add(instruction);

                // Is this a branch instruction?
                branch = instruction as IBranchInstruction;
                if (null != branch)
                {
                    // Complete the current block by saving its stack to be able to
                    // properly flow the stack to the following blocks
                    currentBlock.OutStack = _operandStack.ToArray();
                    Array.Reverse(currentBlock.OutStack);

                    // Next instruction pc
                    int pc = (int)(_codeReader.BaseStream.Position - codeStart);

                    // Fixup branch targets...
                    FixBranchTargets(scheduledBlocks, currentBlock, branch.BranchTargets, pc);

                    // The branch instruction terminates the current block,
                    // get one for the following block.
                    currentBlock = FindOrCreateBlock(currentBlock, pc);
                    if (0 != currentBlock.PreviousBlocks.Count)
                    {
                        _operandStack.Clear();
                        foreach (Operand stackOp in ((DecoderBasicBlock)currentBlock.PreviousBlocks[0]).OutStack)
                            _operandStack.Push(stackOp);
                        RewireStack(currentBlock);
                    }
                    instructions = currentBlock.Instructions;
                }
            }

            // Sort the blocks by instruction index 
            // (brings blocks defined by backwards branches in order)
            _blocks.Sort(delegate(DecoderBasicBlock i1, DecoderBasicBlock i2)
            {
                return (i1.Label - i2.Label);
            });
		}

        private void FixBranchTargets(List<DecoderBasicBlock> scheduledBlocks, DecoderBasicBlock currentBlock, int[] targets, int pc)
        {
            if (null == currentBlock)
                throw new ArgumentNullException(@"currentBlock");
            if (null == targets)
                throw new ArgumentNullException(@"targets");

            DecoderBasicBlock targetBlock;
            bool schedule = true;

            for (int i = targets.Length - 1; 0 <= i; i--)
            {
                // Calculate the absolute target
                Debug.Assert(targets[i] >= 0);
                targets[i] += pc;

                // Create the target block
                targetBlock = FindOrCreateBlock(currentBlock, targets[i]);

                // Check for a forward branch, schedule the block
                int index = scheduledBlocks.FindIndex((Predicate<DecoderBasicBlock>)delegate(DecoderBasicBlock match)
                {
                    if (match.Label == targets[i])
                        schedule = false;

                    return (match.Label > targets[i]);
                });

                // Schedule a new block...
                if (true == schedule)
                {
                    scheduledBlocks.Insert(0, targetBlock);
                }
            }
        }

        private void RewireStack(BasicBlock currentBlock)
        {
            SameInstruction same = null;
            foreach (Instruction instr in currentBlock.Instructions)
            {
                same = instr as SameInstruction;
                if (null == same)
                    break;

                same.Initialize(this);
            }
        }

        /// <summary>
        /// Links two blocks together.
        /// </summary>
        /// <param name="prev">The previous block, the one from which control flows.</param>
        /// <param name="next">The next block, the one receiving control flow.</param>
        private void LinkBlocks(DecoderBasicBlock prev, DecoderBasicBlock next)
        {
        }

        /// <summary>
        /// Finds the labeled block and if it doesn't exist yet, it creates one.
        /// </summary>
        /// <param name="caller">The basic block, which performs a jump into the new block.</param>
        /// <param name="label">The label of the block.</param>
        /// <returns>The basic block corresponding to the label.</returns>
        private DecoderBasicBlock FindOrCreateBlock(DecoderBasicBlock caller, int label)
        {
            // Return value
            DecoderBasicBlock result;
            // Flag, if we need to split an existing block
            int split = -1;

            // Attempt a lookup on the label
            result = _blocks.Find((Predicate<DecoderBasicBlock>)delegate(DecoderBasicBlock match)
            {
                bool b = (match.Label == label);
                if (false == b)
                {
                    // Check if we need to split this block...
                    List<Instruction> instructions = match.Instructions;
                    if (0 != instructions.Count && instructions[0].Offset < label && label <= instructions[instructions.Count - 1].Offset)
                    {
                        split = instructions.FindIndex((Predicate<Instruction>)delegate(Instruction inst) {
                            return (inst.Offset == label);
                        });
                        b = (split != -1);
                    }
                }

                return b;
            });

            // Did we find one?
            if (null == result)
            {
                // No, create a new block
                result = new DecoderBasicBlock(_method.StackSlots, label);

                // Add the block to the list
                _blocks.Add(result);
            }
            else if (-1 != split)
            {
                result = SplitBasicBlock(result, split);
                //result = (DecoderBasicBlock)result.Split(split);
                _blocks.Add(result);
            }

            // Hook the blocks
            if (null != caller)
                LinkBlocks(caller, result);

            return result;
        }

        private DecoderBasicBlock SplitBasicBlock(DecoderBasicBlock result, int split)
        {
            /*
             * This method needs to split a basic block. In order to be able to split
             * the basic block, this method needs to rewind the operand stack in 
             * backwards order so that it can create the outstack for the first half
             * of the split block. The second half keeps its outblock as it isn't assumed
             * to change due to splitting. If it does, we'll have to reevaluate.
             * 
             */
            return null;
        }

        /// <summary>
        /// Removes dead instructions from the instruction list.
        /// </summary>
        private void RemoveDeadInstructions()
        {
            // Iterate all blocks
            foreach (BasicBlock block in _blocks)
            {
                List<Instruction> instr = block.Instructions;
                for (int index = 0; index < instr.Count; index++)
                {
                    if (true == instr[index].Ignore)
                    {
                        instr.RemoveAt(index--);
                    }
                }
            }
        }

        /// <summary>
        /// Removes temporary load and physical store sequences, by storing directly from the operation.
        /// </summary>
        private void RemoveShortTemporaries()
        {
            StoreInstruction store;
            List<Instruction> instr;
            
            // Iterate all blocks
            foreach (BasicBlock block in _blocks)
            {
                instr = block.Instructions;
                for (int index = instr.Count - 1; 0 < index; index--)
                {
                    store = instr[index] as StoreInstruction;
                    if (null != store && store.Operand is VirtualRegister)
                    {
                        // Check if the previous statement creates the virtual register...
                        IOperandDefinition opdef = instr[index - 1] as IOperandDefinition;
                        if (null != opdef && true == Object.ReferenceEquals(opdef.Result, store.Operand))
                        {
                            opdef.Result = store.Result;
                            store.Ignore = true;
                            index--;
                        }
                    }
                }
            }
        }

        int IBasicBlockProvider.Count
        {
            get { return _blocks.Count; }
        }

        BasicBlock IBasicBlockProvider.this[int index]
        {
            get { return _blocks[index]; }
        }

        IEnumerator<BasicBlock> IEnumerable<BasicBlock>.GetEnumerator()
        {
            // For some reason covariance doesn't seem to work here, have
            // to manually yield results :(
            foreach (DecoderBasicBlock block in _blocks)
                yield return block;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _blocks.GetEnumerator();
        }

#endif // #if OLD
}
