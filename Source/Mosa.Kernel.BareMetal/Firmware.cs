// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal;

public struct Firmware
{
	public string Vendor { get; }

	public string Version { get; }

	public string Date { get; }

	public Firmware(string vendor, string version, string date)
	{
		Vendor = vendor;
		Version = version;
		Date = date;
	}
}
