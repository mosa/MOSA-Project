/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System.Reflection;
using System.Runtime.CompilerServices;

namespace System
{
	/// <summary>
	/// Implementation of the "System.Type" class.
	/// </summary>
	public class Type
	{
		private Type(RuntimeTypeHandle handle)
		{
			this.m_handle = handle;
			this.FullName = InternalGetFullName(handle);
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

		public Type GetType(string typeName)
		{
			return GetType(typeName, false, false);
		}

		public Type GetType(string typeName, bool throwOnError)
		{
			return GetType(typeName, throwOnError, false);
		}

		public Type GetType(string typeName, bool throwOnError, bool ignoreCase)
		{
			RuntimeTypeHandle handle = InternalGetTypeHandleByName(typeName, throwOnError, ignoreCase);
			return new Type(handle);
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern RuntimeTypeHandle InternalGetTypeHandleByName(string typeName, bool throwOnError, bool ignoreCase);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern string InternalGetFullName(RuntimeTypeHandle handle);

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
	}
}