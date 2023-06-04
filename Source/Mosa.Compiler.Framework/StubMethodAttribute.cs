// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Framework;

public delegate void StubMethodDelegate(Context context, TransformContext transformContext);

/// <summary>
/// Used for defining targets when using intrinsic replacements
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class StubMethodAttribute : Attribute
{
	public string Target { get; }

	public StubMethodAttribute(string target)
	{
		Target = target;
	}
}
