/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;

using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// 
	/// </summary>
	public static class Intrinsics
	{
		#region Static Data

		private static Dictionary<Type, IInstruction> _map = null; 

		/// <summary>
		/// 
		/// </summary>
		public static readonly BochsDebug BochsDebug = new BochsDebug();
		
		#endregion // Static Data 
		
		/// <summary>
		/// Gets the instruction.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static IInstruction Get(Type type)
		{
			if (_map == null)
				_map = Initialize();

			return _map[type];
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		/// <returns></returns>
		public static Dictionary<Type, IInstruction> Initialize()
		{
			Dictionary<Type, IInstruction> map = new Dictionary<Type, IInstruction>();

			map.Add(typeof(BochsDebug), BochsDebug);
			map.Add(typeof(OutInstruction), CPUx86.Instruction.OutInstruction);
			map.Add(typeof(InInstruction), CPUx86.Instruction.InInstruction);
			map.Add(typeof(CpuIdEaxInstruction), CPUx86.Instruction.CpuIdEaxInstruction);
			map.Add(typeof(CpuIdEbxInstruction), CPUx86.Instruction.CpuIdEbxInstruction);
			map.Add(typeof(CpuIdEcxInstruction), CPUx86.Instruction.CpuIdEcxInstruction);
			map.Add(typeof(CpuIdEdxInstruction), CPUx86.Instruction.CpuIdEdxInstruction);
			map.Add(typeof(CpuIdInstruction), CPUx86.Instruction.CpuIdInstruction);
			map.Add(typeof(InvlpgInstruction), CPUx86.Instruction.InvlpgInstruction);
			map.Add(typeof(NopInstruction), CPUx86.Instruction.NopInstruction);
            map.Add(typeof(IR.MoveInstruction), IR.Instruction.MoveInstruction);

			// TODO - finsh up the list

			return map;
		}

	}
}

