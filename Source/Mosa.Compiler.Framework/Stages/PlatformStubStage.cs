// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Platform Stub Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseMethodCompilerStage" />
	/// <seealso cref="Mosa.Compiler.Framework.IPipelineStage" />
	public sealed class PlatformStubStage : BaseMethodCompilerStage, IPipelineStage
	{
		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>
		/// The name of the compilation stage.
		/// </value>
		string IPipelineStage.Name { get { return "PlatformStubStage"; } }

		#endregion IPipelineStage Members
	}
}
