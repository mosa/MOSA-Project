// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Exception;

/// <summary>
/// Transformations
/// </summary>
public static class ExceptionTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		Throw.Instance,
		FinallyStart.Instance,
		FinallyEnd.Instance,
		ExceptionStart.Instance,
		ExceptionEnd.Instance,
		Flow.Instance,
		TryStart.Instance,
		TryEnd.Instance,
	};
}
