/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.Platform.x86.XSharp
{

	public class Register
	{
		protected XSharpMethod method;
		internal CPURegister CPURegister { get; private set; }
		internal Register(XSharpMethod method, CPURegister cpuRegister) { this.method = method; this.CPURegister = cpuRegister; }

		public XSharpMethod XSharpMethod { get { return method; } }

		public void Push() { method.Push(this); }
		public void Pop() { method.Pop(this); }

		public void Add(int value) { method.Add(this, value); }
		public void Substract(int value) { method.Subtract(this, value); }
		public void Add(uint value) { method.Add(this, value); }
		public void Substract(uint value) { method.Subtract(this, value); }

		public void Add(Register register) { /* TODO */ }
		public void Substract(Register register) { /* TODO */ }

		public void In(int port) { method.In(this, port); }
		public void Out(int port) { method.Out(this, port); }

		public void Test(int interger) { /* TODO */ }
		public void Test(Register register) { /* TODO */ }

		public void Compare(int interger) { /* TODO */ }
		public void Compare(Register register) { /* TODO */ }

		public static Register operator ++(Register register)
		{
			register.method.Increment(register);
			return register;
		}

		public static Register operator --(Register register)
		{
			register.method.Decrement(register);
			return register;
		}

		public void LeftShift(Register register, int shift)
		{
			register.method.LeftShift(register, shift);
		}

		public void RightShift(Register register, int shift)
		{
			register.method.RightShift(register, shift);
		}

		public object Value
		{
			set
			{
				if (value is Int32)
				{
					method.SetValue(this, (int)value);
					return;
				}
				else if (value is UInt32)
				{
					method.SetValue(this, (uint)value);
					return;
				}
				else if (value is Register)
				{
					method.Move(this, value as Register);
					return;
				}
				else if (value is IndexedRegister)
				{
					method.Store(this, value as IndexedRegister);
					return;
				}
			}
		}

		public static Register operator >>(Register register, int shift)
		{
			register.LeftShift(register, shift);
			return register;
		}

		public static Register operator <<(Register register, int shift)
		{
			register.RightShift(register, shift);
			return register;
		}

		public static Register operator +(Register register, int value)
		{
			register.Add(value);
			return register;
		}

		public static Register operator -(Register register, int value)
		{
			register.Substract(value);
			return register;
		}

		public IndexedRegister this[int index]
		{
			get { return new IndexedRegister(this, index); }
		}

	}

}
