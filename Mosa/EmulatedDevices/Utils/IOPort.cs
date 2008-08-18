/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.EmulatedDevices.Utils
{

	public class IOPort<T>
	{
		public delegate T Reader(T newValue);
		public delegate T Writer(T currentValue);

		protected ushort port;
		public T Value;

		private Reader reader;
		private Writer writer;

		public ushort Port
		{
			get
			{
				return port;
			}
		}

		public IOPort(int port)
		{
			this.port = (ushort)port;
		}

		public IOPort(int port, T value)
		{
			this.port = (ushort)port;
			this.Value = value;
		}

		public IOPort(int port, T value, Reader reader, Writer writer)
		{
			this.port = (ushort)port;
			this.Value = value;
			this.reader = reader;
			this.writer = writer;
		}

		public void SetValue(T value)
		{
			if (writer == null) {
				this.Value = value;
				return;
			}

			this.Value = writer(value);
		}

		public T ReadValue()
		{
			if (reader != null)
				this.Value = reader(this.Value);
			return this.Value;
		}
	}
}
