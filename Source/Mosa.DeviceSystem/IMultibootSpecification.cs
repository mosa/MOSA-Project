// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceSystem;

public interface IMultibootSpecification
{
	string CommandLinePointer { get; }

	string BootloaderNamePointer { get; }

	Pointer ACPI_RSDP { get; }
}
