/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.TypeSystem;

namespace Mosa.Runtime.InternalLog
{
	public enum CompilerStage { CompilingMethod, Linking };

	public static class CompilerStageExtension
	{
		public static string ToText(this CompilerStage stage)
		{
			switch (stage)
			{
				case CompilerStage.CompilingMethod: return "Compiling Method";
				case CompilerStage.Linking: return "Linking";
				default: return stage.ToString();
			}
		}
	}
}
