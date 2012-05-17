/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.Platform;

namespace Mosa.Platform.AVR32
{

	/// <summary>
	/// 
	/// </summary>
	public abstract class BaseTransformationStage : BasePlatformTransformationStage
	{

		#region Data members

		#endregion // Data members

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public override string Name { get { return "AVR32." + this.GetType().Name; } }

		#endregion // IPipelineStage Members

		#region Emit Methods

		#endregion // Emit Methods

	}
}
