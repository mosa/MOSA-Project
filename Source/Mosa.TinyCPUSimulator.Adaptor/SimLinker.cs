/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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

		private List<Tuple<LinkerSymbol, int, string>> targetSymbols = new List<Tuple<LinkerSymbol, int, string>>();
		private List<Tuple<LinkerSymbol, long, SimInstruction>> instructions = new List<Tuple<LinkerSymbol, long, SimInstruction>>();
		private List<Tuple<LinkerSymbol, long, string>> source = new List<Tuple<LinkerSymbol, long, string>>();

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

		public void AddTargetSymbol(LinkerSymbol baseSymbol, int offset, string name)
		{
			targetSymbols.Add(new Tuple<LinkerSymbol, int, string>(baseSymbol, offset, name));
		}

		public void AddInstruction(LinkerSymbol baseSymbol, long offset, SimInstruction instruction)
		{
			instructions.Add(new Tuple<LinkerSymbol, long, SimInstruction>(baseSymbol, offset, instruction));
		}

		public void AddSourceInformation(LinkerSymbol baseSymbol, long offset, string information)
		{
			source.Add(new Tuple<LinkerSymbol, long, string>(baseSymbol, offset, information));
		}

		protected override void EmitImplementation(Stream stream)
		{
			base.EmitImplementation(stream);

			foreach (var symbol in Symbols)
			{
				simAdapter.SimCPU.SetSymbol(symbol.Name, symbol.VirtualAddress, (ulong)symbol.Size);
				//Debug.WriteLine("0x" + symbol.VirtualAddress.ToString("X") + " " + symbol.Name);
			}

			foreach (var target in targetSymbols)
			{
				simAdapter.SimCPU.SetSymbol(target.Item3, target.Item1.VirtualAddress + (ulong)target.Item2, 0);
			}

			foreach (var target in instructions)
			{
				simAdapter.SimCPU.AddInstruction(target.Item1.VirtualAddress + (ulong)target.Item2, target.Item3);
			}

			foreach (var target in source)
			{
				simAdapter.SimCPU.AddSourceInformation(target.Item1.VirtualAddress + (ulong)target.Item2, target.Item3);
			}

			targetSymbols = null;
			instructions = null;
			source = null;
		}
	}
}