namespace System.Reflection.PortableExecutable;

public enum Machine : ushort
{
	Unknown = 0,
	I386 = 332,
	WceMipsV2 = 361,
	Alpha = 388,
	SH3 = 418,
	SH3Dsp = 419,
	SH3E = 420,
	SH4 = 422,
	SH5 = 424,
	Arm = 448,
	Thumb = 450,
	ArmThumb2 = 452,
	AM33 = 467,
	PowerPC = 496,
	PowerPCFP = 497,
	IA64 = 512,
	MIPS16 = 614,
	Alpha64 = 644,
	MipsFpu = 870,
	MipsFpu16 = 1126,
	Tricore = 1312,
	Ebc = 3772,
	Amd64 = 34404,
	M32R = 36929,
	Arm64 = 43620,
	LoongArch32 = 25138,
	LoongArch64 = 25188
}
