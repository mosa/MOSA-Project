/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	Stop the method compiler - use in development
	/// </summary>
	public class StopStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			MethodCompiler.Stop();
		}
	}
}