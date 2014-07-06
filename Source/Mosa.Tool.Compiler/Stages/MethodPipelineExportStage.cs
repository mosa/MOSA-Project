/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.IO;

namespace Mosa.Tool.Compiler.Stages
{
	/// <summary>
	/// An compilation stage, which exports each method pipeline stage
	/// </summary>
	public sealed class MethodPipelineExportStage : BaseCompilerStage, ITraceListener
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
		private ConfigurableTraceFilter filter = new ConfigurableTraceFilter();

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MapFileGenerationStage"/> class.
		/// </summary>
		public MethodPipelineExportStage()
		{
		}

		#endregion Construction

		protected override void Setup()
		{
			this.MethodPipelineExportDirectory = CompilerOptions.MethodPipelineExportDirectory;
		}

		protected override void Run()
		{
			bool logging = !string.IsNullOrEmpty(MethodPipelineExportDirectory);

			if (logging)
			{
				filter.MethodMatch = MatchType.Any;
				//filter.StageMatch = MatchType.Exclude;
				//filter.Stage = "PlatformStubStage|ExceptionLayoutStage";
				filter.StageMatch = MatchType.Contains;
				filter.Stage = "CodeGen";

				Compiler.InternalTrace.TraceFilter = filter;
				Compiler.InternalTrace.TraceListener = this;

				Directory.CreateDirectory(MethodPipelineExportDirectory);
			}
		}

		#region ITraceListener Members

		void ITraceListener.SubmitInstructionTraceInformation(MosaMethod method, string stage, string log)
		{
			if (string.IsNullOrEmpty(MethodPipelineExportDirectory))
				return;

			string filename = (method.FullName).Replace("<", "[").Replace(">", "]").Replace(":", "-").Replace("*", "");

			if (filename.Length > 200)
				filename = filename.Substring(0, 200);

			string fullname = Path.Combine(MethodPipelineExportDirectory, filename + ".txt");

			File.AppendAllText(fullname, "[" + stage + "]" + Environment.NewLine + Environment.NewLine + log + Environment.NewLine);
		}

		void ITraceListener.SubmitDebugStageInformation(MosaMethod method, string stage, string line)
		{
			// nothing
		}

		#endregion ITraceListener Members
	}
}