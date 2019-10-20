// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Workspace.Experiment.Debug
{
	static public class YamlCompilerOptions
	{
		public static void Parse(string yamlText, CompilerOptions compilerOptions)
		{
			var optimizations = YamlQuery.Query(yamlText).On("Optimizations");

			optimizations.On("Basic").TryParseBool(value => compilerOptions.EnableBasicOptimizations = value);
			optimizations.On("SCCP").TryParseBool(value => compilerOptions.EnableSparseConditionalConstantPropagation = value);
			optimizations.On("InlineMethods").TryParseBool(value => compilerOptions.EnableInlineMethods = value);
			optimizations.On("InlineMaximum").TryParseInt32(value => compilerOptions.InlineMaximum = value);
			optimizations.On("InlineOnlyExplicit").TryParseBool(value => compilerOptions.InlineOnlyExplicit = value);
			optimizations.On("LongExpansion").TryParseBool(value => compilerOptions.EnableLongExpansion = value);
			optimizations.On("TwoPass").TryParseBool(value => compilerOptions.TwoPassOptimizations = value);
			optimizations.On("ValueNumbering").TryParseBool(value => compilerOptions.EnableValueNumbering = value);
			optimizations.On("SSA").TryParseBool(value => compilerOptions.EnableSSA = value);
			optimizations.On("Platform").TryParseBool(value => compilerOptions.EnablePlatformOptimizations = value);
			optimizations.On("BitTracker").TryParseBool(value => compilerOptions.EnableBitTracker = value);
		}
	}
}
