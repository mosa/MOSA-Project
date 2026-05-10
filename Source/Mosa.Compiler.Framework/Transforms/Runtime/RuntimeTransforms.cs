// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Runtime;

/// <summary>
/// Transformations
/// </summary>
public static class RuntimeTransforms
{
	public static readonly List<BaseTransform> RuntimeList = new List<BaseTransform>
	{
		MemorySet.Instance,
		MemoryCopy.Instance,
		IsInstanceOfInterfaceType.Instance,
		IsInstanceOfType.Instance,
		GetVirtualFunctionPtr.Instance,
		Rethrow.Instance,
		Box.Instance,
		Box32.Instance,
		Box64.Instance,
		BoxR4.Instance,
		BoxR8.Instance,
		UnboxAny.Instance,
		Unbox.Instance,
	};

	public static readonly List<BaseTransform> NewList = new List<BaseTransform>
	{
		NewObject.Instance,
		NewArray.Instance,
	};
}
