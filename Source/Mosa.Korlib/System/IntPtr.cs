﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace System
{
	[Serializable]
	public struct IntPtr
	{
		private readonly unsafe void* _value; // Do not rename (binary serialization)

		[Intrinsic]
		public static readonly IntPtr Zero;

		[Intrinsic]
		[NonVersionable]
		public unsafe IntPtr(int value)
		{
			_value = (void*)value;
		}

		[Intrinsic]
		[NonVersionable]
		public unsafe IntPtr(long value)
		{
			_value = (void*)((int)value);
		}

		[Intrinsic]
		[NonVersionable]
		public unsafe IntPtr(void* value)
		{
			_value = value;
		}

		public unsafe override bool Equals(Object obj)
		{
			if (obj is IntPtr)
			{
				return (_value == ((IntPtr)obj)._value);
			}
			return false;
		}

		public unsafe override int GetHashCode()
		{
			return ((int)_value);
		}

		[Intrinsic]
		[NonVersionable]
		public unsafe int ToInt32()
		{
			return (int)_value;
		}

		[Intrinsic]
		[NonVersionable]
		public unsafe long ToInt64()
		{
			return (long)_value;
		}

		[Intrinsic]
		[NonVersionable]
		public static unsafe explicit operator IntPtr(int value)
		{
			return new IntPtr(value);
		}

		[Intrinsic]
		[NonVersionable]
		public static unsafe explicit operator IntPtr(long value)
		{
			return new IntPtr(value);
		}

		[Intrinsic]
		[NonVersionable]
		public static unsafe explicit operator IntPtr(void* value)
		{
			return new IntPtr(value);
		}

		[Intrinsic]
		[NonVersionable]
		public static unsafe explicit operator void* (IntPtr value)
		{
			return value._value;
		}

		[Intrinsic]
		[NonVersionable]
		public static unsafe explicit operator int(IntPtr value)
		{
			return (int)value._value;
		}

		[Intrinsic]
		[NonVersionable]
		public static unsafe explicit operator long(IntPtr value)
		{
			return (long)value._value;
		}

		[Intrinsic]
		[NonVersionable]
		public static unsafe bool operator ==(IntPtr value1, IntPtr value2)
		{
			return value1._value == value2._value;
		}

		[Intrinsic]
		[NonVersionable]
		public static unsafe bool operator !=(IntPtr value1, IntPtr value2)
		{
			return value1._value != value2._value;
		}

		[NonVersionable]
		public static IntPtr Add(IntPtr pointer, int offset)
		{
			return pointer + offset;
		}

		[Intrinsic]
		[NonVersionable]
		public static unsafe IntPtr operator +(IntPtr pointer, int offset)
		{
			return new IntPtr(pointer.ToInt64() + (uint)offset);
		}

		[NonVersionable]
		public static IntPtr Subtract(IntPtr pointer, int offset)
		{
			return pointer - offset;
		}

		[Intrinsic]
		[NonVersionable]
		public static unsafe IntPtr operator -(IntPtr pointer, int offset)
		{
			return new IntPtr((long)pointer._value - offset);
		}

		public unsafe static int Size
		{
			[NonVersionable]
			get
			{
				return sizeof(void*);
			}
		}

		[Intrinsic]
		[NonVersionable]
		public unsafe void* ToPointer()
		{
			return _value;
		}

		public unsafe override string ToString()
		{
			return ((long)_value).ToString();
		}
	}
}
