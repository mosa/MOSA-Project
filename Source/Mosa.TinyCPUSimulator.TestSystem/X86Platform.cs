/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata;
using Mosa.Platform.x86;
using Mosa.TinyCPUSimulator.Adaptor;
using Mosa.TinyCPUSimulator.x86;
using Mosa.TinyCPUSimulator.x86.Adaptor;
using System;

namespace Mosa.TinyCPUSimulator.TestSystem
{
	public class X86Platform : BasePlatform
	{
		private const uint StopEIP = 0x01000;

		public X86Platform()
			: base("x86")
		{
		}

		public override IArchitecture CreateArchitecture()
		{
			return Architecture.CreateArchitecture(ArchitectureFeatureFlags.AutoDetect);
		}

		public override ISimAdapter CreateSimAdaptor()
		{
			return new SimAdapter();
		}

		public override void InitializeSimulation(ISimAdapter simAdapter)
		{
			simAdapter.SimCPU.AddMemory(0x00000000, 0x000A0000, 1); // First 640kb

			simAdapter.Monitor.AddBreakPoint(StopEIP);
		}

		public override void ResetSimulation(ISimAdapter simAdapter)
		{
			simAdapter.Reset();

			var x86 = simAdapter.SimCPU as CPUx86;

			// Start of stack
			x86.ESP.Value = 0x00080000;
			x86.EBP.Value = x86.ESP.Value;
		}

		private void WriteStackValue(ISimAdapter simAdapter, uint value)
		{
			var x86 = simAdapter.SimCPU as CPUx86;

			x86.ESP.Value = x86.ESP.Value - 4;
			x86.Write32(x86.ESP.Value, value);
		}

		public override void PopulateStack(ISimAdapter simAdapter, object parameter)
		{
			if ((parameter == null) || !(parameter is ValueType))
			{
				throw new InvalidProgramException();
			}

			if (parameter is Boolean)
			{
				WriteStackValue(simAdapter, (bool)parameter ? (uint)1 : (uint)0);
			}
			else if (parameter is Char)
			{
				WriteStackValue(simAdapter, (uint)(char)parameter);
			}
			else if (parameter is SByte)
			{
				WriteStackValue(simAdapter, (uint)(sbyte)parameter);
			}
			else if (parameter is Int16)
			{
				WriteStackValue(simAdapter, (uint)(short)parameter);
			}
			else if (parameter is Int32)
			{
				WriteStackValue(simAdapter, (uint)(int)parameter);
			}
			else if (parameter is Byte)
			{
				WriteStackValue(simAdapter, (uint)(byte)parameter);
			}
			else if (parameter is UInt16)
			{
				WriteStackValue(simAdapter, (uint)(ushort)parameter);
			}
			else if (parameter is UInt32)
			{
				WriteStackValue(simAdapter, (uint)(uint)parameter);
			}
			else if (parameter is UInt64)
			{
				WriteStackValue(simAdapter, (uint)(ulong)parameter);
				WriteStackValue(simAdapter, (uint)((ulong)parameter >> 32));
			}
			else if (parameter is Int64)
			{
				WriteStackValue(simAdapter, (uint)(ulong)parameter);
				WriteStackValue(simAdapter, (uint)((ulong)parameter >> 32));
			}
			else if (parameter is Single)
			{
				// TODO
				WriteStackValue(simAdapter, 0);
				WriteStackValue(simAdapter, 0);
			}
			else if (parameter is Double)
			{
				// TODO
				WriteStackValue(simAdapter, 0);
				WriteStackValue(simAdapter, 0);
			}
			//else  if (parameter is UIntPtr) { WriteStackValue(simAdapter, (uint)parameter);  }
			//else  if (parameter is IntPtr) { WriteStackValue(simAdapter, (uint)parameter); }
			else
				throw new InvalidProgramException();
		}

		public override void PopulateStack(ISimAdapter simAdapter, params object[] parameters)
		{
			for (int i = parameters.Length - 1; i >= 0; i--)
			{
				PopulateStack(simAdapter, parameters[i]);
			}
		}

		public override void PrepareToExecuteMethod(ISimAdapter simAdapter, ulong address)
		{
			var x86 = simAdapter.SimCPU as CPUx86;

			x86.EIP.Value = (uint)address;

			// push the return address on stack
			WriteStackValue(simAdapter, StopEIP);
		}

		public override object GetResult(ISimAdapter simAdapter, CilElementType cilElementType)
		{
			var x86 = simAdapter.SimCPU as CPUx86;

			if (cilElementType == CilElementType.I4)
				return (object)(int)x86.EAX.Value;
			else if (cilElementType == CilElementType.U4)
				return (object)(uint)x86.EAX.Value;

			return null;
		}
	}
}