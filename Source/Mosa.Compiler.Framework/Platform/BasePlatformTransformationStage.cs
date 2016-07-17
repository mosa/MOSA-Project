// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Linker;

namespace Mosa.Compiler.Framework.Platform
{
	/// <summary>
	///
	/// </summary>
	public abstract class BasePlatformTransformationStage : BaseCodeTransformationStage
	{
		protected virtual string Platform { get { return "Generic"; } }

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public override string Name { get { return Platform + "." + base.Name; } }

		#endregion IPipelineStage Members
	}
}
