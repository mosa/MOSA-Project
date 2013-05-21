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
        public static unsafe uint Op_Ext_0F(ref State state, uint param);
        public static unsafe uint Op_Unary_M(ref State state, uint param);
        public static unsafe uint Op_Unary_MX(ref State state, uint param);


        public static unsafe uint Op_ADD_MR(ref State state, uint param); public static unsafe uint Op_ADD_MRX(ref State state, uint param); public static unsafe uint Op_ADD_RM(ref State state, uint param); public static unsafe uint Op_ADD_RMX(ref State state, uint param); public static unsafe uint Op_ADD_AI(ref State state, uint param); public static unsafe uint Op_ADD_AIX(ref State state, uint param);
        public static unsafe uint Op_ADC_MR(ref State state, uint param); public static unsafe uint Op_ADC_MRX(ref State state, uint param); public static unsafe uint Op_ADC_RM(ref State state, uint param); public static unsafe uint Op_ADC_RMX(ref State state, uint param); public static unsafe uint Op_ADC_AI(ref State state, uint param); public static unsafe uint Op_ADC_AIX(ref State state, uint param);
        public static unsafe uint Op_SBB_MR(ref State state, uint param); public static unsafe uint Op_SBB_MRX(ref State state, uint param); public static unsafe uint Op_SBB_RM(ref State state, uint param); public static unsafe uint Op_SBB_RMX(ref State state, uint param); public static unsafe uint Op_SBB_AI(ref State state, uint param); public static unsafe uint Op_SBB_AIX(ref State state, uint param);
        public static unsafe uint Op_OR_MR(ref State state, uint param); public static unsafe uint Op_OR_MRX(ref State state, uint param); public static unsafe uint Op_OR_RM(ref State state, uint param); public static unsafe uint Op_OR_RMX(ref State state, uint param); public static unsafe uint Op_OR_AI(ref State state, uint param); public static unsafe uint Op_OR_AIX(ref State state, uint param);
        public static unsafe uint Op_AND_MR(ref State state, uint param); public static unsafe uint Op_AND_MRX(ref State state, uint param); public static unsafe uint Op_AND_RM(ref State state, uint param); public static unsafe uint Op_AND_RMX(ref State state, uint param); public static unsafe uint Op_AND_AI(ref State state, uint param); public static unsafe uint Op_AND_AIX(ref State state, uint param);
        public static unsafe uint Op_SUB_MR(ref State state, uint param); public static unsafe uint Op_SUB_MRX(ref State state, uint param); public static unsafe uint Op_SUB_RM(ref State state, uint param); public static unsafe uint Op_SUB_RMX(ref State state, uint param); public static unsafe uint Op_SUB_AI(ref State state, uint param); public static unsafe uint Op_SUB_AIX(ref State state, uint param);
        public static unsafe uint Op_XOR_MR(ref State state, uint param); public static unsafe uint Op_XOR_MRX(ref State state, uint param); public static unsafe uint Op_XOR_RM(ref State state, uint param); public static unsafe uint Op_XOR_RMX(ref State state, uint param); public static unsafe uint Op_XOR_AI(ref State state, uint param); public static unsafe uint Op_XOR_AIX(ref State state, uint param);
        public static unsafe uint Op_CMP_MR(ref State state, uint param); public static unsafe uint Op_CMP_MRX(ref State state, uint param); public static unsafe uint Op_CMP_RM(ref State state, uint param); public static unsafe uint Op_CMP_RMX(ref State state, uint param); public static unsafe uint Op_CMP_AI(ref State state, uint param); public static unsafe uint Op_CMP_AIX(ref State state, uint param);
        public static unsafe uint Op_Arith_MI(ref State state, uint param);
        public static unsafe uint Op_Arith_MIX(ref State state, uint param);
        public static unsafe uint Op_Arith_MI8X(ref State state, uint param);
        public static unsafe uint Op_TEST_MR(ref State state, uint param);
        public static unsafe uint Op_TEST_MRX(ref State state, uint param);
        public static unsafe uint Op_TEST_AI(ref State state, uint param);
        public static unsafe uint Op_TEST_AIX(ref State state, uint param);

        public static unsafe uint Op_ArithMisc_MI(ref State state, uint param);
        public static unsafe uint Op_ArithMisc_MIX(ref State state, uint param);
        public static unsafe uint Op_IMUL_MI8X(ref State state, uint param);
        public static unsafe uint Op_IMUL_MIX(ref State state, uint param);
        public static unsafe uint Op_IMUL_RMX(ref State state, uint param);

        public static unsafe uint Op_Shift_MI(ref State state, uint param);
        public static unsafe uint Op_Shift_MI8X(ref State state, uint param);
        public static unsafe uint Op_Shift_M1(ref State state, uint param);
        public static unsafe uint Op_Shift_M1X(ref State state, uint param);
        public static unsafe uint Op_Shift_MCl(ref State state, uint param);
        public static unsafe uint Op_Shift_MClX(ref State state, uint param);
        public static unsafe uint Op_SHLD_I8(ref State state, uint param);
        public static unsafe uint Op_SHLD_Cl(ref State state, uint param);
        public static unsafe uint Op_SHRD_I8(ref State state, uint param);
        public static unsafe uint Op_SHRD_Cl(ref State state, uint param);

        public static unsafe uint Op_INC_Reg(ref State state, uint param);
        public static unsafe uint Op_DEC_Reg(ref State state, uint param);


        public static unsafe uint Op_PUSH_Seg(ref State state, uint param); public static unsafe uint Op_POP_Seg(ref State state, uint param);
        public static unsafe uint Op_PUSH_Reg(ref State state, uint param); public static unsafe uint Op_POP_Reg(ref State state, uint param);
        public static unsafe uint Op_PUSH_A(ref State state, uint param); public static unsafe uint Op_POP_A(ref State state, uint param);
        public static unsafe uint Op_PUSH_F(ref State state, uint param); public static unsafe uint Op_POP_F(ref State state, uint param);
        public static unsafe uint Op_PUSH_MX(ref State state, uint param); public static unsafe uint Op_POP_MX(ref State state, uint param);
        public static unsafe uint Op_PUSH_I(ref State state, uint param);
        public static unsafe uint Op_PUSH_I8(ref State state, uint param);


        public static unsafe uint Op_IN_ADx(ref State state, uint param); public static unsafe uint Op_IN_ADxX(ref State state, uint param);
        public static unsafe uint Op_IN_AI(ref State state, uint param); public static unsafe uint Op_IN_AIX(ref State state, uint param);

        public static unsafe uint Op_OUT_DxA(ref State state, uint param); public static unsafe uint Op_OUT_DxAX(ref State state, uint param);
        public static unsafe uint Op_OUT_AI(ref State state, uint param); public static unsafe uint Op_OUT_AIX(ref State state, uint param);


        public static unsafe uint Op_MOV_MR(ref State state, uint param); public static unsafe uint Op_MOV_MRX(ref State state, uint param);
        public static unsafe uint Op_MOV_RM(ref State state, uint param); public static unsafe uint Op_MOV_RMX(ref State state, uint param);
        public static unsafe uint Op_MOV_MI(ref State state, uint param); public static unsafe uint Op_MOV_MIX(ref State state, uint param);
        public static unsafe uint Op_MOV_AMo(ref State state, uint param); public static unsafe uint Op_MOV_AMoX(ref State state, uint param);
        public static unsafe uint Op_MOV_MoA(ref State state, uint param); public static unsafe uint Op_MOV_MoAX(ref State state, uint param);
        public static unsafe uint Op_MOV_RS(ref State state, uint param);
        public static unsafe uint Op_MOV_SR(ref State state, uint param);
        public static unsafe uint Op_MOV_RegB(ref State state, uint param); public static unsafe uint Op_MOV_Reg(ref State state, uint param);
        public static unsafe uint Op_MOV_Z(ref State state, uint param); public static unsafe uint Op_MOV_ZX(ref State state, uint param);

        public static unsafe uint Op_XCHG_RM(ref State state, uint param); public static unsafe uint Op_XCHG_RMX(ref State state, uint param);
        public static unsafe uint Op_XCHG_Reg(ref State state, uint param);


        public static unsafe uint Op_MOV_SB(ref State state, uint param); public static unsafe uint Op_MOV_SW(ref State state, uint param);
        public static unsafe uint Op_STO_SB(ref State state, uint param); public static unsafe uint Op_STO_SW(ref State state, uint param);
        public static unsafe uint Op_LOD_SB(ref State state, uint param); public static unsafe uint Op_LOD_SW(ref State state, uint param);
        public static unsafe uint Op_CMP_SB(ref State state, uint param); public static unsafe uint Op_CMP_SW(ref State state, uint param);
        public static unsafe uint Op_SCA_SB(ref State state, uint param); public static unsafe uint Op_SCA_SW(ref State state, uint param);
        public static unsafe uint Op_IN_SB(ref State state, uint param); public static unsafe uint Op_IN_SW(ref State state, uint param);
        public static unsafe uint Op_OUT_SB(ref State state, uint param); public static unsafe uint Op_OUT_SW(ref State state, uint param);


        public static unsafe uint Op_JO_S(ref State state, uint param); public static unsafe uint Op_JNO_S(ref State state, uint param);
        public static unsafe uint Op_JC_S(ref State state, uint param); public static unsafe uint Op_JNC_S(ref State state, uint param);
        public static unsafe uint Op_JZ_S(ref State state, uint param); public static unsafe uint Op_JNZ_S(ref State state, uint param);
        public static unsafe uint Op_JBE_S(ref State state, uint param); public static unsafe uint Op_JA_S(ref State state, uint param);
        public static unsafe uint Op_JS_S(ref State state, uint param); public static unsafe uint Op_JNS_S(ref State state, uint param);
        public static unsafe uint Op_JPE_S(ref State state, uint param); public static unsafe uint Op_JPO_S(ref State state, uint param);
        public static unsafe uint Op_JL_S(ref State state, uint param); public static unsafe uint Op_JGE_S(ref State state, uint param);
        public static unsafe uint Op_JLE_S(ref State state, uint param); public static unsafe uint Op_JG_S(ref State state, uint param);

        public static unsafe uint Op_LOOPNZ_S(ref State state, uint param);
        public static unsafe uint Op_LOOPZ_S(ref State state, uint param);
        public static unsafe uint Op_LOOP_S(ref State state, uint param);

        public static unsafe uint Op_JMP_N(ref State state, uint param);
        public static unsafe uint Op_JMP_F(ref State state, uint param);
        public static unsafe uint Op_JMP_S(ref State state, uint param);

        public static unsafe uint Op_JO_N(ref State state, uint param); public static unsafe uint Op_JNO_N(ref State state, uint param);
        public static unsafe uint Op_JC_N(ref State state, uint param); public static unsafe uint Op_JNC_N(ref State state, uint param);
        public static unsafe uint Op_JZ_N(ref State state, uint param); public static unsafe uint Op_JNZ_N(ref State state, uint param);
        public static unsafe uint Op_JBE_N(ref State state, uint param); public static unsafe uint Op_JA_N(ref State state, uint param);
        public static unsafe uint Op_JS_N(ref State state, uint param); public static unsafe uint Op_JNS_N(ref State state, uint param);
        public static unsafe uint Op_JPE_N(ref State state, uint param); public static unsafe uint Op_JPO_N(ref State state, uint param);
        public static unsafe uint Op_JL_N(ref State state, uint param); public static unsafe uint Op_JGE_N(ref State state, uint param);
        public static unsafe uint Op_JLE_N(ref State state, uint param); public static unsafe uint Op_JG_N(ref State state, uint param);
        public static unsafe uint Op_JCXZ_S(ref State state, uint param);


        public static unsafe uint Op_CALL_N(ref State state, uint param);
        public static unsafe uint Op_CALL_F(ref State state, uint param);
        public static unsafe uint Op_RET_N(ref State state, uint param);
        public static unsafe uint Op_RET_F(ref State state, uint param);
        public static unsafe uint Op_RET_iN(ref State state, uint param);
        public static unsafe uint Op_RET_iF(ref State state, uint param);
        public static unsafe uint Op_ENTER_z(ref State state, uint param);
        public static unsafe uint Op_LEAVE_z(ref State state, uint param);

        public static unsafe uint Op_INT_3(ref State state, uint param);
        public static unsafe uint Op_INT_I(ref State state, uint param);
        public static unsafe uint Op_INTO_z(ref State state, uint param);
        public static unsafe uint Op_IRET_z(ref State state, uint param);


        public static unsafe uint Op_Ovr_Seg(ref State state, uint param);
        public static unsafe uint Op_Ovr_OpSize(ref State state, uint param);
        public static unsafe uint Op_Ovr_AddrSize(ref State state, uint param);
        public static unsafe uint Op_Prefix_REP(ref State state, uint param);
        public static unsafe uint Op_Prefix_REPNZ(ref State state, uint param);


        public static unsafe uint Op_Flag_CLC(ref State state, uint param); public static unsafe uint Op_Flag_STC(ref State state, uint param);
        public static unsafe uint Op_Flag_CLI(ref State state, uint param); public static unsafe uint Op_Flag_STI(ref State state, uint param);
        public static unsafe uint Op_Flag_CLD(ref State state, uint param); public static unsafe uint Op_Flag_STD(ref State state, uint param);
        public static unsafe uint Op_Flag_CMC(ref State state, uint param);
        public static unsafe uint Op_Flag_SAHF(ref State state, uint param);
        public static unsafe uint Op_Flag_LAHF(ref State state, uint param);


        public static unsafe uint Op_CBW_z(ref State state, uint param);
        public static unsafe uint Op_BSF_z(ref State state, uint param);
        public static unsafe uint Op_HLT_z(ref State state, uint param);
        public static unsafe uint Op_AAA_z(ref State state, uint param);
        public static unsafe uint Op_AAS_z(ref State state, uint param);
        public static unsafe uint Op_AAM_z(ref State state, uint param);
        public static unsafe uint Op_AAD_z(ref State state, uint param);
        public static unsafe uint Op_DAA_z(ref State state, uint param);
        public static unsafe uint Op_DAS_z(ref State state, uint param);
        public static unsafe uint Op_CWD_z(ref State state, uint param);
        public static unsafe uint Op_LES_z(ref State state, uint param);
        public static unsafe uint Op_LDS_z(ref State state, uint param);
        public static unsafe uint Op_LFS_z(ref State state, uint param);
        public static unsafe uint Op_LGS_z(ref State state, uint param);
        public static unsafe uint Op_LSS_z(ref State state, uint param);
        public static unsafe uint Op_LEA_z(ref State state, uint param);
        public static unsafe uint Op_XLAT_z(ref State state, uint param);
    }
}