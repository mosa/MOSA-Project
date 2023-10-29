// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Runtime;

/// <summary>
/// Transformations
/// </summary>
public static class RuntimeTransforms
{
	public static readonly List<BaseTransform> RuntimeList = new List<BaseTransform>
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

	public static readonly List<BaseTransform> NewList = new List<BaseTransform>
	{
		new NewObject(),
		new NewArray(),
	};
}
