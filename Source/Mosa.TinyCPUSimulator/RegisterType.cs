// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator
{
	public enum RegisterType
	{
		GeneralPurpose,
		ControlRegister,
		FloatingPoint,
		StatusFlag,
		InstructionPointer,
		SegmentRegister,
		HiddenInternal,
	}
}
