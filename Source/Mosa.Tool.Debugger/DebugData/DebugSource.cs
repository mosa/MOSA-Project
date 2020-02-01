// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System.Collections.Generic;

namespace Mosa.Tool.Debugger.DebugData
{
	public class DebugSource
	{
		protected Dictionary<ulong, InstructionInfo> InstructionLookup = new Dictionary<ulong, InstructionInfo>();
		protected KeyedList<ulong, SymbolInfo> SymbolLookup = new KeyedList<ulong, SymbolInfo>();
		protected KeyedList<string, SymbolInfo> SymbolNameLookup = new KeyedList<string, SymbolInfo>();

		protected KeyedList<int, SourceLabelInfo> SourceLabelLookup = new KeyedList<int, SourceLabelInfo>();
		protected KeyedList<int, SourceInfo> SourceLookup = new KeyedList<int, SourceInfo>();

		public List<SymbolInfo> Symbols { get; } = new List<SymbolInfo>();
		public List<SectionInfo> Sections { get; } = new List<SectionInfo>();

		public List<TypeInfo> Types { get; } = new List<TypeInfo>();
		public List<MethodInfo> Methods { get; } = new List<MethodInfo>();
		public List<ParameterInfo> Parameters { get; } = new List<ParameterInfo>();
		public List<FieldInfo> Fields { get; } = new List<FieldInfo>();

		public List<SourceLabelInfo> SourceLabels { get; } = new List<SourceLabelInfo>();
		public List<SourceFileInfo> SourceFileInfos { get; } = new List<SourceFileInfo>();
		public List<SourceInfo> SourceInfos { get; } = new List<SourceInfo>();

		public Dictionary<ulong, TypeInfo> TypeDefLookup = new Dictionary<ulong, TypeInfo>();
		public Dictionary<string, TypeInfo> TypeFullNameLookup = new Dictionary<string, TypeInfo>();
		public Dictionary<string, MethodInfo> MethodFullNameLookup = new Dictionary<string, MethodInfo>();

		public Dictionary<int, TypeInfo> TypeIDLookup = new Dictionary<int, TypeInfo>();
		public Dictionary<int, MethodInfo> MethodIDLookup = new Dictionary<int, MethodInfo>();

		public Dictionary<int, SourceFileInfo> SourceFileLookup = new Dictionary<int, SourceFileInfo>();

		private SymbolInfo cachedSymbol;
		private MethodInfo cachedMethod;
		private int cachedParametersMethodID;
		private List<ParameterInfo> cacheParameters;

		public void Add(SymbolInfo symbol)
		{
			Symbols.Add(symbol);
			SymbolLookup.Add(symbol.Address, symbol);
			SymbolNameLookup.Add(symbol.Name, symbol);
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

			TypeIDLookup.Add(type.ID, type);

			if (type.DefAddress != 0 && !TypeDefLookup.ContainsKey(type.DefAddress))
			{
				TypeDefLookup.Add(type.DefAddress, type);
			}

			if (!TypeFullNameLookup.ContainsKey(type.FullName))
			{
				TypeFullNameLookup.Add(type.FullName, type);
			}
		}

		public void Add(MethodInfo method)
		{
			Methods.Add(method);
			MethodIDLookup.Add(method.ID, method);

			if (!MethodFullNameLookup.ContainsKey(method.FullName))
			{
				MethodFullNameLookup.Add(method.FullName, method);
			}
		}

		public void Add(ParameterInfo parameter)
		{
			Parameters.Add(parameter);
		}

		public void Add(FieldInfo field)
		{
			Fields.Add(field);
		}

		public void Add(SourceLabelInfo sourceLabel)
		{
			SourceLabels.Add(sourceLabel);
			SourceLabelLookup.Add(sourceLabel.MethodID, sourceLabel);
		}

		public void Add(SourceInfo source)
		{
			SourceInfos.Add(source);
			SourceLookup.Add(source.MethodID, source);
		}

		public void Add(SourceFileInfo sourceFile)
		{
			SourceFileInfos.Add(sourceFile);
			SourceFileLookup.Add(sourceFile.SourceFileID, sourceFile);
		}

		public List<SymbolInfo> GetSymbolsStartingAt(ulong address)
		{
			return SymbolLookup[address];
		}

		public SymbolInfo GetFirstSymbolsStartingAt(ulong address)
		{
			var symbols = GetSymbolsStartingAt(address);

			if (symbols == null || symbols.Count == 0)
				return null;

			return symbols[0];
		}

		public InstructionInfo GetInstruction(ulong address)
		{
			InstructionLookup.TryGetValue(address, out InstructionInfo instruction);

			return instruction;
		}

		public List<SymbolInfo> GetSymbols(ulong address)
		{
			var symbols = new List<SymbolInfo>();

			foreach (var symbol in Symbols)
			{
				if (symbol.Address >= address && (symbol.Address + symbol.Length) < address)
				{
					symbols.Add(symbol);
				}
			}

			return symbols;
		}

		public SymbolInfo GetFirstSymbol(ulong address)
		{
			if (cachedSymbol != null)
			{
				if (address >= cachedSymbol.Address && address < (cachedSymbol.Address + cachedSymbol.Length))
				{
					return cachedSymbol;
				}
			}

			foreach (var symbol in Symbols)
			{
				if (address >= symbol.Address && address < (symbol.Address + symbol.Length))
				{
					cachedSymbol = symbol;
					return symbol;
				}
			}

			return null;
		}

		public ulong GetFirstSymbolByName(string name)
		{
			var sublist = SymbolNameLookup.Get(name);

			if (sublist == null)
				return 0;

			return sublist[0].Address;
		}

		public MethodInfo GetMethod(ulong address)
		{
			if (cachedMethod != null)
			{
				if (address >= cachedMethod.Address && address < (cachedMethod.Address + cachedMethod.Size))
				{
					return cachedMethod;
				}
			}

			foreach (var method in Methods)
			{
				if (address >= method.Address && address < (method.Address + method.Size))
				{
					cachedMethod = method;
					return method;
				}
			}

			return null;
		}

		public List<SourceLabelInfo> GetSourceLabels(int methodID)
		{
			return SourceLabelLookup.Get(methodID);
		}

		public SourceLabelInfo GetSourceLabel(int methodID, int offset)
		{
			var list = SourceLabelLookup.Get(methodID);

			if (list == null)
				return null;

			foreach (var sourceLabel in list)
			{
				if (sourceLabel.StartOffset > offset)
					continue;

				if (sourceLabel.StartOffset + sourceLabel.Length < offset)
					continue;

				return sourceLabel;
			}

			return null;
		}

		public List<SourceInfo> GetSources(int methodID)
		{
			return SourceLookup.Get(methodID);
		}

		public SourceInfo GetSource(int methodID, int offset)
		{
			var list = SourceLookup.Get(methodID);

			if (list == null)
				return null;

			foreach (var source in list)
			{
				if (source.Offset == offset)
					return source;
			}

			return null;
		}

		public SourceInfo GetSourcePreviousClosest(int methodID, int offset)
		{
			var list = SourceLookup.Get(methodID);

			if (list == null)
				return null;

			SourceInfo previous = null;

			foreach (var source in list)
			{
				if (source.Offset == offset)
					return source;

				if (source.Offset > offset)
					continue;

				if (previous == null || previous.Offset < source.Offset)
				{
					previous = source;
				}
			}

			return previous;
		}

		public SourceFileInfo GetSourceFile(int sourceFileID)
		{
			SourceFileLookup.TryGetValue(sourceFileID, out SourceFileInfo sourceFileInfo);

			return sourceFileInfo;
		}

		//public List<ParameterInfo> Parameters { get; } = new List<ParameterInfo>();
		public List<ParameterInfo> GetMethodParameters(MethodInfo methodInfo)
		{
			if (methodInfo.ParameterStackSize == 0)
				return null;

			return GetMethodParameters(methodInfo.ID);
		}

		public List<ParameterInfo> GetMethodParameters(int methodID)
		{
			if (cachedParametersMethodID == methodID)
			{
				return cacheParameters;
			}

			List<ParameterInfo> parameters = null;

			foreach (var parameter in Parameters)
			{
				if (parameter.MethodID == methodID)
				{
					if (parameters == null)
					{
						parameters = new List<ParameterInfo>();
					}

					parameters.Add(parameter);
				}
			}

			cachedParametersMethodID = methodID;
			cacheParameters = parameters;

			return parameters;
		}

		public TypeInfo GetTypeInfo(int typeID)
		{
			return TypeIDLookup[typeID];
		}
	}
}
