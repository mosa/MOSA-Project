// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System.Collections.Generic;

namespace Mosa.Tool.GDBDebugger.DebugData
{
	public class DebugSource
	{
		protected Dictionary<ulong, InstructionInfo> InstructionLookup = new Dictionary<ulong, InstructionInfo>();
		protected KeyedList<ulong, SymbolInfo> SymbolLookup = new KeyedList<ulong, SymbolInfo>();

		public List<SymbolInfo> Symbols { get; } = new List<SymbolInfo>();
		public List<SectionInfo> Sections { get; } = new List<SectionInfo>();

		public List<TypeInfo> Types { get; } = new List<TypeInfo>();
		public List<MethodInfo> Methods { get; } = new List<MethodInfo>();
		public List<ParameterInfo> Parameters { get; } = new List<ParameterInfo>();

		public void Add(SymbolInfo symbol)
		{
			Symbols.Add(symbol);
			SymbolLookup.Add(symbol.Address, symbol);
		}

		public void Add(InstructionInfo instruction)
		{
			InstructionLookup.Add(instruction.Address, instruction);
		}

		public void Add(SectionInfo section)
		{
			Sections.Add(section);
		}

		public void Add(TypeInfo section)
		{
			Types.Add(section);
		}

		public void Add(MethodInfo section)
		{
			Methods.Add(section);
		}

		public void Add(ParameterInfo section)
		{
			Parameters.Add(section);
		}

		public List<SymbolInfo> GetSymbolsStartingAt(ulong address)
		{
			return SymbolLookup[address];
		}

		public InstructionInfo GetInstruction(ulong address)
		{
			InstructionInfo instruction = null;

			InstructionLookup.TryGetValue(address, out instruction);

			return instruction;
		}

		public List<SymbolInfo> GetSymbols(ulong address)
		{
			var symbols = new List<SymbolInfo>();

			foreach (var symbol in Symbols)
			{
				if (symbol.Address >= address && (symbol.Address + (ulong)symbol.Length) < address)
				{
					symbols.Add(symbol);
				}
			}

			return symbols;
		}

		public SymbolInfo GetFirstSymbol(ulong address)
		{
			foreach (var symbol in Symbols)
			{
				if (address >= symbol.Address && address < (symbol.Address + (ulong)symbol.Length))
				{
					return symbol;
				}
			}

			return null;
		}
	}
}
