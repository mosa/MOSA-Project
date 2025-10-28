// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.RuntimeCall;

/// <summary>
/// RuntimeCall Transformation List
/// </summary>
public static class RuntimeCallTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		//new DivSigned64(),
		//new DivUnsigned64(),
		new MulCarryOut64(),
		new MulOverflowOut64(),
		new RemR4(),
		new RemR8(),
		//new RemSigned64(),
		//new RemUnsigned64(),
		new ConvertR4ToI64(),
		new ConvertR8ToI32(),
		new ConvertR8ToI64(),
		new ConvertR4ToU32(),
		new ConvertR4ToU64(),
		new ConvertR8ToU32(),
		new ConvertR8ToU64(),
	};
}
