// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Platform
{
	/// <summary>
	/// Base Platform Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCodeTransformationStage" />
	public abstract class BasePlatformTransformationStage : BaseCodeTransformationStage
	{
		/// <summary>
		/// Gets the platform.
		/// </summary>
		/// <value>
		/// The platform.
		/// </value>
		protected virtual string Platform { get { return "Intel"; } }

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public override string Name { get { return Platform + "." + base.Name; } }

		#endregion IPipelineStage Members
	}
}
