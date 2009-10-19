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

namespace Mosa.Platforms.x86.CPUx86.Intrinsics
{
	/// <summary>
	/// 
	/// </summary>
	public static class Instruction
	{
		#region Static Data

		private static Dictionary<Type, IInstruction> _map = null; 

		/// <summary>
		/// 
		/// </summary>
		public static readonly BochsDebug BochsDebug = new BochsDebug();
		/// <summary>
		/// 
		/// </summary>
		public static readonly CldInstruction CldInstruction = new CldInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly CliInstruction CliInstruction = new CliInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly CmpXchgInstruction CmpXchgInstruction = new CmpXchgInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly CpuIdEaxInstruction CpuIdEaxInstruction = new CpuIdEaxInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly CpuIdEbxInstruction CpuIdEbxInstruction = new CpuIdEbxInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly CpuIdEcxInstruction CpuIdEcxInstruction = new CpuIdEcxInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly CpuIdEdxInstruction CpuIdEdxInstruction = new CpuIdEdxInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly CpuIdInstruction CpuIdInstruction = new CpuIdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly HltInstruction HltInstruction = new HltInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly InInstruction InInstruction = new InInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly InvlpgInstruction InvlpgInstruction = new InvlpgInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly IretdInstruction IretdInstruction = new IretdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly LgdtInstruction LgdtInstruction = new LgdtInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly LidtInstruction LidtInstruction = new LidtInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly OutInstruction OutInstruction = new OutInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly PauseInstruction PauseInstruction = new PauseInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly PopadInstruction PopadInstruction = new PopadInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly PopfdInstruction PopfdInstruction = new PopfdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly PopInstruction PopInstruction = new PopInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly PushadInstruction PushadInstruction = new PushadInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly PushfdInstruction PushfdInstruction = new PushfdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly PushInstruction PushInstruction = new PushInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly RcrInstruction RcrInstruction = new RcrInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly RdmsrInstruction RdmsrInstruction = new RdmsrInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly RdpmcInstruction RdpmcInstruction = new RdpmcInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly RdtscInstruction RdtscInstruction = new RdtscInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly RepInstruction RepInstruction = new RepInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly StiInstruction StiInstruction = new StiInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly StosbInstruction StosbInstrucion = new StosbInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly StosdInstruction StosdInstruction = new StosdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly XchgInstruction XchgInstruction = new XchgInstruction();

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
			map.Add(typeof(OutInstruction),  OutInstruction);
			map.Add(typeof(InInstruction),  InInstruction);
			map.Add(typeof(CpuIdEaxInstruction), CpuIdEaxInstruction);
			map.Add(typeof(CpuIdEbxInstruction), CpuIdEbxInstruction);
			map.Add(typeof(CpuIdEcxInstruction), CpuIdEcxInstruction);
			map.Add(typeof(CpuIdEdxInstruction), CpuIdEdxInstruction);
			map.Add(typeof(CpuIdInstruction), CpuIdInstruction);
			map.Add(typeof(InvlpgInstruction), InvlpgInstruction);
			map.Add(typeof(NopInstruction), CPUx86.Instruction.NopInstruction);

			// TODO - finsh up the list

			return map;
		}

	}
}

