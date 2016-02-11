// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker;
using System;
using System.Collections.Generic;

namespace Mosa.TinyCPUSimulator.Adaptor
{
	/// <summary>
	/// Finalizes the Sim linking
	/// </summary>
	public sealed class SimLinkerFinalizationStage : BaseCompilerStage
	{
		private ISimAdapter simAdapter;

		private class SymbolInfo
		{
			public List<Tuple<int, string>> targetSymbols = new List<Tuple<int, string>>();
			public List<Tuple<long, SimInstruction>> instructions = new List<Tuple<long, SimInstruction>>();
			public List<Tuple<long, string>> source = new List<Tuple<long, string>>();
		}

		private Dictionary<LinkerSymbol, SymbolInfo> symbolData = new Dictionary<LinkerSymbol, SymbolInfo>();

		public SimLinkerFinalizationStage(ISimAdapter simAdapter)
		{
			this.simAdapter = simAdapter;
		}

		protected override void Run()
		{
			var stream = new SimStream(simAdapter.SimCPU, Linker.BaseAddress);

			Linker.Emit(stream);

			UpdateSim();

			symbolData = null;
		}

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

		private void UpdateSim()
		{
			foreach (var symbol in Linker.Symbols)
			{
				simAdapter.SimCPU.SetSymbol(symbol.Name, symbol.VirtualAddress, symbol.Size);
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
		}
	}
}
