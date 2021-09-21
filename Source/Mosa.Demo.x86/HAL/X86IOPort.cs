// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;

namespace Mosa.Demo.x86.HAL
{
	/// <summary>
	/// X86IOPortReadWrite
	/// </summary>
	/// <seealso cref="DeviceSystem.BaseIOPortReadWrite" />
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
		public override byte Read8(int offset = 0)
		{
			return IOPort.In8((ushort)(Address + offset));
		}

		/// <summary>
		/// Reads a short from the IO Port
		/// </summary>
		/// <returns></returns>
		public override ushort Read16(int offset = 0)
		{
			return IOPort.In16((ushort)(Address + offset));
		}

		/// <summary>
		/// Reads an integer from the IO Port
		/// </summary>
		/// <returns></returns>
		public override uint Read32(int offset = 0)
		{
			return IOPort.In32((ushort)(Address + offset));
		}

		/// <summary>
		///  Writes a byte to the IO Port
		/// </summary>
		/// <param name="data">The data.</param>
		public override void Write8(byte data, int offset = 0)
		{
			IOPort.Out8((ushort)(Address + offset), data);
		}

		/// <summary>
		///  Writes a short to the IO Port
		/// </summary>
		/// <param name="data">The data.</param>
		public override void Write16(ushort data, int offset = 0)
		{
			IOPort.Out16((ushort)(Address + offset), data);
		}

		/// <summary>
		///  Writes an integer to the IO Port
		/// </summary>
		/// <param name="data">The data.</param>
		public override void Write32(uint data, int offset = 0)
		{
			IOPort.Out32((ushort)(Address + offset), data);
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
		public override byte Read8(int offset = 0)
		{
			return IOPort.In8((ushort)(Address + offset));
		}

		/// <summary>
		/// Reads a short from the IO Port
		/// </summary>
		/// <returns></returns>
		public override ushort Read16(int offset = 0)
		{
			return IOPort.In16((ushort)(Address + offset));
		}

		/// <summary>
		/// Reads an integer from the IO Port
		/// </summary>
		/// <returns></returns>
		public override uint Read32(int offset = 0)
		{
			return IOPort.In32((ushort)(Address + offset));
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
		public override void Write8(byte data, int offset = 0)
		{
			IOPort.Out8((ushort)(Address + offset), data);
		}

		/// <summary>
		/// Writes a short to the IO Port
		/// </summary>
		/// <param name="data">The data.</param>
		public override void Write16(ushort data, int offset = 0)
		{
			IOPort.Out16((ushort)(Address + offset), data);
		}

		/// <summary>
		/// Writes an integer to the IO Port
		/// </summary>
		/// <param name="data">The data.</param>
		public override void Write32(uint data, int offset = 0)
		{
			IOPort.Out32((ushort)(Address + offset), data);
		}
	}
}
