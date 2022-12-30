// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework.Transforms;

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
