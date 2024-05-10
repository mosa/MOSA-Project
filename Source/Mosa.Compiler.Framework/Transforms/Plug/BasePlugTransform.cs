// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Plug
{
	public abstract class BasePlugTransform : BaseTransform
	{
		public BasePlugTransform(BaseInstruction instruction, TransformType type, int priority = 100, bool log = false)
			: base(instruction, type, priority, log)
		{ }

		public override bool Match(Context context, Transform transform)
		{
			return IsPlugged(context, transform);
		}

		#region Helpers

		public static bool IsPlugged(Context context, Transform transform)
		{
			if (context.Operand1.Method == null)
				return false;

			return transform.Compiler.PlugSystem.GetReplacement(context.Operand1.Method) != null;
		}

		public static void Plug(Context context, Transform transform)
		{
			var newTarget = transform.Compiler.PlugSystem.GetReplacement(context.Operand1.Method);

			if (context.Operand1 != null && context.Operand1.IsLabel && context.Operand1.Method != null)
			{
				context.Operand1 = Operand.CreateLabel(newTarget, transform.Is32BitPlatform);
			}
		}

		#endregion Helpers
	}
}
