
using System.Diagnostics;

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.Tweak
{
	/// <summary>
	/// Cmp64
	/// </summary>
	public sealed class Cmp64 : BaseTransform
	{
		public Cmp64() : base(X64.Cmp64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var left = context.Operand1;

			if (left.IsConstant)
			{
				var v1 = transform.AllocateVirtualRegister(left.Type);

				context.InsertBefore().AppendInstruction(X64.Mov64, v1, left);
				context.Operand1 = v1;
			}
		}
	}
}
