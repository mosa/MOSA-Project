// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Transforms;
using System.Collections.Generic;

namespace Mosa.Platform.x86.Transforms.Stack
{
	/// <summary>
	/// Stack Build Transformation List
	/// </summary>
	public static class StackTransforms
	{
		public static readonly List<BaseTransformation> List = new List<BaseTransformation>
		{
			new Epilogue(),
			new Prologue(),
		};
	}
}
