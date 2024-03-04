// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Kernel.BareMetal.IPC;

public readonly struct ServiceIdentification
{
	public readonly uint ID { get; }

	public uint ServiceID { get { return ID & 0xFFFF; } }

	public uint FunctionID { get { return ID >> 16 & 0xFFFF; } }

	public uint VersionID { get { return ID >> 24 & 0xFFFF; } }

	public ServiceIdentification(uint serviceID, uint functionID, uint versionID)
	{
		ID = (serviceID & 0xFFFF) | ((functionID & 0xFF) << 16) | ((versionID & 0xFF) << 24);
	}

	public static bool operator ==(ServiceIdentification a, ServiceIdentification b)
	{
		return a.ID == b.ID;
	}

	public static bool operator !=(ServiceIdentification a, ServiceIdentification b)
	{
		return a.ID != b.ID;
	}
}
