// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Platform.x86
{
	/// <summary>
	///
	/// </summary>
	public abstract class BaseTransformationStage : BasePlatformTransformationStage
	{
		protected override string Platform { get { return "x86"; } }

		public static X86Instruction GetMove(Operand Destination, Operand Source)
		{
			if (Source.IsR8 && Destination.IsR8)
			{
				return X86.Movsd;
			}
			else if (Source.IsR4 && Destination.IsR4)
			{
				return X86.Movss;
			}
			else if (Source.IsR4 && Destination.IsR8)
			{
				return X86.Cvtss2sd;
			}
			else if (Source.IsR8 && Destination.IsR4)
			{
				return X86.Cvtsd2ss;
			}
			else if (Source.IsR8 && Destination.IsMemoryAddress)
			{
				return X86.Movsd;
			}
			else if (Source.IsR4 && Destination.IsMemoryAddress)
			{
				return X86.Movss;
			}
			else
			{
				return X86.Mov;
			}
		}
	}
}
