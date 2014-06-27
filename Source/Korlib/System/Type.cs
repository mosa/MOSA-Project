/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System
{
	/// <summary>
	/// Implementation of the "System.Type" class.
	/// </summary>
	public class Type : MemberInfo
	{
		private Type(RuntimeTypeHandle handle)
		{
			this.m_handle = handle;
			this.FullName = InternalGetFullName(this.m_handle);
			this.Attributes = InternalGetAttributes(this.m_handle);
		}

		private RuntimeTypeHandle m_handle;

		public RuntimeTypeHandle TypeHandle
		{
			get
			{
				return m_handle;
			}
		}

		public string FullName
		{
			get;
			private set;
		}

		public TypeAttributes Attributes
		{
			get;
			private set;
		}

		public Module Module
		{
			get;
			private set;
		}

		public static Type GetType(string typeName)
		{
			return GetType(typeName, false, false);
		}

		public static Type GetType(string typeName, bool throwOnError)
		{
			return GetType(typeName, throwOnError, false);
		}

		public static Type GetType(string typeName, bool throwOnError, bool ignoreCase)
		{
			RuntimeTypeHandle handle = InternalGetTypeHandleByName(typeName, ignoreCase);
			
			// Check that we got a valid handle
			if (handle.Value == new IntPtr(0))
			{
				// The handle is invalid so check to see if we should throw an error, otherwise return null
				if (throwOnError)
					throw new ArgumentNullException();
				else
					return null;
			}

			// If handle is valid then return the type
			return new Type(handle);
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern RuntimeTypeHandle InternalGetTypeHandleByName(string typeName, bool ignoreCase);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern string InternalGetFullName(RuntimeTypeHandle handle);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern TypeAttributes InternalGetAttributes(RuntimeTypeHandle handle);

		public static Type GetTypeFromHandle(RuntimeTypeHandle handle)
		{
			return new Type(handle);
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern RuntimeTypeHandle GetTypeHandle(object obj);

		public override string ToString()
		{
			return FullName;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is System.Type))
				return false;

			return ((Type)obj).m_handle == m_handle;
		}

		public bool Equals(Type obj)
		{
			return (obj).m_handle == m_handle;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override Type DeclaringType
		{
			get { throw new NotImplementedException(); }
		}

		public override MemberTypes MemberType
		{
			get { throw new NotImplementedException(); }
		}

		public override string Name
		{
			get { throw new NotImplementedException(); }
		}

		public override Type ReflectedType
		{
			get { throw new NotImplementedException(); }
		}

		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}
	}
}