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
		public List<FieldInfo> Fields { get; } = new List<FieldInfo>();

		public Dictionary<ulong, TypeInfo> TypeDefLookup = new Dictionary<ulong, TypeInfo>();
		public Dictionary<string, TypeInfo> TypeFullNameLookup = new Dictionary<string, TypeInfo>();
		public Dictionary<string, MethodInfo> MethodFullNameLookup = new Dictionary<string, MethodInfo>();

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

		public void Add(TypeInfo type)
		{
			Types.Add(type);

			if (type.DefAddress != 0)
			{
				TypeDefLookup.Add(type.DefAddress, type);
			}

			TypeFullNameLookup.Add(type.FullName, type);
		}

		public void Add(MethodInfo method)
		{
			Methods.Add(method);

			MethodFullNameLookup.Add(method.FullName, method);
		}

		public void Add(ParameterInfo parameter)
		{
			Parameters.Add(parameter);
		}

		public void Add(FieldInfo field)
		{
			Fields.Add(field);
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
