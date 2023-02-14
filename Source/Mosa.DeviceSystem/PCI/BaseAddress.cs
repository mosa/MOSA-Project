﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceSystem.PCI;

/// <summary>
///
/// </summary>
public class BaseAddress
{
	/// <summary>
	///
	/// </summary>
	protected Pointer address;

	/// <summary>
	///
	/// </summary>
	protected uint size;

	/// <summary>
	///
	/// </summary>
	protected AddressType region;

	/// <summary>
	///
	/// </summary>
	protected bool prefetchable;

	/// <summary>
	/// Gets the address.
	/// </summary>
	/// <value>The address.</value>
	public Pointer Address => address;

	/// <summary>
	/// Gets the size.
	/// </summary>
	/// <value>The size.</value>
	public uint Size => size;

	/// <summary>
	/// Gets the region.
	/// </summary>
	/// <value>The region.</value>
	public AddressType Region => region;

	/// <summary>
	/// Gets a value indicating whether this <see cref="BaseAddress"/> is prefetchable.
	/// </summary>
	/// <value><c>true</c> if prefetchable; otherwise, <c>false</c>.</value>
	public bool Prefetchable => prefetchable;

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseAddress"/> class.
	/// </summary>
	/// <param name="region">The region.</param>
	/// <param name="address">The address.</param>
	/// <param name="size">The size.</param>
	/// <param name="prefetchable">if set to <c>true</c> [prefetchable].</param>
	public BaseAddress(AddressType region, Pointer address, uint size, bool prefetchable)
	{
		this.region = region;
		this.address = address;
		this.size = size;
		this.prefetchable = prefetchable;
	}

	/// <summary>
	/// Returns a <see cref="System.String"/> that represents this instance.
	/// </summary>
	/// <returns>
	/// A <see cref="System.String"/> that represents this instance.
	/// </returns>
	public override string ToString()
	{
		if (region == AddressType.Undefined)
			return string.Empty;

		if (region == AddressType.IO)
			return "I/O Port at 0x" + address.ToInt32().ToString("X") + " [size=" + size + "]";

		if (prefetchable)
			return "Memory at 0x" + address.ToInt32().ToString("X") + " [size=" + size + "] (prefetchable)";

		return "Memory at 0x" + address.ToInt32().ToString("X") + " [size=" + size + "] (non-prefetchable)";
	}
}
