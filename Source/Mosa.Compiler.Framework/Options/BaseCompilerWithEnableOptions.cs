/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


namespace Mosa.Compiler.Options
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
