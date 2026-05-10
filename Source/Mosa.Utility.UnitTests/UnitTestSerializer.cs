// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Utility.UnitTests;

public static class UnitTestSerializer
{
	public static List<int> SerializeUnitTestMessage(UnitTest unitTest)
	{
		var address = unitTest.MosaMethodAddress.ToInt64();

		var cmd = new List<int>(4 + 4 + 4 + 4 + unitTest.MosaMethod.Signature.Parameters.Count)
		{
			(int)address,
			GetReturnResultType(unitTest.MosaMethod.Signature.ReturnType),
			0
		};

		foreach (var param in unitTest.Values)
		{
			AddParameters(cmd, param);
		}

		cmd[2] = cmd.Count - 3;

		return cmd;
	}

	public static void ParseResultData(UnitTest unitTest, ulong data)
	{
		var arr = BitConverter.GetBytes(data);

		unitTest.Result = GetResult(unitTest.MosaMethod.Signature.ReturnType, arr);
	}

	public static object GetResult(MosaType type, byte[] data)
	{
		if (type.IsI1)
		{
			return (sbyte)data[0];
		}
		else if (type.IsI2)
		{
			return (short)(data[0] | (data[1] << 8));
		}
		else if (type.IsI4)
		{
			return data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24);
		}
		else if (type.IsI8)
		{
			ulong low = (uint)(data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24));
			ulong high = (uint)(data[4] | (data[5] << 8) | (data[6] << 16) | (data[7] << 24));

			return (long)(low | (high << 32));
		}
		else if (type.IsU1)
		{
			return data[0];
		}
		else if (type.IsU2)
		{
			return (ushort)(data[0] | (data[1] << 8));
		}
		else if (type.IsU4)
		{
			return (uint)(data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24));
		}
		else if (type.IsU8)
		{
			ulong low = (uint)(data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24));
			ulong high = (uint)(data[4] | (data[5] << 8) | (data[6] << 16) | (data[7] << 24));

			return low | (high << 32);
		}
		else if (type.IsChar)
		{
			return (char)(data[0] | (data[1] << 8));
		}
		else if (type.IsBoolean)
		{
			return data[0] != 0;
		}
		else if (type.IsR4)
		{
			var value = new byte[8];

			for (var i = 0; i < 8; i++)
				value[i] = data[i];

			return BitConverter.ToSingle(value, 0);
		}
		else if (type.IsR8)
		{
			var value = new byte[8];

			for (var i = 0; i < 8; i++)
				value[i] = data[i];

			return BitConverter.ToDouble(value, 0);
		}
		else if (type.IsVoid)
		{
			return null;
		}

		return null;
	}

	public static string FormatUnitTestResult(UnitTest unitTest)
	{
		var sb = new StringBuilder();

		switch (unitTest.Status)
		{
			case UnitTestStatus.Failed: sb.Append("FAILED"); break;
			case UnitTestStatus.FailedByCrash: sb.Append("CRASHED"); break;
			case UnitTestStatus.Skipped: sb.Append("SKIPPED"); break;
			case UnitTestStatus.Passed: sb.Append("OK"); break;
			case UnitTestStatus.Pending: sb.Append("PENDING"); break;
		}

		sb.Append($": {unitTest.MethodTypeName}::{unitTest.MethodName}(");

		foreach (var param in unitTest.Values)
		{
			sb.Append(param);
			sb.Append(',');
		}

		if (unitTest.Values.Length > 0)
		{
			sb.Length--;
		}

		sb.Append(')');

		sb.Append($" Expected: {ObjectToString(unitTest.Expected)}");
		sb.Append($" Result: {ObjectToString(unitTest.Result)}");

		return sb.ToString();
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
		if (parameter == null || !(parameter is ValueType))
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
			cmd.Add((sbyte)parameter);
		}
		else if (parameter is Int16)
		{
			cmd.Add((short)parameter);
		}
		else if (parameter is int)
		{
			cmd.Add((int)parameter);
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
			cmd.Add((int)(uint)parameter);
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
			cmd.Add((int)(long)u);
			cmd.Add((int)((long)u >> 32));
		}
		else
		{
			throw new InvalidProgramException();
		}
	}

	private static string ObjectToString(object o)
	{
		if (o is null)
			return "NULL";
		else
			return o.ToString();
	}
}
