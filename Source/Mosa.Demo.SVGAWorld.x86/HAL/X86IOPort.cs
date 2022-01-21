// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;

namespace Mosa.Demo.SVGAWorld.x86.HAL
{
	/// <summary>
	/// X86IOPortReadWrite
	/// </summary>
	/// <seealso cref="Mosa.DeviceSystem.BaseIOPortReadWrite" />
	public sealed class X86IOPortReadWrite : DeviceSystem.BaseIOPortReadWrite
	{
		public X86IOPortReadWrite(ushort address)
		{
			Address = address;
		}

		/// <summary>
		/// Reads a byte from the IO Port
		/// </summary>
		/// <returns></returns>
		public override byte Read8()
		{
			return IOPort.In8(Address);
		}

		/// <summary>
		/// Reads a short from the IO Port
		/// </summary>
		/// <returns></returns>
		public override ushort Read16()
		{
			return IOPort.In16(Address);
		}

		/// <summary>
		/// Reads an integer from the IO Port
		/// </summary>
		/// <returns></returns>
		public override uint Read32()
		{
			return IOPort.In32(Address);
		}

		/// <summary>
		///  Writes a byte to the IO Port
		/// </summary>
		/// <param name="data">The data.</param>
		public override void Write8(byte data)
		{
			IOPort.Out8(Address, data);
		}

		/// <summary>
		///  Writes a short to the IO Port
		/// </summary>
		/// <param name="data">The data.</param>
		public override void Write16(ushort data)
		{
			IOPort.Out16(Address, data);
		}

		/// <summary>
		///  Writes an integer to the IO Port
		/// </summary>
		/// <param name="data">The data.</param>
		public override void Write32(uint data)
		{
			IOPort.Out32(Address, data);
		}
	}

	/// <summary>
	/// X86IOPortRead
	/// </summary>
	/// <seealso cref="Mosa.DeviceSystem.BaseIOPortRead" />
	public sealed class X86IOPortRead : DeviceSystem.BaseIOPortRead
	{
		public X86IOPortRead(ushort address)
		{
			Address = address;
		}

		/// <summary>
		/// Reads a byte from the IO Port
		/// </summary>
		/// <returns></returns>
		public override byte Read8()
		{
			return IOPort.In8(Address);
		}

		/// <summary>
		/// Reads a short from the IO Port
		/// </summary>
		/// <returns></returns>
		public override ushort Read16()
		{
			return IOPort.In16(Address);
		}

		/// <summary>
		/// Reads an integer from the IO Port
		/// </summary>
		/// <returns></returns>
		public override uint Read32()
		{
			return IOPort.In32(Address);
		}
	}

	/// <summary>
	/// X86IOPortWrite
	/// </summary>
	/// <seealso cref="Mosa.DeviceSystem.BaseIOPortWrite" />
	public sealed class X86IOPortWrite : DeviceSystem.BaseIOPortWrite
	{
		public X86IOPortWrite(ushort address)
		{
			Address = address;
		}

		/// <summary>
		///  Writes a byte to the IO Port
		/// </summary>
		/// <param name="data">The data.</param>
		public override void Write8(byte data)
		{
			IOPort.Out8(Address, data);
		}

		/// <summary>
		/// Writes a short to the IO Port
		/// </summary>
		/// <param name="data">The data.</param>
		public override void Write16(ushort data)
		{
			IOPort.Out16(Address, data);
		}

		/// <summary>
		/// Writes an integer to the IO Port
		/// </summary>
		/// <param name="data">The data.</param>
		public override void Write32(uint data)
		{
			IOPort.Out32(Address, data);
		}
	}
}
