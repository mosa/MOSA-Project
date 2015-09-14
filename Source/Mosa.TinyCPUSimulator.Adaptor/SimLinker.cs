// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Linker.Flat;
using System.Collections.Generic;
using System.IO;

namespace Mosa.TinyCPUSimulator.Adaptor
{
	/// <summary>
	/// </summary>
	public class SimLinker : FlatLinker
	{
		private ISimAdapter simAdapter;

		private class SymbolInfo
		{
			public List<Tuple<int, string>> targetSymbols = new List<Tuple<int, string>>();
			public List<Tuple<long, SimInstruction>> instructions = new List<Tuple<long, SimInstruction>>();
			public List<Tuple<long, string>> source = new List<Tuple<long, string>>();
		}

		private Dictionary<LinkerSymbol, SymbolInfo> symbolData = new Dictionary<LinkerSymbol, SymbolInfo>();

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SimLinker" /> class.
		/// </summary>
		/// <param name="simAdapter">The sim adapter.</param>
		public SimLinker(ISimAdapter simAdapter)
		{
			this.simAdapter = simAdapter;
		}

		#endregion Construction

		private SymbolInfo GetSymbolInfo(LinkerSymbol linkerSymbol)
		{
			SymbolInfo symbolInfo = null;

			lock (symbolData)
			{
				if (!symbolData.TryGetValue(linkerSymbol, out symbolInfo))
				{
					symbolInfo = new SymbolInfo();
					symbolData.Add(linkerSymbol, symbolInfo);
				}
			}

			return symbolInfo;
		}

		public void ClearSymbolInformation(LinkerSymbol linkerSymbol)
		{
			lock (symbolData)
			{
				symbolData.Remove(linkerSymbol);
			}
		}

		public void AddTargetSymbol(LinkerSymbol baseSymbol, int offset, string name)
		{
			var symbolInfo = GetSymbolInfo(baseSymbol);

			symbolInfo.targetSymbols.Add(new Tuple<int, string>(offset, name));
		}

		public void AddInstruction(LinkerSymbol baseSymbol, long offset, SimInstruction instruction)
		{
			var symbolInfo = GetSymbolInfo(baseSymbol);

			symbolInfo.instructions.Add(new Tuple<long, SimInstruction>(offset, instruction));
		}

		public void AddSourceInformation(LinkerSymbol baseSymbol, long offset, string information)
		{
			var symbolInfo = GetSymbolInfo(baseSymbol);

			symbolInfo.source.Add(new Tuple<long, string>(offset, information));
		}

		protected override void EmitImplementation(Stream stream)
		{
			base.EmitImplementation(stream);

			foreach (var symbol in Symbols)
			{
				simAdapter.SimCPU.SetSymbol(symbol.Name, symbol.VirtualAddress, (ulong)symbol.Size);
			}

			foreach (var symbol in symbolData)
			{
				foreach (var target in symbol.Value.targetSymbols)
				{
					simAdapter.SimCPU.SetSymbol(target.Item2, symbol.Key.VirtualAddress + (ulong)target.Item1, 0);
				}

				foreach (var target in symbol.Value.instructions)
				{
					simAdapter.SimCPU.AddInstruction(symbol.Key.VirtualAddress + (ulong)target.Item1, target.Item2);
				}

				foreach (var target in symbol.Value.source)
				{
					simAdapter.SimCPU.AddSourceInformation(symbol.Key.VirtualAddress + (ulong)target.Item1, target.Item2);
				}
			}

			symbolData = null;
		}
	}
}
