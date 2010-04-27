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

namespace Mosa.Platforms.x86.Intrinsic
{
	/// <summary>
	/// 
	/// </summary>
	public static class Method
	{

		#region Static Data

		private static Dictionary<Type, IIntrinsicMethod> _map = null;

		#endregion // Static Data

		/// <summary>
		/// Gets the new 
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static IIntrinsicMethod Get(Type type)
		{
			if (_map == null)
				_map = Initialize();

			return _map[type];
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		/// <returns></returns>
		public static Dictionary<Type, IIntrinsicMethod> Initialize()
		{
			Dictionary<Type, IIntrinsicMethod> map = new Dictionary<Type, IIntrinsicMethod>();

			map.Add(typeof(BochsDebug), new BochsDebug());
			map.Add(typeof(Out), new Out());
			map.Add(typeof(In), new In());
			map.Add(typeof(Invlpg), new Invlpg());
			map.Add(typeof(Lgdt), new Lgdt());
			map.Add(typeof(Lidt), new Lidt());
			map.Add(typeof(Nop), new Nop());
			map.Add(typeof(Cli), new Cli());
			map.Add(typeof(Sti), new Sti());
			map.Add(typeof(SetCR0), new SetCR0());
			map.Add(typeof(SetCR2), new SetCR2());
			map.Add(typeof(SetCR3), new SetCR3());
			map.Add(typeof(SetCR4), new SetCR4());
			map.Add(typeof(GetCR0), new GetCR0());
			map.Add(typeof(GetCR2), new GetCR2());
			map.Add(typeof(GetCR3), new GetCR3());
			map.Add(typeof(GetCR4), new GetCR4());
			map.Add(typeof(GetIDTJumpLocation), new GetIDTJumpLocation());
			map.Add(typeof(CpuIdEax), new CpuIdEax());
			map.Add(typeof(CpuIdEbx), new CpuIdEbx());
			map.Add(typeof(CpuIdEcx), new CpuIdEcx());
			map.Add(typeof(CpuIdEdx), new CpuIdEdx());
			map.Add(typeof(CpuId), new CpuId());
			map.Add(typeof(SpinLock), new SpinLock());
			map.Add(typeof(SpinUnlock), new SpinUnlock());
			map.Add(typeof(Hlt), new Hlt());
			map.Add(typeof(Get), new Get());
			map.Add(typeof(Set), new Set());
			map.Add(typeof(SwitchTask), new SwitchTask());

			map.Add(typeof(Test), new Test());

			return map;
		}

	}
}

