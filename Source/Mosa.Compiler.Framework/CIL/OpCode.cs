// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// All CIL opcodes as defined in ISO/IEC 23271:2006 (E).
	/// </summary>
	public enum OpCode
	{
		/// <summary>
		///
		/// </summary>
		Nop = 0x000,

		/// <summary>
		///
		/// </summary>
		Break = 0x001,

		/// <summary>
		///
		/// </summary>
		Ldarg_0 = 0x002,

		/// <summary>
		///
		/// </summary>
		Ldarg_1 = 0x003,

		/// <summary>
		///
		/// </summary>
		Ldarg_2 = 0x004,

		/// <summary>
		///
		/// </summary>
		Ldarg_3 = 0x005,

		/// <summary>
		///
		/// </summary>
		Ldloc_0 = 0x006,

		/// <summary>
		///
		/// </summary>
		Ldloc_1 = 0x007,

		/// <summary>
		///
		/// </summary>
		Ldloc_2 = 0x008,

		/// <summary>
		///
		/// </summary>
		Ldloc_3 = 0x009,

		/// <summary>
		///
		/// </summary>
		Stloc_0 = 0x00A,

		/// <summary>
		///
		/// </summary>
		Stloc_1 = 0x00B,

		/// <summary>
		///
		/// </summary>
		Stloc_2 = 0x00C,

		/// <summary>
		///
		/// </summary>
		Stloc_3 = 0x00D,

		/// <summary>
		///
		/// </summary>
		Ldarg_s = 0x00E,

		/// <summary>
		///
		/// </summary>
		Ldarga_s = 0x00F,

		/// <summary>
		///
		/// </summary>
		Starg_s = 0x010,

		/// <summary>
		///
		/// </summary>
		Ldloc_s = 0x011,

		/// <summary>
		///
		/// </summary>
		Ldloca_s = 0x012,

		/// <summary>
		///
		/// </summary>
		Stloc_s = 0x013,

		/// <summary>
		///
		/// </summary>
		Ldnull = 0x014,

		/// <summary>
		///
		/// </summary>
		Ldc_i4_m1 = 0x015,

		/// <summary>
		///
		/// </summary>
		Ldc_i4_0 = 0x016,

		/// <summary>
		///
		/// </summary>
		Ldc_i4_1 = 0x017,

		/// <summary>
		///
		/// </summary>
		Ldc_i4_2 = 0x018,

		/// <summary>
		///
		/// </summary>
		Ldc_i4_3 = 0x019,

		/// <summary>
		///
		/// </summary>
		Ldc_i4_4 = 0x01A,

		/// <summary>
		///
		/// </summary>
		Ldc_i4_5 = 0x01B,

		/// <summary>
		///
		/// </summary>
		Ldc_i4_6 = 0x01C,

		/// <summary>
		///
		/// </summary>
		Ldc_i4_7 = 0x01D,

		/// <summary>
		///
		/// </summary>
		Ldc_i4_8 = 0x01E,

		/// <summary>
		///
		/// </summary>
		Ldc_i4_s = 0x01F,

		/// <summary>
		///
		/// </summary>
		Ldc_i4 = 0x020,

		/// <summary>
		///
		/// </summary>
		Ldc_i8 = 0x021,

		/// <summary>
		///
		/// </summary>
		Ldc_r4 = 0x022,

		/// <summary>
		///
		/// </summary>
		Ldc_r8 = 0x023,

		/// <summary>
		///
		/// </summary>
		Dup = 0x025,

		/// <summary>
		///
		/// </summary>
		Pop = 0x026,

		/// <summary>
		///
		/// </summary>
		Jmp = 0x027,

		/// <summary>
		///
		/// </summary>
		Call = 0x028,

		/// <summary>
		///
		/// </summary>
		Calli = 0x029,

		/// <summary>
		///
		/// </summary>
		Ret = 0x02A,

		/// <summary>
		///
		/// </summary>
		Br_s = 0x02B,

		/// <summary>
		///
		/// </summary>
		Brfalse_s = 0x02C,

		/// <summary>
		///
		/// </summary>
		Brtrue_s = 0x02D,

		/// <summary>
		///
		/// </summary>
		Beq_s = 0x02E,

		/// <summary>
		///
		/// </summary>
		Bge_s = 0x02F,

		/// <summary>
		///
		/// </summary>
		Bgt_s = 0x030,

		/// <summary>
		///
		/// </summary>
		Ble_s = 0x031,

		/// <summary>
		///
		/// </summary>
		Blt_s = 0x032,

		/// <summary>
		///
		/// </summary>
		Bne_un_s = 0x033,

		/// <summary>
		///
		/// </summary>
		Bge_un_s = 0x034,

		/// <summary>
		///
		/// </summary>
		Bgt_un_s = 0x035,

		/// <summary>
		///
		/// </summary>
		Ble_un_s = 0x036,

		/// <summary>
		///
		/// </summary>
		Blt_un_s = 0x037,

		/// <summary>
		///
		/// </summary>
		Br = 0x038,

		/// <summary>
		///
		/// </summary>
		Brfalse = 0x039,

		/// <summary>
		///
		/// </summary>
		Brtrue = 0x03A,

		/// <summary>
		///
		/// </summary>
		Beq = 0x03B,

		/// <summary>
		///
		/// </summary>
		Bge = 0x03C,

		/// <summary>
		///
		/// </summary>
		Bgt = 0x03D,

		/// <summary>
		///
		/// </summary>
		Ble = 0x03E,

		/// <summary>
		///
		/// </summary>
		Blt = 0x03F,

		/// <summary>
		///
		/// </summary>
		Bne_un = 0x040,

		/// <summary>
		///
		/// </summary>
		Bge_un = 0x041,

		/// <summary>
		///
		/// </summary>
		Bgt_un = 0x042,

		/// <summary>
		///
		/// </summary>
		Ble_un = 0x043,

		/// <summary>
		///
		/// </summary>
		Blt_un = 0x044,

		/// <summary>
		///
		/// </summary>
		Switch = 0x045,

		/// <summary>
		///
		/// </summary>
		Ldind_i1 = 0x046,

		/// <summary>
		///
		/// </summary>
		Ldind_u1 = 0x047,

		/// <summary>
		///
		/// </summary>
		Ldind_i2 = 0x048,

		/// <summary>
		///
		/// </summary>
		Ldind_u2 = 0x049,

		/// <summary>
		///
		/// </summary>
		Ldind_i4 = 0x04A,

		/// <summary>
		///
		/// </summary>
		Ldind_u4 = 0x04B,

		/// <summary>
		///
		/// </summary>
		Ldind_i8 = 0x04C,

		/// <summary>
		///
		/// </summary>
		Ldind_i = 0x04D,

		/// <summary>
		///
		/// </summary>
		Ldind_r4 = 0x04E,

		/// <summary>
		///
		/// </summary>
		Ldind_r8 = 0x04F,

		/// <summary>
		///
		/// </summary>
		Ldind_ref = 0x050,

		/// <summary>
		///
		/// </summary>
		Stind_ref = 0x051,

		/// <summary>
		///
		/// </summary>
		Stind_i1 = 0x052,

		/// <summary>
		///
		/// </summary>
		Stind_i2 = 0x053,

		/// <summary>
		///
		/// </summary>
		Stind_i4 = 0x054,

		/// <summary>
		///
		/// </summary>
		Stind_i8 = 0x055,

		/// <summary>
		///
		/// </summary>
		Stind_r4 = 0x056,

		/// <summary>
		///
		/// </summary>
		Stind_r8 = 0x057,

		/// <summary>
		///
		/// </summary>
		Add = 0x058,

		/// <summary>
		///
		/// </summary>
		Sub = 0x059,

		/// <summary>
		///
		/// </summary>
		Mul = 0x05A,

		/// <summary>
		///
		/// </summary>
		Div = 0x05B,

		/// <summary>
		///
		/// </summary>
		Div_un = 0x05C,

		/// <summary>
		///
		/// </summary>
		Rem = 0x05D,

		/// <summary>
		///
		/// </summary>
		Rem_un = 0x05E,

		/// <summary>
		///
		/// </summary>
		And = 0x05F,

		/// <summary>
		///
		/// </summary>
		Or = 0x060,

		/// <summary>
		///
		/// </summary>
		Xor = 0x061,

		/// <summary>
		///
		/// </summary>
		Shl = 0x062,

		/// <summary>
		///
		/// </summary>
		Shr = 0x063,

		/// <summary>
		///
		/// </summary>
		Shr_un = 0x064,

		/// <summary>
		///
		/// </summary>
		Neg = 0x065,

		/// <summary>
		///
		/// </summary>
		Not = 0x066,

		/// <summary>
		///
		/// </summary>
		Conv_i1 = 0x067,

		/// <summary>
		///
		/// </summary>
		Conv_i2 = 0x068,

		/// <summary>
		///
		/// </summary>
		Conv_i4 = 0x069,

		/// <summary>
		///
		/// </summary>
		Conv_i8 = 0x06A,

		/// <summary>
		///
		/// </summary>
		Conv_r4 = 0x06B,

		/// <summary>
		///
		/// </summary>
		Conv_r8 = 0x06C,

		/// <summary>
		///
		/// </summary>
		Conv_u4 = 0x06D,

		/// <summary>
		///
		/// </summary>
		Conv_u8 = 0x06E,

		/// <summary>
		///
		/// </summary>
		Callvirt = 0x06F,

		/// <summary>
		///
		/// </summary>
		Cpobj = 0x070,

		/// <summary>
		///
		/// </summary>
		Ldobj = 0x071,

		/// <summary>
		///
		/// </summary>
		Ldstr = 0x072,

		/// <summary>
		///
		/// </summary>
		Newobj = 0x073,

		/// <summary>
		///
		/// </summary>
		Castclass = 0x074,

		/// <summary>
		///
		/// </summary>
		Isinst = 0x075,

		/// <summary>
		///
		/// </summary>
		Conv_r_un = 0x076,

		/// <summary>
		///
		/// </summary>
		Unbox = 0x079,

		/// <summary>
		///
		/// </summary>
		Throw = 0x07A,

		/// <summary>
		///
		/// </summary>
		Ldfld = 0x07B,

		/// <summary>
		///
		/// </summary>
		Ldflda = 0x07C,

		/// <summary>
		///
		/// </summary>
		Stfld = 0x07D,

		/// <summary>
		///
		/// </summary>
		Ldsfld = 0x07E,

		/// <summary>
		///
		/// </summary>
		Ldsflda = 0x07F,

		/// <summary>
		///
		/// </summary>
		Stsfld = 0x080,

		/// <summary>
		///
		/// </summary>
		Stobj = 0x081,

		/// <summary>
		///
		/// </summary>
		Conv_ovf_i1_un = 0x082,

		/// <summary>
		///
		/// </summary>
		Conv_ovf_i2_un = 0x083,

		/// <summary>
		///
		/// </summary>
		Conv_ovf_i4_un = 0x084,

		/// <summary>
		///
		/// </summary>
		Conv_ovf_i8_un = 0x085,

		/// <summary>
		///
		/// </summary>
		Conv_ovf_u1_un = 0x086,

		/// <summary>
		///
		/// </summary>
		Conv_ovf_u2_un = 0x087,

		/// <summary>
		///
		/// </summary>
		Conv_ovf_u4_un = 0x088,

		/// <summary>
		///
		/// </summary>
		Conv_ovf_u8_un = 0x089,

		/// <summary>
		///
		/// </summary>
		Conv_ovf_i_un = 0x08A,

		/// <summary>
		///
		/// </summary>
		Conv_ovf_u_un = 0x08B,

		/// <summary>
		///
		/// </summary>
		Box = 0x08C,

		/// <summary>
		///
		/// </summary>
		Newarr = 0x08D,

		/// <summary>
		///
		/// </summary>
		Ldlen = 0x08E,

		/// <summary>
		///
		/// </summary>
		Ldelema = 0x08F,

		/// <summary>
		///
		/// </summary>
		Ldelem_i1 = 0x090,

		/// <summary>
		///
		/// </summary>
		Ldelem_u1 = 0x091,

		/// <summary>
		///
		/// </summary>
		Ldelem_i2 = 0x092,

		/// <summary>
		///
		/// </summary>
		Ldelem_u2 = 0x093,

		/// <summary>
		///
		/// </summary>
		Ldelem_i4 = 0x094,

		/// <summary>
		///
		/// </summary>
		Ldelem_u4 = 0x095,

		/// <summary>
		///
		/// </summary>
		Ldelem_i8 = 0x096,

		/// <summary>
		///
		/// </summary>
		Ldelem_i = 0x097,

		/// <summary>
		///
		/// </summary>
		Ldelem_r4 = 0x098,

		/// <summary>
		///
		/// </summary>
		Ldelem_r8 = 0x099,

		/// <summary>
		///
		/// </summary>
		Ldelem_ref = 0x09A,

		/// <summary>
		///
		/// </summary>
		Stelem_i = 0x09B,

		/// <summary>
		///
		/// </summary>
		Stelem_i1 = 0x09C,

		/// <summary>
		///
		/// </summary>
		Stelem_i2 = 0x09D,

		/// <summary>
		///
		/// </summary>
		Stelem_i4 = 0x09E,

		/// <summary>
		///
		/// </summary>
		Stelem_i8 = 0x09F,

		/// <summary>
		///
		/// </summary>
		Stelem_r4 = 0x0A0,

		/// <summary>
		///
		/// </summary>
		Stelem_r8 = 0x0A1,

		/// <summary>
		///
		/// </summary>
		Stelem_ref = 0x0A2,

		/// <summary>
		///
		/// </summary>
		Ldelem = 0x0A3,

		/// <summary>
		///
		/// </summary>
		Stelem = 0x0A4,

		/// <summary>
		///
		/// </summary>
		Unbox_any = 0x0A5,

		/// <summary>
		///
		/// </summary>
		Conv_ovf_i1 = 0x0B3,

		/// <summary>
		///
		/// </summary>
		Conv_ovf_u1 = 0x0B4,

		/// <summary>
		///
		/// </summary>
		Conv_ovf_i2 = 0x0B5,

		/// <summary>
		///
		/// </summary>
		Conv_ovf_u2 = 0x0B6,

		/// <summary>
		///
		/// </summary>
		Conv_ovf_i4 = 0x0B7,

		/// <summary>
		///
		/// </summary>
		Conv_ovf_u4 = 0x0B8,

		/// <summary>
		///
		/// </summary>
		Conv_ovf_i8 = 0x0B9,

		/// <summary>
		///
		/// </summary>
		Conv_ovf_u8 = 0x0BA,

		/// <summary>
		///
		/// </summary>
		Refanyval = 0x0C2,

		/// <summary>
		///
		/// </summary>
		Ckfinite = 0x0C3,

		/// <summary>
		///
		/// </summary>
		Mkrefany = 0x0C6,

		/* 0x0C7-0x0CF: Not valid for MSIL binaries. Opcodes used in IR transforms. */

		/// <summary>
		///
		/// </summary>
		Ann_call = 0x0C7,

		/// <summary>
		///
		/// </summary>
		Ann_catch = 0x0C8,

		/// <summary>
		///
		/// </summary>
		Ann_dead = 0x0C9,

		/// <summary>
		///
		/// </summary>
		Ann_hoisted = 0x0CA,

		/// <summary>
		///
		/// </summary>
		Ann_hoistedcall = 0x0CB,

		/// <summary>
		///
		/// </summary>
		Ann_lab = 0x0CC,

		/// <summary>
		///
		/// </summary>
		Ann_def = 0x0CD,

		/// <summary>
		///
		/// </summary>
		Ann_ref_s = 0x0CE,

		/// <summary>
		///
		/// </summary>
		Ann_phi = 0x0CF,

		/// <summary>
		///
		/// </summary>
		Ldtoken = 0x0D0,

		/// <summary>
		///
		/// </summary>
		Conv_u2 = 0x0D1,

		/// <summary>
		///
		/// </summary>
		Conv_u1 = 0x0D2,

		/// <summary>
		///
		/// </summary>
		Conv_i = 0x0D3,

		/// <summary>
		///
		/// </summary>
		Conv_ovf_i = 0x0D4,

		/// <summary>
		///
		/// </summary>
		Conv_ovf_u = 0x0D5,

		/// <summary>
		///
		/// </summary>
		Add_ovf = 0x0D6,

		/// <summary>
		///
		/// </summary>
		Add_ovf_un = 0x0D7,

		/// <summary>
		///
		/// </summary>
		Mul_ovf = 0x0D8,

		/// <summary>
		///
		/// </summary>
		Mul_ovf_un = 0x0D9,

		/// <summary>
		///
		/// </summary>
		Sub_ovf = 0x0DA,

		/// <summary>
		///
		/// </summary>
		Sub_ovf_un = 0x0DB,

		/// <summary>
		///
		/// </summary>
		Endfinally = 0x0DC,

		/// <summary>
		///
		/// </summary>
		Leave = 0x0DD,

		/// <summary>
		///
		/// </summary>
		Leave_s = 0x0DE,

		/// <summary>
		///
		/// </summary>
		Stind_i = 0x0DF,

		/// <summary>
		///
		/// </summary>
		Conv_u = 0x0E0,

		/// <summary>
		///
		/// </summary>
		Extop = 0x0FE,

		/// <summary>
		///
		/// </summary>
		Arglist = 0x100,

		/// <summary>
		///
		/// </summary>
		Ceq = 0x101,

		/// <summary>
		///
		/// </summary>
		Cgt = 0x102,

		/// <summary>
		///
		/// </summary>
		Cgt_un = 0x103,

		/// <summary>
		///
		/// </summary>
		Clt = 0x104,

		/// <summary>
		///
		/// </summary>
		Clt_un = 0x105,

		/// <summary>
		///
		/// </summary>
		Ldftn = 0x106,

		/// <summary>
		///
		/// </summary>
		Ldvirtftn = 0x107,

		/// <summary>
		///
		/// </summary>
		Ldarg = 0x109,

		/// <summary>
		///
		/// </summary>
		Ldarga = 0x10A,

		/// <summary>
		///
		/// </summary>
		Starg = 0x10B,

		/// <summary>
		///
		/// </summary>
		Ldloc = 0x10C,

		/// <summary>
		///
		/// </summary>
		Ldloca = 0x10D,

		/// <summary>
		///
		/// </summary>
		Stloc = 0x10E,

		/// <summary>
		///
		/// </summary>
		Localalloc = 0x10F,

		/// <summary>
		///
		/// </summary>
		Endfilter = 0x111,

		/// <summary>
		///
		/// </summary>
		PreUnaligned = 0x112,

		/// <summary>
		///
		/// </summary>
		PreVolatile = 0x113,

		/// <summary>
		///
		/// </summary>
		PreTail = 0x114,

		/// <summary>
		///
		/// </summary>
		InitObj = 0x115,

		/// <summary>
		///
		/// </summary>
		PreConstrained = 0x116,

		/// <summary>
		///
		/// </summary>
		Cpblk = 0x117,

		/// <summary>
		///
		/// </summary>
		Initblk = 0x118,

		/// <summary>
		///
		/// </summary>
		PreNo = 0x119,

		/// <summary>
		///
		/// </summary>
		Rethrow = 0x11A,

		/// <summary>
		///
		/// </summary>
		Sizeof = 0x11C,

		/// <summary>
		///
		/// </summary>
		Refanytype = 0x11D,

		/// <summary>
		///
		/// </summary>
		PreReadOnly = 0x11E
	}
}
