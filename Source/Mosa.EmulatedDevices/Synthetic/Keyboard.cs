/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;
using System;
using System.Collections.Generic;

namespace Mosa.EmulatedDevices.Synthetic
{
	/// <summary>
	/// Implements an emulated keyboard
	/// </summary>
	public class Keyboard : Device, IKeyboard
	{
		/// <summary>
		///
		/// </summary>
		public delegate void KeyPressed(Key key);

		/// <summary>
		/// Initializes a new instance of the <see cref="Keyboard"/> class.
		/// </summary>
		public Keyboard()
		{
			base.name = "EmulatedKeyboard";
			base.parent = null;
			base.deviceStatus = DeviceStatus.Online;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Keyboard"/> class.
		/// </summary>
		public Keyboard(DisplayForm display)
		{
			base.name = "EmulatedKeyboard";
			base.parent = null;
			base.deviceStatus = DeviceStatus.Online;
			display.onKeyPressed = this.AcceptKey;
		}

		private Queue<Key> keys = new Queue<Key>();

		/// <summary>
		/// Accepts the key.
		/// </summary>
		/// <param name="key">The key.</param>
		public void AcceptKey(Key key)
		{
			keys.Enqueue(key);
		}

		/// <summary>
		/// Gets the key.
		/// </summary>
		/// <returns></returns>
		public Key GetKeyPressed()
		{
			if (keys.Count == 0)
				return null;

			return keys.Dequeue();
		}

		/// <summary>
		/// Gets the key pressed.
		/// </summary>
		/// <returns></returns>
		public Key GetKeyPressed_OLD()
		{
			Key key = new Key();

			ConsoleKeyInfo keyInfo = Console.ReadKey(true);

			key.Character = keyInfo.KeyChar;
			key.Alt = (keyInfo.Modifiers & ConsoleModifiers.Alt) == ConsoleModifiers.Alt;
			key.Control = (keyInfo.Modifiers & ConsoleModifiers.Control) == ConsoleModifiers.Control;
			key.Shift = (keyInfo.Modifiers & ConsoleModifiers.Shift) == ConsoleModifiers.Shift;

			key.KeyType = KeyType.RegularKey;

			switch (keyInfo.Key)
			{
				case ConsoleKey.Home: key.KeyType = KeyType.Home; break;
				case ConsoleKey.UpArrow: key.KeyType = KeyType.UpArrow; break;
				case ConsoleKey.PageUp: key.KeyType = KeyType.PageUp; break;
				case ConsoleKey.LeftArrow: key.KeyType = KeyType.LeftArrow; break;
				case ConsoleKey.RightArrow: key.KeyType = KeyType.RightArrow; break;
				case ConsoleKey.End: key.KeyType = KeyType.End; break;
				case ConsoleKey.DownArrow: key.KeyType = KeyType.DownArrow; break;
				case ConsoleKey.PageDown: key.KeyType = KeyType.PageDown; break;
				case ConsoleKey.Insert: key.KeyType = KeyType.Insert; break;
				case ConsoleKey.Delete: key.KeyType = KeyType.Delete; break;
				case ConsoleKey.F1: key.KeyType = KeyType.F1; break;
				case ConsoleKey.F2: key.KeyType = KeyType.F2; break;
				case ConsoleKey.F3: key.KeyType = KeyType.F3; break;
				case ConsoleKey.F4: key.KeyType = KeyType.F4; break;
				case ConsoleKey.F5: key.KeyType = KeyType.F5; break;
				case ConsoleKey.F6: key.KeyType = KeyType.F6; break;
				case ConsoleKey.F7: key.KeyType = KeyType.F7; break;
				case ConsoleKey.F8: key.KeyType = KeyType.F8; break;
				case ConsoleKey.F9: key.KeyType = KeyType.F9; break;
				case ConsoleKey.F10: key.KeyType = KeyType.F10; break;
				case ConsoleKey.F11: key.KeyType = KeyType.F11; break;
				case ConsoleKey.F12: key.KeyType = KeyType.F12; break;
				case ConsoleKey.LeftWindows: key.KeyType = KeyType.LeftWindow; break;
				case ConsoleKey.RightWindows: key.KeyType = KeyType.RightWindow; break;
				case ConsoleKey.Sleep: key.KeyType = KeyType.Sleep; break;
				case ConsoleKey.PrintScreen: key.KeyType = KeyType.PrintScreen; break;
				default: break;
			}

			return key;
		}

		/// <summary>
		/// Gets a value indicating whether [the scroll lock key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the scroll lock key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool ScrollLock { get { return false; } set { ; } }

		/// <summary>
		/// Gets a value indicating whether [the cap lock key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the cap lock key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool CapLock { get { return Console.CapsLock; } set { ; } }

		/// <summary>
		/// Gets a value indicating whether [the num lock key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the num lock key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool NumLock { get { return Console.NumberLock; } set { ; } }

		/// <summary>
		/// Gets a value indicating whether [the left control key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the left control key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool LeftControl { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether [the right control key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the right control key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool RightControl { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether [the left alt key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the left alt key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool LeftAlt { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether [the right alt key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the right alt key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool RightAlt { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether [the left shift key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the left shift key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool LeftShift { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether [the right shift key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the right shift key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool RightShift { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether [any left control key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [any control key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool Control { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether [any alt key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [any alt key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool Alt { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether [any shift key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [any shift key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool Shift { get { return false; } }
	}
}