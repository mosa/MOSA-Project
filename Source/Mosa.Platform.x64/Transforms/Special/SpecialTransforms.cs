// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework.Transforms;
using Mosa.Platform.x64.Transforms.Special;

namespace Mosa.Platform.x64.Transforms.Stack
{
	/// <summary>
	/// Special Transformation List
	/// </summary>
	public static class SpecialTransforms
	{
		public static readonly List<BaseTransform> List = new List<BaseTransform>
		{
			new AddressModeConversion(),
		};
	}
}
