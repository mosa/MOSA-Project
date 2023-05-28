// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Managers;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion
{
	public abstract class BaseCodeMotionTransform : BaseTransform
	{
		public const int MotionWindows = 30;

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

			// find locations before any memory store operation or use
			var location = GetMotionLocation(context.Node, context.Result.Uses[0], MotionWindows);

			if (location == null || location.Previous == null)
				return false;

			if (context.Node == location.PreviousNonEmpty)
				return false;

			return !CheckCodeMotion(transform, context);
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.GetManager<CodeMotionManager>().MarkMotion(context.Node);

			var location = GetMotionLocation(context.Node, context.Result.Uses[0], MotionWindows);

			location.Previous.MoveFrom(context.Node);
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

		protected static InstructionNode GetMotionLocation(InstructionNode start, InstructionNode end, int window)
		{
			var count = 0;

			var next = start.Next;

			while (count < window)
			{
				if (next == end)
					return end;

				if (next.IsEmptyOrNop)
				{
					next = next.Next;
					continue;
				}

				if (next.IsBlockEndInstruction
					|| next.Instruction.IsMemoryWrite
					|| next.Instruction.IsFlowNext
					|| next.Instruction.HasUnspecifiedSideEffect)
					return next;

				next = next.Next;
				count++;
			}

			return next;
		}

		#endregion Helpers
	}
}
