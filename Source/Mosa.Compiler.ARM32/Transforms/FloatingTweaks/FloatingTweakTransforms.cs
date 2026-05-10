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
		ConvertI64ToR4.Instance,
		ConvertI64ToR8.Instance,
		ConvertR4ToI64.Instance,
		ConvertR8ToI64.Instance,
		ConvertR4ToU64.Instance,
		ConvertR8ToU64.Instance,

		RemR4NotSupported.Instance,
		RemR8NotSupported.Instance,
	};
}
