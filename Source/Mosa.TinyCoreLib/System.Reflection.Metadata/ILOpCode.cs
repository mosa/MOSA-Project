namespace System.Reflection.Metadata;

public enum ILOpCode : ushort
{
	Nop = 0,
	Break = 1,
	Ldarg_0 = 2,
	Ldarg_1 = 3,
	Ldarg_2 = 4,
	Ldarg_3 = 5,
	Ldloc_0 = 6,
	Ldloc_1 = 7,
	Ldloc_2 = 8,
	Ldloc_3 = 9,
	Stloc_0 = 10,
	Stloc_1 = 11,
	Stloc_2 = 12,
	Stloc_3 = 13,
	Ldarg_s = 14,
	Ldarga_s = 15,
	Starg_s = 16,
	Ldloc_s = 17,
	Ldloca_s = 18,
	Stloc_s = 19,
	Ldnull = 20,
	Ldc_i4_m1 = 21,
	Ldc_i4_0 = 22,
	Ldc_i4_1 = 23,
	Ldc_i4_2 = 24,
	Ldc_i4_3 = 25,
	Ldc_i4_4 = 26,
	Ldc_i4_5 = 27,
	Ldc_i4_6 = 28,
	Ldc_i4_7 = 29,
	Ldc_i4_8 = 30,
	Ldc_i4_s = 31,
	Ldc_i4 = 32,
	Ldc_i8 = 33,
	Ldc_r4 = 34,
	Ldc_r8 = 35,
	Dup = 37,
	Pop = 38,
	Jmp = 39,
	Call = 40,
	Calli = 41,
	Ret = 42,
	Br_s = 43,
	Brfalse_s = 44,
	Brtrue_s = 45,
	Beq_s = 46,
	Bge_s = 47,
	Bgt_s = 48,
	Ble_s = 49,
	Blt_s = 50,
	Bne_un_s = 51,
	Bge_un_s = 52,
	Bgt_un_s = 53,
	Ble_un_s = 54,
	Blt_un_s = 55,
	Br = 56,
	Brfalse = 57,
	Brtrue = 58,
	Beq = 59,
	Bge = 60,
	Bgt = 61,
	Ble = 62,
	Blt = 63,
	Bne_un = 64,
	Bge_un = 65,
	Bgt_un = 66,
	Ble_un = 67,
	Blt_un = 68,
	Switch = 69,
	Ldind_i1 = 70,
	Ldind_u1 = 71,
	Ldind_i2 = 72,
	Ldind_u2 = 73,
	Ldind_i4 = 74,
	Ldind_u4 = 75,
	Ldind_i8 = 76,
	Ldind_i = 77,
	Ldind_r4 = 78,
	Ldind_r8 = 79,
	Ldind_ref = 80,
	Stind_ref = 81,
	Stind_i1 = 82,
	Stind_i2 = 83,
	Stind_i4 = 84,
	Stind_i8 = 85,
	Stind_r4 = 86,
	Stind_r8 = 87,
	Add = 88,
	Sub = 89,
	Mul = 90,
	Div = 91,
	Div_un = 92,
	Rem = 93,
	Rem_un = 94,
	And = 95,
	Or = 96,
	Xor = 97,
	Shl = 98,
	Shr = 99,
	Shr_un = 100,
	Neg = 101,
	Not = 102,
	Conv_i1 = 103,
	Conv_i2 = 104,
	Conv_i4 = 105,
	Conv_i8 = 106,
	Conv_r4 = 107,
	Conv_r8 = 108,
	Conv_u4 = 109,
	Conv_u8 = 110,
	Callvirt = 111,
	Cpobj = 112,
	Ldobj = 113,
	Ldstr = 114,
	Newobj = 115,
	Castclass = 116,
	Isinst = 117,
	Conv_r_un = 118,
	Unbox = 121,
	Throw = 122,
	Ldfld = 123,
	Ldflda = 124,
	Stfld = 125,
	Ldsfld = 126,
	Ldsflda = 127,
	Stsfld = 128,
	Stobj = 129,
	Conv_ovf_i1_un = 130,
	Conv_ovf_i2_un = 131,
	Conv_ovf_i4_un = 132,
	Conv_ovf_i8_un = 133,
	Conv_ovf_u1_un = 134,
	Conv_ovf_u2_un = 135,
	Conv_ovf_u4_un = 136,
	Conv_ovf_u8_un = 137,
	Conv_ovf_i_un = 138,
	Conv_ovf_u_un = 139,
	Box = 140,
	Newarr = 141,
	Ldlen = 142,
	Ldelema = 143,
	Ldelem_i1 = 144,
	Ldelem_u1 = 145,
	Ldelem_i2 = 146,
	Ldelem_u2 = 147,
	Ldelem_i4 = 148,
	Ldelem_u4 = 149,
	Ldelem_i8 = 150,
	Ldelem_i = 151,
	Ldelem_r4 = 152,
	Ldelem_r8 = 153,
	Ldelem_ref = 154,
	Stelem_i = 155,
	Stelem_i1 = 156,
	Stelem_i2 = 157,
	Stelem_i4 = 158,
	Stelem_i8 = 159,
	Stelem_r4 = 160,
	Stelem_r8 = 161,
	Stelem_ref = 162,
	Ldelem = 163,
	Stelem = 164,
	Unbox_any = 165,
	Conv_ovf_i1 = 179,
	Conv_ovf_u1 = 180,
	Conv_ovf_i2 = 181,
	Conv_ovf_u2 = 182,
	Conv_ovf_i4 = 183,
	Conv_ovf_u4 = 184,
	Conv_ovf_i8 = 185,
	Conv_ovf_u8 = 186,
	Refanyval = 194,
	Ckfinite = 195,
	Mkrefany = 198,
	Ldtoken = 208,
	Conv_u2 = 209,
	Conv_u1 = 210,
	Conv_i = 211,
	Conv_ovf_i = 212,
	Conv_ovf_u = 213,
	Add_ovf = 214,
	Add_ovf_un = 215,
	Mul_ovf = 216,
	Mul_ovf_un = 217,
	Sub_ovf = 218,
	Sub_ovf_un = 219,
	Endfinally = 220,
	Leave = 221,
	Leave_s = 222,
	Stind_i = 223,
	Conv_u = 224,
	Arglist = 65024,
	Ceq = 65025,
	Cgt = 65026,
	Cgt_un = 65027,
	Clt = 65028,
	Clt_un = 65029,
	Ldftn = 65030,
	Ldvirtftn = 65031,
	Ldarg = 65033,
	Ldarga = 65034,
	Starg = 65035,
	Ldloc = 65036,
	Ldloca = 65037,
	Stloc = 65038,
	Localloc = 65039,
	Endfilter = 65041,
	Unaligned = 65042,
	Volatile = 65043,
	Tail = 65044,
	Initobj = 65045,
	Constrained = 65046,
	Cpblk = 65047,
	Initblk = 65048,
	Rethrow = 65050,
	Sizeof = 65052,
	Refanytype = 65053,
	Readonly = 65054
}
