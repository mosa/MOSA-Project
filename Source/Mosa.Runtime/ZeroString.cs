// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceSystem;

public class ZeroString
{
	public Pointer Address;

	public ZeroString(Pointer address)
	{
		Address = address;
	}

	public byte this[int i] => Address.Load8(i);

	public int Length
	{
		get
		{
			for (int i = 0; ; i++)
			{
				if (Address.Load8(i) == 0)
					return i + 1;
			}
		}
	}

	public string Substring(int startIndex, int length)
	{
		unsafe
		{
			return new string((sbyte*)Address.ToUInt64(), startIndex, length);
		}
	}

	public override string ToString()
	{
		unsafe
		{
			return new string((sbyte*)Address.ToUInt64());
		}
	}
}
