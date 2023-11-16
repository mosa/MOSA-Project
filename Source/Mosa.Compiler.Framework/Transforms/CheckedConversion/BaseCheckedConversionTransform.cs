// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion
{
	public abstract class BaseCheckedConversionTransform : BaseTransform
	{
		public BaseCheckedConversionTransform(BaseInstruction instruction, TransformType type, bool log = false)
			: base(instruction, type, log)
		{ }

		public void CallCheckOverflow(Transform transform, Context context, string vmcall)
		{
			var result = context.Result;
			var source = context.Operand1;

			var method = transform.GetMethod("Mosa.Runtime.Math.CheckedConversion", vmcall);

			Debug.Assert(method != null);

			var symbol = Operand.CreateLabel(method, transform.Is32BitPlatform);

			context.SetInstruction(IR.CallStatic, result, symbol, source);

			transform.MethodScanner.MethodInvoked(method, transform.Method);
		}
	}
}
