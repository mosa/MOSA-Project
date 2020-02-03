// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Configuration;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Compiler Options
	/// </summary>
	public class CompilerSettings
	{
		#region Properties

		public Settings Settings { get; } = new Settings();

		public string Platform { get { return Settings.GetValue("Compiler.Platform", "x86"); } }

		public string OutputFile { get { return Settings.GetValue("Compiler.OutputFile", "_main.exe"); } }

		public string MapFile { get { return Settings.GetValue("CompilerDebug.MapFile", null); } }

		public string CompileTimeFile { get { return Settings.GetValue("CompilerDebug.CompileTimeFile", null); } }

		public string DebugFile { get { return Settings.GetValue("CompilerDebug.DebugFile", null); } }

		public string InlinedFile { get { return Settings.GetValue("CompilerDebug.InlinedFile", null); } }

		public string PreLinkHashFile { get { return Settings.GetValue("CompilerDebug.PreLinkHashFile", null); } }

		public string PostLinkHashFile { get { return Settings.GetValue("CompilerDebug.PostLinkHashFile", null); } }

		public bool SSA { get { return Settings.GetValue("Optimizations.SSA", true); } }

		public bool BasicOptimizations { get { return Settings.GetValue("Optimizations.Basic", true); } }

		public bool ValueNumbering { get { return Settings.GetValue("Optimizations.ValueNumbering", true); } }

		public bool SparseConditionalConstantPropagation { get { return Settings.GetValue("Optimizations.SCCP", true); } }

		public bool Devirtualization { get { return Settings.GetValue("Optimizations.Devirtualization", true); } }

		public bool LoopInvariantCodeMotion { get { return Settings.GetValue("Optimizations.LoopInvariantCodeMotion", true); } }

		public bool InlineMethods { get { return Settings.GetValue("Optimizations.Inline", true); } }

		public bool InlineExplicitOnly { get { return Settings.GetValue("Optimizations.Inline.ExplicitOnly", false); } }

		public bool LongExpansion { get { return Settings.GetValue("Optimizations.LongExpansion", true); } }

		public bool TwoPassOptimizations { get { return Settings.GetValue("Optimizations.TwoPass", true); } }

		public bool BitTracker { get { return Settings.GetValue("Optimizations.BitTracker", true); } }

		public int InlineMaximum { get { return Settings.GetValue("Optimizations.Inline.Maximum", 12); } }

		public int InlineAggressiveMaximum { get { return Settings.GetValue("Optimizations.Inline.AggressiveMaximum", 24); } }

		public bool PlatformOptimizations { get { return Settings.GetValue("Optimizations.Platform", true); } }

		public string LinkerFormat { get { return Settings.GetValue("Linker.Format", "elf32"); } }

		public bool EmitBinary { get { return Settings.GetValue("Compiler.Binary", true); } }

		public bool EmitDrawf { get { return Settings.GetValue("Linker.Drawf", false); } }

		public bool TwoPass { get { return Settings.GetValue("Optimizations.TwoPass", true); } }

		public bool Statistics { get { return Settings.GetValue("CompilerDebug.Statistics", true); } }

		public int TraceLevel { get { return Settings.GetValue("Compiler.TraceLevel", 0); } }

		public bool MethodScanner { get { return Settings.GetValue("Compiler.MethodScanner", false); } }

		public bool EmitInline { get { return Settings.GetValue("Compiler.EmitInline", false); } }

		public int MaxThreads { get { return Settings.GetValue("Compiler.Multithreading.MaxThreads", 0); } }

		public List<string> SearchPaths { get { return Settings.GetValueList("SearchPaths"); } }

		public List<string> SourceFiles { get { return Settings.GetValueList("Compiler.SourceFiles"); } }

		public List<string> InlineAggressiveList { get { return Settings.GetValueList("Optimizations.Inline.Aggressive"); } }

		public List<string> InlineExcludeList { get { return Settings.GetValueList("Optimizations.Inline.Exclude"); } }

		#endregion Properties

		public CompilerSettings(Settings settings)
		{
			// defaults
			Settings.SetValue("Compiler.MethodScanner", false);
			Settings.SetValue("Compiler.Binary", true);
			Settings.SetValue("Compiler.TraceLevel", 0);
			Settings.SetValue("Compiler.Platform", "x86");
			Settings.SetValue("Compiler.Multithreading", true);
			Settings.SetValue("Optimizations.SSA", true);
			Settings.SetValue("Optimizations.Basic", true);
			Settings.SetValue("Optimizations.ValueNumbering", true);
			Settings.SetValue("Optimizations.SCCP", true);
			Settings.SetValue("Optimizations.Devirtualization", true);
			Settings.SetValue("Optimizations.BitTracker", true);
			Settings.SetValue("Optimizations.LoopInvariantCodeMotion", true);
			Settings.SetValue("Optimizations.LongExpansion", true);
			Settings.SetValue("Optimizations.TwoPass", true);
			Settings.SetValue("Optimizations.Platform", true);
			Settings.SetValue("Optimizations.Inline", true);
			Settings.SetValue("Optimizations.Inline.ExplicitOnly", false);
			Settings.SetValue("Optimizations.Inline.Maximum", 12);
			Settings.SetValue("Optimizations.Inline.AggressiveMaximum", 24);
			Settings.SetValue("Multiboot.Version", "v1");
			Settings.SetValue("Compiler.Platform", "x86");

			Settings.Merge(settings);
		}
	}
}
