/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.IO;
using Mosa.ClassLib;
using Mosa.DeviceDrivers;
using Mosa.EmulatedDevices.Utils;

namespace Mosa.EmulatedDevices
{
	#region Definitions

	internal struct CRTCommands
	{
		internal const byte HorizontalTotal = 0x00;
		internal const byte HorizontalDisplayed = 0x01;
		internal const byte HorizontalSyncPosition = 0x02;
		internal const byte HorizontalSyncPulseWidth = 0x03;
		internal const byte VerticalTotal = 0x04;
		internal const byte VerticalDisplayed = 0x05;
		internal const byte VerticalSyncPosition = 0x06;
		internal const byte VerticalSuncPulseWidth = 0x07;
		internal const byte InterlaceMode = 0x08;
		internal const byte MaximumScanLines = 0x09;
		internal const byte CursorStart = 0x0A;
		internal const byte CursorEnd = 0x0B;
		internal const byte StartAddress = 0x0C;
		internal const byte StartAddressHigh = 0x0C;
		internal const byte StartAddressLow = 0x0D;
		internal const byte CursorLocationHigh = 0x0E;
		internal const byte CursorLocationLow = 0x0F;
		internal const byte LightPenHigh = 0x10;
		internal const byte LightPenLow = 0x11;
	}

	#endregion

	/// <summary>
	/// Represents an emulated vga text device
	/// </summary>
	public class VGATextDriver : IDisposable
	{
        /// <summary>
        /// 
        /// </summary>
		public const ushort StandardIOBase = 0x03B0;

        /// <summary>
        /// 
        /// </summary>
		public const uint StandardAddressBase = 0xB0000;

        /// <summary>
        /// 
        /// </summary>
		protected IOPort<byte> miscellaneousOutput;

        /// <summary>
        /// 
        /// </summary>
		protected IOPort<byte> crtControllerIndex;

        /// <summary>
        /// 
        /// </summary>
		protected IOPort<byte> crtControllerData;

        /// <summary>
        /// 
        /// </summary>
		protected IOPort<byte> crtControllerIndexColor;

        /// <summary>
        /// 
        /// </summary>
		protected IOPort<byte> crtControllerDataColor;

        /// <summary>
        /// 
        /// </summary>
		protected byte[] memory = new byte[0x10000];

        /// <summary>
        /// 
        /// </summary>
		protected uint baseAddress;

        /// <summary>
        /// 
        /// </summary>
		protected byte height;

        /// <summary>
        /// 
        /// </summary>
		protected byte width;

        /// <summary>
        /// 
        /// </summary>
		protected ushort cursorPosition;

        /// <summary>
        /// 
        /// </summary>
		protected byte lastCommand;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseAddress"></param>
		public VGATextDriver(uint baseAddress)
		{
			this.baseAddress = baseAddress;

			miscellaneousOutput = new IOPort<byte>(0x3CC, 0, null, null);
			crtControllerIndex = new IOPort<byte>(0x3B4, 0, null, CommandWrite);
			crtControllerData = new IOPort<byte>(0x3B5, 0, null, IndexWrite);
			crtControllerIndexColor = new IOPort<byte>(0x3D4, 0, null, CommandWriteColor);
			crtControllerDataColor = new IOPort<byte>(0x3D5, 0, null, IndexWrite);

			IOPortDispatch.RegisterPort(miscellaneousOutput);
			IOPortDispatch.RegisterPort(crtControllerIndex);
			IOPortDispatch.RegisterPort(crtControllerData);
			IOPortDispatch.RegisterPort(crtControllerIndexColor);
			IOPortDispatch.RegisterPort(crtControllerDataColor);

			MemoryDispatch.RegisterMemory(0xB0000, 0x10000, Read8, Write8);

			width = 80;
			height = 25;
			cursorPosition = 0;
			lastCommand = 0;

			miscellaneousOutput.Value = 0x01;	// set color mode

			Console.Clear();
			Console.SetWindowPosition(0, 0);
			Console.SetCursorPosition(0, 0);
			Console.SetWindowSize(width, height + 1);
			Console.SetBufferSize(width, height + 1);
			Console.CursorSize = 1;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="c"></param>
		public void PutChar(ushort x, ushort y, char c)
		{
			int cl = Console.CursorLeft;
			int ct = Console.CursorTop;

			Console.SetCursorPosition(x, y);
			Console.Write(c);

			Console.SetCursorPosition(cl, ct);
		}

        /// <summary>
        /// 
        /// </summary>
		public void Dispose()
		{
			IOPortDispatch.UnregisterPort(miscellaneousOutput);
			IOPortDispatch.UnregisterPort(crtControllerIndex);
			IOPortDispatch.UnregisterPort(crtControllerData);
			IOPortDispatch.UnregisterPort(crtControllerIndexColor);
			IOPortDispatch.UnregisterPort(crtControllerDataColor);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
		public byte Read8(uint address)
		{
			return memory[address - baseAddress];
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
		public void Write8(uint address, byte value)
		{
			if ((value != 0) && (memory[address - baseAddress] == value))
				return;

			memory[address - baseAddress] = value;

			uint index = address - 0xB8000;

			ushort y = (ushort)(index / ((uint)width * 2));
			ushort x2 = (ushort)(index - (y * (uint)width * 2));

			ushort x = (ushort)(x2 >> 1);

			if (x2 % 2 == 0)
				PutChar(x, y, (char)value);
		}

        /// <summary>
        /// 
        /// </summary>
		protected void SetCursor()
		{
			int y = (int)(cursorPosition / width);
			int x = (int)(cursorPosition - (y * width));

			Console.SetCursorPosition(x, y);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
		public byte CommandWrite(byte data)
		{
			switch (data) {
				case CRTCommands.HorizontalDisplayed: crtControllerData.Value = (byte)(width); break;
				case CRTCommands.VerticalDisplayed: crtControllerData.Value = height; break;
				case CRTCommands.CursorLocationHigh: break;
				case CRTCommands.CursorLocationLow: break;
				default: break;
			}

			lastCommand = data;

			return crtControllerIndex.Value;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
		public byte CommandWriteColor(byte data)
		{
			switch (data) {
				case CRTCommands.HorizontalDisplayed: crtControllerDataColor.Value = (byte)(width * 2); break;
				case CRTCommands.VerticalDisplayed: crtControllerDataColor.Value = height; break;
				case CRTCommands.CursorLocationHigh: break;
				case CRTCommands.CursorLocationLow: break;
				default: break;
			}

			lastCommand = data;

			return crtControllerIndexColor.Value;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
		public byte IndexWrite(byte data)
		{

			switch (lastCommand) {
				case CRTCommands.CursorLocationHigh: cursorPosition = (ushort)(((cursorPosition & 0x00FF) | (data << 8))); SetCursor(); break;
				case CRTCommands.CursorLocationLow: cursorPosition = (ushort)(((cursorPosition & 0xFF00) | data)); SetCursor(); break;
				default: break;
			}

			return data;
		}
	}
}
