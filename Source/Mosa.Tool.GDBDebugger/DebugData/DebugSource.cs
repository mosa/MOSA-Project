// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System.Collections.Generic;

namespace Mosa.Tool.GDBDebugger.DebugData
{
	public class DebugSource
	{
		protected Dictionary<ulong, Instruction> InstructionLookup = new Dictionary<ulong, Instruction>();
		protected KeyedList<ulong, Symbol> SymbolLookup = new KeyedList<ulong, Symbol>();

		public List<Symbol> Symbols { get; private set; } = new List<Symbol>();
		public List<Section> Sections { get; private set; } = new List<Section>();

		public void Add(Symbol symbol)
		{
			Symbols.Add(symbol);
			SymbolLookup.Add(symbol.Address, symbol);
		}

		public void Add(Instruction instruction)
		{
			InstructionLookup.Add(instruction.Address, instruction);
		}

		public void Add(Section section)
		{
			Sections.Add(section);
		}

		public List<Symbol> GetSymbolsStartingAt(ulong address)
		{
			var symbol = SymbolLookup[address];

			return symbol;
		}

		public Instruction GetInstruction(ulong address)
		{
			Instruction instruction = null;

			InstructionLookup.TryGetValue(address, out instruction);

			return instruction;
		}

		public List<Symbol> GetSymbols(ulong address)
		{
			List<Symbol> symbols = new List<Symbol>();

			foreach (var symbol in Symbols)
			{
				if (symbol.Address >= address && (symbol.Address + (ulong)symbol.Length) < address)
				{
					symbols.Add(symbol);
				}
			}

			return symbols;
		}

		public Symbol GetFirstSymbol(ulong address)
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
