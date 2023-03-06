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
			var motion = transform.GetManager<CodeMotionManager>();

			if (motion == null)
				return false;

			return motion.CheckMotion(context.Node);
		}

		#endregion Helpers
	}
}
