// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.FloatingTweaks;

/// <summary>
/// Floating Tweak Transformation List
/// </summary>
public static class FloatingTweakTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		new ConvertI64ToR4(),
		new ConvertI64ToR8(),
		new ConvertR4ToI64(),
		new ConvertR8ToI64(),
		new ConvertR4ToU64(),
		new ConvertR8ToU64(),

		new RemR4NotSupported(),
		new RemR8NotSupported(),
	};
}
