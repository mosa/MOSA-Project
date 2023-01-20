// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Common.Configuration;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Compiler Options
	/// </summary>
	public class CompilerSettings
	{
		#region Properties

		public Settings Settings { get; } = new Settings();

		public string Platform => Settings.GetValue("Compiler.Platform", "x86");

		public string OutputFile => Settings.GetValue("Compiler.OutputFile", "_main.exe");

		public string MapFile => Settings.GetValue("CompilerDebug.MapFile", null);

		public string CompileTimeFile => Settings.GetValue("CompilerDebug.CompileTimeFile", null);

		public string DebugFile => Settings.GetValue("CompilerDebug.DebugFile", null);

		public string InlinedFile => Settings.GetValue("CompilerDebug.InlinedFile", null);

		public string PreLinkHashFile => Settings.GetValue("CompilerDebug.PreLinkHashFile", null);

		public string PostLinkHashFile => Settings.GetValue("CompilerDebug.PostLinkHashFile", null);

		public bool SSA => Settings.GetValue("Optimizations.SSA", true);

		public bool BasicOptimizations => Settings.GetValue("Optimizations.Basic", true);

		public bool ValueNumbering => Settings.GetValue("Optimizations.ValueNumbering", true);

		public bool SparseConditionalConstantPropagation => Settings.GetValue("Optimizations.SCCP", true);

		public bool Devirtualization => Settings.GetValue("Optimizations.Devirtualization", true);

		public bool LoopInvariantCodeMotion => Settings.GetValue("Optimizations.LoopInvariantCodeMotion", true);

		public bool InlineMethods => Settings.GetValue("Optimizations.Inline", true);

		public bool InlineExplicit => Settings.GetValue("Optimizations.Inline.Explicit", true);

		public bool LongExpansion => Settings.GetValue("Optimizations.LongExpansion", true);

		public bool TwoPassOptimizations => Settings.GetValue("Optimizations.TwoPass", true);

		public bool BitTracker
		{ get { return Settings.GetValue("Optimizations.BitTracker", true); } }

		public int OptimizationWindow => Settings.GetValue("Optimizations.Basic.Window", 5);

		public int InlineMaximum => Settings.GetValue("Optimizations.Inline.Maximum", 12);

		public int InlineAggressiveMaximum => Settings.GetValue("Optimizations.Inline.AggressiveMaximum", 24);

		public bool PlatformOptimizations => Settings.GetValue("Optimizations.Platform", true);

		public string LinkerFormat => Settings.GetValue("Linker.Format", "elf32");

		public bool EmitBinary => Settings.GetValue("Compiler.Binary", true);

		public bool EmitDrawf => Settings.GetValue("Linker.Drawf", false);

		public bool TwoPass => Settings.GetValue("Optimizations.TwoPass", true);

		public bool Statistics => Settings.GetValue("CompilerDebug.Statistics", true);

		public int TraceLevel => Settings.GetValue("Compiler.TraceLevel", 0);

		public bool MethodScanner => Settings.GetValue("Compiler.MethodScanner", false);

		public bool EmitInline => Settings.GetValue("Compiler.EmitInline", false);

		public bool Multithreading => Settings.GetValue("Compiler.Multithreading", true);

		public int MaxThreads => Settings.GetValue("Compiler.Multithreading.MaxThreads", 0);

		public List<string> SearchPaths => Settings.GetValueList("SearchPaths");

		public List<string> SourceFiles => Settings.GetValueList("Compiler.SourceFiles");

		public List<string> InlineAggressiveList => Settings.GetValueList("Optimizations.Inline.Aggressive");

		public List<string> InlineExcludeList => Settings.GetValueList("Optimizations.Inline.Exclude");

		public bool FullCheckMode => Settings.GetValue("CompilerDebug.FullCheckMode", false);

		public bool CILDecodingStageV2 => Settings.GetValue("CompilerDebug.CILDecodingStageV2", false);

		#endregion Properties

		public CompilerSettings(Settings settings)
		{
			// defaults
			Settings.SetValue("Compiler.MethodScanner", false);
			Settings.SetValue("Compiler.Binary", true);
			Settings.SetValue("Compiler.TraceLevel", 0);
			Settings.SetValue("Compiler.Platform", "x86");
			Settings.SetValue("Compiler.Multithreading", true);
			//Settings.SetValue("Compiler.Multithreading.MaxThreads", 1);
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
			Settings.SetValue("Optimizations.Inline.Explicit", true);
			Settings.SetValue("Optimizations.Inline.Maximum", 12);
			Settings.SetValue("Optimizations.Basic.Window", 5);
			Settings.SetValue("Optimizations.Inline.AggressiveMaximum", 24);
			Settings.SetValue("Multiboot.Version", "v1");
			Settings.SetValue("Compiler.Platform", "x86");
			Settings.SetValue("OS.Name", "MOSA");

			Settings.Merge(settings);
		}
	}
}
