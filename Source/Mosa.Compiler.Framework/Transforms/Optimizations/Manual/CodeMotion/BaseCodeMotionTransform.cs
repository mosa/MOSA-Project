// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Managers;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion
{
	public abstract class BaseCodeMotionTransform : BaseTransform
	{
		public BaseCodeMotionTransform(BaseInstruction instruction, TransformType type, bool log = false)
			: base(instruction, type, log)
		{ }

		#region Overrides

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Result.IsVirtualRegister)
				return false;

			if (context.Result.Uses.Count != 1)
				return false;

			if (context.Result.Uses[0].Block != context.Block)
				return false;

			if (context.Node == context.Result.Uses[0].PreviousNonEmpty)
				return false;

			// FIXME: and no memory store operation between load and use, or load can't move past memory store operation

			return !CheckCodeMotion(transform, context);
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.GetManager<CodeMotionManager>().MarkMotion(context.Node);

			context.Result.Uses[0].Previous.MoveFrom(context.Node);
		}

		#endregion Overrides

		#region Helpers

		public static bool CheckCodeMotion(TransformContext transform, Context context)
		{
			var codeMotion = transform.GetManager<CodeMotionManager>();

			if (codeMotion == null)
				return false;

			return codeMotion.CheckMotion(context.Node);
		}

		#endregion Helpers
	}
}
