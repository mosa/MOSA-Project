// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms
{
	public static class PlugTransformHelper
	{
		public static bool IsPlugged(Context context, TransformContext transform)
		{
			if (context.Operand1.Method == null)
				return false;

			return transform.Compiler.PlugSystem.GetReplacement(context.Operand1.Method) != null;
		}

		public static void Plug(Context context, TransformContext transform)
		{
			var newTarget = transform.Compiler.PlugSystem.GetReplacement(context.Operand1.Method);

			if (context.InvokeMethod != null)
			{
				context.InvokeMethod = newTarget;
			}

			if (context.Operand1 != null && context.Operand1.IsLabel && context.Operand1.Method != null)
			{
				context.Operand1 = Operand.CreateSymbolFromMethod(newTarget, transform.TypeSystem);
			}
		}
	}
}
