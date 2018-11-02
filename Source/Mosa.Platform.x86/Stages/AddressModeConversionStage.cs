// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Address Mode Conversion Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.Intel.Stages.AddressModeConversionStage" />
	public sealed class AddressModeConversionStage : Intel.Stages.AddressModeConversionStage
	{
		protected override string Platform { get { return "x86"; } }

		protected override bool IsThreeTwoAddressRequired(BaseInstruction instruction)
		{
			return (instruction as X86Instruction)?.ThreeTwoAddressConversion == true;
		}

		protected override BaseInstruction GetMoveFromType(MosaType type)
		{
			if (type.IsR4)
			{
				return X86.Movss;
			}
			else if (type.IsR8)
			{
				return X86.Movsd;
			}

			return X86.Mov32;
		}
	}
}
