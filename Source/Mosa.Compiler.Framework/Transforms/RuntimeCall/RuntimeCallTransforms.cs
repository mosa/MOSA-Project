// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Transforms.RuntimeCall;

/// <summary>
/// Transformations
/// </summary>
public static class RuntimeCallTransforms
{
	public static readonly List<BaseTransform> List = new List<BaseTransform>
	{
		new MemorySet(),
		new MemoryCopy(),
		new IsInstanceOfInterfaceType(),
		new IsInstanceOfType(),
		new GetVirtualFunctionPtr(),
		new Rethrow(),
		new Box(),
		new Box32(),
		new Box64(),
		new BoxR4(),
		new BoxR8(),
		new UnboxAny(),
		new Unbox(),
	};
}
