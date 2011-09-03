/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using NDesk.Options;

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Tools.Compiler.Options
{

	public abstract class BaseCompilerStageWithEnableOptions : BaseCompilerStageOptions
	{

		public bool Enabled { get; set; }

		protected BaseCompilerStageWithEnableOptions()
		{
			this.Enabled = true;
		}

	}
}
