// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Call;

/// <summary>
/// Transformations
/// </summary>
public static class CallTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		SetReturnObject.Instance,
		SetReturnManagedPointer.Instance,
		SetReturn32.Instance,
		SetReturn64.Instance,
		SetReturnR4.Instance,
		SetReturnR8.Instance,
		SetReturnCompound.Instance,
		CallInterface.Instance,
		CallStatic.Instance,
		CallVirtual.Instance,
		CallDynamic.Instance,
	};
}
