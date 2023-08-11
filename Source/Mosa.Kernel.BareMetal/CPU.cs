// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal;

public struct CPU
{
	public uint Cores { get; }

	public string Vendor { get; }

	public string Model { get; }

	public CPU(uint cores, string vendor, string model)
	{
		Cores = cores;
		Vendor = vendor;
		Model = model;
	}
}
