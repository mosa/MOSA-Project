// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// LoadObject
	/// </summary>
	public sealed class LoadObject : BaseTransform
	{
		public LoadObject() : base(IRInstruction.LoadObject, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			transform.OrderLoadStoreOperands(context);

			context.SetInstruction(X64.MovLoad64, context.Result, context.Operand1, context.Operand2);
		}
	}
}
