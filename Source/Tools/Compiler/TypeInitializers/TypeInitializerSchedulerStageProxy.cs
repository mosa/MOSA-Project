/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <sharpos@michaelruck.de>
 */

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Vm;

namespace Mosa.Tools.Compiler.TypeInitializers
{
	public class TypeInitializerSchedulerStageProxy : BaseAssemblyCompilerStage, ITypeInitializerSchedulerStage, IAssemblyCompilerStage
	{
		private readonly ITypeInitializerSchedulerStage realStage;

		#region IPipelineStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public string Name { get { return @"TypeInitializerSchedulerStageProxy"; } }

		#endregion // IPipelineStage Members

		#region IAssemblyCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public void Run()
		{

		}

		#endregion // IAssemblyCompilerStage Members

		public TypeInitializerSchedulerStageProxy(ITypeInitializerSchedulerStage realStage)
		{
			this.realStage = realStage;
		}

		/// <summary>
		/// Gets the intializer method.
		/// </summary>
		/// <value>The method.</value>
		public CompilerGeneratedMethod Method
		{
			get
			{
				return this.realStage.Method;
			}
		}

		/// <summary>
		/// Schedules the specified method for invocation in the main.
		/// </summary>
		/// <param name="method">The method.</param>
		public void Schedule(RuntimeMethod method)
		{
			this.realStage.Schedule(method);
		}


	}
}