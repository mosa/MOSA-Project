// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion
{
	public abstract class BaseCheckedConversionTransform : BaseTransform
	{
		public BaseCheckedConversionTransform(BaseInstruction instruction, TransformType type, bool log = false)
			: base(instruction, type, log)
		{ }

		public void CallCheckOverflow(TransformContext transform, Context context, string vmcall)
		{
			var result = context.Result;
			var source = context.Operand1;

			var method = transform.GetMethod("Mosa.Runtime.Math.CheckedConversion", vmcall);

			Debug.Assert(method != null);

			var symbol = Operand.CreateSymbolFromMethod(method, transform.TypeSystem);

			context.AppendInstruction(IRInstruction.CallStatic, result, symbol, source);

			transform.MethodScanner.MethodInvoked(method, transform.Method);
		}
	}
}
