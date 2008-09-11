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
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public class IOPort<T>
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
		public delegate T Reader(T newValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentValue"></param>
        /// <returns></returns>
		public delegate T Writer(T currentValue);

        /// <summary>
        /// 
        /// </summary>
		protected ushort port;

        /// <summary>
        /// 
        /// </summary>
		public T Value;

        /// <summary>
        /// 
        /// </summary>
		private Reader reader;

        /// <summary>
        /// 
        /// </summary>
		private Writer writer;

        /// <summary>
        /// 
        /// </summary>
		public ushort Port
		{
			get
			{
				return port;
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
		public IOPort(int port)
		{
			this.port = (ushort)port;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="value"></param>
		public IOPort(int port, T value)
		{
			this.port = (ushort)port;
			this.Value = value;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="value"></param>
        /// <param name="reader"></param>
        /// <param name="writer"></param>
		public IOPort(int port, T value, Reader reader, Writer writer)
		{
			this.port = (ushort)port;
			this.Value = value;
			this.reader = reader;
			this.writer = writer;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
		public void SetValue(T value)
		{
			if (writer == null) {
				this.Value = value;
				return;
			}

			this.Value = writer(value);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public T ReadValue()
		{
			if (reader != null)
				this.Value = reader(this.Value);
			return this.Value;
		}
	}
}
