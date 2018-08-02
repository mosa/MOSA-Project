﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Linker;
using System;
using System.Collections.Generic;

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
		/// <value>
		/// The base address.
		/// </value>
		public ulong BaseAddress { get; set; }

		/// <summary>
		/// Gets or sets the architecture.
		/// </summary>
		/// <value>The architecture.</value>
		public BaseArchitecture Architecture { get; set; }

		/// <summary>
		/// Gets or sets the output file.
		/// </summary>
		/// <value>The output file.</value>
		public string OutputFile { get; set; }

		/// <summary>
		/// Gets or sets the map file.
		/// </summary>
		/// <value>The map file.</value>
		public string MapFile { get; set; }

		/// <summary>
		/// Gets or sets the map file.
		/// </summary>
		/// <value>The map file.</value>
		public string DebugFile { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether SSA is enabled.
		/// </summary>
		/// <value><c>true</c> if SSA is enabled; otherwise, <c>false</c>.</value>
		public bool EnableSSA { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable IR optimizations].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [enable IR optimizations]; otherwise, <c>false</c>.
		/// </value>
		public bool EnableIROptimizations { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable value numbering].
		/// </summary>
		/// <value>
		///   <c>true</c> if [enable IR optimizations]; otherwise, <c>false</c>.
		/// </value>
		public bool EnableValueNumbering { get; set; }

		/// <summary>
		/// Gets or sets the debug restrict optimization by count.
		/// </summary>
		/// <value>
		/// The debug restrict optimization by count.
		/// </value>
		public int DebugRestrictOptimizationByCount { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable conditional constant propagation].
		/// </summary>
		/// <value>
		/// <c>true</c> if [enable conditional constant propagation]; otherwise, <c>false</c>.
		/// </value>
		public bool EnableSparseConditionalConstantPropagation { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable inlined methods].
		/// </summary>
		/// <value>
		/// <c>true</c> if [enable inlined methods]; otherwise, <c>false</c>.
		/// </value>
		public bool EnableInlinedMethods { get; set; }

		/// <summary>
		/// Gets or sets the maximum IR numbers for inlined optimization.
		/// </summary>
		/// <value>
		/// The maximum IR numbers for inlined optimization.
		/// </value>
		public int InlinedIRMaximum { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether static allocations are enabled.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if static allocations are enabled; otherwise, <c>false</c>.
		/// </value>
		public bool EnableStaticAllocations { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable ir long operand conversion].
		/// </summary>
		/// <value>
		///   <c>true</c> if [enable ir long operand conversion]; otherwise, <c>false</c>.
		/// </value>
		public bool IRLongExpansion { get; set; }

		/// <summary>
		/// Gets or sets the block order analysis.
		/// </summary>
		/// <value>
		/// The block order analysis.
		/// </value>
		public Func<BaseBlockOrder> BlockOrderAnalysisFactory { get; set; }

		/// <summary>
		/// Gets or sets the type of the elf.
		/// </summary>
		public LinkerFormatType LinkerFormatType { get; set; }

		/// <summary>
		/// Gets or sets the compiler stage responsible for booting.
		/// </summary>
		public Func<BaseCompilerStage> BootStageFactory { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [emit binary].
		/// </summary>
		/// <value>
		///   <c>true</c> if [emit binary]; otherwise, <c>false</c>.
		/// </value>
		public bool EmitBinary { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [emit symbols].
		/// </summary>
		/// <value>
		///   <c>true</c> if [emit symbols]; otherwise, <c>false</c>.
		/// </value>
		public bool EmitSymbols { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [emit relocations].
		/// </summary>
		/// <value>
		///   <c>true</c> if [emit relocations]; otherwise, <c>false</c>.
		/// </value>
		public bool EmitRelocations { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [aggressive optimizations].
		/// </summary>
		/// <value>
		/// <c>true</c> if [aggressive optimizations]; otherwise, <c>false</c>.
		/// </value>
		public bool TwoPassOptimizations { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable statistics].
		/// </summary>
		/// <value>
		///   <c>true</c> if [enable statistics]; otherwise, <c>false</c>.
		/// </value>
		public bool EnableStatistics { get; set; }

		#endregion Properties

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
			EnableSSA = true;
			EnableIROptimizations = true;
			EnableSparseConditionalConstantPropagation = true;
			EnableInlinedMethods = false;
			BaseAddress = 0x00400000;
			BlockOrderAnalysisFactory = delegate { return new LoopAwareBlockOrder(); };
			EmitBinary = true;
			InlinedIRMaximum = 8;
			DebugRestrictOptimizationByCount = 0;
			EmitSymbols = true;
			EmitRelocations = true;
			TwoPassOptimizations = true;
			EnableStatistics = true;
			IRLongExpansion = true;
			EnableValueNumbering = true;
		}
	}
}
