// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Platform.x86;
using Mosa.TinyCPUSimulator.Adaptor;
using Mosa.TinyCPUSimulator.TestSystem;
using System;

namespace Mosa.TinyCPUSimulator.x86.Adaptor
{
	public class TestPlatform : BaseTestPlatform
	{
		private const uint StopEIP = 0x01000;

		public TestPlatform()
			: base("x86")
		{
		}

		public override BaseArchitecture CreateArchitecture()
		{
			return Architecture.CreateArchitecture(ArchitectureFeatureFlags.AutoDetect);
		}

		public override ISimAdapter CreateSimAdaptor()
		{
			return new SimTestSystemAdapter();
		}

		public override void InitializeSimulation(ISimAdapter simAdapter)
		{
			//simAdapter.SimCPU.AddInstruction(StopEIP, new SimInstruction(Opcode.Nop, 1));
		}

		public override void ResetSimulation(ISimAdapter simAdapter)
		{
			simAdapter.SimCPU.Monitor.ClearBreakPoints();
			simAdapter.SimCPU.Monitor.AddBreakPoint(StopEIP);
			simAdapter.SimCPU.Tick = 0;
			simAdapter.SimCPU.LastException = null;
			simAdapter.SimCPU.Monitor.Stop = false;

			var x86 = simAdapter.SimCPU as CPUx86;

			x86.Reset();

			// Set start of stack
			x86.ESP.Value = 0x00080000;
			x86.EBP.Value = x86.ESP.Value;

			simAdapter.SimCPU.Write8(StopEIP, 0x90);
		}

		public override void PrepareToExecuteMethod(ISimAdapter simAdapter, ulong address, params object[] parameters)
		{
			var x86 = simAdapter.SimCPU as CPUx86;

			// Clear last exception
			x86.LastException = null;

			// Clear registers
			x86.EFLAGS.Value = 0;
			x86.EAX.Value = 0;
			x86.EBX.Value = 0;
			x86.ECX.Value = 0;
			x86.EDX.Value = 0;
			x86.ESI.Value = 0;

			// Set start of stack
			x86.ESP.Value = 0x00080000;
			x86.EBP.Value = x86.ESP.Value;

			// Write NOP instruction to 0x01000
			simAdapter.SimCPU.Write8(StopEIP, 0x90);

			// Set starting address
			x86.EIP.Value = (uint)address;

			for (int i = parameters.Length - 1; i >= 0; i--)
			{
				PopulateStack(simAdapter, parameters[i]);
			}

			// Put return address on stack
			WriteStackValue(simAdapter, StopEIP);

			// Clear stop
			simAdapter.SimCPU.Monitor.Stop = false;
		}

		private void WriteStackValue(ISimAdapter simAdapter, uint value)
		{
			var x86 = simAdapter.SimCPU as CPUx86;

			x86.ESP.Value = x86.ESP.Value - 4;
			x86.Write32(x86.ESP.Value, value);
		}

		private void WriteStackValue(ISimAdapter simAdapter, ulong value)
		{
			var x86 = simAdapter.SimCPU as CPUx86;

			x86.ESP.Value = x86.ESP.Value - 8;
			x86.Write64(x86.ESP.Value, value);
		}

		public void PopulateStack(ISimAdapter simAdapter, object parameter)
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
				WriteStackValue(simAdapter, (uint)((ulong)parameter >> 32));
				WriteStackValue(simAdapter, (uint)(ulong)parameter);
			}
			else if (parameter is Int64)
			{
				WriteStackValue(simAdapter, (uint)((long)parameter >> 32));
				WriteStackValue(simAdapter, (uint)(long)parameter);
			}
			else if (parameter is Single)
			{
				var b = BitConverter.GetBytes((float)parameter);
				var u = BitConverter.ToUInt32(b, 0);
				WriteStackValue(simAdapter, u);
			}
			else if (parameter is Double)
			{
				var b = BitConverter.GetBytes((double)parameter);
				var u = BitConverter.ToUInt64(b, 0);
				WriteStackValue(simAdapter, u);
			}

			//else  if (parameter is UIntPtr) { WriteStackValue(simAdapter, (uint)parameter);  }
			//else  if (parameter is IntPtr) { WriteStackValue(simAdapter, (uint)parameter); }
			else
			{
				throw new InvalidProgramException();
			}
		}

		public override object GetResult(ISimAdapter simAdapter, MosaType type)
		{
			var x86 = simAdapter.SimCPU as CPUx86;

			if (type.IsI1)
				return (object)(sbyte)x86.EAX.Value;
			else if (type.IsI2)
				return (object)(short)x86.EAX.Value;
			else if (type.IsI4)
				return (object)(int)x86.EAX.Value;
			else if (type.IsI8)
				return (object)(long)(((ulong)x86.EAX.Value) | ((ulong)x86.EDX.Value << 32));
			else if (type.IsU1)
				return (object)(byte)x86.EAX.Value;
			else if (type.IsU2)
				return (object)(ushort)x86.EAX.Value;
			else if (type.IsU4)
				return (object)(uint)x86.EAX.Value;
			else if (type.IsU8)
				return (object)(ulong)(((ulong)x86.EAX.Value) | ((ulong)x86.EDX.Value << 32));
			else if (type.IsChar)
				return (object)(char)x86.EAX.Value;
			else if (type.IsBoolean)
				return (object)(bool)(x86.EAX.Value != 0);
			else if (type.IsR4)
				return (object)(float)x86.XMM0.Value.LowF;
			else if (type.IsR8)
				return (object)(double)x86.XMM0.Value.Low;
			else if (type.IsVoid)
				return null;

			return null;
		}
	}
}
