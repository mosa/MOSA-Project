/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using Mosa.Compiler.Metadata;

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
			if (Source.Type.Type == CilElementType.R4 && Destination.Type.Type == CilElementType.R4)
			{
				return ARMv6.Mov; // FIXME
			}
			else if (Source.Type.Type == CilElementType.R8 && Destination.Type.Type == CilElementType.R8)
			{
				return ARMv6.Mov; // FIXME
			}
			else if (Source.Type.Type == CilElementType.R4 && Destination.Type.Type == CilElementType.R8)
			{
				return ARMv6.Mov; // FIXME
			}
			else if (Source.Type.Type == CilElementType.R8 && Destination.Type.Type == CilElementType.R4)
			{
				return ARMv6.Mov; // FIXME
			}
			else
			{
				return ARMv6.Mov;
			}
		}

		#region Emit Methods

		#endregion Emit Methods
	}
}