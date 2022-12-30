
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// AddressOf
	/// </summary>
	public sealed class AddressOf : BaseTransform
	{
		public AddressOf() : base(IRInstruction.AddressOf, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Operand1.IsOnStack || context.Operand1.IsStaticField);

			if (context.Operand1.IsStaticField)
			{
				context.SetInstruction(X86.Mov32, context.Result, context.Operand1);
			}
			else if (context.Operand1.IsStackLocal)
			{
				context.SetInstruction(X86.Lea32, context.Result, transform.StackFrame, context.Operand1);
			}
			else
			{
				var offset = transform.CreateConstant32(context.Operand1.Offset);

				context.SetInstruction(X86.Lea32, context.Result, transform.StackFrame, offset);
			}
		}
	}
}
