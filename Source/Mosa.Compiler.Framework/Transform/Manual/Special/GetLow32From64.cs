// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transform.Manual.Special
{
	/// <summary>
	/// GetLow32To64
	/// </summary>
	public sealed class GetLow32From64 : BaseTransformation
	{
		public GetLow32From64() : base(IRInstruction.GetLow32, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!transformContext.Is32BitPlatform)
				return false;

			if (!transformContext.LowerTo32)
				return false;

			if (!context.Operand1.IsVirtualRegister)
				return false;

			//if (context.Operand1.Definitions.Count != 1)
			//	return false;

			if (context.Operand1.IsInteger32)
				return true;

			if (context.Operand1.IsReferenceType)
				return true;

			if (context.Operand1.IsPointer || context.Operand1.IsManagedPointer)
				return true;

			return false;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(IRInstruction.Move32, context.Result, context.Operand1);
		}
	}
}
