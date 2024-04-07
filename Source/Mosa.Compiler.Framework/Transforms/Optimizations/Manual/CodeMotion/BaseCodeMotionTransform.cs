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

		public override bool Match(Context context, Transform transform)
		{
			if (transform.Window == 0)
				return false;

			if (!context.Result.IsVirtualRegister)
				return false;

			if (!context.Result.IsUsedOnce)
				return false;

			if (context.Result.Uses[0].Block != context.Block)
				return false;

			if (context.Node == context.Result.Uses[0].PreviousNonEmpty)
				return false;

			// find locations before any memory store operation or use
			var location = GetMotionLocation(context.Node, context.Result.Uses[0], transform.Window);

			if (location == null || location.Previous == null)
				return false;

			if (context.Node == location.PreviousNonEmpty)
				return false;

			return !CheckCodeMotion(transform, context);
		}

		public override void Transform(Context context, Transform transform)
		{
			transform.GetManager<CodeMotionManager>().MarkMotion(context.Node);

			var location = GetMotionLocation(context.Node, context.Result.Uses[0], transform.Window);

			location.Previous.MoveFrom(context.Node);
		}

		#endregion Overrides

		#region Helpers

		public static bool CheckCodeMotion(Transform transform, Context context)
		{
			var codeMotion = transform.GetManager<CodeMotionManager>();

			if (codeMotion == null)
				return false;

			return codeMotion.CheckMotion(context.Node);
		}

		protected static Node GetMotionLocation(Node start, Node end, int window)
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
					|| next.Instruction.IsMemoryRead
					|| next.Instruction.IsIOOperation
					|| !next.Instruction.IsFlowNext
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
