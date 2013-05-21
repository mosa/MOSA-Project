/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using Mosa.Platform.x86.Intrinsic;

namespace Mosa.Kernel.x86.RealModeEmulator
{
    public static partial class RealEmulator
    {
        public class Operation
        {
            public readonly string Name;
            public readonly string Type;
            public OpcodeFunction Function;
            public uint Argument;
            public readonly string[] ModRMNames;

            public unsafe Operation(string name, string type, OpcodeFunction function, uint argument, string[] modRMNames)
            {
                Name = name;
                Type = type;
                Function = function;
                Argument = argument;
                ModRMNames = modRMNames;
            }
        }

        public static const string[] casArithOps = { "ADD", "OR", "ADC", "SBB", "AND", "SUB", "XOR", "CMP" };
        public static const string[] casMiscOps = { "TEST", "M1-", "NOT", "NEG", "MUL", "IMUL", "DIV", "IDIV" };
        public static const string[] casShiftOps = { "ROL", "ROR", "RCL", "RCR", "SHL", "SHR", "SAL", "SAR" };

        public const Operation[] caOperations = {

            new Operation("ADD","MR",Op_ADD_MR,0,null), new Operation("ADD","MRX",Op_ADD_MRX,0,null), new Operation("ADD","RM",Op_ADD_RM,0,null), new Operation("ADD","RMX",Op_ADD_RMX,0,null), new Operation("ADD","AI",Op_ADD_AI,0,null), new Operation("ADD","AIX",Op_ADD_AIX,0,null), new Operation("PUSH","Seg",Op_PUSH_Seg,SRegisters.SREG_ES,null), new Operation("POP","Seg",Op_POP_Seg,SRegisters.SREG_ES,null),

            new Operation("OR","MR",Op_OR_MR,0,null), new Operation("OR","MRX",Op_OR_MRX,0,null), new Operation("OR","RM",Op_OR_RM,0,null), new Operation("OR","RMX",Op_OR_RMX,0,null), new Operation("OR","AI",Op_OR_AI,0,null), new Operation("OR","AIX",Op_OR_AIX,0,null), new Operation("PUSH","Seg",Op_PUSH_Seg,SRegisters.SREG_CS,null), new Operation("Ext","0F",Op_Ext_0F,0,null),

            new Operation("ADC","MR",Op_ADC_MR,0,null), new Operation("ADC","MRX",Op_ADC_MRX,0,null), new Operation("ADC","RM",Op_ADC_RM,0,null), new Operation("ADC","RMX",Op_ADC_RMX,0,null), new Operation("ADC","AI",Op_ADC_AI,0,null), new Operation("ADC","AIX",Op_ADC_AIX,0,null), new Operation("PUSH","Seg",Op_PUSH_Seg,SRegisters.SREG_SS,null), new Operation("Undef","",null,0,null),

            new Operation("SBB","MR",Op_SBB_MR,0,null), new Operation("SBB","MRX",Op_SBB_MRX,0,null), new Operation("SBB","RM",Op_SBB_RM,0,null), new Operation("SBB","RMX",Op_SBB_RMX,0,null), new Operation("SBB","AI",Op_SBB_AI,0,null), new Operation("SBB","AIX",Op_SBB_AIX,0,null), new Operation("PUSH","Seg",Op_PUSH_Seg,SRegisters.SREG_DS,null), new Operation("POP","Seg",Op_POP_Seg,SRegisters.SREG_DS,null),

            new Operation("AND","MR",Op_AND_MR,0,null), new Operation("AND","MRX",Op_AND_MRX,0,null), new Operation("AND","RM",Op_AND_RM,0,null), new Operation("AND","RMX",Op_AND_RMX,0,null), new Operation("AND","AI",Op_AND_AI,0,null), new Operation("AND","AIX",Op_AND_AIX,0,null), new Operation("Ovr","Seg",Op_Ovr_Seg,SRegisters.SREG_ES,null), new Operation("DAA","z",Op_DAA_z,0,null),

            new Operation("SUB","MR",Op_SUB_MR,0,null), new Operation("SUB","MRX",Op_SUB_MRX,0,null), new Operation("SUB","RM",Op_SUB_RM,0,null), new Operation("SUB","RMX",Op_SUB_RMX,0,null), new Operation("SUB","AI",Op_SUB_AI,0,null), new Operation("SUB","AIX",Op_SUB_AIX,0,null), new Operation("Ovr","Seg",Op_Ovr_Seg,SRegisters.SREG_CS,null), new Operation("DAS","z",Op_DAS_z,0,null),

            new Operation("XOR","MR",Op_XOR_MR,0,null), new Operation("XOR","MRX",Op_XOR_MRX,0,null), new Operation("XOR","RM",Op_XOR_RM,0,null), new Operation("XOR","RMX",Op_XOR_RMX,0,null), new Operation("XOR","AI",Op_XOR_AI,0,null), new Operation("XOR","AIX",Op_XOR_AIX,0,null), new Operation("Ovr","Seg",Op_Ovr_Seg,SRegisters.SREG_SS,null), new Operation("AAA","z",Op_AAA_z,0,null),

            new Operation("CMP","MR",Op_CMP_MR,0,null), new Operation("CMP","MRX",Op_CMP_MRX,0,null), new Operation("CMP","RM",Op_CMP_RM,0,null), new Operation("CMP","RMX",Op_CMP_RMX,0,null), new Operation("CMP","AI",Op_CMP_AI,0,null), new Operation("CMP","AIX",Op_CMP_AIX,0,null), new Operation("Ovr","Seg",Op_Ovr_Seg,SRegisters.SREG_DS,null), new Operation("AAS","z",Op_AAS_z,0,null),

            new Operation("INC","Reg",Op_INC_Reg,GPRegistersX.AX,null), new Operation("INC","Reg",Op_INC_Reg,GPRegistersX.CX,null), new Operation("INC","Reg",Op_INC_Reg,GPRegistersX.DX,null), new Operation("INC","Reg",Op_INC_Reg,GPRegistersX.BX,null), new Operation("INC","Reg",Op_INC_Reg,GPRegistersX.SP,null), new Operation("INC","Reg",Op_INC_Reg,GPRegistersX.BP,null), new Operation("INC","Reg",Op_INC_Reg,GPRegistersX.SI,null), new Operation("INC","Reg",Op_INC_Reg,GPRegistersX.DI,null),

            new Operation("DEC","Reg",Op_DEC_Reg,GPRegistersX.AX,null), new Operation("DEC","Reg",Op_DEC_Reg,GPRegistersX.CX,null), new Operation("DEC","Reg",Op_DEC_Reg,GPRegistersX.DX,null), new Operation("DEC","Reg",Op_DEC_Reg,GPRegistersX.BX,null), new Operation("DEC","Reg",Op_DEC_Reg,GPRegistersX.SP,null), new Operation("DEC","Reg",Op_DEC_Reg,GPRegistersX.BP,null), new Operation("DEC","Reg",Op_DEC_Reg,GPRegistersX.SI,null), new Operation("DEC","Reg",Op_DEC_Reg,GPRegistersX.DI,null),

            new Operation("PUSH","Reg",Op_PUSH_Reg,GPRegistersX.AX,null), new Operation("PUSH","Reg",Op_PUSH_Reg,GPRegistersX.CX,null), new Operation("PUSH","Reg",Op_PUSH_Reg,GPRegistersX.DX,null), new Operation("PUSH","Reg",Op_PUSH_Reg,GPRegistersX.BX,null), new Operation("PUSH","Reg",Op_PUSH_Reg,GPRegistersX.SP,null), new Operation("PUSH","Reg",Op_PUSH_Reg,GPRegistersX.BP,null), new Operation("PUSH","Reg",Op_PUSH_Reg,GPRegistersX.SI,null), new Operation("PUSH","Reg",Op_PUSH_Reg,GPRegistersX.DI,null),

            new Operation("POP","Reg",Op_POP_Reg,GPRegistersX.AX,null), new Operation("POP","Reg",Op_POP_Reg,GPRegistersX.CX,null), new Operation("POP","Reg",Op_POP_Reg,GPRegistersX.DX,null), new Operation("POP","Reg",Op_POP_Reg,GPRegistersX.BX,null), new Operation("POP","Reg",Op_POP_Reg,GPRegistersX.SP,null), new Operation("POP","Reg",Op_POP_Reg,GPRegistersX.BP,null), new Operation("POP","Reg",Op_POP_Reg,GPRegistersX.SI,null), new Operation("POP","Reg",Op_POP_Reg,GPRegistersX.DI,null),

            new Operation("PUSH","A",Op_PUSH_A,0,null), new Operation("POP","A",Op_POP_A,0,null), new Operation("Undef","",null,0,null), new Operation("Undef","",null,0,null),

            new Operation("Undef","",null,0,null), new Operation("Undef","",null,0,null), new Operation("Ovr","OpSize",Op_Ovr_OpSize,0,null), new Operation("Ovr","AddrSize",Op_Ovr_AddrSize,0,null),

            new Operation("PUSH","I",Op_PUSH_I,0,null), new Operation("IMUL","MIX",Op_IMUL_MIX,0,null), new Operation("PUSH","I8",Op_PUSH_I8,0,null), new Operation("IMUL","MI8X",Op_IMUL_MI8X,0,null),
            new Operation("IN","SB",Op_IN_SB,0,null), new Operation("IN","SW",Op_IN_SW,0,null), new Operation("OUT","SB",Op_OUT_SB,0,null), new Operation("OUT","SW",Op_OUT_SW,0,null),

            new Operation("JO","S",Op_JO_S,0,null), new Operation("JNO","S",Op_JNO_S,0,null), new Operation("JC","S",Op_JC_S,0,null), new Operation("JNC","S",Op_JNC_S,0,null),
            new Operation("JZ","S",Op_JZ_S,0,null), new Operation("JNZ","S",Op_JNZ_S,0,null), new Operation("JBE","S",Op_JBE_S,0,null), new Operation("JA","S",Op_JA_S,0,null),
            new Operation("JS","S",Op_JS_S,0,null), new Operation("JNS","S",Op_JNS_S,0,null), new Operation("JPE","S",Op_JPE_S,0,null), new Operation("JPO","S",Op_JPO_S,0,null),
            new Operation("JL","S",Op_JL_S,0,null), new Operation("JGE","S",Op_JGE_S,0,null), new Operation("JLE","S",Op_JLE_S,0,null), new Operation("JG","S",Op_JG_S,0,null),
            new Operation("Arith","MI",Op_Arith_MI,0,casArithOps), new Operation("Arith","MIX",Op_Arith_MIX,0,casArithOps), new Operation("Undef","",null,0,null), new Operation("Arith","MI8X",Op_Arith_MI8X,0,casArithOps),
            new Operation("TEST","MR",Op_TEST_MR,0,null), new Operation("TEST","MRX",Op_TEST_MRX,0,null), new Operation("XCHG","RM",Op_XCHG_RM,0,null), new Operation("XCHG","RMX",Op_XCHG_RMX,0,null),
            new Operation("MOV","MR",Op_MOV_MR,0,null), new Operation("MOV","MRX",Op_MOV_MRX,0,null), new Operation("MOV","RM",Op_MOV_RM,0,null), new Operation("MOV","RMX",Op_MOV_RMX,0,null),
            new Operation("MOV","RS",Op_MOV_RS,0,null), new Operation("LEA","z",Op_LEA_z,0,null), new Operation("MOV","SR",Op_MOV_SR,0,null), new Operation("POP","MX",Op_POP_MX,0,null),
            new Operation("XCHG","Reg",Op_XCHG_Reg,GPRegistersX.AX,null), new Operation("XCHG","Reg",Op_XCHG_Reg,GPRegistersX.CX,null), new Operation("XCHG","Reg",Op_XCHG_Reg,GPRegistersX.DX,null), new Operation("XCHG","Reg",Op_XCHG_Reg,GPRegistersX.BX,null), new Operation("XCHG","Reg",Op_XCHG_Reg,GPRegistersX.SP,null), new Operation("XCHG","Reg",Op_XCHG_Reg,GPRegistersX.BP,null), new Operation("XCHG","Reg",Op_XCHG_Reg,GPRegistersX.SI,null), new Operation("XCHG","Reg",Op_XCHG_Reg,GPRegistersX.DI,null),
            new Operation("CBW","z",Op_CBW_z,0,null), new Operation("CWD","z",Op_CWD_z,0,null), new Operation("CALL","F",Op_CALL_F,0,null), new Operation("Undef","",null,0,null),
            new Operation("PUSH","F",Op_PUSH_F,0,null), new Operation("POP","F",Op_POP_F,0,null), new Operation("Flag","SAHF",Op_Flag_SAHF,0,null), new Operation("Flag","LAHF",Op_Flag_LAHF,0,null),
            new Operation("MOV","AMo",Op_MOV_AMo,0,null), new Operation("MOV","AMoX",Op_MOV_AMoX,0,null), new Operation("MOV","MoA",Op_MOV_MoA,0,null), new Operation("MOV","MoAX",Op_MOV_MoAX,0,null),
            new Operation("MOV","SB",Op_MOV_SB,0,null), new Operation("MOV","SW",Op_MOV_SW,0,null), new Operation("CMP","SB",Op_CMP_SB,0,null), new Operation("CMP","SW",Op_CMP_SW,0,null),
            new Operation("TEST","AI",Op_TEST_AI,0,null), new Operation("TEST","AIX",Op_TEST_AIX,0,null), new Operation("STO","SB",Op_STO_SB,0,null), new Operation("STO","SW",Op_STO_SW,0,null),
            new Operation("LOD","SB",Op_LOD_SB,0,null), new Operation("LOD","SW",Op_LOD_SW,0,null), new Operation("SCA","SB",Op_SCA_SB,0,null), new Operation("SCA","SW",Op_SCA_SW,0,null),
            new Operation("MOV","RegB",Op_MOV_RegB,GPRegistersX.AX,null), new Operation("MOV","RegB",Op_MOV_RegB,GPRegistersX.CX,null), new Operation("MOV","RegB",Op_MOV_RegB,GPRegistersX.DX,null), new Operation("MOV","RegB",Op_MOV_RegB,GPRegistersX.BX,null), new Operation("MOV","RegB",Op_MOV_RegB,GPRegistersX.SP,null), new Operation("MOV","RegB",Op_MOV_RegB,GPRegistersX.BP,null), new Operation("MOV","RegB",Op_MOV_RegB,GPRegistersX.SI,null), new Operation("MOV","RegB",Op_MOV_RegB,GPRegistersX.DI,null),
            new Operation("MOV","Reg",Op_MOV_Reg,GPRegistersX.AX,null), new Operation("MOV","Reg",Op_MOV_Reg,GPRegistersX.CX,null), new Operation("MOV","Reg",Op_MOV_Reg,GPRegistersX.DX,null), new Operation("MOV","Reg",Op_MOV_Reg,GPRegistersX.BX,null), new Operation("MOV","Reg",Op_MOV_Reg,GPRegistersX.SP,null), new Operation("MOV","Reg",Op_MOV_Reg,GPRegistersX.BP,null), new Operation("MOV","Reg",Op_MOV_Reg,GPRegistersX.SI,null), new Operation("MOV","Reg",Op_MOV_Reg,GPRegistersX.DI,null),
            new Operation("Shift","MI",Op_Shift_MI,0,casShiftOps), new Operation("Shift","MI8X",Op_Shift_MI8X,0,casShiftOps), new Operation("RET","iN",Op_RET_iN,0,null), new Operation("RET","N",Op_RET_N,0,null),
            new Operation("LES","z",Op_LES_z,0,null), new Operation("LDS","z",Op_LDS_z,0,null), new Operation("MOV","MI",Op_MOV_MI,0,null), new Operation("MOV","MIX",Op_MOV_MIX,0,null),
            new Operation("ENTER","z",Op_ENTER_z,0,null), new Operation("LEAVE","z",Op_LEAVE_z,0,null), new Operation("RET","iF",Op_RET_iF,0,null), new Operation("RET","F",Op_RET_F,0,null),
            new Operation("INT","3",Op_INT_3,0,null), new Operation("INT","I",Op_INT_I,0,null), new Operation("INTO","z",Op_INTO_z,0,null), new Operation("IRET","z",Op_IRET_z,0,null),
            new Operation("Shift","M1",Op_Shift_M1,0,casShiftOps), new Operation("Shift","M1X",Op_Shift_M1X,0,casShiftOps), new Operation("Shift","MCl",Op_Shift_MCl,0,casShiftOps), new Operation("Shift","MClX",Op_Shift_MClX,0,casShiftOps),
            new Operation("AAM","z",Op_AAM_z,0,null), new Operation("AAD","z",Op_AAD_z,0,null), new Operation("Undef","",null,0,null), new Operation("XLAT","z",Op_XLAT_z,0,null),
            new Operation("Undef","",null,0,null), new Operation("Undef","",null,0,null), new Operation("Undef","",null,0,null), new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null), new Operation("Undef","",null,0,null), new Operation("Undef","",null,0,null), new Operation("Undef","",null,0,null),
            new Operation("LOOPNZ","S",Op_LOOPNZ_S,0,null), new Operation("LOOPZ","S",Op_LOOPZ_S,0,null), new Operation("LOOP","S",Op_LOOP_S,0,null), new Operation("JCXZ","S",Op_JCXZ_S,0,null),
            new Operation("IN","AI",Op_IN_AI,0,null), new Operation("IN","AIX",Op_IN_AIX,0,null), new Operation("OUT","AI",Op_OUT_AI,0,null), new Operation("OUT","AIX",Op_OUT_AIX,0,null),
            new Operation("CALL","N",Op_CALL_N,0,null), new Operation("JMP","N",Op_JMP_N,0,null), new Operation("JMP","F",Op_JMP_F,0,null), new Operation("JMP","S",Op_JMP_S,0,null),
            new Operation("IN","ADx",Op_IN_ADx,0,null), new Operation("IN","ADxX",Op_IN_ADxX,0,null), new Operation("OUT","DxA",Op_OUT_DxA,0,null), new Operation("OUT","DxAX",Op_OUT_DxAX,0,null),
            new Operation("Undef","",null,0,null), new Operation("Undef","",null,0,null), new Operation("Prefix","REPNZ",Op_Prefix_REPNZ,0,null), new Operation("Prefix","REP",Op_Prefix_REP,0,null),
            new Operation("HLT","z",Op_HLT_z,0,null), new Operation("Flag","CMC",Op_Flag_CMC,0,null), new Operation("ArithMisc","MI",Op_ArithMisc_MI,0,casMiscOps), new Operation("ArithMisc","MIX",Op_ArithMisc_MIX,0,casMiscOps),
            new Operation("Flag","CLC",Op_Flag_CLC,0,null), new Operation("Flag","STC",Op_Flag_STC,0,null), new Operation("Flag","CLI",Op_Flag_CLI,0,null), new Operation("Flag","STI",Op_Flag_STI,0,null),
            new Operation("Flag","CLD",Op_Flag_CLD,0,null), new Operation("Flag","STD",Op_Flag_STD,0,null), new Operation("Unary","M",Op_Unary_M,0,null), new Operation("Unary","MX",Op_Unary_MX,0,null)
                                                       };

        public const Operation[] caOperations0F = {
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("JO","N",Op_JO_N,0,null), new Operation("JNO","N",Op_JNO_N,0,null), new Operation("JC","N",Op_JC_N,0,null), new Operation("JNC","N",Op_JNC_N,0,null),
            new Operation("JZ","N",Op_JZ_N,0,null), new Operation("JNZ","N",Op_JNZ_N,0,null), new Operation("JBE","N",Op_JBE_N,0,null), new Operation("JA","N",Op_JA_N,0,null),
            new Operation("JS","N",Op_JS_N,0,null), new Operation("JNS","N",Op_JNS_N,0,null), new Operation("JPE","N",Op_JPE_N,0,null), new Operation("JPO","N",Op_JPO_N,0,null),
            new Operation("JL","N",Op_JL_N,0,null), new Operation("JGE","N",Op_JGE_N,0,null), new Operation("JLE","N",Op_JLE_N,0,null), new Operation("JG","N",Op_JG_N,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("PUSH","Seg",Op_PUSH_Seg,SRegisters.SREG_FS,null), new Operation("POP","Seg",Op_POP_Seg,SRegisters.SREG_FS,null), new Operation("Undef","",null,0,null), new Operation("Undef","",null,0,null),
            new Operation("SHLD","I8",Op_SHLD_I8,0,null), new Operation("SHLD","Cl",Op_SHLD_Cl,0,null), new Operation("Undef","",null,0,null), new Operation("Undef","",null,0,null),
            new Operation("PUSH","Seg",Op_PUSH_Seg,SRegisters.SREG_GS,null), new Operation("POP","Seg",Op_POP_Seg,SRegisters.SREG_GS,null), new Operation("Undef","",null,0,null), new Operation("Undef","",null,0,null),
            new Operation("SHRD","I8",Op_SHRD_I8,0,null), new Operation("SHRD","Cl",Op_SHRD_Cl,0,null), new Operation("Undef","",null,0,null), new Operation("IMUL","RMX",Op_IMUL_RMX,0,null),
            new Operation("Undef","",null,0,null), new Operation("Undef","",null,0,null), new Operation("LSS","z",Op_LSS_z,0,null), new Operation("Undef","",null,0,null),
            new Operation("LFS","z",Op_LFS_z,0,null), new Operation("LGS","z",Op_LGS_z,0,null), new Operation("MOV","Z",Op_MOV_Z,0,null), new Operation("MOV","ZX",Op_MOV_ZX,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("BSF","z",Op_BSF_z,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),
            new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null),new Operation("Undef","",null,0,null)
                                                  };
    }
}