// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Transforms.Exception;

public abstract class BaseExceptionTransform : BaseTransform
{
	public BaseExceptionTransform(BaseInstruction instruction, TransformType type, bool log = false)
		: base(instruction, type, log)
	{ }

	#region Overrides

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	#endregion Overrides

	#region Helpers

	public MosaExceptionHandler FindImmediateExceptionHandler(TransformContext transform, int label)
	{
		foreach (var handler in transform.Method.ExceptionHandlers)
		{
			if (handler.IsLabelWithinTry(label) || handler.IsLabelWithinHandler(label))
			{
				return handler;
			}
		}

		return null;
	}

	public MosaExceptionHandler FindNextEnclosingFinallyHandler(TransformContext transform, MosaExceptionHandler exceptionHandler)
	{
		var index = transform.Method.ExceptionHandlers.IndexOf(exceptionHandler);
		var at = exceptionHandler.TryStart;

		for (int i = index + 1; i < transform.Method.ExceptionHandlers.Count; i++)
		{
			var entry = transform.Method.ExceptionHandlers[i];

			if (entry.ExceptionHandlerType != ExceptionHandlerType.Finally
				|| !entry.IsLabelWithinTry(at))
				continue;

			return entry;
		}

		return null;
	}

	public static BasicBlock TraverseBackToNativeBlock(BasicBlock block)
	{
		var start = block;

		while (start.IsCompilerBlock)
		{
			if (!start.HasPreviousBlocks)
				return null;

			start = start.PreviousBlocks[0]; // any one
		}

		return start;
	}

	#endregion Helpers
}
