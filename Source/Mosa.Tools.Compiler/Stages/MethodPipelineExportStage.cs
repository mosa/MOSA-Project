/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.IO;
using System.Collections.Generic;

using Mosa.Compiler.Linker;
using Mosa.Compiler.Framework;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Tools.Compiler.Stages
{
	/// <summary>
	/// An assembly compilation stage, which exports each method pipeline stage
	/// </summary>
	public sealed class MethodPipelineExportStage : BaseAssemblyCompilerStage, IAssemblyCompilerStage, IPipelineStage, IInstructionTraceListener
	{
		#region Data members

		/// <summary>
		/// Gets or sets the method pipeline export directory.
		/// </summary>
		/// <value>The method pipeline export directory.</value>
		public string MethodPipelineExportDirectory { get; set; }

		/// <summary>
		/// 
		/// </summary>
		private ConfigurableInstructionTraceFilter filter = new ConfigurableInstructionTraceFilter();

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MapFileGenerationStage"/> class.
		/// </summary>
		public MethodPipelineExportStage()
		{
		}

		#endregion // Construction

		#region IAssemblyCompilerStage Members

		void IAssemblyCompilerStage.Setup(AssemblyCompiler compiler)
		{
			base.Setup(compiler);

			this.MethodPipelineExportDirectory = compiler.CompilerOptions.MethodPipelineExportDirectory;
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IAssemblyCompilerStage.Run()
		{
			filter.IsLogging = !string.IsNullOrEmpty(MethodPipelineExportDirectory);

			if (filter.IsLogging)
			{
				filter.MethodMatch = MatchType.Any;
				filter.StageMatch = MatchType.Exclude;
				filter.Stage = "PlatformStubStage|ExceptionLayoutStage";
				
				compiler.InternalTrace.InstructionTraceFilter = filter;
				compiler.InternalTrace.InstructionTraceListener = this;

				Directory.CreateDirectory(MethodPipelineExportDirectory);
			}
		}

		#endregion // IAssemblyCompilerStage Members

		#region IInstructionTraceListener Members

		void IInstructionTraceListener.NotifyNewInstructionTrace(RuntimeMethod method, string stage, string log)
		{
			if (string.IsNullOrEmpty(MethodPipelineExportDirectory))
				return;

			string filename = (method.FullName + ".txt").Replace("<", "[").Replace(">", "]");

			if (filename.Length > 200)
				filename = filename.Substring(0, 200);

			string fullname = Path.Combine(MethodPipelineExportDirectory, filename);

			File.AppendAllText(fullname, "[" + stage + "]" + Environment.NewLine + Environment.NewLine + log + Environment.NewLine);
		}

		#endregion // IInstructionTraceListener Members

	}
}
