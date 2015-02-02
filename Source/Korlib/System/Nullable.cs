using System;

namespace System
{
	using System.Collections.Generic;
	using System.Globalization;
	using System.Reflection;
	using System.Runtime;
	using System.Runtime.CompilerServices;
	using System.Security;

	// Warning, don't put System.Runtime.Serialization.On*Serializ*Attribute
	// on this class without first fixing ObjectClone::InvokeVtsCallbacks
	// Also, because we have special type system support that says a a boxed Nullable<T>
	// can be used where a boxed<T> is use, Nullable<T> can not implement any intefaces
	// at all (since T may not).   Do NOT add any interfaces to Nullable!
	//
	[Serializable]
	public struct Nullable<T> where T : struct
	{
		private bool hasValue;
		internal T value;

		public Nullable(T value)
		{
			this.value = value;
			this.hasValue = true;
		}

		public bool HasValue
		{
			get
			{
				return hasValue;
			}
		}

		public T Value
		{
			get
			{
				if (!HasValue)
				{
					throw new Exception("Nullable has no value");
				}
				return value;
			}
		}

		public T GetValueOrDefault()
		{
			return value;
		}

		public T GetValueOrDefault(T defaultValue)
		{
			return HasValue ? value : defaultValue;
		}

		public override bool Equals(object other)
		{
			if (!HasValue) return other == null;
			if (other == null) return false;
			return value.Equals(other);
		}

		public override int GetHashCode()
		{
			return HasValue ? value.GetHashCode() : 0;
		}

		public override string ToString()
		{
			return HasValue ? value.ToString() : "";
		}

		public static implicit operator Nullable<T>(T value)
		{
			return new Nullable<T>(value);
		}

		public static explicit operator T(Nullable<T> value)
		{
			return value.Value;
		}

		// The following already obsoleted methods were removed:
		//   public int CompareTo(object other)
		//   public int CompareTo(Nullable<T> other)
		//   public bool Equals(Nullable<T> other)
		//   public static Nullable<T> FromObject(object value)
		//   public object ToObject()
		//   public string ToString(string format)
		//   public string ToString(IFormatProvider provider)
		//   public string ToString(string format, IFormatProvider provider)

		// The following newly obsoleted methods were removed:
		//   string IFormattable.ToString(string format, IFormatProvider provider)
		//   int IComparable.CompareTo(object other)
		//   int IComparable<Nullable<T>>.CompareTo(Nullable<T> other)
		//   bool IEquatable<Nullable<T>>.Equals(Nullable<T> other)
	}

	//[System.Runtime.InteropServices.ComVisible(true)]
	//public static class Nullable
	//{
	//	[System.Runtime.InteropServices.ComVisible(true)]
	//	public static int Compare<T>(Nullable<T> n1, Nullable<T> n2) where T : struct
	//	{
	//		//if (n1.HasValue)
	//		//{
	//		//	if (n2.HasValue) return Comparer<T>.Default.Compare(n1.value, n2.value);
	//		//	return 1;
	//		//}
	//		//if (n2.HasValue) return -1;
	//		return 0;
	//	}

	//	[System.Runtime.InteropServices.ComVisible(true)]
	//	public static bool Equals<T>(Nullable<T> n1, Nullable<T> n2) where T : struct
	//	{
	//		//if (n1.HasValue)
	//		//{
	//		//	if (n2.HasValue) return EqualityComparer<T>.Default.Equals(n1.value, n2.value);
	//		//	return false;
	//		//}
	//		//if (n2.HasValue) return false;
	//		return true;
	//	}

	//	// If the type provided is not a Nullable Type, return null.
	//	// Otherwise, returns the underlying type of the Nullable type
	//	public static Type GetUnderlyingType(Type nullableType)
	//	{
	//		//if ((object)nullableType == null)
	//		//{
	//		//	throw new ArgumentNullException("nullableType");
	//		//}
	//		//Type result = null;
	//		//if (nullableType.IsGenericType && !nullableType.IsGenericTypeDefinition)
	//		//{
	//		//	// instantiated generic type only
	//		//	Type genericType = nullableType.GetGenericTypeDefinition();
	//		//	if (Object.ReferenceEquals(genericType, typeof(Nullable<>)))
	//		//	{
	//		//		result = nullableType.GetGenericArguments()[0];
	//		//	}
	//		//}
	//		//return result;
	//		return null;
	//	}
	//}
}