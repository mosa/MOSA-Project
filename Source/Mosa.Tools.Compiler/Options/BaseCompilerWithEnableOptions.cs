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

	public abstract class BaseCompilerWithEnableOptions : BaseCompilerOptions
	{

		public bool Enabled { get; protected set; }

		protected BaseCompilerWithEnableOptions()
		{
			this.Enabled = true;
		}

	}
}
