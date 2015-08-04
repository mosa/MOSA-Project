// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Platform.ARMv6
{
	/// <summary>
	///
	/// </summary>
	public abstract class BaseTransformationStage : BasePlatformTransformationStage
	{
		protected override string Platform { get { return "ARMv6"; } }

		public static ARMv6Instruction GetMove(Operand Destination, Operand Source)
		{
			if (Source.IsR4 && Destination.IsR4)
			{
				return ARMv6.Mov; // FIXME
			}
			else if (Source.IsR8 && Destination.IsR8)
			{
				return ARMv6.Mov; // FIXME
			}
			else if (Source.IsR4 && Destination.IsR8)
			{
				return ARMv6.Mov; // FIXME
			}
			else if (Source.IsR8 && Destination.IsR4)
			{
				return ARMv6.Mov; // FIXME
			}
			else
			{
				return ARMv6.Mov;
			}
		}
	}
}
