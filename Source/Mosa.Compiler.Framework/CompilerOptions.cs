// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Linker;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Compiler Options
	/// </summary>
	public class CompilerOptions
	{
		private readonly Dictionary<string, string> options = new Dictionary<string, string>();

		#region Properties

		/// <summary>
		/// Gets or sets the base address.
		/// </summary>
		public ulong BaseAddress { get; set; }

		/// <summary>
		/// Gets or sets the architecture.
		/// </summary>
		public BaseArchitecture Architecture { get; set; }

		/// <summary>
		/// Gets or sets the output file.
		/// </summary>
		public string OutputFile { get; set; }

		/// <summary>
		/// Gets or sets the map file.
		/// </summary>
		public string MapFile { get; set; }

		/// <summary>
		/// Gets or sets the compile time file.
		/// </summary>
		public string CompileTimeFile { get; set; }

		/// <summary>
		/// Gets or sets the map file.
		/// </summary>
		public string DebugFile { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether SSA is enabled.
		/// </summary>
		public bool EnableSSA { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable IR optimizations].
		/// </summary>
		public bool EnableIROptimizations { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable value numbering].
		/// </summary>
		public bool EnableValueNumbering { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable conditional constant propagation].
		/// </summary>
		public bool EnableSparseConditionalConstantPropagation { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable loop invariant code motion].
		/// </summary>
		public bool EnableLoopInvariantCodeMotion { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable inlined methods].
		/// </summary>
		public bool EnableInlinedMethods { get; set; }

		/// <summary>
		/// Gets or sets the maximum IR numbers for inlined optimization.
		/// </summary>
		public int InlinedIRMaximum { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether static allocations are enabled.
		/// </summary>
		public bool EnableStaticAllocations { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable IR long operand conversion].
		/// </summary>
		public bool EnableIRLongExpansion { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable bit estimator].
		/// </summary>
		public bool EnableBitTracker { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable platform optimizations].
		/// </summary>
		public bool EnablePlatformOptimizations { get; set; }

		/// <summary>
		/// Gets or sets the type of the elf.
		/// </summary>
		public LinkerFormatType LinkerFormatType { get; set; }

		/// <summary>
		/// Gets or sets the multiboot specification.
		/// </summary>
		public MultibootSpecification MultibootSpecification { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [emit binary].
		/// </summary>
		public bool EmitBinary { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [emit symbols].
		/// </summary>
		public bool EmitAllSymbols { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [emit relocations].
		/// </summary>
		public bool EmitStaticRelocations { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [aggressive optimizations].
		/// </summary>
		public bool TwoPassOptimizations { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable statistics].
		/// </summary>
		public bool EnableStatistics { get; set; }

		/// <summary>
		/// Gets or sets the trace level.
		/// </summary>
		public int TraceLevel { get; set; }

		/// <summary>
		/// Gets or sets the include paths.
		/// </summary>
		public List<string> SearchPaths { get; set; } = new List<string>();

		/// <summary>
		/// Gets or sets the source files.
		/// </summary>
		public List<string> SourceFiles { get; set; } = new List<string>();

		/// <summary>
		/// Gets or sets a value indicating whether [enable method scanner].
		/// </summary>
		public bool EnableMethodScanner { get; set; }

		/// <summary>
		/// Adds additional sections to the Elf-File.
		/// </summary>
		public MosaLinker.CreateExtraSectionsDelegate CreateExtraSections { get; set; }

		/// <summary>
		/// Adds additional program headers to the Elf-File.
		/// </summary>
		public MosaLinker.CreateExtraProgramHeaderDelegate CreateExtraProgramHeaders { get; set; }

		#endregion Properties

		/// <summary>
		/// Adds the search path.
		/// </summary>
		/// <param name="path">The path.</param>
		public void AddSearchPath(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				return;

			SearchPaths.AddIfNew(path);
		}

		/// <summary>
		/// Adds the search paths.
		/// </summary>
		/// <param name="files">The files.</param>
		public void AddSearchPaths(IEnumerable<FileInfo> files)
		{
			foreach (var file in files)
			{
				if (file == null)
					continue;

				AddSearchPath(Path.GetDirectoryName(file.FullName));
			}
		}

		/// <summary>
		/// Adds the search paths.
		/// </summary>
		/// <param name="paths">The paths.</param>
		public void AddSearchPaths(IList<string> paths)
		{
			foreach (var path in paths)
			{
				AddSearchPath(Path.GetDirectoryName(path));
			}
		}

		/// <summary>
		/// Adds the source file.
		/// </summary>
		/// <param name="path">The path.</param>
		public void AddSourceFile(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				return;

			SourceFiles.AddIfNew(path);
		}

		/// <summary>
		/// Adds the source files.
		/// </summary>
		/// <param name="files">The files.</param>
		public void AddSourceFiles(IEnumerable<FileInfo> files)
		{
			foreach (var file in files)
			{
				if (file == null)
					continue;

				AddSourceFile(file.FullName);
			}
		}

		/// <summary>
		/// Sets the custom option.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public void SetCustomOption(string name, string value)
		{
			if (options.ContainsKey(name))
				options.Remove(name);

			options.Add(name, value);
		}

		public string GetCustomOption(string name)
		{
			if (options.ContainsKey(name))
				return options[name];

			return null;
		}

		/// <summary>
		/// Gets the custom option as boolean.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="default">if set to <c>true</c> [default].</param>
		/// <returns></returns>
		public bool GetCustomOptionAsBoolean(string name, bool @default = false)
		{
			var value = GetCustomOption(name);

			if (value == null)
				return @default;

			value = value.ToLower();

			return value.Contains("y") || value.Contains("t") || value.Contains("1");
		}

		/// <summary>
		/// Gets the custom option as integer.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="default">The default.</param>
		/// <returns></returns>
		public int? GetCustomOptionAsInteger(string name, int? @default = null)
		{
			var value = GetCustomOption(name);

			if (value == null)
				return @default;

			if (int.TryParse(value, out int val))
				return val;
			else
				return @default;
		}

		/// <summary>
		/// Gets the custom option as integer.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="default">The default.</param>
		/// <returns></returns>
		public int GetCustomOptionAsInteger(string name, int @default = 0)
		{
			var value = GetCustomOption(name);

			if (value == null)
				return @default;

			if (int.TryParse(value, out int val))
				return val;
			else
				return @default;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CompilerOptions"/> class.
		/// </summary>
		public CompilerOptions()
		{
			TraceLevel = 0;
			EnableSSA = true;
			EnableIROptimizations = true;
			EnableSparseConditionalConstantPropagation = true;
			EnableInlinedMethods = false;
			BaseAddress = 0x00400000;
			EmitBinary = true;
			InlinedIRMaximum = 8;
			EmitAllSymbols = true;
			EmitStaticRelocations = true;
			TwoPassOptimizations = true;
			EnableStatistics = true;
			EnableIRLongExpansion = true;
			EnableValueNumbering = true;
			EnableLoopInvariantCodeMotion = true;
			EnablePlatformOptimizations = true;
			EnableMethodScanner = false;
			EnableBitTracker = true;
		}
	}
}
