// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.ARMv8A32.Stages
{
	/// <summary>
	/// Runtime Call Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv8A32.BaseTransformationStage" />
	public sealed class RuntimeCallStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.DivSigned32, DivSigned32);     // sdiv32
			AddVisitation(IRInstruction.DivSigned64, DivSigned64);     // sdiv64
			AddVisitation(IRInstruction.DivUnsigned64, DivUnsigned64); // udiv64
			AddVisitation(IRInstruction.DivUnsigned32, DivUnsigned32); // udiv32
			AddVisitation(IRInstruction.RemSigned32, RemSigned32);     // smod32
			AddVisitation(IRInstruction.RemSigned64, RemSigned64);     // smod64
			AddVisitation(IRInstruction.RemUnsigned64, RemUnsigned64); // umod64
			AddVisitation(IRInstruction.RemUnsigned32, RemUnsigned32); // umod32

			AddVisitation(IRInstruction.ConvertR4ToR8, ConvertFloatR4ToFloatR8);
			AddVisitation(IRInstruction.ConvertR8ToR4, ConvertFloatR8ToFloatR4);
			AddVisitation(IRInstruction.BitCopyR4To32, BitCopyFloatR4ToInt32);
			AddVisitation(IRInstruction.BitCopy32ToR4, BitCopyInt32ToFloatR4);
		}

		#region Visitation Methods

		private void DivSigned32(Context context)
		{
			ReplaceWithCall(context, "Mosa.Runtime.Math", "Division", "sdiv32");
		}

		private void DivSigned64(Context context)
		{
			ReplaceWithCall(context, "Mosa.Runtime.Math", "Division", "sdiv64");
		}

		private void DivUnsigned32(Context context)
		{
			ReplaceWithCall(context, "Mosa.Runtime.Math", "Division", "udiv32");
		}

		private void DivUnsigned64(Context context)
		{
			ReplaceWithCall(context, "Mosa.Runtime.Math", "Division", "udiv64");
		}

		private void RemSigned32(Context context)
		{
			ReplaceWithCall(context, "Mosa.Runtime.Math", "Division", "smod32");
		}

		private void RemSigned64(Context context)
		{
			ReplaceWithCall(context, "Mosa.Runtime.Math", "Division", "smod64");
		}

		private void RemUnsigned32(Context context)
		{
			ReplaceWithCall(context, "Mosa.Runtime.Math", "Division", "umod32");
		}

		private void RemUnsigned64(Context context)
		{
			ReplaceWithCall(context, "Mosa.Runtime.Math", "Division", "umod64");
		}

		private void ConvertFloatR4ToFloatR8(Context context)
		{
			ReplaceWithCall(context, "Mosa.Runtime.ARMv8A32.Math", "FloatingPoint", "FloatToDouble");
		}

		private void ConvertFloatR8ToFloatR4(Context context)
		{
			ReplaceWithCall(context, "Mosa.Runtime.ARMv8A32.Math", "FloatingPoint", "DoubleToFloat");
		}

		private void BitCopyFloatR4ToInt32(Context context)
		{
			ReplaceWithCall(context, "Mosa.Runtime.ARMv8A32.Math", "FloatingPoint", "BitCopyFloatR4ToInt32");
		}

		private void BitCopyInt32ToFloatR4(Context context)
		{
			ReplaceWithCall(context, "Mosa.Runtime.ARMv8A32.Math", "FloatingPoint", "BitCopyInt32ToFloatR4");
		}

		#endregion Visitation Methods
	}
}
