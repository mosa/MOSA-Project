/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Runtime.InteropServices;
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
		}

		RuntimeTypeHandle m_handle;
		public RuntimeTypeHandle TypeHandle
		{
			get
			{
				return m_handle;
			}
		}

		string m_fullName;
		public string FullName
		{
			get
			{
				//if (m_fullName == null)
				{
					m_fullName = InternalGetFullName(m_handle.Value);
				}
				return m_fullName;
			}
		}

		unsafe string InternalGetFullName(IntPtr handle)
		{
			int* namePtr = *(int**)(handle.ToInt32() + 8);
			int length = *namePtr;
			namePtr++;

			return new string((sbyte*)namePtr, 0, length);
		}

		public static Type GetTypeFromHandle(RuntimeTypeHandle handle)
		{
			return new Type(handle);
		}

		[DllImport("Mosa.Compiler.Framework.Intrinsics.InternalGetType, Mosa.Compiler.Framework")]
		public static extern RuntimeTypeHandle GetTypeHandle(object obj);

		public static Type GetType(string typeName, bool throwOnError)
		{
			return null;
		}
	}
}