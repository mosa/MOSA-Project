// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.Framework.Call;

/// <summary>
/// Transformations
/// </summary>
public static class CallTransforms
{
	public static readonly List<BaseTransform> List = new List<BaseTransform>
	{
		new SetReturnObject(),
		new SetReturn32(),
		new SetReturn64(),
		new SetReturnR4(),
		new SetReturnR8(),
		new SetReturnCompound(),
		new CallInterface(),
		new CallStatic(),
		new CallVirtual(),
		new CallDynamic(),
	};
}
