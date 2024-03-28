// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Exception;

/// <summary>
/// Transformations
/// </summary>
public static class ExceptionTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		new Throw(),
		new FinallyStart(),
		new FinallyEnd(),
		new ExceptionStart(),
		new ExceptionEnd(),
		new Flow(),
		new TryStart(),
		new TryEnd(),
	};
}
