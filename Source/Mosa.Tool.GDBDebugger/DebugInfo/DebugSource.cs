// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System.Collections.Generic;

namespace Mosa.Tool.GDBDebugger.DebugInfo
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

		public List<Symbol> LookupSymbols(ulong address)
		{
			var symbol = SymbolLookup[address];

			return symbol;
		}

		public Instruction LookupInstruction(ulong address)
		{
			Instruction instruction = null;

			InstructionLookup.TryGetValue(address, out instruction);

			return instruction;
		}

		//public Section LookupSection(ulong address)
		//{
		//	Section section = null;

		//	SectionLookup.TryGetValue(address, out section);

		//	return section;
		//}
	}
}
