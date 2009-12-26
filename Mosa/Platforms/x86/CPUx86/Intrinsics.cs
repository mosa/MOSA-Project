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

		private static Dictionary<Type, IIntrinsicInstruction> _map = null; 

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
		public static IIntrinsicInstruction Get(Type type)
		{
			if (_map == null)
				_map = Initialize();

			return _map[type];
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		/// <returns></returns>
		public static Dictionary<Type, IIntrinsicInstruction> Initialize()
		{
			Dictionary<Type, IIntrinsicInstruction> map = new Dictionary<Type, IIntrinsicInstruction>();

			map.Add(typeof(BochsDebug), BochsDebug);
			map.Add(typeof(OutInstruction), Instruction.OutInstruction);
			map.Add(typeof(InInstruction), Instruction.InInstruction);
			map.Add(typeof(CpuIdEaxInstruction), Instruction.CpuIdEaxInstruction);
			map.Add(typeof(CpuIdEbxInstruction), Instruction.CpuIdEbxInstruction);
			map.Add(typeof(CpuIdEcxInstruction), Instruction.CpuIdEcxInstruction);
			map.Add(typeof(CpuIdEdxInstruction), Instruction.CpuIdEdxInstruction);
			map.Add(typeof(CpuIdInstruction), Instruction.CpuIdInstruction);
			map.Add(typeof(InvlpgInstruction), Instruction.InvlpgInstruction);
			map.Add(typeof(NopInstruction), Instruction.NopInstruction);
            map.Add(typeof(MovInstruction), Instruction.MovInstruction);
            map.Add(typeof(SetCRInstruction), Instruction.SetCRInstruction);
			map.Add(typeof(GetCRInstruction), Instruction.GetCRInstruction);

			// TODO - finish up the list

			return map;
		}

	}
}

