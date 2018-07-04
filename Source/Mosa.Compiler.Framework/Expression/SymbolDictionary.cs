// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Expression
{
	public class SymbolDictionary
	{
		protected Dictionary<string, BaseInstruction> instructionMap = new Dictionary<string, BaseInstruction>();
		protected Dictionary<string, PhysicalRegister> physicalRegisterMap = new Dictionary<string, PhysicalRegister>();

		public void AddIRInstructions()
		{
			Add(IRInstructions.List);
		}

		public void Add(BaseArchitecture architecture)
		{
			Add(architecture.RegisterSet);
			Add(architecture.Instructions);
		}

		public void Add(List<BaseInstruction> instructions)
		{
			foreach (var instruction in instructions)
			{
				instructionMap.Add(instruction.FullName, instruction);
			}
		}

		public void Add(Dictionary<string, PhysicalRegister> instructions)
		{
			foreach (var entry in instructions)
			{
				physicalRegisterMap.Add(entry.Key, entry.Value);
			}
		}

		public void Add(PhysicalRegister[] registers)
		{
			foreach (var entry in registers)
			{
				physicalRegisterMap.Add(entry.ToString(), entry);
			}
		}

		public BaseInstruction GetInstruction(string name, string family)
		{
			return GetInstruction(family + "." + name);
		}

		public BaseInstruction GetInstruction(string fullname)
		{
			instructionMap.TryGetValue(fullname, out BaseInstruction result);

			return result;
		}

		public PhysicalRegister GetPhysicalRegister(string name, string family)
		{
			return GetPhysicalRegister(family + "." + name);
		}

		public PhysicalRegister GetPhysicalRegister(string fullname)
		{
			physicalRegisterMap.TryGetValue(fullname, out PhysicalRegister physicalRegister);

			return physicalRegister;
		}
	}
}
