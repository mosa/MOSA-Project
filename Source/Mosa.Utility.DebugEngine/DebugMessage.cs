// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.DebugEngine;

public delegate void CallBack(DebugMessage response);

public class DebugMessage
{
	public int ID { get; internal set; }

	public byte Code { get; }

	public IList<byte> CommandData { get; }

	public List<byte> ResponseData { get; internal set; }

	public CallBack CallBack { get; set; }

	public object Other { get; set; }

	public DebugMessage(byte code, IList<byte> data)
	{
		Code = code;
		CommandData = data;
	}

	public DebugMessage(byte code, byte[] data)
	{
		Code = code;
		CommandData = new List<byte>(data.Length);

		foreach (var b in data)
		{
			CommandData.Add(b);
		}
	}

	public DebugMessage(byte code, byte[] data, int length)
	{
		Code = code;
		CommandData = new List<byte>(data.Length);

		for (var i = 0; i < length; i++)
		{
			CommandData.Add(data[i]);
		}
	}

	public DebugMessage(byte code, IList<int> data)
	{
		Code = code;
		CommandData = new List<byte>(data.Count * 4);

		foreach (var i in data)
		{
			CommandData.Add((byte)(i & 0xFF));
			CommandData.Add((byte)((i >> 8) & 0xFF));
			CommandData.Add((byte)((i >> 16) & 0xFF));
			CommandData.Add((byte)((i >> 24) & 0xFF));
		}
	}

	public DebugMessage(byte code, IList<int> data, object other)
		: this(code, data)
	{
		Other = other;
	}

	public DebugMessage(byte code, IList<int> data, CallBack callback)
		: this(code, data)
	{
		CallBack = callback;
	}

	public DebugMessage(byte code, IList<byte> data, CallBack callback)
		: this(code, data)
	{
		CallBack = callback;
	}

	public DebugMessage(byte code, int[] data, CallBack callback)
		: this(code, data)
	{
		CallBack = callback;
	}

	public int GetInt32(int index)
	{
		return (ResponseData[index + 3] << 24) | (ResponseData[index + 2] << 16) | (ResponseData[index + 1] << 8) | ResponseData[index];
	}

	public uint GetUInt32(int index)
	{
		return (uint)GetInt32(index);
	}

	public override string ToString()
	{
		return Code switch
		{
			DebugCode.Connected => "Connected",
			DebugCode.Connecting => "Connecting",
			DebugCode.Disconnected => "Disconnected",
			DebugCode.UnknownData => "Unknown Data: " +
			                         System.Text.Encoding.UTF8.GetString(CreateByteArray(ResponseData)),
			DebugCode.InformationalMessage => "Informational Message: " +
			                                  System.Text.Encoding.UTF8.GetString(CreateByteArray(ResponseData)),
			DebugCode.ErrorMessage => "Error Message: " +
			                          System.Text.Encoding.UTF8.GetString(CreateByteArray(ResponseData)),
			DebugCode.WarningMessage => "Warning Message: " +
			                            System.Text.Encoding.UTF8.GetString(CreateByteArray(ResponseData)),
			DebugCode.Ping => "Ping ACK",
			DebugCode.Alive => "Alive",
			DebugCode.ReadCR3 => "ReadCR3",
			DebugCode.ReadMemory => "ReadMemory",
			DebugCode.Scattered32BitReadMemory => "Scattered32BitReadMemory",
			DebugCode.WriteMemory => "WriteMemory",
			DebugCode.CompressedWriteMemory => "CompressedWriteMemory",
			DebugCode.SendNumber => "#: " +
			                        ((ResponseData[0] << 24) | (ResponseData[1] << 16) | (ResponseData[2] << 8) |
			                         ResponseData[3]),
			_ => "Code: " + Code
		};
	}

	private static byte[] CreateByteArray(List<byte> data)
	{
		var array = new byte[data.Count];
		data.CopyTo(array);
		return array;
	}
}
