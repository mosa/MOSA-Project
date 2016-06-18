// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.UnitTest.Engine
{
	internal class UnitTestRequest
	{
		public string ID { get; set; }
		public string MethodNamespaceName { get; set; }
		public string MethodTypeName { get; set; }
		public string MethodName { get; set; }
		public object[] Parameters { get; set; }

		public ulong Address { get; set; }
		public MosaMethod RuntimeMethod { get; private set; }
		public LinkerSymbol LinkerSymbol { get; private set; }

		public object Result { get; private set; }

		public bool HasResult { get { return Result != null; } }

		public UnitTestRequest(string ns, string type, string method, object[] parameters)
		{
			MethodNamespaceName = ns;
			MethodTypeName = type;
			MethodName = method;
			Parameters = parameters;
		}

		public void Resolve(TypeSystem typeSystem, BaseLinker linker)
		{
			// Find the test method to execute
			RuntimeMethod = FindMethod(
				typeSystem,
				MethodNamespaceName,
				MethodTypeName,
				MethodName,
				Parameters
			);

			Debug.Assert(RuntimeMethod != null, MethodNamespaceName + "." + MethodTypeName + "." + MethodName);

			var symbol = linker.GetSymbol(RuntimeMethod.FullName, SectionKind.Text);

			Address = symbol.VirtualAddress;
		}

		public object ParseResultData(List<byte> data)
		{
			Result = GetResult(RuntimeMethod.Signature.ReturnType, data);

			return Result;
		}

		private MosaMethod FindMethod(TypeSystem typeSystem, string ns, string type, string method, params object[] parameters)
		{
			foreach (var t in typeSystem.AllTypes)
			{
				if (t.Name != type)
					continue;

				if (!string.IsNullOrEmpty(ns))
					if (t.Namespace != ns)
						continue;

				foreach (var m in t.Methods)
				{
					if (m.Name == method)
					{
						if (m.Signature.Parameters.Count == parameters.Length)
						{
							return m;
						}
					}
				}

				break;
			}

			return null;
		}

		public List<int> CreateRequestMessage()
		{
			var cmd = new List<int>(4 + 4 + 4 + RuntimeMethod.Signature.Parameters.Count);

			cmd.Add((int)Address);
			cmd.Add(GetReturnResultType(RuntimeMethod.Signature.ReturnType));
			cmd.Add(0);

			foreach (var parm in Parameters)
			{
				AddParameters(cmd, parm);
			}

			cmd[2] = cmd.Count - 3;

			return cmd;
		}

		private static int GetReturnResultType(MosaType type)
		{
			if (type.IsI1)
				return 1;
			else if (type.IsI2)
				return 1;
			else if (type.IsI4)
				return 1;
			else if (type.IsI8)
				return 2;
			else if (type.IsU1)
				return 1;
			else if (type.IsU2)
				return 1;
			else if (type.IsU4)
				return 1;
			else if (type.IsU8)
				return 2;
			else if (type.IsChar)
				return 1;
			else if (type.IsBoolean)
				return 1;
			else if (type.IsR4)
				return 3;
			else if (type.IsR8)
				return 3;
			else if (type.IsVoid)
				return 0;

			return 0;
		}

		private static void AddParameters(List<int> cmd, object parameter)
		{
			if ((parameter == null) || !(parameter is ValueType))
			{
				throw new InvalidProgramException();
			}

			if (parameter is Boolean)
			{
				cmd.Add((bool)parameter ? 1 : 0);
			}
			else if (parameter is Char)
			{
				cmd.Add((char)parameter);
			}
			else if (parameter is SByte)
			{
				cmd.Add((int)(sbyte)parameter);
			}
			else if (parameter is Int16)
			{
				cmd.Add((int)(short)parameter);
			}
			else if (parameter is int)
			{
				cmd.Add((int)(int)parameter);
			}
			else if (parameter is Byte)
			{
				cmd.Add((byte)parameter);
			}
			else if (parameter is UInt16)
			{
				cmd.Add((ushort)parameter);
			}
			else if (parameter is UInt32)
			{
				cmd.Add((int)((uint)parameter));
			}
			else if (parameter is UInt64)
			{
				cmd.Add((int)(ulong)parameter);
				cmd.Add((int)((ulong)parameter >> 32));
			}
			else if (parameter is Int64)
			{
				cmd.Add((int)(long)parameter);
				cmd.Add((int)((long)parameter >> 32));
			}
			else if (parameter is Single)
			{
				var b = BitConverter.GetBytes((float)parameter);
				var u = BitConverter.ToUInt32(b, 0);
				cmd.Add((int)u);
			}
			else if (parameter is Double)
			{
				var b = BitConverter.GetBytes((double)parameter);
				var u = BitConverter.ToUInt64(b, 0);
				cmd.Add((int)((long)u));
				cmd.Add((int)((long)u >> 32));
			}
			else
			{
				throw new InvalidProgramException();
			}
		}

		public static object GetResult(MosaType type, List<byte> data)
		{
			if (type.IsI1)
				return (sbyte)data[0];
			else if (type.IsI2)
				return (short)(data[0] | (data[1] << 8));
			else if (type.IsI4)
				return (int)(data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24));
			else if (type.IsI8)
			{
				ulong low = (uint)(data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24));
				ulong high = (uint)(data[4] | (data[5] << 8) | (data[6] << 16) | (data[7] << 24));

				return (long)(low | (high << 32));
			}
			else if (type.IsU1)
				return (byte)data[0];
			else if (type.IsU2)
				return (ushort)(data[0] | (data[1] << 8));
			else if (type.IsU4)
				return (uint)(data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24));
			else if (type.IsU8)
			{
				ulong low = (uint)(data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24));
				ulong high = (uint)(data[4] | (data[5] << 8) | (data[6] << 16) | (data[7] << 24));

				return (ulong)(low | (high << 32));
			}
			else if (type.IsChar)
				return (char)(data[0] | (data[1] << 8));
			else if (type.IsBoolean)
				return (bool)(data[0] != 0);
			else if (type.IsR4)
			{
				var value = new byte[8];

				for (int i = 0; i < 8; i++)
					value[i] = data[i];

				var d = BitConverter.ToSingle(value, 0);

				return d;
			}
			else if (type.IsR8)
			{
				var value = new byte[8];

				for (int i = 0; i < 8; i++)
					value[i] = data[i];

				var d = BitConverter.ToDouble(value, 0);

				return d;
			}
			else if (type.IsVoid)
				return null;

			return null;
		}
	}
}
