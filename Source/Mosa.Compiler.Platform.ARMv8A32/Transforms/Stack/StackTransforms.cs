// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.ARMv8A32.Transforms.Stack
{
	/// <summary>
	/// Stack Build Transformation List
	/// </summary>
	public static class StackTransforms
	{
		public static readonly List<BaseTransform> List = new List<BaseTransform>
		{
			new Epilogue(),
			new Prologue(),
		};
	}
}
